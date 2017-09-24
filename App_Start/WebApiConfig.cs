using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace web_api_icc_valsys_no_mvc
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            
            
            ////to JSON default
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
            ////to JSON default


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional}
            );

            //HUDA's routes
            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {
                    id = RouteParameter.Optional,
                    action = RouteParameter.Optional
                }
            );

            config.Routes.MapHttpRoute(
                name: "ActionApi_8params",
                routeTemplate: "api/{controller}/{action}/11params/{id1}/{id2}/{id3}/{id4}/{id5}/{id6}/{id7}/{id8}",
                defaults: new
                {
                    id1 = RouteParameter.Optional,
                    id2 = RouteParameter.Optional,
                    id3 = RouteParameter.Optional,
                    id4 = RouteParameter.Optional,
                    id5 = RouteParameter.Optional,
                    id6 = RouteParameter.Optional,
                    id7 = RouteParameter.Optional,
                    id8 = RouteParameter.Optional,
                }
            );

            config.Routes.MapHttpRoute(
                name: "ActionApi_11params",
                routeTemplate: "api/{controller}/{action}/11params/{id1}/{id2}/{id3}/{id4}/{id5}/{id6}/{id7}/{id8}/{id9}/{id10}/{id11}",
                defaults: new {
                    id1 = RouteParameter.Optional,
                    id2 = RouteParameter.Optional,
                    id3 = RouteParameter.Optional,
                    id4 = RouteParameter.Optional,
                    id5 = RouteParameter.Optional,
                    id6 = RouteParameter.Optional,
                    id7 = RouteParameter.Optional,
                    id8 = RouteParameter.Optional,
                    id9 = RouteParameter.Optional,
                    id10 = RouteParameter.Optional,
                    id11 = RouteParameter.Optional,
                }
            );


        }
    }
}
