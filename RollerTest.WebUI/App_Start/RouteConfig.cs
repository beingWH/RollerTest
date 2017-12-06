using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RollerTest.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Login",
                url: "Login/{name}",
                defaults: new { Controller = "Account", action = "Login" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
           // routes.MapRoute(
           //     name: "Default1",
           //     url: "{controller}/{action}/{id}/{Nid}",
           //     defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional,Nid= UrlParameter.Optional }
           //);
        }
    }
}
