using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirtusBI;
using VirtusDataModel;

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

        [Route("GetMilestonesOrProjectManager/{projectId}/{IsPorject}/{IsDone}")]
        public HttpResponseMessage GetProjectUserRequestId(int projectId, bool IsProject, bool IsDone)
        {
            var result = repository.fnGetMilestonesOrProjectManagers(IsProject, projectId, IsDone);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetProjectKind/{DepartmentId}/{IsPorject}")]
        public HttpResponseMessage GetProjectKindList(int DepartmentId, bool IsProject)
        {
            var result = repository.GetKindBindingDS(IsProject, DepartmentId);
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
            var result = repository.fnIsContributionExceeds(contribution, mileStoneId, recordId);
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



        [HttpPost]
        [Route("SaveProjectTask/{recordId}/{childTasksForUseSepcialRights}/{childDecisionsForUseSpecialRights}")]
        public HttpResponseMessage SaveProjectTaskDetails(string recordId, string childTasksForUseSepcialRights, string childDecisionsForUseSpecialRights, [FromBody] ProjectTaskActionData data)
        {
            bool IsSuccess = default(bool);
            string parentIdsTobeOpened = string.Empty;
            string childTaskIdsTobeOpened = string.Empty;
            string childDecisionIdsTobeOpened = string.Empty;


            string record = repository.fnSave(recordId, data, ref IsSuccess, ref parentIdsTobeOpened, ref childTaskIdsTobeOpened, ref childDecisionIdsTobeOpened, childTasksForUseSepcialRights, childDecisionsForUseSpecialRights);

            if (IsSuccess)
            {
                var result = new { RecordId = record, SavedSuccessfully = IsSuccess, ParentIdsTobeOpened = parentIdsTobeOpened, ChildTaskIdsTobeOpened = childTaskIdsTobeOpened, ChildDecisionIdsTobeOpened = childDecisionIdsTobeOpened };
                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
            }
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Some Error while Saving Data");



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


        ////[Route("GetTaskInitiationSourceList")]
        ////public HttpResponseMessage GetTaskInitiationSourceList()
        ////{
        ////    var result = repository.GetTaskInitiationSourcesList();
        ////    return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        ////}


        //Processed By



        [Route("GetProjectTaskProcessedBy")]
        public HttpResponseMessage GetProjectTaskProcessedBy(string recordId, int objectType)
        {
            var result = processedRepository.GetTaskProcessedBy(recordId, objectType);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }

        [Route("GetProjectTaskProcessedByAddress")]
        public HttpResponseMessage GetProjectTaskProcessedByAddress(string recordId, int objectType)
        {
            var result = processedRepository.GetProcessedByAddresses(recordId, objectType);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }



        //Deadline Extensions







    }
}
