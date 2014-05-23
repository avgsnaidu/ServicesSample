using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtusDataModel
{
    public class UserRequest
    {
        public UserRequest() { }

        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }
        private string _Code;


        public int RequestType
        {
            get { return _RequestType; }
            set { _RequestType = value; }
        }
        private int _RequestType;


        public string Subject
        {
            get { return _Subject; }
            set { _Subject = value; }
        }
        private string _Subject;


        public int PriorityId
        {
            get { return _PriorityId; }
            set { _PriorityId = value; }
        }
        private int _PriorityId;


        public decimal InitialBudget
        {
            get { return _InitialBudget; }
            set { _InitialBudget = value; }
        }
        private decimal _InitialBudget;


        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        private string _Description;


        public bool IsDone
        {
            get { return _IsDone; }
            set { _IsDone = value; }
        }
        private bool _IsDone;


        public System.DateTime? DoneDate
        {
            get { return _DoneDate; }
            set { _DoneDate = (DateTime?)value; }
        }
        private System.DateTime? _DoneDate;


        public bool IsDesignRequired
        {
            get { return _IsDesignRequired; }
            set { _IsDesignRequired = value; }
        }
        private bool _IsDesignRequired;


        public bool IsTenderRequired
        {
            get { return _IsTenderRequired; }
            set { _IsTenderRequired = value; }
        }
        private bool _IsTenderRequired;


        public bool IsContractRequired
        {
            get { return _IsContractRequired; }
            set { _IsContractRequired = value; }
        }
        private bool _IsContractRequired;


        public string CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        private string _CreatedBy;


        public System.DateTime? CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = (DateTime?)value; }
        }
        private System.DateTime? _CreatedOn;


        public string ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }
        private string _ModifiedBy;


        public System.DateTime? ModifiedOn
        {
            get { return _ModifiedOn; }
            set { _ModifiedOn = (DateTime?)value; }
        }
        private System.DateTime? _ModifiedOn;


        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private int _Status;


        public int OriginalRequestId
        {
            get { return _OriginalRequest; }
            set { _OriginalRequest = value; }
        }
        private int _OriginalRequest;


        public string ForwardedTo
        {
            get { return _ForwardedTo; }
            set { _ForwardedTo = value; }
        }
        private string _ForwardedTo;


        public System.DateTime? ForwardedOn
        {
            get { return _ForwardedOn; }
            set { _ForwardedOn = (DateTime?)value; }
        }
        private System.DateTime? _ForwardedOn;


        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }
        private bool _IsActive;


        public int Project
        {
            get { return _Project; }
            set { _Project = value; }
        }
        private int _Project;


        public int OriginalReqProject
        {
            get { return _OriginalReqProject; }
            set { _OriginalReqProject = value; }
        }
        private int _OriginalReqProject;


        public bool IsRead
        {
            get { return _IsRead; }
            set { _IsRead = value; }
        }
        private bool _IsRead;


        public int DepartmentId
        {
            get { return _DepartmentId; }
            set { _DepartmentId = value; }
        }
        private int _DepartmentId;


        public int UserRequestId_Old
        {
            get { return _UserRequestId_Old; }
            set { _UserRequestId_Old = value; }
        }
        private int _UserRequestId_Old;


        ////RecepientId,IsRead,IsDone,Department,OrgSubject,OriginalSatuas,ToUser--
        /// <summary>
        /// User defined Contructor
        /// <summary>
        //public UserRequests(string Code,
        //    int RequestType,
        //    string Subject,
        //    int PriorityId,
        //    decimal InitialBudget,
        //    string Description,
        //    bool IsDone,
        //    System.DateTime DoneDate,
        //    bool IsDesignRequired,
        //    bool IsTenderRequired,
        //    bool IsContractRequired,
        //    string CreatedBy,
        //    System.DateTime CreatedOn,
        //    string ModifiedBy,
        //    System.DateTime ModifiedOn,
        //    int Status,
        //    int OriginalRequest,
        //    string ForwardedTo,
        //    System.DateTime ForwardedOn,
        //    bool IsActive,
        //    int Project,
        //    int OriginalReqProject,
        //    bool IsRead,
        //    int DepartmentId,
        //    int UserRequestId_Old)
        //{
        //    this._Code = Code;                                                         
        //    this._RequestType = RequestType;
        //    this._Subject = Subject;
        //    this._PriorityId = PriorityId;
        //    this._InitialBudget = InitialBudget;
        //    this._Description = Description;
        //    this._IsDone = IsDone;
        //    this._DoneDate = DoneDate;
        //    this._IsDesignRequired = IsDesignRequired;
        //    this._IsTenderRequired = IsTenderRequired;
        //    this._IsContractRequired = IsContractRequired;
        //    this._CreatedBy = CreatedBy;
        //    this._CreatedOn = CreatedOn;
        //    this._ModifiedBy = ModifiedBy;
        //    this._ModifiedOn = ModifiedOn;
        //    this._Status = Status;
        //    this._OriginalRequest = OriginalRequest;
        //    this._ForwardedTo = ForwardedTo;
        //    this._ForwardedOn = ForwardedOn;
        //    this._IsActive = IsActive;
        //    this._Project = Project;
        //    this._OriginalReqProject = OriginalReqProject;
        //    this._IsRead = IsRead;
        //    this._DepartmentId = DepartmentId;
        //    this._UserRequestId_Old = UserRequestId_Old;
        //}
    }

    public class UserRequestActionData1 : UserRequest
    {

        public string ToUserids { get; set; }
        public string OriginalSubject { get; set; }
        public bool DeadlineRemind { get; set; }
        public int DeadlineReminderDays { get; set; }
        public DateTime? DeadlineEndDate { get; set; }
        public bool AlreadyApproved { get; set; }
        public int ToUserId { get; set; }
        public string ToCCUsers { get; set; }
        public string RequestComponents { get; set; }
        public string Comments { get; set; }
        public int OriginalStatus { get; set; }
        //public bool IsArchived { get; set; }
        //public bool IsManualCode { get; set; }
        public DateTime? RequestDate { get; set; }
        //public int OriginalRequest { get; set; }
        public string DesignContractCode { get; set; }
        public bool IsDesign { get; set; }
        public bool CreateNextStep { get; set; }
        public DateTime? HoldUntilDate { get; set; }
        public int HoldRemindDays { get; set; }
        public bool IsKeptHold { get; set; }
        public int OldReminderMonths { get; set; }
        public bool IsHoldRemind { get; set; }
        public bool IsSaveDraft = false;

    }

    public class NewUserRequestActionData
    {
        public string Code
        {
            get;
            set;
        }
        public int RequestType
        {
            get;
            set;
        }

        public string Subject
        {
            get;
            set;
        }

        public decimal InitialBudget
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string CreatedBy
        {
            get;
            set;
        }

        public System.DateTime? CreatedOn
        {
            get;
            set;
        }

        public string ModifiedBy
        {
            get;
            set;
        }

        public System.DateTime? ModifiedOn
        {
            get;
            set;
        }

        public int Status
        {
            get;
            set;
        }

        public bool IsActive
        {
            get;
            set;
        }
        public int DepartmentId
        {
            get;
            set;
        }



        public string ToUserids { get; set; }

        public int ToUserId { get; set; }
        public string ToCCUsers { get; set; }

        public string Comments { get; set; }

        public DateTime? RequestDate { get; set; }
        public bool IsSaveDraft { get; set; }



    }


    public class EditUserRequestActionData : NewUserRequestActionData
    {
        public EditUserRequestActionData() { }
        public EditUserRequestActionData(NewUserRequestActionData data)
        {
            base.Code = data.Code;
            base.Comments = data.Comments;
            base.CreatedBy = data.CreatedBy;
            base.CreatedOn = data.CreatedOn;
            base.DepartmentId = data.DepartmentId;
            base.Description = data.Description;
            base.InitialBudget = data.InitialBudget;
            base.IsActive = data.IsActive;
            //base.IsRead = data.IsRead;
            base.ModifiedBy = data.ModifiedBy;
            base.ModifiedOn = data.ModifiedOn;
            base.IsSaveDraft = data.IsSaveDraft;
            //base.OriginalRequestId = data.OriginalRequestId;
            base.RequestDate = data.RequestDate;
            base.RequestType = data.RequestType;
            base.Status = data.Status;
            base.Subject = data.Subject;
            base.ToCCUsers = data.ToCCUsers;
            base.ToUserId = data.ToUserId;
            base.ToUserids = data.ToUserids;
        }

        public string RequestComponents { get; set; }

        public string OriginalSubject { get; set; }
        public bool DeadlineRemind { get; set; }
        public int DeadlineReminderDays { get; set; }
        public DateTime? DeadlineEndDate { get; set; }
        public bool AlreadyApproved { get; set; }

        public int OriginalStatus { get; set; }
        public bool IsArchived { get; set; }
        public bool IsManualCode { get; set; }

        public int OriginalRequest { get; set; }
        public string DesignContractCode { get; set; }
        public bool IsDesign { get; set; }
        public bool CreateNextStep { get; set; }
        public DateTime? HoldUntilDate { get; set; }
        public int HoldRemindDays { get; set; }
        public bool IsKeptHold { get; set; }
        public int OldReminderMonths { get; set; }
        public bool IsHoldRemind { get; set; }
        public bool IsDone
        {
            get;
            set;
        }
        public System.DateTime? DoneDate
        {
            get;
            set;
        }
        public bool IsDesignRequired
        {
            get;
            set;
        }

        public bool IsTenderRequired
        {
            get;
            set;
        }

        public bool IsContractRequired
        {
            get;
            set;
        }

        public string ForwardedTo
        {
            get;
            set;
        }

        public System.DateTime? ForwardedOn
        {
            get;
            set;
        }
        public int Project
        {
            get;
            set;
        }
        public int OriginalReqProject
        {
            get;
            set;
        }

        public int UserRequestId_Old
        {
            get;
            set;
        }
        public bool IsRead
        {
            get;
            set;
        }

        public int PriorityId
        {
            get;
            set;
        }
        public int OriginalRequestId
        {
            get;
            set;
        }



    }








    public class UserRequestReceived : UserRequest
    {

        public int RecipientId { get; set; }
        public string RecipientName { get; set; }
        public bool IsRequestRead { get; set; }
        public bool IsRequestDone { get; set; }
        public int Department { get; set; }
        public string OriginalSubject { get; set; }
        public int OriginalStatus { get; set; }
        public string ToUserIDs { get; set; }
        public DateTime? DeadLineEndDate { get; set; }
        public string ToCCUsers { get; set; }
        public string RequestComponents { get; set; }

    }

    public class SentRequest
    {
        public int UserRequestId { get; set; }
        public string Icon { get; set; }
        public int PriorityId { get; set; }
        public int PriorityName { get; set; }
        public string Code { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public DateTime? RequestDate { get; set; }
        public string CurrentStatus { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string RecepientName { get; set; }
        public DateTime? DeadLineDate { get; set; }
        public string RequestComponents { get; set; }
        public decimal Budget { get; set; }
        public string IsDone { get; set; }
        public string ToUserIDs { get; set; }
        public string ToOUs { get; set; }
        public string RequestComponentIds { get; set; }
        public string IconValue { get; set; }
        public int OriginalRequestId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int Status { get; set; }

    }
    public class MyUserRequest : SentRequest
    {
        public int ObjectTypeID { get; set; }
        public string OriginalRequest { get; set; }
        public string Sender { get; set; }
        public string Department { get; set; }
        public int IsRead { get; set; }
        public bool IsCCUser { get; set; }

    }

    public class RequestComponent
    {
        //public int RequestComponentId { get; set; }
        //public string RequestComponentName { get; set; }
        //public int DepartmentId { get; set; }

        public string RequestComponentIds { get; set; }
        public string RequestComponentNames { get; set; }

    }

    public class NextObjectRecordDetails
    {
        public int RecordId { get; set; }
        public int NextObjectEnumId { get; set; }
    }

    public class UserRequest_ToUsersParams
    {
        public string LoginName { get; set; }
        public int UserRequestId { get; set; }
        public string SearchCondition { get; set; }
        public int NoOfRecords { get; set; }
        public string OrderBy { get; set; }
        public string SortOrder { get; set; }
        public int TotalCount { get; set; }
        public bool IsForward { get; set; }

    }





}
