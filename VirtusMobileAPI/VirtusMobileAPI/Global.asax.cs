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
        }
    }
}