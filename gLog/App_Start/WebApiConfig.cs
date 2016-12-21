using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace gLog
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // New code
            config.EnableCors();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Location",
                routeTemplate: "api/Log/{location}",
                defaults: new { location = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute(
            //    name: "Location",
            //    routeTemplate: "api/Log/{macaddress}/{latitude}/{longtitude}/{battery}",
            //    defaults: new { location = RouteParameter.Optional }
            //);
        }
    }
}
