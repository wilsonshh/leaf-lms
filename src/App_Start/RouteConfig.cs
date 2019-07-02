using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "DashboardV1", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default-404",
                url: "{*url}",
                defaults: new { lang = "en", controller = "Error", action = "NotFound" }
            );
            ;
        }
    }
}