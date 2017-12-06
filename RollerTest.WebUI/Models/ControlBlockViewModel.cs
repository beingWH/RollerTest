using RollerTest.Domain.Entities;
using RollerTest.WebUI.IniFiles;
using System.Collections.Generic;

namespace RollerTest.WebUI.Models
{
    public class ControlBlockViewModel
    {
        public IEnumerable<RollerBaseStation> rollerbasestations { get; set; }
        public TimeData td { get; set; }
        
        public string tdstr(string TimeName)
        {
            string str = "";
            switch (TimeName)
            {
                case "Timer1":str = td.TimeData1; break;
                case "Timer2": str = td.TimeData2; break;
                case "Timer3": str = td.TimeData3; break;
                case "Timer4": str = td.TimeData4; break;
                case "Timer5": str = td.TimeData5; break;
                case "Timer6": str = td.TimeData6; break;
                case "Timer7": str = td.TimeData7; break;
                case "Timer8": str = td.TimeData8; break;
                case "Timer9": str = td.TimeData9; break;
                case "Timer10": str = td.TimeData10; break;
                case "Timer11": str = td.TimeData11; break;
                case "Timer12": str = td.TimeData12; break;
            }
            return str;
        }
    }
}