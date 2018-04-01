using System.Web.Mvc;
using System.Web.Routing;

namespace ClientPagerProto
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "PagedListData",
                url: "{controller}/{action}/{variable}/{pageNumber}/{*pathInfo}",
                defaults: new
                {
                    controller = "DemoList",
                    action = "PageData",
                    pageNumber = "1"                    
                },

                constraints: new { pageNumber = @"(\d+)|()" }
            );

            /* routes.Add(new Route
            (
                "Report/{reportId}/Filters/{componentId}", defaults, JoinConstraints(constraints, new[] { "GET", "PUT", "POST" }),
                new VikingRouteHandler<ReportalFilterDesignerService>()
            )); */
        }
    }
}
