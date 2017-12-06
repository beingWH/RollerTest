using RollerTest.Domain.Abstract;
using RollerTest.Domain.Entities;
using RollerTest.WebUI.ExternalProgram;
using RollerTest.WebUI.IniFiles;
using RollerTest.WebUI.Models;
using RollerTest.WebUI.Models.PROCEDURE;
using RollerTest.WebUI.Models.WTTESTMODEL;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace RollerTest.WebUI.Controllers
{
    public class SampleController : Controller
    {
        private ISampleinfoRepository repository;
        private IProjectRepository projectrepo;
        private IBaseRepository baserepository;
        private RollerTimer rollertimer = RollerTimer.GetInstance();
        private RollerForcer rollerforcer = RollerForcer.GetInstance();
        private IReadRepository<WTTESTEQUIPMENT> wtequipmentrepo;
        private IReadRepository<WTSAMPLEINFO> wtsampleinfo;
        public SampleController(ISampleinfoRepository repo,IProjectRepository prepo,IBaseRepository baserepo,
            IReadRepository<WTTESTEQUIPMENT> wtequipmentrepo, IReadRepository<WTSAMPLEINFO> wtsampleinfo)
        {
            repository = repo;
            projectrepo = prepo;
            baserepository = baserepo;
            this.wtequipmentrepo = wtequipmentrepo;
            this.wtsampleinfo = wtsampleinfo;
        }
        // GET: Project
        public ActionResult Index()
        {
            ProjectListViewModel projectlistviewModel = new ProjectListViewModel()
            {
                rollerprojectinfos = projectrepo.RollerProjectInfos
            };
            
            return View(projectlistviewModel);
        }
        public ActionResult ViewInfo(int RollerProjectInfoID)
        {
            SampleViewModel sampleviewmodel = new SampleViewModel()
            {
                rollerprojectinfo = projectrepo.RollerProjectInfos.FirstOrDefault(a => a.RollerProjectInfoID == RollerProjectInfoID),
                rollersampleinfos = repository.RollerSampleInfos.Where(a => a.RollerProjectInfo.RollerProjectInfoID == RollerProjectInfoID&&!a.State.Equals("结束")).Include(x => x.RollerBaseStation),
                 projectlistviewmodel = new ProjectListViewModel() { rollerprojectinfos = projectrepo.RollerProjectInfos }
            };
            return View(sampleviewmodel);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public ViewResult CreateSample(int RollerProjectInfoID)
        {
            int TestID = projectrepo.RollerProjectInfos.FirstOrDefault(x => x.RollerProjectInfoID == RollerProjectInfoID).TestID;
            SettingViewModel settingviewModel = new SettingViewModel(baserepository, wtequipmentrepo, wtsampleinfo);
            ViewData["Device"] = settingviewModel.GetLISDeviceList();
            ViewData["TestTypeList"] = settingviewModel.GetTestTypeList();
            ViewData["StationList"] = settingviewModel.GetStationList() ;
            ViewData["SampleIDList"] = settingviewModel.GetSampleIDList(TestID);
            return View("EditSample", new RollerSampleInfo() { RollerProjectInfoID = RollerProjectInfoID, State = "准备",TestID=TestID });
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        [HttpPost]
        public ActionResult EditSample(RollerSampleInfo rollersampleinfo)
        {
            if (ModelState.IsValid) {
                rollersampleinfo.State = "准备";
                TestSampleInfo.GetInstance().AddTestSample(rollersampleinfo);
                repository.SaveRollerSampleInfo(rollersampleinfo);
                //同步中间数据库
                Entities context = new Entities();
                context.PROCEDURE_ROLLERSAMPLEINFO(0);
                context.SaveChanges();

                RollerSampleInfo rsi = repository.RollerSampleInfos.Include(x=>x.RollerBaseStation).Include(x=>x.RollerBaseStation.TimerCfg).Include(x=>x.RollerBaseStation.ForcerCfg).FirstOrDefault(x => x.RollerSampleInfoID == rollersampleinfo.RollerSampleInfoID);
                TestSampleInfo.GetInstance().EditSampleList(rsi);
                return RedirectToAction("ViewInfo", new { RollerProjectInfoID = rollersampleinfo.RollerProjectInfoID });
            }
            else
            {
                SettingViewModel settingviewModel = new SettingViewModel(baserepository, wtequipmentrepo, wtsampleinfo);
                int TestID = projectrepo.RollerProjectInfos.FirstOrDefault(x => x.RollerProjectInfoID == rollersampleinfo.RollerProjectInfoID).TestID;
                ViewData["Device"] = settingviewModel.GetLISDeviceList();
                ViewData["StationList"] = settingviewModel.GetStationList();
                ViewData["SampleIDList"] = settingviewModel.GetSampleIDList(TestID);
                ViewData["TestTypeList"] = settingviewModel.GetTestTypeList();
                return View(rollersampleinfo);
            }
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        [HttpGet]
        public  ViewResult EditSample(int RollerSampleInfoID,int RollerProjectInfoID)
        {
            SettingViewModel settingviewModel = new SettingViewModel(baserepository, wtequipmentrepo, wtsampleinfo);
            RollerSampleInfo rollersampleinfo = repository.RollerSampleInfos.FirstOrDefault(p => p.RollerSampleInfoID == RollerSampleInfoID);
            rollersampleinfo.State = "结束";
            repository.SaveRollerSampleInfo(rollersampleinfo);
            int TestID = projectrepo.RollerProjectInfos.FirstOrDefault(x => x.RollerProjectInfoID==RollerProjectInfoID).TestID;
            ViewData["Device"] = settingviewModel.GetLISDeviceList();
            ViewData["StationList"] = settingviewModel.GetStationList();
            ViewData["SampleIDList"] = settingviewModel.GetSampleIDList(TestID);
            ViewData["TestTypeList"] = settingviewModel.GetTestTypeList();
            rollertimer.CloseRollerTimeSwitch(rollersampleinfo.RollerBaseStation.TimerCfg.TimerName);
            rollerforcer.CloseRollerForcerSwitch(rollersampleinfo.RollerBaseStation.ForcerCfg.ForcerName);
            return View(rollersampleinfo);
        }
        [Authorize(Roles = "Tester,Admin,Manager"), ActionName("Delete")]
        [HttpPost]
        public ActionResult DeleteSample(RollerSampleInfo rsi)
        {
            Entities context = new Entities();
            context.PROCEDURE_ROLLERSAMPLEINFO(rsi.RollerSampleInfoID);
            context.SaveChanges();
            //Delete post提交上来的对象其实只含有ID值，其他均为空，注意！！！
            int RollerProjectInfoID = repository.RollerSampleInfos.FirstOrDefault(x=>x.RollerSampleInfoID==rsi.RollerSampleInfoID).RollerProjectInfoID;
            rollertimer.CloseRollerTimeSwitch(repository.RollerSampleInfos.FirstOrDefault(x => x.RollerSampleInfoID == rsi.RollerSampleInfoID).RollerBaseStation.TimerCfg.TimerName);
            repository.DeleteRollerSampleInfo(rsi.RollerSampleInfoID);
            TestSampleInfo.GetInstance().RemoveSampleList(rsi.RollerSampleInfoID);
            return RedirectToAction("ViewInfo", new { RollerProjectInfoID = RollerProjectInfoID });
        }
        [Authorize(Roles = "Tester,Admin,Manager"),ActionName("Delete")]
        public ViewResult DeleteSample(int RollerSampleInfoID)
        {
            RollerSampleInfo rsi=repository.RollerSampleInfos.FirstOrDefault(x => x.RollerSampleInfoID == RollerSampleInfoID);
            return View("DeleteSample",rsi);
        }
        public JsonResult StationList(string device)
        {
            SettingViewModel settingviewModel = new SettingViewModel(baserepository, wtequipmentrepo, wtsampleinfo);
            Object stationlist=settingviewModel.GetStationList(device);       
            return Json(stationlist, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SampleName(string SampleID) {
            string samplename = wtsampleinfo.QueryEntities.FirstOrDefault(x => x.SERIAL_NO == SampleID).NAME;
            return Json(samplename, JsonRequestBehavior.AllowGet);
        }


    }
}