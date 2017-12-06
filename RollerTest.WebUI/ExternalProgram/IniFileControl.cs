using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using RollerTest.Domain.Concrete;
using RollerTest.Domain.Entities;
using RollerTest.WebUI.ExternalProgram;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RollerTest.WebUI.IniFiles
{
    public class IniFileControl
    {
        private static IniFileControl instance;
        private static readonly object locker = new object();
        public static System.Timers.Timer controlTimer = new System.Timers.Timer(1000);
        public static System.Timers.Timer controlTimer2= new System.Timers.Timer(1000);
        private RollerTimer rollerTime = RollerTimer.GetInstance();
        private TestSampleInfo tsi = TestSampleInfo.GetInstance();
        private RollerForcer rollerforcer = RollerForcer.GetInstance();
        private TimeSpan ts1, ts2, ts3, ts4, ts5, ts6, ts7, ts8, ts9, ts10, ts11, ts12;
        System.TimeSpan duration = new System.TimeSpan(0, 0, 0, 1);
        private EFBaseRepository baserepo = new EFBaseRepository();
        private EFSampleinfoRepository samplerepo = new EFSampleinfoRepository();
        private CancellationTokenSource relativects;
        private Task relativetask;
        private IniFileControl() {
            controlTimer.Elapsed += ControlTimer_Elapsed;
            controlTimer.Enabled = false;
            controlTimer2.Elapsed += ControlTimer2_Elapsed;
            controlTimer2.Enabled = false;
        }

        public static IniFileControl GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new IniFileControl();
                    }
                }
            }
            return instance;
        }
        private void ControlTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach(var p in tsi.rollersampleinfos)
            {
               rollerTime.WriteTimeData(p.RollerBaseStation.TimerCfg.TimerName);
            }
        }
        public bool TimerState()
        {
            return controlTimer.Enabled;
        }
        public void OpenTimer()
        {
            controlTimer.Enabled = true;
        }
        public void CloseTimer()
        {
            controlTimer.Enabled = false;
        }
        private string DayToHours(TimeSpan ts)
        {
            string hrs = (ts.Days * 24 + ts.Hours).ToString("D2");
            return hrs + ":" + ts.Minutes.ToString("D2") + ":" + ts.Seconds.ToString("D2");
        }

        private void ControlTimer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ts1 = TimeSpan.Parse(rollerTime.td.TimeData1);                 //累计时间的累加
            ts1 = ts1.Add(duration);
            ts2 = TimeSpan.Parse(rollerTime.td.TimeData2);
            ts2 = ts2.Add(duration);
            ts3 = TimeSpan.Parse(rollerTime.td.TimeData3);
            ts3 = ts3.Add(duration);
            ts4 = TimeSpan.Parse(rollerTime.td.TimeData4);
            ts4 = ts4.Add(duration);
            ts5 = TimeSpan.Parse(rollerTime.td.TimeData5);
            ts5 = ts5.Add(duration);
            ts6 = TimeSpan.Parse(rollerTime.td.TimeData6);
            ts6 = ts6.Add(duration);
            ts7 = TimeSpan.Parse(rollerTime.td.TimeData7);
            ts7 = ts7.Add(duration);
            ts8 = TimeSpan.Parse(rollerTime.td.TimeData8);
            ts8 = ts8.Add(duration);
            ts9 = TimeSpan.Parse(rollerTime.td.TimeData9);
            ts9 = ts9.Add(duration);
            ts10 = TimeSpan.Parse(rollerTime.td.TimeData10);
            ts10 = ts10.Add(duration);
            ts11 = TimeSpan.Parse(rollerTime.td.TimeData11);
            ts11 = ts11.Add(duration);
            ts12 = TimeSpan.Parse(rollerTime.td.TimeData12);
            ts12 = ts12.Add(duration);
            if (rollerTime.tds.TimeDataSwitch1)
            {
                rollerTime.td.TimeData1 = ts1.Days.ToString("D1") + "." + ts1.Hours.ToString("D2") + ":" + ts1.Minutes.ToString("D2") + ":" + ts1.Seconds.ToString("D2");
                Send("AI1-1-01", DayToHours(ts1));
            }
            if (rollerTime.tds.TimeDataSwitch2)
            {
                rollerTime.td.TimeData2 = ts2.Days.ToString("D1") + "." + ts2.Hours.ToString("D2") + ":" + ts2.Minutes.ToString("D2") + ":" + ts2.Seconds.ToString("D2");
                Send("AI1-1-02", DayToHours(ts2));
            }
            if (rollerTime.tds.TimeDataSwitch3)
            {
                rollerTime.td.TimeData3 = ts3.Days.ToString("D1") + "." + ts3.Hours.ToString("D2") + ":" + ts3.Minutes.ToString("D2") + ":" + ts3.Seconds.ToString("D2");
                Send("AI1-1-03", DayToHours(ts3));
            }
            if (rollerTime.tds.TimeDataSwitch4)
            {
                rollerTime.td.TimeData4 = ts4.Days.ToString("D1") + "." + ts4.Hours.ToString("D2") + ":" + ts4.Minutes.ToString("D2") + ":" + ts4.Seconds.ToString("D2");
                Send("AI1-1-04", DayToHours(ts4));
            }
            if (rollerTime.tds.TimeDataSwitch5)
            {
                rollerTime.td.TimeData5 = ts5.Days.ToString("D1") + "." + ts5.Hours.ToString("D2") + ":" + ts5.Minutes.ToString("D2") + ":" + ts5.Seconds.ToString("D2");
                Send("AI1-1-05", DayToHours(ts5));
            }
            if (rollerTime.tds.TimeDataSwitch6)
            {
                rollerTime.td.TimeData6 = ts6.Days.ToString("D1") + "." + ts6.Hours.ToString("D2") + ":" + ts6.Minutes.ToString("D2") + ":" + ts6.Seconds.ToString("D2");
                Send("AI1-1-06", DayToHours(ts6));
            }
            if (rollerTime.tds.TimeDataSwitch7)
            {
                rollerTime.td.TimeData7 = ts7.Days.ToString("D1") + "." + ts7.Hours.ToString("D2") + ":" + ts7.Minutes.ToString("D2") + ":" + ts7.Seconds.ToString("D2");
                Send("AI1-1-07", DayToHours(ts7));
            }
            if (rollerTime.tds.TimeDataSwitch8)
            {
                rollerTime.td.TimeData8 = ts8.Days.ToString("D1") + "." + ts8.Hours.ToString("D2") + ":" + ts8.Minutes.ToString("D2") + ":" + ts8.Seconds.ToString("D2");
                Send("AI1-1-08", DayToHours(ts8));
            }
            if (rollerTime.tds.TimeDataSwitch9)
            {
                rollerTime.td.TimeData9 = ts9.Days.ToString("D1") + "." + ts9.Hours.ToString("D2") + ":" + ts9.Minutes.ToString("D2") + ":" + ts9.Seconds.ToString("D2");
                Send("AI1-1-09", DayToHours(ts9));
            }
            if (rollerTime.tds.TimeDataSwitch10)
            {
                rollerTime.td.TimeData10 = ts10.Days.ToString("D1") + "." + ts10.Hours.ToString("D2") + ":" + ts10.Minutes.ToString("D2") + ":" + ts10.Seconds.ToString("D2");
                Send("AI1-1-10", DayToHours(ts10));
            }
            if (rollerTime.tds.TimeDataSwitch11)
            {
                rollerTime.td.TimeData11 = ts11.Days.ToString("D1") + "." + ts11.Hours.ToString("D2") + ":" + ts11.Minutes.ToString("D2") + ":" + ts11.Seconds.ToString("D2");
                Send("AI1-1-11", DayToHours(ts11));
            }
            if (rollerTime.tds.TimeDataSwitch12)
            {
                rollerTime.td.TimeData12 = ts12.Days.ToString("D1") + "." + ts12.Hours.ToString("D2") + ":" + ts12.Minutes.ToString("D2") + ":" + ts12.Seconds.ToString("D2");
                Send("AI1-1-12", DayToHours(ts12));
            }
            relativetask = new Task(() => RelativeMethod(), relativects.Token);
            relativetask.Start();
            relativetask.Wait();
        }
        private void RelativeMethod()
        {
                lock (TestSampleInfo.GetInstance().rollersampleinfos)
                {
                    if (TestSampleInfo.GetInstance().rollersampleinfos.Count() != 0)
                    {
                        foreach (var p in TestSampleInfo.GetInstance().rollersampleinfos)
                        {
                            if (p.TestType == "定时截尾试验" && rollerforcer.getJudgeSwitch(p.RollerBaseStation.ForcerCfg.ForcerName))
                            {
                                TimeSpan a = TimeSpan.FromHours(p.TestTime);
                                TimeSpan b = TimeSpan.Parse(rollerTime.getRollerTime(p.RollerBaseStation.TimerCfg.TimerName));
                                if (a <= b)
                                {
                                    samplerepo.setsampleState(p.RollerSampleInfoID, "待结束");
                                    rollerTime.CloseRollerTimeSwitch(p.RollerBaseStation.TimerCfg.TimerName);
                                    rollerforcer.CloseRollerForcerSwitch(p.RollerBaseStation.ForcerCfg.ForcerName);
                                    rollerforcer.setJudgeSwitch(p.RollerBaseStation.ForcerCfg.ForcerName, false);
                                }

                            }

                        }
                    }
                }
        }

        public bool Timer2State()
        {
            return controlTimer2.Enabled;
        }
        public void OpenTimer2()
        {
            rollerTime.td=rollerTime.ReadAllRollerTime();
            rollerTime.ReadAllRollerTimeSwitch();
            controlTimer2.Enabled = true;
            relativects = new CancellationTokenSource();
        }
        public void CloseTimer2()
        {
            controlTimer2.Enabled = false;
            relativects.Cancel();
        }

        public void Send(string station, string time)
        {
            var timeHub = GlobalHost.ConnectionManager.GetHubContext("timeHub");
            timeHub.Clients.All.addNewTimeToPage(station, time);
        }


    }
    public class RollerTimer
    {
        private RollerTimer(){
            }
        private static RollerTimer instance;
        private static readonly object locker = new object();
        private EFBaseRepository baserepo = new EFBaseRepository();
        public TimeData td = new TimeData();
        public TimeDataSwitch tds = new TimeDataSwitch();
        public static RollerTimer GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new RollerTimer();
                    }
                }
            }
            return instance;
        }


        public void OpenRollerTimeSwitch(string TimerName)
        {
            switch (TimerName)
            {
                case "Timer1": tds.TimeDataSwitch1 = true; baserepo.SaveTimeSwitch("Timer1", true);
                    break;
                case "Timer2":
                    tds.TimeDataSwitch2 = true; baserepo.SaveTimeSwitch("Timer2", true);
                    break;
                case "Timer3":
                    tds.TimeDataSwitch3 = true; baserepo.SaveTimeSwitch("Timer3", true);
                    break;
                case "Timer4":
                    tds.TimeDataSwitch4 = true; baserepo.SaveTimeSwitch("Timer4", true);
                    break;
                case "Timer5":
                    tds.TimeDataSwitch5 = true; baserepo.SaveTimeSwitch("Timer5", true);
                    break;
                case "Timer6":
                    tds.TimeDataSwitch6 = true; baserepo.SaveTimeSwitch("Timer6", true);
                    break;
                case "Timer7":
                    tds.TimeDataSwitch7 = true; baserepo.SaveTimeSwitch("Timer7", true);
                    break;
                case "Timer8":
                    tds.TimeDataSwitch8 = true; baserepo.SaveTimeSwitch("Timer8", true);
                    break;
                case "Timer9":
                    tds.TimeDataSwitch9 = true; baserepo.SaveTimeSwitch("Timer9", true);
                    break;
                case "Timer10":
                    tds.TimeDataSwitch10 = true; baserepo.SaveTimeSwitch("Timer10", true);
                    break;
                case "Timer11":
                    tds.TimeDataSwitch11 = true; baserepo.SaveTimeSwitch("Timer11", true);
                    break;
                case "Timer12":
                    tds.TimeDataSwitch12 = true; baserepo.SaveTimeSwitch("Timer12", true);
                    break;
                default: break;
            }
        }
        public void CloseRollerTimeSwitch(string TimerName)
        {
            switch (TimerName)
            {
                case "Timer1": tds.TimeDataSwitch1 = false; baserepo.SaveTimeSwitch("Timer1", false); break;
                case "Timer2": tds.TimeDataSwitch2 = false; baserepo.SaveTimeSwitch("Timer2", false); break;
                case "Timer3": tds.TimeDataSwitch3 = false; baserepo.SaveTimeSwitch("Timer3", false); break;
                case "Timer4": tds.TimeDataSwitch4 = false; baserepo.SaveTimeSwitch("Timer4", false); break;
                case "Timer5": tds.TimeDataSwitch5 = false; baserepo.SaveTimeSwitch("Timer5", false); break;
                case "Timer6": tds.TimeDataSwitch6 = false; baserepo.SaveTimeSwitch("Timer6", false); break;
                case "Timer7": tds.TimeDataSwitch7 = false; baserepo.SaveTimeSwitch("Timer7", false); break;
                case "Timer8": tds.TimeDataSwitch8 = false; baserepo.SaveTimeSwitch("Timer8", false); break;
                case "Timer9": tds.TimeDataSwitch9 = false; baserepo.SaveTimeSwitch("Timer9", false); break;
                case "Timer10": tds.TimeDataSwitch10 = false; baserepo.SaveTimeSwitch("Timer10", false); break;
                case "Timer11": tds.TimeDataSwitch11 = false; baserepo.SaveTimeSwitch("Timer11", false); break;
                case "Timer12": tds.TimeDataSwitch12 = false; baserepo.SaveTimeSwitch("Timer12", false); break;
                default: break;
            }
        }
        public void OpenAllTimeSwitch()
        {
            OpenRollerTimeSwitch("Timer1");
            OpenRollerTimeSwitch("Timer2");
            OpenRollerTimeSwitch("Timer3");
            OpenRollerTimeSwitch("Timer4");
            OpenRollerTimeSwitch("Timer5");
            OpenRollerTimeSwitch("Timer6");
            OpenRollerTimeSwitch("Timer7");
            OpenRollerTimeSwitch("Timer8");
            OpenRollerTimeSwitch("Timer9");
            OpenRollerTimeSwitch("Timer10");
            OpenRollerTimeSwitch("Timer11");
            OpenRollerTimeSwitch("Timer12");

        }
        public void CloseAllTimeSwitch()
        {
            CloseRollerTimeSwitch("Timer1");
            CloseRollerTimeSwitch("Timer2");
            CloseRollerTimeSwitch("Timer3");
            CloseRollerTimeSwitch("Timer4");
            CloseRollerTimeSwitch("Timer5");
            CloseRollerTimeSwitch("Timer6");
            CloseRollerTimeSwitch("Timer7");
            CloseRollerTimeSwitch("Timer8");
            CloseRollerTimeSwitch("Timer9");
            CloseRollerTimeSwitch("Timer10");
            CloseRollerTimeSwitch("Timer11");
            CloseRollerTimeSwitch("Timer12");

        }
        public void CleanRollerTime(string TimerName)
        {
            switch (TimerName)
            {
                case "Timer1": td.TimeData1 = "0.00:00:00"; WriteTimeData("Timer1"); break;
                case "Timer2": td.TimeData2 = "0.00:00:00"; WriteTimeData("Timer2"); break;
                case "Timer3": td.TimeData3 = "0.00:00:00"; WriteTimeData("Timer3"); break;
                case "Timer4": td.TimeData4 = "0.00:00:00"; WriteTimeData("Timer4"); break;
                case "Timer5": td.TimeData5 = "0.00:00:00"; WriteTimeData("Timer5"); break;
                case "Timer6": td.TimeData6 = "0.00:00:00"; WriteTimeData("Timer6"); break;
                case "Timer7": td.TimeData7 = "0.00:00:00"; WriteTimeData("Timer7"); break;
                case "Timer8": td.TimeData8 = "0.00:00:00"; WriteTimeData("Timer8"); break;
                case "Timer9": td.TimeData9 = "0.00:00:00"; WriteTimeData("Timer9"); break;
                case "Timer10": td.TimeData10 = "0.00:00:00"; WriteTimeData("Timer10"); break;
                case "Timer11": td.TimeData11 = "0.00:00:00"; WriteTimeData("Timer11"); break;
                case "Timer12": td.TimeData12 = "0.00:00:00"; WriteTimeData("Timer12"); break;
                default: break;
            }
          
        }
        public string getRollerTime(string TimerName)
        {
            string timedata;
            switch (TimerName)
            {
                case "Timer1": timedata = td.TimeData1; break;
                case "Timer2": timedata = td.TimeData2; break;
                case "Timer3": timedata = td.TimeData3; break;
                case "Timer4": timedata = td.TimeData4; break;
                case "Timer5": timedata = td.TimeData5; break;
                case "Timer6": timedata = td.TimeData6; break;
                case "Timer7": timedata = td.TimeData7; break;
                case "Timer8": timedata = td.TimeData8; break;
                case "Timer9": timedata = td.TimeData9; break;
                case "Timer10": timedata = td.TimeData10; break;
                case "Timer11": timedata = td.TimeData11; break;
                case "Timer12": timedata = td.TimeData12; break;
                default: timedata = ""; break;
            }
            return timedata;
        }
        public void WriteTimeData(string TimeName)
        {
            TimeData ntd = ReadAllRollerTime();
            switch (TimeName)
            {
                case "Timer1":ntd.TimeData1 = td.TimeData1;break;
                case "Timer2": ntd.TimeData2 = td.TimeData2; break;
                case "Timer3": ntd.TimeData3 = td.TimeData3; break;
                case "Timer4": ntd.TimeData4 = td.TimeData4; break;
                case "Timer5": ntd.TimeData5 = td.TimeData5; break;
                case "Timer6": ntd.TimeData6 = td.TimeData6; break;
                case "Timer7": ntd.TimeData7 = td.TimeData7; break;
                case "Timer8": ntd.TimeData8 = td.TimeData8; break;
                case "Timer9": ntd.TimeData9 = td.TimeData9; break;
                case "Timer10": ntd.TimeData10 = td.TimeData10; break;
                case "Timer11": ntd.TimeData11 = td.TimeData11; break;
                case "Timer12": ntd.TimeData12 = td.TimeData12; break;
            }
            string json = JsonConvert.SerializeObject(ntd);
            File.WriteAllText("C:\\TimeDataJson.txt", json);
        }

        public string ReadTimeData(string TimerName)
        {
            string json = File.ReadAllText("C:\\TimeDataJson.txt");
            TimeData ntd = JsonConvert.DeserializeObject<TimeData>(json);
            string str = "";
            switch (TimerName)
            {
                case "Timer1": td.TimeData1 = ntd.TimeData1;str = ntd.TimeData1; break;
                case "Timer2": td.TimeData2 = ntd.TimeData2; str = ntd.TimeData2; break;
                case "Timer3": td.TimeData3 = ntd.TimeData3; str = ntd.TimeData3; break;
                case "Timer4": td.TimeData4 = ntd.TimeData4; str = ntd.TimeData4; break;
                case "Timer5": td.TimeData5 = ntd.TimeData5; str = ntd.TimeData5; break;
                case "Timer6": td.TimeData6 = ntd.TimeData6; str = ntd.TimeData6; break;
                case "Timer7": td.TimeData7 = ntd.TimeData7; str = ntd.TimeData7; break;
                case "Timer8": td.TimeData8 = ntd.TimeData8; str = ntd.TimeData8; break;
                case "Timer9": td.TimeData9 = ntd.TimeData9; str = ntd.TimeData9; break;
                case "Timer10": td.TimeData10 = ntd.TimeData10; str = ntd.TimeData10; break;
                case "Timer11": td.TimeData11 = ntd.TimeData11; str = ntd.TimeData11; break;
                case "Timer12": td.TimeData12 = ntd.TimeData12; str = ntd.TimeData12; break;
            }
            return str;
        }


        public TimeData ReadAllRollerTime()
        {
            string json = File.ReadAllText("C:\\TimeDataJson.txt");
            TimeData ntd = JsonConvert.DeserializeObject<TimeData>(json);
            TimeData td = new TimeData() {
                TimeData1 = ntd.TimeData1,
                TimeData2 = ntd.TimeData2,
                TimeData3 = ntd.TimeData3,
                TimeData4 = ntd.TimeData4,
                TimeData5 = ntd.TimeData5,
                TimeData6 = ntd.TimeData6,
                TimeData7 = ntd.TimeData7,
                TimeData8 = ntd.TimeData8,
                TimeData9 = ntd.TimeData9,
                TimeData10 = ntd.TimeData10,
                TimeData11 = ntd.TimeData11,
                TimeData12 = ntd.TimeData12
            };
            return td;

        }
        public void ReadAllTimeBuffer()
        {
            TimeData tda = ReadAllRollerTime();
            td.TimeData1 = tda.TimeData1;
            td.TimeData2 = tda.TimeData2;
            td.TimeData3 = tda.TimeData3;
            td.TimeData4 = tda.TimeData4;
            td.TimeData5 = tda.TimeData5;
            td.TimeData6 = tda.TimeData6;
            td.TimeData7 = tda.TimeData7;
            td.TimeData8 = tda.TimeData8;
            td.TimeData9 = tda.TimeData9;
            td.TimeData10 = tda.TimeData10;
            td.TimeData11 = tda.TimeData11;
            td.TimeData12 = tda.TimeData12;
        }
        public void ReadAllRollerTimeSwitch()
        {
            tds.TimeDataSwitch1 = baserepo.TimerCfgs.FirstOrDefault(x => x.TimerName == "Timer1").TimerSwitch;
            tds.TimeDataSwitch2 = baserepo.TimerCfgs.FirstOrDefault(x => x.TimerName == "Timer2").TimerSwitch;
            tds.TimeDataSwitch3 = baserepo.TimerCfgs.FirstOrDefault(x => x.TimerName == "Timer3").TimerSwitch;
            tds.TimeDataSwitch4 = baserepo.TimerCfgs.FirstOrDefault(x => x.TimerName == "Timer4").TimerSwitch;
            tds.TimeDataSwitch5 = baserepo.TimerCfgs.FirstOrDefault(x => x.TimerName == "Timer5").TimerSwitch;
            tds.TimeDataSwitch6 = baserepo.TimerCfgs.FirstOrDefault(x => x.TimerName == "Timer6").TimerSwitch;
            tds.TimeDataSwitch7 = baserepo.TimerCfgs.FirstOrDefault(x => x.TimerName == "Timer7").TimerSwitch;
            tds.TimeDataSwitch8 = baserepo.TimerCfgs.FirstOrDefault(x => x.TimerName == "Timer8").TimerSwitch;
            tds.TimeDataSwitch9 = baserepo.TimerCfgs.FirstOrDefault(x => x.TimerName == "Timer9").TimerSwitch;
            tds.TimeDataSwitch10 = baserepo.TimerCfgs.FirstOrDefault(x => x.TimerName == "Timer10").TimerSwitch;
            tds.TimeDataSwitch11 = baserepo.TimerCfgs.FirstOrDefault(x => x.TimerName == "Timer11").TimerSwitch;
            tds.TimeDataSwitch12 = baserepo.TimerCfgs.FirstOrDefault(x => x.TimerName == "Timer12").TimerSwitch;


        }


    }
    public class TimeData
    {
        public string TimeData1 { get; set; }
        public string TimeData2 { get; set; }
        public string TimeData3 { get; set; }
        public string TimeData4 { get; set; }
        public string TimeData5 { get; set; }
        public string TimeData6 { get; set; }
        public string TimeData7 { get; set; }
        public string TimeData8 { get; set; }
        public string TimeData9 { get; set; }
        public string TimeData10 { get; set; }
        public string TimeData11 { get; set; }
        public string TimeData12 { get; set; }
    }
    public class TimeDataSwitch
    {
        public bool TimeDataSwitch1 { get; set; }
        public bool TimeDataSwitch2 { get; set; }
        public bool TimeDataSwitch3 { get; set; }
        public bool TimeDataSwitch4 { get; set; }
        public bool TimeDataSwitch5 { get; set; }
        public bool TimeDataSwitch6 { get; set; }
        public bool TimeDataSwitch7 { get; set; }
        public bool TimeDataSwitch8 { get; set; }
        public bool TimeDataSwitch9 { get; set; }
        public bool TimeDataSwitch10 { get; set; }
        public bool TimeDataSwitch11 { get; set; }
        public bool TimeDataSwitch12 { get; set; }
    }
}
