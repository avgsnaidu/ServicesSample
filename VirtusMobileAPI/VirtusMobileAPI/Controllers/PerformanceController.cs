using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtusBI;

namespace VirtusMobileAPI.Controllers
{
    /// <summary>
    /// Timesheet Services to do all actions related to persons services/expances of particular project or Task.
    /// </summary>
    [RoutePrefix("VirtusApi/Performances")]
    public class PerformanceController : ApiController
    {
        clsPerformance repository = new clsPerformance();

        /// <summary>
        /// Returns the list of Services would perform for the Project or Task
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetServicesTypeList")]
        public HttpResponseMessage GetTaskServiceTypeList()
        {
            int defualtLanguage = 3;
            var result = repository.fnGetTaskServiceTypesList(defualtLanguage);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Retruns the list of products in which expenses would be done
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProductsTypeList")]
        public HttpResponseMessage GetProductsTypeList()
        {
            int defualtLanguage = 3;
            var result = repository.fnGetProductsList(defualtLanguage);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }
        

        

    }
}
