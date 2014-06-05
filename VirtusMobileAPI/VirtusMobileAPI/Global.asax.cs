using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using VirtusBI;

namespace VirtusMobileAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            Common.SetConnectionString(ConfigurationManager.AppSettings["DBConnectionString"].ToString());

            HttpConfiguration config = GlobalConfiguration.Configuration;
            config.Formatters.JsonFormatter.SerializerSettings.Formatting =
                Newtonsoft.Json.Formatting.Indented;
        


            ////json.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            ////json.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            ////json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            ////json.Formatting = Newtonsoft.Json.Formatting.Indented;
            ////json.ContractResolver = new CamelCasePropertyNamesContractResolver();
            ////json.Culture = new CultureInfo("it-IT");
        }
    }
}