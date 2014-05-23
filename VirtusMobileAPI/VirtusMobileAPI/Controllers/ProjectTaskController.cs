using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtusBI;
using VirtusDataModel;
using VirtusMobileService.Models;

namespace VirtusMobileAPI.Controllers
{
    [RoutePrefix("VirtusApi/ProjectTask")]
    public class ProjectTaskController : ApiController
    {
        clsProjectTask repository = new clsProjectTask();
        clsTasksProcessedBy processedRepository = new clsTasksProcessedBy();
        clsTaskMilestones mileStoneRepository = new clsTaskMilestones();
        clsDeadlineExtension deadlineRepository = new clsDeadlineExtension();


        [ActionName("GetParentTaskId")]
        [Route("GetParentTaskId/{TaskId}")]
        public HttpResponseMessage GetParentTaskId(string TaskId)
        {
            var result = repository.GetParentTaskIds(TaskId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetProjectTreeForContracts/{StartSerialNumber}/{ParentNodeId}/{LoginUserName}")]
        public HttpResponseMessage GetProjectTreeForContracts(int StartSerialNumber, int ParentNodeId, string LoginUserName)
        {
            var result = repository.fnGetProjectTreeForContracts(StartSerialNumber, ParentNodeId, LoginUserName);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }




        [Route("GetMyWorkPlaceWhereCondition/{IsProjectList}/{loginUserId}")]
        public HttpResponseMessage GetMyWorkPlaceWhereCondition(bool IsProjectList, string loginUserId)
        {
            var result = repository.GetWorkPlaceWhereCondition(IsProjectList, loginUserId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        [Route("GetProjectTasksList/{whereCondition}/{IsProject}/{NoOfRecords}/{IsHasViewRight}/{loginUserName}")]
        public HttpResponseMessage GetListViewDataSet(string whereCondition, bool IsProject, int NoOfRecords, bool IsHasViewRight, string loginUserName)
        {
            int TotalCount = default(int);
            var ProjectsOrTasksList = repository.GetListViewDataSet(ConverterHelper.CheckSingleQuote(whereCondition), IsProject, "", NoOfRecords, "", "", ConverterHelper.CheckSingleQuote(loginUserName), ref TotalCount, IsHasViewRight);
            return Request.CreateResponse(HttpStatusCode.OK, new { ProjectsOrTasksList, TotalCount }, Configuration.Formatters.JsonFormatter);
        }


        [Route("GetProjectTask/{taskId}")]
        public HttpResponseMessage GetProjectTaskDetails(string taskId)
        {
            var result = repository.fnGetData(taskId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetUserRequestId/{taskId}")]
        public HttpResponseMessage GetProjectUserRequestId(int taskId)
        {
            var result = repository.fnGetUserRequestId(taskId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetProjectManagerOrMileStones/{isProject}/{projectId}/{isDone}")]
        public HttpResponseMessage GetMilestonesOrProjectManager(bool isProject, string projectId, bool isDone)
        {
            var result = repository.fnGetMilestonesOrProjectManagers(isProject, Convert.ToInt32(projectId), isDone);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }
        [HttpGet]
        [Route("GetProjectKinds/{departmentId}/{isProject}")]
        public HttpResponseMessage GetProjectKindList(int departmentId, bool isProject)
        {
            var result = repository.GetKindBindingDS(isProject, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }



        [Route("GetProjectStatusList")]
        public HttpResponseMessage GetProjectStatusBindingList()
        {
            var result = repository.GetProjectStatusBindingDS();
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetProjectTaskCount/{ObjectType}/{RecordId}/{LoginName}/{IsArchived}")]
        public HttpResponseMessage GetProjectTaskCount(int ObjectType, int RecordId, string LoginName, bool IsArchived)
        {
            var result = repository.GetListViewDataSetForCount(ObjectType, RecordId, LoginName, IsArchived);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetParentObjectCustomerAndKind/{taskId}")]
        public HttpResponseMessage GetParentObjectCustomerAndKind(int taskId)
        {
            int customerId = default(int);
            string customerName = default(string);
            int kindId = default(int);
            int milestoneId = default(int);
            int userRequestId = default(int);

            repository.fnGetParentObjectCustomerAndKind(taskId, ref customerId, ref customerName, ref kindId, ref milestoneId, ref userRequestId);
            ParentObjectCustomerAndKind result = new ParentObjectCustomerAndKind();
            result.CustomerId = customerId;
            result.CustomerName = customerName;
            result.KindId = kindId;
            result.MileStoneId = milestoneId;
            result.UserRequestId = userRequestId;
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }





        [Route("GetTaskFunctions/{recordId}/{objectType}")]
        public HttpResponseMessage GetProjectTaskFunctions(string recordId, int objectType)
        {
            var result = repository.GetTaskFunctions();
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetProcessedByContact/{AddresseId}")]
        public HttpResponseMessage GetProcessedByContact(string AddresseId)
        {
            var result = repository.fnGetProcessedByContact(AddresseId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        [Route("GetIsContributionExceeds/{contribution}/{mileStoneId}/{recordId}")]
        public HttpResponseMessage GetIsContributionExceeds(string contribution, int mileStoneId, string recordId)
        {
            var result = repository.fnIsContributionExceeds(ConverterHelper.CheckSingleQuote(contribution), mileStoneId, recordId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }


        [Route("GetIsInValidBudget/{currentBudget}/{PreviousBudget}/{recordId}/{ParentId}")]
        public HttpResponseMessage GetIsInValidBudget(decimal currentBudget, decimal PreviousBudget, string recordId, int ParentId)
        {
            string message = default(string);
            bool isInvalid = repository.fnIsInvalidBudget(currentBudget, PreviousBudget, recordId, ParentId, ref message);
            var result = new { IsInvalidBudget = isInvalid, MessageText = message };
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetIsExceedsContainerBudget/{containerId}/{changedBudget}/{recordId}")]
        public HttpResponseMessage GetIsExceedContainerBudget(int containerId, decimal changedBudget, string recordId)
        {
            var result = repository.fnIsExceedsContainerBudget(containerId, changedBudget, recordId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetIsContainer/{recordId}")]
        public HttpResponseMessage GetIsContainer(int recordId)
        {

            var result = repository.fnIsContainer(recordId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetIsProjectCanbeDone/{recordId}")]
        public HttpResponseMessage GetIsProjectCanbeDone(string recordId)
        {
            string message = default(string);
            bool canbeDone = repository.fnProjectCanbeDone(recordId, ref message);
            var result = new { ProjectCanbeDone = canbeDone, Message = message };
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetDependentProjects/{recordId}")]
        public HttpResponseMessage GetDependentProjectsTable(string recordId)
        {
            var resultTable = repository.GetDependentProjectTable(recordId);
            if (resultTable.Tables[0].Rows.Count > 0)
                return Request.CreateResponse(HttpStatusCode.OK, resultTable, Configuration.Formatters.JsonFormatter);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, resultTable);
        }


        [Route("GetChildProjectIds/{recordId}")]
        public HttpResponseMessage GetChildProjectIds(string recordId)
        {
            var result = repository.GetChildProjectIds(recordId);
            if (!string.IsNullOrEmpty(result))
                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            else
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, result);
        }

        [Route("GetChildProjectTaskNotDoneCount/{recordId}")]
        public HttpResponseMessage GetChildProjectTaskNotDoneCount(string recordId)
        {
            var result = repository.GetCountChildsNotDone(recordId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }

        [Route("GetParentProjectTaskDoneCount/{recordId}")]
        public HttpResponseMessage GetParentProjectTaskDoneCount(string recordId)
        {
            string message = default(string);
            int done = repository.GetCountParentsDone(recordId, ref message);
            var result = new { ParentsDoneCount = done, MilestoneIsDone = message };
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }


        [Route("IsMilestoneDependentOn/{mileStoneId}/{isForDelete}")]
        public HttpResponseMessage IsMilestoneDependentOn(string mileStoneId, bool isForDelete)
        {
            string message = default(string);
            bool dependent = mileStoneRepository.fnCheckDependency(mileStoneId, isForDelete, ref message);
            var result = new { IsDependent = dependent, Message = message };
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }


        [Route("IsChildObjectExists/{recordId}")]
        public HttpResponseMessage IsChildObjectExists(string recordId)
        {
            bool IsDecisionsExist = !default(bool);
            bool IsTasksExist = !default(bool);
            repository.fnGetIsChildObjectsExists(recordId, ref IsTasksExist, ref IsDecisionsExist);
            var result = new { IsChildTasksExists = IsTasksExist, IsChildDecisionsExists = IsDecisionsExist };
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }


        [Route("GetProjectTaskTenderOrContractId/{recordId}/{loginName}")]
        public HttpResponseMessage GetProjectTaskTenderOrContractId(int recordId, string loginName)
        {
            int objectTypeId = default(int);
            int objectId = default(int);
            try
            {
                repository.fnGetTenderOrContractId(recordId, loginName, ref objectTypeId, ref objectId);
                var result = new { ObjectTypeId = objectTypeId, ObjectId = objectId };
                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error while getting data. - " + ex.Message);
            }
        }





        [HttpPost]
        [Route("SaveProjectTask/{recordId}/{childTasksForUseSepcialRights}/{childDecisionsForUseSpecialRights}")]
        public HttpResponseMessage SaveProjectTaskDetails(string recordId, string childTasksForUseSepcialRights, string childDecisionsForUseSpecialRights, [FromBody] ProjectTaskActionData data)
        {
            bool IsSuccess = default(bool);
            string parentIdsTobeOpened = string.Empty;
            string childTaskIdsTobeOpened = string.Empty;
            string childDecisionIdsTobeOpened = string.Empty;

            try
            {
                repository.BeginTrans();

                string record = repository.fnSave(recordId, data, ref IsSuccess, ref parentIdsTobeOpened, ref childTaskIdsTobeOpened, ref childDecisionIdsTobeOpened, ConverterHelper.CheckSingleQuote(childTasksForUseSepcialRights), ConverterHelper.CheckSingleQuote(childDecisionsForUseSpecialRights));

                if (IsSuccess)
                {
                    var result = new { RecordId = record, SavedSuccessfully = IsSuccess, ParentIdsTobeOpened = parentIdsTobeOpened, ChildTaskIdsTobeOpened = childTaskIdsTobeOpened, ChildDecisionIdsTobeOpened = childDecisionIdsTobeOpened };
                    repository.CommitTrans();
                    return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
                }
                else
                {
                    repository.RollbackTrans();
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Some Error while Saving Data");
                }
            }
            catch (Exception ex)
            {
                repository.RollbackTrans();
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);

            }


        }



        [HttpPost]
        [Route("DeleteProjectGroupDetails/{ProjectId}")]
        public HttpResponseMessage DeleteGroupProjectDetailsData(string ProjectId)
        {
            try
            {
                var result = repository.fnDeleteDetialsData(ProjectId);
                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error Occured while Deleting.");
            }

        }

        [HttpPost]
        [Route("SaveProjectTaskDeadLine/{fromDate}/{toDate}/{recordId}/{loginName}")]
        public HttpResponseMessage SaveProjectDeadLines(string fromDate, string toDate, string recordId, string loginName)
        {
            try
            {
                var result = repository.SaveNewDeadlineEntry(fromDate, toDate, recordId, loginName);
                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error Occured while Saving. - " + ex.Message);
            }
        }


        ////[Route("GetTaskInitiationSourceList")]
        ////public HttpResponseMessage GetTaskInitiationSourceList()
        ////{
        ////    var result = repository.GetTaskInitiationSourcesList();
        ////    return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        ////}


        //Processed By



        [Route("GetProjectTaskProcessedBy/{recordId}/{objectType}")]
        public HttpResponseMessage GetProjectTaskProcessedBy(string recordId, int objectType)
        {
            var result = processedRepository.GetTaskProcessedBy(recordId, objectType);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetProjectTaskProcessedByAddress/{recordId}/{objectType}")]
        public HttpResponseMessage GetProjectTaskProcessedByAddress(string recordId, int objectType)
        {
            var result = processedRepository.GetProcessedByAddresses(recordId, objectType);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [HttpPost]
        [Route("SaveProcessedBy/{recordId}/{objectType}")]
        public HttpResponseMessage SaveProjectProcessedBy([FromBody]ProjectTaskProcessedByActionData data, string recordId, int objectType)
        {
            try
            {
                DataTable dt = ConverterHelper.ConvertToDataTable<ProjectTaskProcessedByActionData>(new List<ProjectTaskProcessedByActionData> { data });
                var result = processedRepository.SaveTaskProcessedBy(dt, objectType, recordId);
                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error Occured while Saving. - " + ex.Message);
            }
        }




        //Deadline Extensions

        [Route("GetDeadLineExtension/{recordId}")]
        public HttpResponseMessage GetProjectDeadLineExtension(string recordId)
        {
            try
            {
                var result = deadlineRepository.GetDeadlineExtn(recordId);
                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error Occured while getting data. - " + ex.Message);
            }
        }



        [HttpPost]
        [Route("SaveDeadLineExtension/{recordId}/{loginName}")]
        public HttpResponseMessage SaveProjectDeadLineExtensions([FromBody]DeadLineExtensionActionData data, string recordId, string loginName)
        {
            try
            {
                DataTable dt = ConverterHelper.ConvertToDataTable<DeadLineExtensionActionData>(new List<DeadLineExtensionActionData> { data });
                var result = deadlineRepository.SaveDeadlines(dt, recordId, loginName);
                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error Occured while Saving. - " + ex.Message);
            }
        }


    }
}
