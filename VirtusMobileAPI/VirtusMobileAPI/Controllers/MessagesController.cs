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
    /// Messages Services to do all actions regarding sending and receving etc.
    /// </summary>
    [RoutePrefix("VirtusApi/Messages")]
    public class MessagesController : ApiController
    {
        clsMessages repository = new clsMessages();
        /// <summary>
        /// Returns the all the list of inbox messages corresponding to login user.
        /// </summary>
        /// <param name="loginUserName"> specify login username</param>
        /// <param name="IsUndone">specify which type of message want to return i.e only undone or with done also</param>
        /// <param name="IsCanViewProject"> specify can user has project view right</param>
        /// <param name="IsCanViewTask"></param>
        /// <param name="IsAllMessages">specify has to show all messages</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetInboxMessages/{loginUserName}/{IsUndone}/{IsCanViewProject}/{IsCanViewTask}/{IsAllMessages}")]
        public HttpResponseMessage GetInboxMessagesList(string loginUserName, bool IsUndone, bool IsCanViewProject, bool IsCanViewTask, bool IsAllMessages)
        {
            bool IsCanviewLinkFolder = !default(bool);
            bool IsCanViewQueryFolder = !default(bool);
            bool IsCanViewMeeting = !default(bool);
            bool IsCanViewAgendaItem = !default(bool);
            bool IsCanViewDecision = !default(bool);
            int languageId = 3;

            var result = repository.fnGetInboxData(loginUserName, IsUndone, languageId, IsCanViewProject, IsCanViewTask, IsCanViewDecision, IsCanViewAgendaItem, IsCanViewMeeting, IsCanviewLinkFolder, IsCanViewQueryFolder, IsAllMessages);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }

        /// <summary>
        /// Returns all the list of sent itmes corresponding to login user
        /// </summary>
        /// <param name="loginUserName"></param>
        /// <param name="IsCanViewProject"></param>
        /// <param name="IsCanViewTask"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSentMessages/{loginUserName}/{IsCanViewProject}/{IsCanViewTask}")]
        public HttpResponseMessage GetSentMessagesList(string loginUserName, bool IsCanViewProject, bool IsCanViewTask)
        {
            bool IsCanviewLinkFolder = !default(bool);
            bool IsCanViewQueryFolder = !default(bool);
            bool IsCanViewMeeting = !default(bool);
            bool IsCanViewAgendaItem = !default(bool);
            bool IsCanViewDecision = !default(bool);
            int languageId = 3;

            var result = repository.fnGetSentItemsData(loginUserName, languageId, IsCanViewProject, IsCanViewTask, IsCanViewDecision, IsCanViewAgendaItem, IsCanViewMeeting, IsCanviewLinkFolder, IsCanViewQueryFolder);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }

        /// <summary>
        /// Returns the message details
        /// </summary>
        /// <param name="messageId">specify the message id</param>
        /// <param name="loginUserName">specify the login username</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetMessageDetails/{messageId}/{loginUserName}")]
        public HttpResponseMessage GetMessageDetails(int messageId, string loginUserName)
        {
            var result = repository.fnGetMessageDetails(messageId, loginUserName);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }


        /// <summary>
        /// Returns the all the users to whom the messages have to send.
        /// </summary>
        /// <param name="loginUserName">specify login username</param>
        /// <param name="noOfRecords">specify how many records would be display.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUsers/{loginUserName}/{noOfRecords}")]
        public HttpResponseMessage GetUsers(string loginUserName, int noOfRecords)
        {
            bool IsforScannedDocs = default(bool);
            string searchCondition = string.Empty;
            string strOrderBy = string.Empty;
            string strSortOrder = string.Empty;
            int totalCount = default(int);

            var result = repository.fnGetUsers(loginUserName, IsforScannedDocs, searchCondition, noOfRecords, strOrderBy, strSortOrder, totalCount);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }

        /// <summary>
        /// Returns all the persons, organization names (separated with coma(',')) involved in current message.
        /// </summary>
        /// <param name="messageId">specify messageid</param>
        /// <param name="loginUserId">specify loginId</param>
        /// <param name="IsIncludeSender">specify true if need to include the sender in the list else false.</param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetReplyAllPersonNames/{messageId}/{loginUserId}/{IsIncludeSender}")]
        public HttpResponseMessage GetUsGetReplyAllPersonNames(int messageId, int loginUserId, bool IsIncludeSender)
        {
            string strPersons = string.Empty;
            string strOrgs = string.Empty;

            repository.fnGetReplyAllNames(messageId, ref strPersons, ref strOrgs, IsIncludeSender, loginUserId);
            return Request.CreateResponse(HttpStatusCode.OK, new { ReplyToPersons = strPersons, ReplyToOrganizations = strOrgs }, Configuration.Formatters.JsonFormatter);

        }


        /// <summary>
        /// Returns all the persons, organization ids and names involved in current message.
        /// </summary>
        /// <param name="messageId">specify messageid</param>
        /// <param name="loginUserId">specify loginId</param>
        /// <param name="IsIncludeSender">specify true if need to include the sender in the list else false.</param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetReplyAllIds/{messageId}/{loginUserId}/{IsIncludeSender}")]
        public HttpResponseMessage GetReplyAllIds(int messageId, int loginUserId, bool IsIncludeSender)
        {
            var result = repository.fnGetReplyAllIds(messageId, IsIncludeSender, loginUserId);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }

        /// <summary>
        /// Returns the list of Internal Organizations
        /// </summary>
        /// <param name="IsParentRequired">Specify if you want to include parent organizaions also.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetInternalOrganizations/{IsParentRequired}")]
        public HttpResponseMessage GetInternalOrganizations(bool IsParentRequired)
        {
            var result = repository.fnGetInternalOrganisations(IsParentRequired);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }

        /// <summary>
        /// Returns the list of Recipients names 
        /// </summary>
        /// <param name="messageId">specify messageid</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetReceipientNames/{messageId}")]
        public HttpResponseMessage GetReceipientNames(int messageId)
        {
            string strPersons = string.Empty;
            string strOrgs = string.Empty;

            var result = repository.fnGetReceipentsName(messageId, ref strPersons, ref strOrgs);
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Name = result,
                RecpPersons = strPersons,
                RecpOrganizations = strOrgs
            }, Configuration.Formatters.JsonFormatter);

        }

        /// <summary>
        /// Returns the UnRead messages Count
        /// </summary>
        /// <param name="loginUserName">specify login UserName</param>
        /// <param name="IsCanViewProject">specify can view project </param>
        /// <param name="IsCanViewTask">specify can view task</param>
        /// <returns></returns>

        [HttpGet]
        [Route("GetUnreadMeassagesCount/{loginUserName}/{IsCanViewProject}/{IsCanViewTask}")]
        public HttpResponseMessage GetUnreadMeassagesCount(string loginUserName, bool IsCanViewProject, bool IsCanViewTask)
        {
            bool IsCanviewLinkFolder = !default(bool);
            bool IsCanViewQueryFolder = !default(bool);
            bool IsCanViewMeeting = !default(bool);
            bool IsCanViewAgendaItem = !default(bool);
            bool IsCanViewDecision = !default(bool);
            int languageId = 3;

            var result = repository.fnGetUnReadMessagesCount(loginUserName, languageId, IsCanViewProject, IsCanViewTask, IsCanViewDecision, IsCanViewAgendaItem,
                IsCanViewMeeting, IsCanviewLinkFolder, IsCanViewQueryFolder);
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }

        /// <summary>
        /// Returns the object Status
        /// </summary>
        /// <param name="objectTypeId"> Object Type Id</param>
        /// <param name="recordId">object id or record Id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetObjectStatus/{objectTypeId}/{recordId}")]
        public HttpResponseMessage GetReceipientNames(int objectTypeId, int recordId)
        {
            bool isDeleted = default(bool);
            bool isActive = default(bool);
            bool isArchive = default(bool);

            repository.fnCheckObjectStatus(objectTypeId, recordId, ref isDeleted, ref isActive, ref isArchive);
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                IsDeleted = isDeleted,
                IsActive = isActive,
                IsArchived = isArchive
            }, Configuration.Formatters.JsonFormatter);

        }




        /// <summary>
        /// Sets the Read Status of the Message from UnRead to Read vice versa.
        /// </summary>
        /// <param name="messageId">specify the message id</param>
        /// <param name="loginUserName">specify the login username</param>
        /// <param name="IsReaded">specify true if status has to set reader else false</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetMessageReadStatus/{messageId}/{loginUserName}/{IsReaded}")]
        public HttpResponseMessage SetMessageReadStatus(int messageId, string loginUserName, bool IsReaded)
        {
            var result = repository.fnSetReadMessage(messageId, loginUserName, Convert.ToInt32(IsReaded));
            return Request.CreateResponse(HttpStatusCode.OK, result, Configuration.Formatters.JsonFormatter);

        }

         

        /// <summary>
        /// Message Details have to specify to whom the message should be sent.
        /// </summary>
        /// <param name="messageId">specify message id for new message it should be -0 </param>
        /// <param name="messageDetails">specify the message details (MessageActionData type) with following properties.</param>
        /// <returns></returns>

        [HttpPost]
        [Route("SendMessage/{messageId}")]
        public HttpResponseMessage SendMessage(int messageId, [FromBody]MessageActionData messageDetails)
        {
            bool IsRefresh = default(bool);
            int language = 3;
            if (messageDetails == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Provide valid message details,those should not be null.");
            else if (String.IsNullOrEmpty(ConverterHelper.CheckSingleQuote(messageDetails.UserName)))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Provide valid username,those should not be null or empty.");
            else if (String.IsNullOrEmpty(ConverterHelper.CheckSingleQuote(messageDetails.ToPersons)))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Provide valid ToPersons,those should not be null or empty.");

            try
            {
                repository.BeginTrans();
                var result = repository.fnInsertMessages(messageDetails.PriorityId, messageDetails.UserName, messageDetails.ToPersons
                    , messageDetails.ToOrganizations, messageDetails.Subject, messageDetails.Message, Convert.ToInt16(messageDetails.IsSendEmail)
                    , Convert.ToInt16(messageDetails.IsSendSMS), messageDetails.LinkedObjectTypeId, messageDetails.LinkedObjectId,
                    messageId, ref IsRefresh, messageDetails.IsReplyToAll, messageDetails.LinkFileId, language);
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
