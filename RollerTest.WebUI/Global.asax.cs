using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using RollerTest.WebUI.Infrastructure;
using System;
using log4net.Config;
using RollerTest.WebUI.Helpers;
using RollerTest.Domain.Concrete;
using RollerTest.Domain.Entities;
using RollerTest.WebUI.Models.PROCEDURE;

namespace RollerTest.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
            XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("log4net.xml")));
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //Application_End其他代码 
            try
            {
                System.Threading.Thread.Sleep(5000);
                string strUrl = "http://192.9.190.197:8080/";
                System.Net.HttpWebRequest _HttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(strUrl);
                System.Net.HttpWebResponse _HttpWebResponse = (System.Net.HttpWebResponse)_HttpWebRequest.GetResponse();
                System.IO.Stream _Stream = _HttpWebResponse.GetResponseStream();//得到回写的字节流

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(this.GetType(), ex);
                
            }
        }
        protected void Application_Error(object sender,EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            LogHelper.WriteLog(this.GetType(), ex);
            Server.ClearError();
        }



    }

}
