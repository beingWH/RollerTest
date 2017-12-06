using RollerTest.Domain.Abstract;
using RollerTest.Domain.Entities;
using RollerTest.WebUI.ExternalProgram;
using RollerTest.WebUI.IniFiles;
using RollerTest.WebUI.Models;
using RollerTest.WebUI.Models.PROCEDURE;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace RollerTest.WebUI.Controllers
{
    [Authorize(Roles = "Tester,Admin,Manager")]
    public class SampleTestingRecordController : Controller
    {
        private IRecordinfoRepository recordrepo;
        private ISampleinfoRepository samplerepo;
        private IBaseRepository baserepo;
        private RollerTimer rollertimer = RollerTimer.GetInstance();
        private RollerForcer rollerforcer = RollerForcer.GetInstance();
        public SampleTestingRecordController(IRecordinfoRepository recordrepo, ISampleinfoRepository samplerepo, IBaseRepository baserepo)
        {
            this.recordrepo = recordrepo;
            this.samplerepo = samplerepo;
            this.baserepo = baserepo;
        }
        // GET: SampleTestingRecord
        public ActionResult Index(int RollerSampleInfoId)
        {
            SampleTestingRecordViewModel testingrecordviewModel = new SampleTestingRecordViewModel()
            {
                rollerrecordinfos = recordrepo.RollerRecordInfos.Where(x => x.RollerSampleInfoID == RollerSampleInfoId).Include(x=>x.RollerSampleInfo),
                SampleId=RollerSampleInfoId
            };
            return View(testingrecordviewModel);
        }

        public ViewResult CreateSampleTestingRecord(int RollerSampleInfoID)
        {
            RollerSampleInfo rollersampleinfo = samplerepo.RollerSampleInfos.FirstOrDefault(a => a.RollerSampleInfoID == RollerSampleInfoID);
            return View("EditSampleTestingRecord", new RollerRecordInfo() { RollerSampleInfoID = RollerSampleInfoID,RollerSampleInfo= rollersampleinfo });
        }
        [HttpGet]
        public ViewResult EditSampleTestingRecord(int RollerRecordInfoID)
        {
            RollerRecordInfo rollerrecordinfo = recordrepo.RollerRecordInfos.FirstOrDefault(a => a.RollerRecordInfoID == RollerRecordInfoID);
            return View(rollerrecordinfo);
        }
        [HttpPost]
        public ActionResult EditSampleTestingRecord(RollerRecordInfo rollerrecordinfo)
        {
            if (ModelState.IsValid)
            {
                RollerSampleInfo rollersampleinfo = samplerepo.RollerSampleInfos.FirstOrDefault(x => x.RollerSampleInfoID == rollerrecordinfo.RollerSampleInfoID);
                int sampleId = rollerrecordinfo.RollerSampleInfoID;
                string totaltime = rollertimer.ReadTimeData(baserepo.RollerBaseStations.FirstOrDefault(x => x.Station == rollersampleinfo.RollerBaseStation.Station).TimerCfg.TimerName);
                if (rollerrecordinfo.SampleStatus == false)
                {
                    samplerepo.setsampleState(sampleId, "故障");
                    rollertimer.CloseRollerTimeSwitch(rollersampleinfo.RollerBaseStation.TimerCfg.TimerName);
                    rollerforcer.CloseRollerForcerSwitch(rollersampleinfo.RollerBaseStation.ForcerCfg.ForcerName);
                }
                rollerrecordinfo.CurrentTime = DateTime.Now;
                rollerrecordinfo.TotalTime = totaltime;
                recordrepo.SaveRollerRecordInfo(rollerrecordinfo);

                Entities context = new Entities();
                context.PROCEDURE_ROLLERRECORDINFO(0);
                context.SaveChanges();
                return RedirectToActionPermanent("Index", new { RollerSampleInfoID = rollerrecordinfo.RollerSampleInfoID });
            }
            else
            {
                RollerSampleInfo rollersamleinfo = samplerepo.RollerSampleInfos.FirstOrDefault(a => a.RollerSampleInfoID == rollerrecordinfo.RollerSampleInfoID);
                return View("EditSampleTestingRecord", new RollerRecordInfo() { RollerSampleInfoID = rollerrecordinfo.RollerSampleInfoID, RollerSampleInfo = rollersamleinfo });
            }
        }
        [HttpPost]
        public ActionResult DeleteSampleTestingRecord(int RollerRecordInfoId, int RollerSampleInfoID)
        {
            Entities context = new Entities();
            context.PROCEDURE_ROLLERRECORDINFO(RollerRecordInfoId);
            context.SaveChanges();
            recordrepo.DeleteRollerRecordInfo(RollerRecordInfoId);
            return RedirectToAction("Index", new { RollerSampleInfoId = RollerSampleInfoID });
        }

    }
}