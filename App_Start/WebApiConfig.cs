using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace OSUDental
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "Commands",
                routeTemplate: "api/{controller}/count",
                defaults: new { action = "GET", command="count" }
            );
            config.Routes.MapHttpRoute(
                name: "AllItems",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "ClientItems",
                routeTemplate: "api/client/{clientid}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional, command="client" }
            );
            config.Filters.Add(new AuthorizeAttribute());
        }
    }
}
