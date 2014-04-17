using ImageOrganizer.Web.CustomLogic.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ImageOrganizer.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "LocalImage",
                url: PathDefinition.LOCAL_PATH_PREFIX_IMAGE + "/{*pathInfo}",
                defaults: new { controller = "Image", action = "Get", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "LocalPath",
                url: PathDefinition.LOCAL_PATH_PREFIX + "/{*pathInfo}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
