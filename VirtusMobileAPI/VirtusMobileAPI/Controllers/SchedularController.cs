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
    /// Services for Schedular objects to display
    /// </summary>
    [RoutePrefix("VirtusApi/Schedular")]
    public class SchedularController : ApiController
    {
        clsSchedular repository = new clsSchedular();

        /// <summary>
        /// Returns all the appointments related to object and user specific.
        /// </summary>
        /// <param name="loginUserName"></param>
        /// <param name="personsAddresseIds"></param>
        /// <param name="OrgnizationIds"></param>
        /// <param name="ObjectTypeIds"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <param name="IsCanViewProject"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSchedularAppointments/{loginUserName}/{personsAddresseIds}/{OrgnizationIds}/{ObjectTypeIds}/{FromDate}/{ToDate}/{IsCanViewProject}")]
        public HttpResponseMessage GetSchedularAppointments(string loginUserName, string personsAddresseIds, string OrgnizationIds, string ObjectTypeIds, DateTime FromDate, DateTime ToDate, bool IsCanViewProject)
        {
            bool IsCanviewTask = !default(bool);
            bool IsShowOpenObjects = !default(bool);
            bool IsCanViewMeeting = !default(bool);
            bool IsCanViewDecision = !default(bool);
            bool IsCanViewMyAppointment = !default(bool);

            int languageId = 3;

            var result = repository.fnGetApplointments(personsAddresseIds, OrgnizationIds, ObjectTypeIds, FromDate, ToDate, languageId, false, IsCanViewProject, IsCanviewTask, IsCanViewDecision, IsCanViewMeeting, IsCanViewMyAppointment, loginUserName, IsShowOpenObjects);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }


        /// <summary>
        /// Returns persons names by specifing the id's
        /// </summary>
        /// <param name="personsAddresseIds"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetPersonNames/{personsAddresseIds}")]
        public HttpResponseMessage GetPersonNames(string personsAddresseIds)
        {
            var result = repository.fnGetPersonNames(personsAddresseIds);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }

        


    }
}
