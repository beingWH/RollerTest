using RollerTest.WebUI.IniFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RollerTest.WebUI.ExternalProgram.Commands
{
    public class FileWirteOn : ICommand
    {
        public void execute()
        {
            IniFileControl.GetInstance().OpenTimer();
        }

        public string getDescription()
        {
            return "开启";
        }
    }
    public class FileWriteOff : ICommand
    {
        public void execute()
        {
            IniFileControl.GetInstance().CloseTimer();
        }
        public string getDescription()
        {
            return "关闭";
        }
    }
    public class FileReadOn : ICommand
    {
        public void execute()
        {
            IniFileControl.GetInstance().OpenTimer2();
        }
        public string getDescription()
        {
            return "开启";
        }
    }
    public class FileReadOff : ICommand
    {
        public void execute()
        {
            IniFileControl.GetInstance().CloseTimer2();
        }
        public string getDescription()
        {
            return "关闭";
        }
    }
}