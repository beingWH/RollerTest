using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RollerTest.WebUI.ExternalProgram.Commands
{
    public class DealRun : ICommand
    {
        public void execute()
        {
            DealControl.GetInstance().DealConnect();
        }

        public string getDescription()
        {
            return "连接";
        }
    }
    public class DealStop : ICommand
    {
        public void execute()
        {
            DealControl.GetInstance().DealConnectDis();
        }

        public string getDescription()
        {
            return "断开";
        }
    }
}