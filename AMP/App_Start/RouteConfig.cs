using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AMP
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            
            
           routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
           routes.MapMvcAttributeRoutes();

           routes.MapRoute(
            name: "Default",
            url: "{controller}/{action}/{id}",
            defaults: new { controller = "Project", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
           "TwoParamRoute", // Route name
           "{controller}/{action}/{id1}/{id2}",                           // URL with parameters
           new { controller = "Admin", action = "EditReviewMaster", id1 = "", id2= "" }  // Parameter defaults
       );

           routes.MapRoute(
           "AdminReviewExemption", // Route name
           "{controller}/{action}/{id}/{reviewType}",                           // URL with parameters
           new { controller = "Admin", action = "EditReviewExemption", id = "", reviewType = "" }  // Parameter defaults
       );

           routes.MapRoute(
          name: "WorkflowAction",
          url: "{controller}/{action}/{id}/{taskId}",
          defaults: new { controller = "Workflow", action = "Edit", id = "", taskId = "" }
        );

          routes.MapRoute(
        "fiveParamRoute", // Route name
        "{controller}/{action}/{id1}/{id2}/{id3}/{id4}",                           // URL with parameters
        new { controller = "Component", action = "SearchPartner", id1 = "", id2 = "", id3 = "" , id4= "", id5 =""}  // Parameter defaults
        );

            routes.MapRoute(
       "AddfirstTierSearch", // Route name
       "{controller}/{action}/{id1}",                           // URL with parameters
       new { controller = "Component", action = "SearchForAFirstTierPartner", id1 = "" }  // Parameter defaults
       );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{sortOrder}/{page}/{currentFilter}",
            //    defaults: new
            //    {
            //        controller = "Project",
            //        action = "Index",
            //        sortOrder = UrlParameter.Optional,
            //        page = UrlParameter.Optional,
            //        currentFilter = UrlParameter.Optional
            //    }
            //);
        }
    }
}
