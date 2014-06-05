using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtusDataModel
{
    public class TenderActionData
    {

        public string Code { get; set; }

        public string Subject { get; set; }
        public int ProjectManagerID { get; set; }

        public string Description { get; set; }

        public int SelectedConsultant { get; set; }

        public int ApprovingAuthority { get; set; }

        public bool DeadlineRemind { get; set; }

        public int DeadlineRemindDays { get; set; }

        public DateTime? DeadlineEndDate { get; set; }

        public bool IsDone { get; set; }

        public DateTime? DoneDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public int Status { get; set; }

        public bool IsArchived { get; set; }

        public decimal ApprovedBudget { get; set; }

        public bool IsDesign { get; set; }

        public int UserRequestId { get; set; }

        public int TenderDesignID { get; set; }

        public string ModifiedBy { get; set; }

        public int ObjectTypeId { get; set; }

        public bool UseSpecialRights { get; set; }

        public bool OwnerHasFullRights { get; set; }

    }

    public class TenderConsultantActionData
    {
        public int RecordId { get; set; }
        public bool IsSelected { get; set; }
        //public int DesignId { get; set; }
        public int AddresseId { get; set; }
        public string Component { get; set; }
        public string Flag { get; set; }
        public string Comments { get; set; }
    }

    public class TenderUpdateActionData
    {
        public TenderActionData TenderActionData { get; set; }
        public List<TenderConsultantActionData> TenderConsultantList { get; set; }
    }

    public class TenderContractActionData
    {
        public int UserReqeustId { get; set; }
        public int TenderRecordId { get; set; }
        public string Subject { get; set; }
        public string NewCodeToContract { get; set; }
        public string CreatedBy { get; set; }
        /// <summary>
        /// If you Need to Create Tender -True else False
        /// </summary>
        public bool IsNeedToCreateTender { get; set; }
        public string NewDesignContractCode { get; set; }
    }
}
