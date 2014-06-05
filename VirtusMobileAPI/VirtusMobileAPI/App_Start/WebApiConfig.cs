using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ValueProviders;
using System.Web.Http.ValueProviders.Providers;
using VirtusMobileAPI.Models;

namespace VirtusMobileAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.Formatters.Insert(0, new MultiFormDataMediaTypeFormatter());

            //json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            config.Services.Replace(typeof(ValueProviderFactory), new RouteDataValueProviderFactory());


            config.Filters.Add(new UnhandledExceptionFilter());
        }
    }
}
