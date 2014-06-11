using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtusDataModel
{
    public class ContractActionData
    {
        public int ProjectMangaer { get; set; }
        public int DeadlineRemindDays { get; set; }
        public int ApprovingAuthority { get; set; }
        public decimal ApprovedBudget { get; set; }
        public int Status { get; set; }
        public int RecordID { get; set; }
        public int UserRequestId { get; set; }
        public int DesignId { get; set; }
        public int Project { get; set; }

        public int DeadLineEndDate { get; set; }
        public int DoneDate { get; set; }


        public String ProjectCode { get; set; }
        public String VendorSubject { get; set; }
        public String Remarks { get; set; }
        public String CreatedBy { get; set; }
        public String ModifiedBy { get; set; }
        public String Subject { get; set; }

        public bool IsManualCode { get; set; }
        public bool IsDone { get; set; }
        public bool UseSplRights { get; set; }
        public bool OwnerHasFullRights { get; set; }
        public bool DeadlineRemind { get; set; }
        public bool StumbleExists { get; set; }
        public bool CreateProject { get; set; }
        public bool IsProjectManagerChanged { get; set; }
        public bool IsToDateChanged { get; set; }


    }

    public class MileStoneActionData
    {

        public int ID { get; set; }
        public int VendorId { get; set; }
        public decimal Contribution { get; set; }
        public decimal AllotedBudget { get; set; }
        public int RemindBefore { get; set; }
        public int Project { get; set; }
        public int FinbaseProjectId { get; set; }
        public int FinbaseMileStoneId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public bool Remind { get; set; }
        public bool Falg { get; set; }
        public bool IsSentToFinBase { get; set; }
        public bool CanDelete { get; set; }
        public bool HasInvoice { get; set; }




    }

    public class ContractVendorsData
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        //public int CurrencyId { get; set; }
        //public int PenaltyType { get; set; }
        public int Project { get; set; }
        //public int FinbaseProjectId { get; set; }
        //public int Type { get; set; }

        public decimal Budget { get; set; }
        //public decimal ExchangeRate { get; set; }
        //public decimal PenaltyAmount { get; set; }
        //public decimal PenaltyMaxAmount { get; set; }
        //public decimal SuretyAmount { get; set; }
        public decimal PrevBudget { get; set; }

        //public DateTime SuretyDate { get; set; }
        public DateTime VendorMaxDate { get; set; }

        //public string Icon { get; set; }
        public string VendorName { get; set; }
        public string Component { get; set; }
        //public string SuretyRemarks { get; set; }
        //public string SuretyBankName { get; set; }
        //public string Gender { get; set; }
        //public string Code { get; set; }

        //public bool IsPenalty { get; set; }
        //public bool IsActive { get; set; }
        public bool Flag { get; set; }
        public bool IsBudgetChanged { get; set; }
        public bool IsStumbled { get; set; }
        //public bool IsInternal { get; set; }
        //public bool IsDone { get; set; }
    }

    public class ContractSaveActionData
    {
        public ContractActionData ContractActionData { get; set; }
        public List<MileStoneActionData> MileStoneActionData { get; set; }
        public List<ContractVendorsData> ContractVendorsData { get; set; }

    }


}
