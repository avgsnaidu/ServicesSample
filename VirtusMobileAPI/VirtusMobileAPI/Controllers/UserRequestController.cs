using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using VirtusBI;
using VirtusDataModel;
using VirtusMobileService.Models;

namespace VirtusMobileAPI.Controllers
{
    /// <summary>
    /// UserRequest Related Services
    /// </summary>
    public class UserRequestController : ApiController
    {
        clsUserRequests repository = new clsUserRequests();


        /// <summary>
        /// Get the Details of the UserRequests (but if here the list didn't have ToPerson ,to whom the request is passed then
        /// To Get the userrequest To-UserId, need to Call the Service "GetUserRequestToUser")
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [ActionName("GetUserRequest")]
        [Route("VirtusApi/UserRequest/GetUserRequest/{id}/{userName}")]
        public HttpResponseMessage GetUserRequestDetails(int Id, string userName)
        {
            var result = repository.fnGetUserRequestDetails(Id, userName);
            if (result != null)
            {
                //if (result.Tables[0].Rows.Count == 1)
                //    return Request.CreateResponse(HttpStatusCode.OK, result.Tables[0], Configuration.Formatters.JsonFormatter);
                //else
                return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

            }
            else
                return Request.CreateResponse(HttpStatusCode.NotFound, result, Configuration.Formatters.JsonFormatter);

            //return result;
        }

        [ActionName("GetMyRequests")]
        [Route("VirtusApi/UserRequest/GetMyRequests/{userName}/{IsUndone}/{IsReject}/{IsCc}/{IsAllRequests}")]
        public HttpResponseMessage GetUserRequestDetails(string userName, bool IsUndone, bool IsReject, bool IsCc, bool IsAllRequests)
        {
            DataSet ds = repository.fnGetMyRequestData(userName, IsUndone, IsReject, IsCc, IsAllRequests);
            return Request.CreateResponse(HttpStatusCode.OK, ds, Configuration.Formatters.JsonFormatter);
        }

        [ActionName("GetSentRequests")]
        [Route("VirtusApi/UserRequest/GetSentRequests/{userName}/{IsUndone}/{IsReject}")]
        public HttpResponseMessage GetUserRequestDetails(string userName, bool IsUndone, bool IsReject)
        {
            DataSet ds = repository.fnGetSentItemsData(userName, IsUndone, IsReject);
            return Request.CreateResponse(HttpStatusCode.OK, ds, Configuration.Formatters.JsonFormatter);
        }

        //[ActionName("GetToUser")]
        //[Route("VirtusApi/UserRequest/GetToUsers")]
        //public HttpResponseMessage GetToUsersForUserRequest([FromBody]UserRequest_ToUsersParams data)
        //{
        //    DataSet ds = repository.fnGetUsersForUserRequest(data.LoginName, data.UserRequestId, data.SearchCondition, data.NoOfRecords, data.OrderBy, data.SortOrder, data.TotalCount, data.IsForward);
        //    return Request.CreateResponse(HttpStatusCode.OK, ds, Configuration.Formatters.JsonFormatter);
        //}


        /// <summary>
        ///  To Get the list of  available persons for To-Users or Cc-Users list
        /// </summary>
        /// <param name="LoginName"></param>
        /// <param name="UserRequestId">For new Request pass default value of int i.e " 0 " </param>
        /// <param name="IsForward"> True if it to fill forward persons list</param>
        /// <param name="IsForCcUsers"> True if it to fill Cc-Users persons list</param>
        /// <param name="selectedToUserIfAny"> pass the selectedToUserId If Any otherwise pass '0' </param>
        /// <returns></returns>
        /// 

        //[ActionName("GetToUser")]
        //[Route("VirtusApi/UserRequest/GetToUsers/{LoginName}/{UserRequestId}/{SearchCondition}/{NoOfRecords}/{OrderBy}/{SortOrder}/{TotalCount}/{IsForward}")]
        //public HttpResponseMessage GetToUsersForUserRequest(string LoginName, int UserRequestId, int NoOfRecords, int TotalCount, bool IsForward = false, string OrderBy = "", string SortOrder = "", string SearchCondition = "")
        //{
        [ActionName("GetToUser")]
        [Route("VirtusApi/UserRequest/GetUsers/{LoginName}/{UserRequestId}/{selectedToUserIfAny}/{IsForward}/{IsForCcUsers}")]
        public HttpResponseMessage GetToUsersForUserRequest(string LoginName, int UserRequestId, int selectedToUserIfAny, bool IsForward = false, bool IsForCcUsers = false)
        {
            int noOfRecords = default(int);
            int totalCount = default(int);
            string orderBy = "";
            string sortOrder = "";
            string SearchCondition = string.Empty;

            if (IsForCcUsers)
            {
                SearchCondition = "A.AddresseId not in (" + selectedToUserIfAny + ") AND  A.IsActive=1 ";
                sortOrder = "4";
            }
            else
            {
                SearchCondition = "A.LoginName is not null And A.AddressType='P' and A.IsActive=1 And dbo.fnGetSplFunctionSecurity(A.LoginUserTypeId,49) = 1 ";
            }

            DataSet ds = repository.fnGetUsersForUserRequest(ConverterHelper.CheckSingleQuote(LoginName), UserRequestId,
                ConverterHelper.CheckSingleQuote(SearchCondition), noOfRecords, ConverterHelper.CheckSingleQuote(orderBy), ConverterHelper.CheckSingleQuote(sortOrder), totalCount, IsForward);
            return Request.CreateResponse(HttpStatusCode.OK, ds, Configuration.Formatters.JsonFormatter);
        }


        /// <summary>
        ///  Get the ToUserId of userRequest based on UserRequestId
        /// </summary>
        /// <param name="userRequestId"></param>
        /// <returns></returns>
        [Route("VirtusApi/UserRequest/GetUserRequestToUser/{userRequestId}/")]
        public HttpResponseMessage GetUserRequestToUser(int userRequestId)
        {
            int unReadCount = repository.fnGetUserRequestToUser(userRequestId);
            return Request.CreateResponse(HttpStatusCode.OK, unReadCount, Configuration.Formatters.JsonFormatter);
        }



        [ActionName("GetUnReadCount")]
        [Route("VirtusApi/UserRequest/GetUnReadRequestsCount/{LoginName}/{ObjectEnumId}")]
        public HttpResponseMessage GetUnReadRequestsCount(string LoginName, int ObjectEnumId)
        {
            int unReadCount = repository.fnGetUnReadObjectsCount(LoginName, ObjectEnumId);
            return Request.CreateResponse(HttpStatusCode.OK, unReadCount, Configuration.Formatters.JsonFormatter);
        }


        [ActionName("GetRequestsOnTypeForUser")]
        [Route("VirtusApi/UserRequest/GetRequestsOnType/{LoginName}/{RequestType}")]
        public HttpResponseMessage GetRequestsBasedOnTypeForUser(string LoginName, int RequestType)
        {
            DataSet ds = repository.fnGetRequestForUsers(LoginName, RequestType);
            return Request.CreateResponse(HttpStatusCode.OK, ds, Configuration.Formatters.JsonFormatter);
        }

        [ActionName("GetProjectComponents")]
        [Route("VirtusApi/UserRequest/GetProjectComponents/{requestId}/{IsMaintanenceRequest}")]
        public HttpResponseMessage GetProjectComponents(int requestId, bool IsMaintanenceRequest)
        {
            DataSet ds = repository.fnGetProjectComponents(requestId, IsMaintanenceRequest);
            return Request.CreateResponse(HttpStatusCode.OK, ds, Configuration.Formatters.JsonFormatter);
        }

        [ActionName("GetReportingToPersonId")]
        [Route("VirtusApi/UserRequest/GetReportingToPersonId/{loginId}")]
        public HttpResponseMessage GetReportingToPersonId(int loginId)
        {
            int reportingTo = repository.fnGetReportingToId(loginId);
            return Request.CreateResponse(HttpStatusCode.OK, reportingTo, Configuration.Formatters.JsonFormatter);
        }

        [ActionName("GetReportingToPersonName")]
        [Route("VirtusApi/UserRequest/GetReportingToName/{loginId}")]
        public HttpResponseMessage GetReportingToPersonName(int loginId)
        {
            string reportingTo = repository.fnGetReportingToNames(loginId);
            return Request.CreateResponse(HttpStatusCode.OK, reportingTo, Configuration.Formatters.JsonFormatter);
        }

        [ActionName("GetIsLoginNameExists")]
        [Route("VirtusApi/UserRequest/GetIsLoginNameExists/{loginName}/{reqeuestId}")]
        public HttpResponseMessage GetIsLoginNameExists(string loginName, int reqeuestId)
        {
            bool loginNameExists = repository.fnLoginNameExists(loginName, reqeuestId);
            return Request.CreateResponse(HttpStatusCode.OK, loginNameExists, Configuration.Formatters.JsonFormatter);
        }

        [ActionName("GetRequestComponent")]
        [Route("VirtusApi/UserRequest/GetRequestComponent/{userReqeuestId}")]
        public HttpResponseMessage GetRequestComponent(int userReqeuestId)
        {
            string requestComponentIds = string.Empty;

            string componentNames = repository.fnGetRequestComponents(userReqeuestId, ref requestComponentIds);

            RequestComponent objRC = new RequestComponent();
            objRC.RequestComponentIds = requestComponentIds;
            objRC.RequestComponentNames = componentNames;

            return Request.CreateResponse(HttpStatusCode.OK, objRC, Configuration.Formatters.JsonFormatter);
        }


        [ActionName("GetRequestComponentOnDepartment")]
        [Route("VirtusApi/UserRequest/GetRequestComponentBasedOnDepartment/{RequestTypeId}/{departmentId}")]
        public HttpResponseMessage GetRequestComponentBasedOnDepartment(int RequestTypeId, int departmentId)
        {
            DataSet ds = repository.fnGetRequestComponents(RequestTypeId, departmentId);
            return Request.CreateResponse(HttpStatusCode.OK, ds, Configuration.Formatters.JsonFormatter);
        }


        [ActionName("GetNextObjectRecord")]
        [Route("VirtusApi/UserRequest/GetNextObjectRecord/{RequestId}/{userRequestEnumId}")]
        public HttpResponseMessage GetNextObjectRecord(int RequestId, int userRequestEnumId)
        {
            var result = repository.fnGetObjectRecordId(RequestId, userRequestEnumId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);
        }




        [HttpPost]
        [ActionName("SetReadStatusRequest")]
        [Route("VirtusApi/UserRequest/SetReadStaus/{loginName}/{reqeuestId}")]
        public HttpResponseMessage SetReadStatusRequest(string loginName, int reqeuestId)
        {
            int status = repository.fnSetReadStatusRequest(reqeuestId, loginName);
            return Request.CreateResponse(HttpStatusCode.OK, status, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// For sending the New UserRequest, need to pass the NewRequestActionData Type in json String format
        /// </summary>
        /// <param name="userRequestId"></param>
        /// <param name="newUserRequestActionData"> Need to pass the sample data like :  {  "Code": "sample string 1",   "RequestType": 2,   "Subject": "sample string 3",   "InitialBudget": 4.0,   "Description": "sample string 5",   "CreatedBy": "sample string 6",  "CreatedOn": "2014-06-09",   "ModifiedBy": "sample string 7",   "ModifiedOn": "2014-06-09",   "Status": 8,   "IsActive": true,   "DepartmentId": 10,   "ToUserids": "sample string 11",   "ToUserId": 12,   "ToCCUsers": "sample string 13",   "Comments": "sample string 14",   "RequestDate": "2014-06-09",  "IsSaveDraft": true} </param>
        /// <returns></returns>
        [HttpPost]
        [AcceptVerbs("POST", "PUT")]
        [Route("VirtusApi/UserRequest/SendUserRequest")]
        public HttpResponseMessage SaveOrSendUserRequest([FromBody]dynamic newUserRequestActionData)
        {

            int userRequestId=default(int);

            bool bSuccess = default(bool);
            try
            {
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(newUserRequestActionData, Newtonsoft.Json.Formatting.None);

                var newUserdata = Newtonsoft.Json.JsonConvert.DeserializeObject<NewUserRequestActionData>(jsonString);
                EditUserRequestActionData data = new EditUserRequestActionData(newUserdata);
                repository.BeginTrans();
                int iRecordId = repository.fnSave(userRequestId, ref bSuccess, data);

                if (!bSuccess)
                {
                    repository.RollbackTrans();
                }
                else
                {
                    repository.CommitTrans();
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { RecordId = iRecordId, IsSavedSucessfully = bSuccess }, Configuration.Formatters.JsonFormatter);

            }
            catch (Exception ex)
            {
                repository.RollbackTrans();
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not Saved" + ex);
            }
        }

        /// <summary>
        /// 
        /// This service is used to Process the "User Request" for next steps, by providing the meaning full values.     
        /// This service is used to implement following actions like :  "Approve", "Approve and Forward ", " Need More Info ", "Reject ", "Hold "   - 
        /// For Approving the Request Need to Change the Staus with following like : Status = (int)Enums.ObjectStatus.Approved,   CreateNextStep = true  , DoneDate = current date , IsDone = true ,  ModifiedBy = "loginUserName " ,  IsSaveDraft = false ,IsKeptHold = false etc..
        /// Here while approving CreateNextStep should be true for creating next Object like tender or contract.   
        /// For Approving and Forward need to  CreateNextStep = false , Status = (int)Enums.ObjectStatus.Approve_Forward ,IsDone=false   
        /// For Hold --  CreateNextStep = false , Status = (int)Enums.ObjectStatus.Hold ,IsDone=false , IsKeptHold = true ,AlreadyApproved = true
        /// For Need More Info --  CreateNextStep = false , Status =  (int)Enums.ObjectStatus.NeedMoreinfo ,IsDone=false , IsKeptHold = false
        /// </summary>
        /// <param name="userRequestId"> UserRequestId for which status to be changed</param>
        /// <param name="editUserRequestActionData">need to pass json-type string data sample like :          {  "RequestComponents": "sample string 1",  "OriginalSubject": "sample string 2",  "DeadlineRemind": true,  "DeadlineReminderDays": 4,  "DeadlineEndDate": "2014-06-09",  "AlreadyApproved": true,  "OriginalStatus": 6,  "IsArchived": true,  "IsManualCode": true,  "OriginalRequest": 9,  "DesignContractCode": "sample string 10",  "IsDesign": true,  "CreateNextStep": true,  "HoldUntilDate": "2014-06-09",  "HoldRemindDays": 13,  "IsKeptHold": true,  "OldReminderMonths": 15,  "IsHoldRemind": true,  "IsDone": true,  "DoneDate": "2014-06-09T12:41:55.8755868+05:30",  "IsDesignRequired": true,  "IsTenderRequired": true,  "IsContractRequired": true,  "ForwardedTo": "sample string 21",  "ForwardedOn": "2014-06-09",  "Project": 22,  "OriginalReqProject": 23,  "UserRequestId_Old": 24,  "IsRead": true,  "PriorityId": 26,  "OriginalRequestId": 27,  "Code": "sample string 28",  "RequestType": 29,  "Subject": "sample string 30",  "InitialBudget": 31.0,  "Description": "sample string 32",  "CreatedBy": "sample string 33",  "CreatedOn": "2014-06-09",  "ModifiedBy": "sample string 34",  "ModifiedOn": "2014-06-09",  "Status": 35,  "IsActive": true,  "DepartmentId": 37,  "ToUserids": "sample string 38",  "ToUserId": 39,  "ToCCUsers": "sample string 40",  "Comments": "sample string 41",  "RequestDate": "2014-06-09",  "IsSaveDraft": true} </param>
        /// <returns> If action is sucess then is will give Status Code as : 200 other wise, it may give status code : 400 which is an error.</returns>
        [HttpPost]
        [AcceptVerbs("POST", "PUT")]
        [Route("VirtusApi/UserRequest/ProcessUserRequest/{userRequestId}")]
        public HttpResponseMessage HoldOrProcessUserRequest(int userRequestId, [FromBody]dynamic editUserRequestActionData)
        {


            bool bSuccess = default(bool);
            try
            {
                string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(editUserRequestActionData, Newtonsoft.Json.Formatting.None);

                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<EditUserRequestActionData>(jsonString);

                repository.BeginTrans();
                int iRecordId = repository.fnSave(userRequestId, ref bSuccess, data);

                if (!bSuccess)
                {
                    repository.RollbackTrans();
                }
                else
                {
                    repository.CommitTrans();
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { RecordId = iRecordId, IsSavedSucessfully = bSuccess }, Configuration.Formatters.JsonFormatter);

            }
            catch (Exception ex)
            {
                repository.RollbackTrans();
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Not Saved" + ex.Message);
            }
        }




    }


}


