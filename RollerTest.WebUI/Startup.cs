using Microsoft.Owin;
using Owin;
using Newtonsoft.Json;
using System.IO;
using RollerTest.WebUI.IniFiles;
using System.Data.Entity;
using RollerTest.WebUI.Models;

[assembly: OwinStartupAttribute(typeof(RollerTest.WebUI.Startup))]
namespace RollerTest.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
            if (!File.Exists("C:\\TimeDataJson.txt"))
            {
                WriteTimeDataIni();
            }
        }
        public void WriteTimeDataIni()
        {
            TimeData td = new TimeData()
            {
                TimeData1 = "0.00:00:00",
                TimeData2 = "0.00:00:00",
                TimeData3 = "0.00:00:00",
                TimeData4 = "0.00:00:00",
                TimeData5 = "0.00:00:00",
                TimeData6 = "0.00:00:00",
                TimeData7 = "0.00:00:00",
                TimeData8 = "0.00:00:00",
                TimeData9 = "0.00:00:00",
                TimeData10 = "0.00:00:00",
                TimeData11 = "0.00:00:00",
                TimeData12 = "0.00:00:00"

            };
            string json = JsonConvert.SerializeObject(td);
            File.WriteAllText("C:\\TimeDataJson.txt", json);
        }
    }
}
