using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtusBI;
using VirtusMobileAPI.Models;

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

        /// <summary>
        /// Returns the specified record performance i.e services 
        /// </summary>
        /// <param name="objectType"> specify object type like Project = 1,Task = 2 </param>
        /// <param name="recordId"> specify record id of particular object type </param>
        /// <param name="IsShowSubObjects"> specify Is also have to show sub objects </param>
        /// <param name="IsShowBilled"> specify is need to show billed records also </param>
        /// <param name="IsCanView"> specify is have view right for particular object Type.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPerformanceOnServices/{objectType}/{recordId}/{IsShowSubObjects}/{IsShowBilled}/{IsCanView}")]
        public HttpResponseMessage GetPerformanceOnServices(int objectType, int recordId, bool IsShowSubObjects, bool IsShowBilled = true, bool IsCanView = true)
        {
            var result = repository.fnGetPreformanceServiceDataSet(objectType, recordId, IsShowSubObjects, IsShowBilled, IsCanView);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        ///  Returns the specified record performance i.e Expances 
        /// </summary>
        /// <param name="objectType">specify object type like Project = 1,Task = 2</param>
        /// <param name="recordId"> specify record id of particular object type </param>
        /// <param name="IsShowSubObjects"> specify Is also have to show sub objects </param>
        /// <param name="IsShowBilled"> specify is need to show billed records also </param>
        /// <param name="IsCanView"> specify is have view right for particular object Type.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPerformanceOnExpances/{objectType}/{recordId}/{IsShowSubObjects}/{IsShowBilled}/{IsCanView}")]
        public HttpResponseMessage GetPerformanceOnExpances(int objectType, int recordId, bool IsShowSubObjects, bool IsShowBilled = true, bool IsCanView = true)
        {
            var result = repository.fnGetPreformanceExpensesDataSet(objectType, recordId, IsShowSubObjects, IsShowBilled, IsCanView);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Returns the Users with details involoved in Performances (Specific to Object)
        /// </summary>
        /// <param name="IsExpenses">specify is it expenses or services related</param>
        /// <param name="objectType">specify object Type i.e value of  project or task</param>
        /// <param name="recordId">specify recordId  </param>
        /// <param name="processedByUsers">specify the addresseIds of processed by users if any which are relates to particular project/task separted with coma(',') </param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetObjectPerformedByUsers/{IsExpenses}/{objectType}/{recordId}/{processedByUsers}")]
        public HttpResponseMessage GetObjectPerformedByUsers(bool IsExpenses, int objectType, int recordId, string processedByUsers)
        {
            var result = repository.fnGetObjectPerformedByDropdown(IsExpenses, objectType, recordId, ConverterHelper.CheckSingleQuote(processedByUsers));
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Returns the users available and performed between the timelines, in any services/expences(this is specific to loginUser not any object)
        /// </summary>
        /// <param name="IsExpenses">specify is service or expense</param>
        /// <param name="fromDate">specify the start time , from the time users available</param>
        /// <param name="toDate">specify the end time , up to the time users available</param>
        /// <param name="loginUserId">specify the loginUserId details</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPerformedByUsers/{IsExpenses}/{fromDate}/{toDate}/{loginUserId}")]
        public HttpResponseMessage GetPerformedByUsers(bool IsExpenses, DateTime fromDate, DateTime toDate, int loginUserId)
        {
            if (fromDate < new DateTime(1900, 1, 1) || toDate < new DateTime(1900, 1, 1) || fromDate > toDate)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please provide the valid data or date range.");

            var result = repository.fnGetPerformedByDropdown(IsExpenses, fromDate, toDate, loginUserId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Returns all the projects avalilable to perform actions i.e to ledger services and expenses
        /// </summary>
        /// <param name="IsExpenses">specify is service or expense</param>
        /// <param name="fromDate">specify the start time , from the time users available</param>
        /// <param name="toDate">specify the end time , up to the time users available</param>
        /// <param name="loginUserId">specify the loginUserId details</param>
        /// <param name="existingUserIds">specify if if any existing users(i.e alredy selected users if any, else pass 0 or empty)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetProjectsToPerform/{IsExpenses}/{fromDate}/{toDate}/{loginUserId}/{existingUserIds}")]
        public HttpResponseMessage GetProjectsForPerformance(bool IsExpenses, DateTime fromDate, DateTime toDate, int loginUserId, string existingUserIds)
        {
            var result = repository.fnGetProjectDropdowns(loginUserId, fromDate, toDate, IsExpenses, ConverterHelper.CheckSingleQuote(existingUserIds));
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Returns all the tasks avalilable in selected parent project.
        /// </summary>
        /// <param name="IsExpenses">specify is service or expense</param>
        /// <param name="fromDate">specify the start time , from the time users available</param>
        /// <param name="toDate">specify the end time , up to the time users available</param>
        /// <param name="loginUserId">specify the loginUserId details</param>
        /// <param name="existingUserIds">specify if if any existing users</param>
        /// <param name="parentProjectId">specify selected parent project id</param>
        /// <param name="IsCanViewProject">specify can the login user have project view right</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTasksToPerform/{IsExpenses}/{fromDate}/{toDate}/{loginUserId}/{existingUserIds}/{parentProjectId}/{IsCanViewProject}")]
        public HttpResponseMessage GetTasksForPerformance(bool IsExpenses, DateTime fromDate, DateTime toDate, int loginUserId, string existingUserIds, int parentProjectId, bool IsCanViewProject = true)
        {
            var result = repository.fnGetTasksDropdowns(loginUserId, fromDate, toDate, IsExpenses, parentProjectId, ConverterHelper.CheckSingleQuote(existingUserIds), IsCanViewProject);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        /// <summary>
        /// Returns all the Performanc services between the timeintervals (specific to user not object)
        /// </summary>
        /// <param name="fromDate">specify fromDate</param>
        /// <param name="toDate">specify toDate</param>
        /// <param name="loginUserId">specify loginUserId</param>
        /// <param name="IsCanViewProject">specify is can view project or not</param>
        /// <param name="IsCanViewTask">specify is can view task or not</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPerformanceServices/{fromDate}/{toDate}/{loginUserId}/{IsCanViewProject}/{IsCanViewTask}")]
        public HttpResponseMessage GetPerformanceServices(DateTime fromDate, DateTime toDate, int loginUserId, bool IsCanViewProject = true, bool IsCanViewTask = true)
        {
            var result = repository.fnGetPreformanceServicesDataSet(fromDate, toDate, loginUserId, IsCanViewProject, IsCanViewTask);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Returns all the Performanc Expances between the timeintervals (specific to user not object)
        /// </summary>
        /// <param name="fromDate">specify fromDate</param>
        /// <param name="toDate">specify toDate</param>
        /// <param name="loginUserId">specify loginUserId</param>
        /// <param name="IsCanViewProject">specify is can view project or not</param>
        /// <param name="IsCanViewTask">specify is can view task or not</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPerformanceExpances/{fromDate}/{toDate}/{loginUserId}/{IsCanViewProject}/{IsCanViewTask}")]
        public HttpResponseMessage GetPerformanceExpances(DateTime fromDate, DateTime toDate, int loginUserId, bool IsCanViewProject = true, bool IsCanViewTask = true)
        {
            var result = repository.fnGetPreformanceExpensesDataSet(fromDate, toDate, loginUserId, IsCanViewProject, IsCanViewTask);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }
          
        /// <summary>
        /// Saves the TimeSheet/Performances of the Persons corresponding to project and task.
        /// </summary>
        /// <param name="loginUserId">Specify the loginUserId details</param>
        /// <param name="IsServiceModified">If services have to save in datasource then specify 'True' else 'False'</param>
        /// <param name="IsExpancesModified">If expances have to save in datasource then specify 'True' else 'False'</param>
        /// <param name="performaanceDetails">Specify the PerformanceActionData which is comination of collection of Expances and collection of Services.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SavePerformance/{loginUserId}/{IsServiceModified}/{IsExpancesModified}")]
        public HttpResponseMessage SavePerformance(int loginUserId, bool IsServiceModified, bool IsExpancesModified, [FromBody]PerformanceActionData performaanceDetails)
        {

            if (performaanceDetails == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Provide valid performance details,those should not be null.");

            DataTable dtServices = ConverterHelper.ConvertToDataTable(performaanceDetails.ServicesCollection);
            DataTable dtExpances = ConverterHelper.ConvertToDataTable(performaanceDetails.ExpancesCollection);


            if (dtServices != null && dtExpances != null && dtServices.Rows.Count > 0 && dtExpances.Rows.Count > 0)
            {
                string savedFileId = string.Empty;
                try
                {
                    DataSet dsServices = new DataSet();
                    dsServices.Tables.Add(dtServices);

                    DataSet dsExpances = new DataSet();
                    dsExpances.Tables.Add(dtExpances);

                    repository.BeginTrans();
                    var result = repository.fnSavePerformances(dsServices, dsExpances, IsServiceModified, IsExpancesModified, loginUserId);
                    repository.CommitTrans();
                    return Request.CreateResponse(HttpStatusCode.OK, new { SavedSuccessfully = result, SavedFileId = savedFileId }, Configuration.Formatters.JsonFormatter);

                }
                catch (Exception ex)
                {
                    repository.RollbackTrans();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Some error while saving data. " + ex.Message);
                }
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Provide Valid Performance Details");
        }


    }
}
