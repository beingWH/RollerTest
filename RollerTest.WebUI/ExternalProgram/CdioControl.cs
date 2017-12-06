using RollerTest.Domain.Entities;
using System.Timers;

namespace RollerTest.WebUI.ExternalProgram
{
    public class CdioControl
    {
        private static CdioControl instance;
        private static readonly object locker = new object();
        public static System.Timers.Timer controlTimer = new System.Timers.Timer(1000);
        private MoterService moterState = new MoterService();
        private CdioMethod cdioMethod = new CdioMethod();
        private CdioControl()
        {
            controlTimer.Elapsed += ControlTimer_Elapsed;
            controlTimer.Enabled = true;
            cdioMethod.InitDio();
        }
        public static CdioControl GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new CdioControl();
                    }
                }
            }
            return instance;
        }
        public void MonitorMoto()
        {
            if (moterState.IsMotoRunning == false)
            {
                cdioMethod.MotoStop();
            }
            else
            {
                if (moterState.MotoREV == 0)
                {
                    cdioMethod.MotoRun(0);
                }
                else
                {
                    cdioMethod.MotoRun(1);
                }
            }
        }

        private void ControlTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            MonitorMoto();
        }

        public string InitMotoState()
        {
            return cdioMethod.InitDioState();
        }
        public bool TimerState()
        {
            return controlTimer.Enabled;
        }
        public MoterService getMotoState()
        {
            return this.moterState;
        }
        public void setMotoRunning()
        {
            this.moterState.MotoREV = 0;
            this.moterState.IsMotoRunning = true;
        }
        public void setMotoStopping()
        {
            this.moterState.IsMotoRunning = false;
        }
        public void setMotoRunningREV()
        {
            this.moterState.MotoREV = 1;
            this.moterState.IsMotoRunning = true;
        }

    }
}