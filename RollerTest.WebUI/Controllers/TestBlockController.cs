using RollerTest.Domain.Abstract;
using RollerTest.Domain.Entities;
using RollerTest.WebUI.ExternalProgram;
using RollerTest.WebUI.IniFiles;
using RollerTest.WebUI.Models;
using RollerTest.WebUI.Models.PROCEDURE;
using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RollerTest.WebUI.Controllers
{
    public class TestBlockController : Controller
    {
        private ISampleinfoRepository samplerepo;
        private IBaseRepository baserepo;
        private RollerTimer rollertimer = RollerTimer.GetInstance();
        private RollerForcer rollerforcer = RollerForcer.GetInstance();
        private TestSampleInfo tsi = TestSampleInfo.GetInstance();

        public TestBlockController(ISampleinfoRepository repo,IBaseRepository baserepo)
        {
            samplerepo = repo;
            this.baserepo = baserepo;
        }
        // GET: TestBlock
        public ActionResult Index()
        {
            TestListViewModel testlistviewModel = new TestListViewModel()
            {
                rollersampleinfos = samplerepo.RollerSampleInfos.Where(x => !x.State.Equals("结束")).Include(x => x.RollerBaseStation).Include(x => x.RollerProjectInfo).Include(x=>x.RollerRecordInfo)
            };
            return View(testlistviewModel);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult OpenTest(string station)
        {
            Task dbtask = new Task(() => {
                RollerSampleInfo rollersampleinfo = samplerepo.RollerSampleInfos.FirstOrDefault(x => x.RollerBaseStation.Station == station && !x.State.Equals("结束"));
                int StationId = rollersampleinfo.RollerBaseStationID;
                baserepo.ChangeStationState(StationId, true);
                baserepo.SaveForceInfo(rollersampleinfo.RollerBaseStation.ForcerCfg.ForcerName, rollersampleinfo.UpLimit, rollersampleinfo.DnLimit, rollersampleinfo.SetValue, true);
                samplerepo.setsampleStartTime(rollersampleinfo);
                samplerepo.setsampleState(rollersampleinfo.RollerSampleInfoID, "开始");
                rollersampleinfo= samplerepo.RollerSampleInfos.Include(x=>x.RollerBaseStation).Include(x=>x.RollerBaseStation.ForcerCfg).Include(x=>x.RollerBaseStation.TimerCfg).FirstOrDefault(x => x.RollerBaseStation.Station == station && !x.State.Equals("结束"));
                tsi.AddTestSample(rollersampleinfo);
                rollertimer.ReadTimeData(baserepo.RollerBaseStations.FirstOrDefault(x => x.RollerBaseStationID == StationId).TimerCfg.TimerName);
                rollerforcer.setJudgeSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.RollerBaseStationID == StationId).ForcerCfg.ForcerName, true);
                rollerforcer.OpenRollerForcerSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.RollerBaseStationID == StationId).ForcerCfg.ForcerName);
                rollertimer.OpenRollerTimeSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.RollerBaseStationID == StationId).TimerCfg.TimerName);
                Entities context = new Entities();
                context.PROCEDURE_ROLLERSAMPLEINFO(0);
                context.SaveChanges();
            });
            dbtask.Start();
            dbtask.Wait();
            string str = "成功开启试验！";
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult PauseTest(string station)
        {
            Task dbtask = new Task(()=> {
                RollerSampleInfo rollersampleinfo = samplerepo.RollerSampleInfos.FirstOrDefault(x => x.RollerBaseStation.Station == station && !x.State.Equals("结束"));
                samplerepo.setsampleState(rollersampleinfo.RollerSampleInfoID, "暂停");
                rollertimer.CloseRollerTimeSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.Station == station).TimerCfg.TimerName);
                rollerforcer.setJudgeSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.Station == station).ForcerCfg.ForcerName, false);
                rollerforcer.CloseRollerForcerSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.Station == station).ForcerCfg.ForcerName);
                Entities context = new Entities();
                context.PROCEDURE_ROLLERSAMPLEINFO(0);
                context.SaveChanges();

            });
            dbtask.Start();
            dbtask.Wait();
            string str = "成功暂停试验！";
            return Json(str, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult CloseTest(string station)
        {
            Task dbtask = new Task(() =>
            {
                RollerSampleInfo rollersampleinfo = samplerepo.RollerSampleInfos.FirstOrDefault(x => x.RollerBaseStation.Station == station && !x.State.Equals("结束"));
                int StationId = rollersampleinfo.RollerBaseStation.RollerBaseStationID;
                int sampleId = rollersampleinfo.RollerSampleInfoID;
                baserepo.ChangeStationState(StationId, false);
                samplerepo.setsampleState(sampleId, "结束");
                samplerepo.setsampleEndTime(sampleId);
                string totaltime = rollertimer.ReadTimeData(rollersampleinfo.RollerBaseStation.TimerCfg.TimerName);
                if (totaltime != null) { samplerepo.setsampleTotalTime(sampleId, TimeSpan.Parse(totaltime)); }
                rollerforcer.setJudgeSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.RollerBaseStationID == StationId).ForcerCfg.ForcerName, false);
                rollerforcer.CloseRollerForcerSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.RollerBaseStationID == StationId).ForcerCfg.ForcerName);
                baserepo.SaveForceInfo(baserepo.RollerBaseStations.FirstOrDefault(x => x.RollerBaseStationID == StationId).ForcerCfg.ForcerName,0,0,0,false);
                rollertimer.CloseRollerTimeSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.RollerBaseStationID == StationId).TimerCfg.TimerName);
                rollertimer.CleanRollerTime(baserepo.RollerBaseStations.FirstOrDefault(x => x.RollerBaseStationID == StationId).TimerCfg.TimerName);
                tsi.RemoveSampleList(sampleId);
                Entities context = new Entities();
                context.PROCEDURE_ROLLERSAMPLEINFO(0);
                context.SaveChanges();
            });
            dbtask.Start();
            dbtask.Wait();
            string str = "成功结束试验！";
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult OpenTimer(string station)
        {
            rollertimer.OpenRollerTimeSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.Station == station).TimerCfg.TimerName);
            return Json(station,JsonRequestBehavior.AllowGet);

        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult PauseTimer(string station)
        {
            rollertimer.CloseRollerTimeSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.Station == station).TimerCfg.TimerName);
            return Json(station, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult CleanTimer(string station)
        {
            rollertimer.CleanRollerTime(baserepo.RollerBaseStations.FirstOrDefault(x => x.Station == station).TimerCfg.TimerName);
            return Json(station, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult OpenForce(string station)
        {
            rollerforcer.OpenRollerForcerSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.Station == station).ForcerCfg.ForcerName);
            return Json(station, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult CloseForce(string station)
        {
            rollerforcer.CloseRollerForcerSwitch(baserepo.RollerBaseStations.FirstOrDefault(x => x.Station == station).ForcerCfg.ForcerName);
            return Json(station, JsonRequestBehavior.AllowGet);
        }
        public JsonResult StateLoad()
        {
            string str1 = ""; string str2 = ""; string str3 = ""; string str4 = ""; string str5 = ""; string str6 = ""; string str7 = ""; string str8 = ""; string str9 = ""; string str10 = ""; string str11 = ""; string str12 = "";
            foreach (var p in samplerepo.RollerSampleInfos.Where(x => !x.State.Equals("结束")).Include(x => x.RollerBaseStation))
            {
                switch (p.RollerBaseStation.Mark)
                {
                    case "1":str1 = p.State;break;
                    case "2": str2 = p.State; break;
                    case "3": str3 = p.State; break;
                    case "4": str4 = p.State; break;
                    case "5": str5 = p.State; break;
                    case "6": str6 = p.State; break;
                    case "7": str7 = p.State; break;
                    case "8": str8 = p.State; break;
                    case "9": str9 = p.State; break;
                    case "10": str10 = p.State; break;
                    case "11": str11 = p.State; break;
                    case "12": str12 = p.State; break;
                }
            }
           
            var state = new
            {
                state1=str1,
                state2 = str2,
                state3 = str3,
                state4 = str4,
                state5 = str5,
                state6 = str6,
                state7 = str7,
                state8 = str8,
                state9 = str9,
                state10 = str10,
                state11 = str11,
                state12 = str12
            };
            return Json(state, JsonRequestBehavior.AllowGet);
        }


    }
}