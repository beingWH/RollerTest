using RollerTest.Domain.Abstract;
using RollerTest.WebUI.IniFiles;
using RollerTest.WebUI.ExternalProgram;
using RollerTest.WebUI.Models;
using System;
using System.Web.Mvc;
using System.Threading.Tasks;
using RollerTest.WebUI.Helpers;
using System.Data.Entity;
using RollerTest.WebUI.ExternalProgram.Commands;

namespace RollerTest.WebUI.Controllers
{
    public class ControlBlockController : Controller
    {
        private IniFileControl inifileControl = IniFileControl.GetInstance();
        private CdioControl cdioControl = CdioControl.GetInstance();
        private DealControl dealControl = DealControl.GetInstance();
        private IBaseRepository baserepo;
        public ControlBlockController(IBaseRepository repo)
        {
            baserepo = repo;
        }
        // GET: ControlBlock
        public ActionResult Index()
        {
            ControlBlockViewModel cbvm = new ControlBlockViewModel()
            {
                rollerbasestations = baserepo.RollerBaseStations.Include(x => x.ForcerCfg).Include(x=>x.TimerCfg),
                td = RollerTimer.GetInstance().ReadAllRollerTime()
            };
            return View(cbvm);
        }

        public JsonResult StateLoad()
        {
            try
            {

                string runState = "";
                string cdioconnectState = "";
                if (cdioControl.InitMotoState().Equals("Success"))
                {
                    cdioconnectState = "连接";
                }
                else
                {
                    cdioconnectState = cdioControl.InitMotoState();
                }
                if (cdioControl.getMotoState().IsMotoRunning == false)
                {
                    runState = "停止";
                }
                else if (cdioControl.getMotoState().MotoREV == 0)
                {
                    runState = "正转";
                }
                else if (cdioControl.getMotoState().MotoREV == 1)
                {
                    runState = "反转";
                }

                var state = new
                {
                    filestate = inifileControl.TimerState() == false ? "关闭" : "开启",
                    cdiostate = cdioControl.TimerState() == false ? "关闭" : "开启",
                    timestate = inifileControl.Timer2State() == false ? "关闭" : "开启",
                    runstate = runState,
                    cdioconnectstate = cdioconnectState,
                    dealstate = dealControl.getConnectState() == false ? "断开" : "连接"

                };
                return Json(state, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var state = new
                {
                    filestate = "错误",
                    cdiostate = "错误",
                    timestate = "错误",
                    runstate = "错误",
                    cdioconnectstate = "错误",
                    dealstate = "错误"
            };
                LogHelper.WriteLog(this.GetType(), ex);
                return Json(state, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult FileOpen()
        {
            ICommand command = new FileWirteOn();
            Invoker invoker = new Invoker(command);
            invoker.Action();
            return Json(invoker.getDescription(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult FileClose()
        {
            ICommand command = new FileWriteOff();
            Invoker invoker = new Invoker(command);
            invoker.Action();
            return Json(invoker.getDescription(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult TimerOpen()
        {
            ICommand command = new FileReadOn();
            Invoker invoker = new Invoker(command);
            invoker.Action();
            return Json(invoker.getDescription(), JsonRequestBehavior.AllowGet);      
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult TimerClose()
        {
            ICommand command = new FileReadOff();
            Invoker invoker = new Invoker(command);
            invoker.Action();
            return Json(invoker.getDescription(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult MotoRunning()
        {
            ICommand command = new MotoRun();
            Invoker invoker = new Invoker(command);
            invoker.Action();
            return Json(invoker.getDescription(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult MotoRunningREV()
        {
            ICommand command = new MotoREVRun();
            Invoker invoker = new Invoker(command);
            invoker.Action();
            return Json(invoker.getDescription(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult MotoStopping()
        {
            ICommand command = new MotoStop();
            Invoker invoker = new Invoker(command);
            invoker.Action();
            return Json(invoker.getDescription(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult OpenDeal()
        {
            ICommand command = new DealRun();
            Invoker invoker = new Invoker(command);
            invoker.Action();
            return Json(invoker.getDescription(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult CloseDeal()
        {
            ICommand command = new DealStop();
            Invoker invoker = new Invoker(command);
            invoker.Action();
            return Json(invoker.getDescription(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult OpenTimePort()
        {
            Task task = new Task(()=> {
                RollerTimer.GetInstance().OpenAllTimeSwitch();
            });
            task.Start();
            task.Wait();
            string str = "已开启所有计数端口!";
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult CloseTimePort()
        {
            Task task = new Task(() => {
                RollerTimer.GetInstance().CloseAllTimeSwitch();
            });
            task.Start();
            string str = "已关闭所有计数端口!";
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult OpenForcePort()
        {
            RollerForcer.GetInstance().OpenAllLimitSwtich();
            string str = "已开启所有应力端口!";
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Tester,Admin,Manager")]
        public JsonResult CloseForcePort()
        {
            RollerForcer.GetInstance().CloseAllLimitSwtich();
            string str = "已关闭所有应力端口!";
            return Json(str, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ReadTimeToBuffer() {
            RollerTimer.GetInstance().ReadAllTimeBuffer();
            string str = "已读取时间至内存!";
            return Json(str, JsonRequestBehavior.AllowGet);
        }


    }
}