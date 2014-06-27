using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtusMobileAPI.Models
{
    class MessagesModel
    {
    }

    public class MessageActionData
    {
        ///// <summary>
        ///// Message Id ,for new record - 0
        ///// </summary>
        //public int MessageId { get; set; }

        /// <summary>
        /// message prirority i.e high-3,normal-2,low-1
        /// </summary>
        public int PriorityId { get; set; }
        /// <summary>
        /// login username
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// To persons list separated with comas(',')
        /// </summary>
        public string ToPersons { get; set; }
        /// <summary>
        /// To Org list separated with comas(',')
        /// </summary>
        public string ToOrganizations { get; set; }
        /// <summary>
        /// message subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Message Content or description
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Have to send mail so true
        /// </summary>
        public bool IsSendEmail { get; set; }
        /// <summary>
        /// Have to send Sms so true
        /// </summary>
        public bool IsSendSMS { get; set; }
        /// <summary>
        /// Object Type which is linked to message if any
        /// </summary>
        public int LinkedObjectTypeId { get; set; }
        /// <summary>
        /// Object Id which is linked to message if any 
        /// </summary>
        public int LinkedObjectId { get; set; }
        /// <summary>
        /// Is this message replay to all if so true
        /// </summary>
        public bool IsReplyToAll { get; set; }
        /// <summary>
        /// Link file id if any.
        /// </summary>
        public int LinkFileId { get; set; }

    }
}