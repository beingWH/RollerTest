using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RollerTest.WebUI.ExternalProgram.Commands
{
    public class MotoRun : ICommand
    {

        public void execute()
        {
            CdioControl.GetInstance().setMotoRunning();
        }
        public string getDescription()
        {
            return "正转";
        }
    }
    public class MotoREVRun : ICommand
    {
        public void execute()
        {
            CdioControl.GetInstance().setMotoRunningREV();
        }
        public string getDescription()
        {
            return "反转";
        }
    }
    public class MotoStop : ICommand
    {
        public void execute()
        {
            CdioControl.GetInstance().setMotoStopping();
        }
        public string getDescription()
        {
            return "停止";
        }
    }
}