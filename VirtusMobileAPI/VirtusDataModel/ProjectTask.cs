using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtusDataModel
{
    public class ProjectTaskActionData
    {
        public int DoneDate { get; set; }
        public int SpecialProjectType { get; set; }
        public int Priority { get; set; }
        public int Customer { get; set; }
        public int ProjectKind { get; set; }
        public int TaskKind { get; set; }
        public int ProjectStatus { get; set; }
        public int IsPartOf { get; set; }
        public int InboxDate { get; set; }
        public int InitiationSource { get; set; }
        public int OnBehalfOf { get; set; }
        public int DeadlineRemindDays { get; set; }
        public int ProjectMangaer { get; set; }
        public int Milestone { get; set; }
        public int Contribution { get; set; }

        public decimal Budget { get; set; }

        public string ProjectTaskCode { get; set; }
        public string Subject { get; set; }
        public string Remarks { get; set; }
        public string InboxRemarks { get; set; }
        public string RemarksDetail { get; set; }
        public string ProjectIcon { get; set; }
        public string Keywords { get; set; }
        public string CreatedBy { get; set; }
        public string TaskCode { get; set; }
        public string ModifiedBy { get; set; }


        public bool IsProject { get; set; }
        public bool IsDone { get; set; }
        public bool IsInternalProject { get; set; }
        public bool IsSpecialProject { get; set; }
        public bool IsDeadlineRemind { get; set; }
        public bool IsActive { get; set; }
        public bool IsManualCode { get; set; }
        public bool IsUseSplRights { get; set; }
        public bool IsOwnerHasFullRights { get; set; }
        public bool IsAlternateContractor { get; set; }
        public bool IsParentGroup { get; set; }


    }

    //public class ProjectTaskCountActionData
    //{
    //    //public EPMEnums.Enums.VTSObjects ObjectType { get; set; }
    //    public int RecordId { get; set; }
    //    public string LoginName { get; set; }
    //    public bool IsArchived { get; set; }
    //}

    public class ParentObjectCustomerAndKind
    {
        public int UserRequestId { get; set; }
        public int KindId { get; set; }
        public int CustomerId { get; set; }
        public int MileStoneId { get; set; }
        public string CustomerName { get; set; }
    }


    public class DeadLineExtensionActionData
    {

        public int ID { get; set; }
        public int TaskId { get; set; }
        public int Duration { get; set; }

        public string CreatedBy { get; set; }
        public string Reason { get; set; }
        public string Flag { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTill { get; set; }
        public DateTime CreatedOn { get; set; }
    }



}
