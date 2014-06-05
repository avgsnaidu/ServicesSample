﻿using System;
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


        [HttpPost]
        [AcceptVerbs("POST", "PUT")]
        [Route("VirtusApi/UserRequest/SendUserRequest/{userRequestId}")]
        public HttpResponseMessage SaveOrSendUserRequest(int userRequestId, [FromBody]dynamic newUserRequestActionData)
        {


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


