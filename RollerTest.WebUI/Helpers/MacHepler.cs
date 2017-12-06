using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RollerTest.WebUI.Helpers
{
    public class MacHepler
    {
        public static List<string> GetMacByIPConfig()
        {
            List<string> macs = new List<string>();

            ProcessStartInfo startInfo = new ProcessStartInfo("ipconfig", "/all");
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;
            Process p = Process.Start(startInfo);
            StreamReader reader = p.StandardOutput;
            string line = reader.ReadLine();

            while (!reader.EndOfStream)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    line = line.Trim();

                    if (line.StartsWith("物理地址") || line.StartsWith("Physical Address"))
                    {
                        string[] sp = line.Split(':');
                        for (int i = 0; i < sp.Count(); i++)
                        {
                            if (Regex.IsMatch(sp[i].Trim(), @"^([A-Fa-f0-9]{2}-){5}[A-Fa-f0-9]{2}$"))
                            {
                                macs.Add(sp[i].Trim());
                            }
                        }

                    }
                }
                line = reader.ReadLine();
            }
            //等待程序执行完退出进程
            p.WaitForExit();
            p.Close();
            reader.Close();
            return macs;
        }
    }
}