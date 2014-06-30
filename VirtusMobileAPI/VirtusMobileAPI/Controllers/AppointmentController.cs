using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtusBI;
using VirtusMobileAPI.Models;

namespace VirtusMobileAPI.Controllers
{
    /// <summary>
    /// Services for adding or viewing an appointment.
    /// </summary>
    [RoutePrefix("VirtusApi/Appointment")]
    public class AppointmentController : ApiController
    {
        clsAppointments repository = new clsAppointments();

        /// <summary>
        /// Returns all the appointments list.
        /// </summary>
        /// <param name="loginUserName">specify login username</param>
        /// <param name="IsUndone">specify is done or undone records to fetch</param>
        /// <param name="IsAllAppointments">specify is need to display all apointments or not</param>
        /// <param name="period">specify the period of time, i.e like for   All = 0,Today = 1,ThisWeek = 2,ThisMonth = 3,ThisYear = 4 </param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAppointmentsList/{loginUserName}/{IsUndone}/{IsAllAppointments}/{period}")]
        public HttpResponseMessage GetAppointmentsList(string loginUserName, bool IsUndone, bool IsAllAppointments, int period)
        {
            var result = repository.fnGetAppointments(loginUserName, IsUndone, IsAllAppointments, period);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }


        /// <summary>
        /// Returns appointment details 
        /// </summary>
        /// <param name="loginUserName">specify login username</param>
        /// <param name="appointmentId">specify appointment id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAppointmentDetails/{loginUserName}/{appointmentId}")]
        public HttpResponseMessage GetAppointmentDetails(string loginUserName, int appointmentId)
        {
            var result = repository.fnGetAppointmentDetails(appointmentId, loginUserName);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }


        /// <summary>
        /// Returns the all the appointment occurrence details.
        /// </summary>
        /// <param name="occurrSeriesId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAppointmentOccurrenceDetails/{occurrSeriesId}")]
        public HttpResponseMessage GetAppointmentOccurrenceDetails(int occurrSeriesId)
        {
            var result = repository.fnGetAppOccurrenceDetails(occurrSeriesId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        /// <summary>
        /// Returns the available persons to fill in appointment persons list.
        /// </summary>
        /// <param name="noOfRecords">specify how many records to fetch</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUsers/{noOfRecords}")]
        public HttpResponseMessage GetUsers(int noOfRecords)
        {
            string searchCondition = string.Empty;
            string OrderBy = string.Empty;
            string SortOrder = string.Empty;
            int totalCount = default(int);
            searchCondition = "A.Isactive=1 ";

            var result = repository.fnGetUsers(searchCondition, noOfRecords, OrderBy, SortOrder, totalCount);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        /// <summary>
        /// Saves the appointment
        /// </summary>
        /// <param name="appointmentId">specify 0 for new appointment</param>
        /// <param name="loginUserName"> login username </param>
        /// <param name="appointmentDetails">specify the AppointmentActionData type data.</param>
        /// <returns></returns>

        [HttpPost]
        [Route("SaveAppointment/{appointmentId}/{loginUserName}")]
        public HttpResponseMessage SaveAppointment(int appointmentId, string loginUserName, [FromBody]AppointmentActionData appointmentDetails)
        {
            bool IsRefresh = default(bool);
            if (appointmentDetails == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Provide valid appoint details,those should not be null.");
            else if (String.IsNullOrEmpty(ConverterHelper.CheckSingleQuote(appointmentDetails.UserName)))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Provide valid username,those should not be null or empty.");
            else if (String.IsNullOrEmpty(ConverterHelper.CheckSingleQuote(appointmentDetails.ToPersons)))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Provide valid ToPersons,those should not be null or empty.");

            try
            {
                repository.BeginTrans();
                var result = repository.fnSaveAppointment(appointmentDetails.PriorityId,
                    loginUserName, appointmentDetails.ToPersons, appointmentDetails.ToOrganizations,
                    appointmentDetails.Subject, appointmentDetails.AppointmentDate,
                    appointmentDetails.AppointmentFrom, appointmentDetails.AppointmentUntil,
                    appointmentDetails.Location, appointmentDetails.Message,
                    Convert.ToInt16(appointmentDetails.CannotModify), Convert.ToInt16(appointmentDetails.PrivateAppointment),
                    appointmentId, Convert.ToInt16(appointmentDetails.IsNeedReminder), appointmentDetails.ReminderDuration, Convert.ToInt16(appointmentDetails.IsDone),
                    appointmentDetails.IsReOccur, appointmentDetails.OccurrenceSeriesId,
                    appointmentDetails.IsOpenSeries, appointmentDetails.CreatedBy, appointmentDetails.CreationDate,
                    appointmentDetails.IsAllDay, appointmentDetails.EntryId,
                    (DateTime)appointmentDetails.LastModificationTime, ref IsRefresh);
                repository.CommitTrans();
                return Request.CreateResponse(HttpStatusCode.OK, new { SentSuccessfully = result }, Configuration.Formatters.JsonFormatter);

            }
            catch (Exception ex)
            {
                repository.RollbackTrans();
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Some error while saving data. " + ex.Message);
            }

        }






    }
}
