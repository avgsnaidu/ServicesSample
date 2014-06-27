using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtusMobileAPI.Models
{
     class PerformanceModel
    {

    }

    public class PerformanceActionData
    {
        public List<ServicesActionData> ServicesCollection { get; set; }
        public List<ExpancesActionData> ExpancesCollection { get; set; }
    }

    public class ServicesActionData
    {
        /// <summary>
        /// recordId of the Services record for new record-0
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// For new record it must be 'N' otherwise 'M' for deletion 'D'
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        /// To do any actions ie. insertion, updation, deletion RecType is 1 else 0
        /// </summary>
        public string RecType { get; set; }

        public int ServiceTaskId { get; set; }
        public int ServiceProjectId { get; set; }
        /// <summary>
        /// service done on the date
        /// </summary>
        public DateTime? ServiceDate { get; set; }
        /// <summary>
        /// services performed by userId in not userName
        /// </summary>
        public string PerformedBy { get; set; }
        public int TaskServiceTypeId { get; set; }
        /// <summary>
        /// Start time,Time format should be from 01:00 to 24:00
        /// </summary>
        public string TimeFrom { get; set; }
        /// <summary>
        /// End tiem,Time format should be from 01:00 to 24:00
        /// </summary>
        public string TimeUntil { get; set; }
        /// <summary>
        /// total time served, i.e differences between from and to time ,Time format should be from 01:00 to 24:00
        /// </summary>
        public string Effort { get; set; }

        public string ExternalEffort { get; set; }
        /// <summary>
        /// hourly rate for the selected user 
        /// </summary>
        public decimal HourlyRate { get; set; }

        public decimal BillingAmount { get; set; }
        public decimal RoundingDifference { get; set; }
        public string Comment { get; set; }

    }

    public class ExpancesActionData
    {
        /// <summary>
        /// recordId of the Expances record for new record-0
        /// </summary>
        public int RecordId { get; set; }
        /// <summary>
        /// For new record it must be 'N' otherwise 'M' for deletion 'D'
        /// </summary>
        public string Flag { get; set; }
        /// <summary>
        /// To do any actions ie. insertion, updation, deletion RecType is 1 else 0
        /// </summary>
        public string RecType { get; set; }
        public int ProductTaskId { get; set; }
        public int ProductProjectId { get; set; }
        /// <summary>
        /// Expances done on the date
        /// </summary>
        public DateTime? ProductDate { get; set; }
        /// <summary>
        /// services performed by userId in not userName
        /// </summary>
        public string PerformedBy { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public string ProductRate { get; set; }
        public string VATPercent { get; set; }
        public bool IsVATIncluded { get; set; }
        public bool IsBillable { get; set; }
        public bool IsApproved { get; set; }
        public bool IsPayback { get; set; }
        public decimal PriceInternal { get; set; }
        public decimal AmountInternal { get; set; }
        public decimal AmountExternal { get; set; }
        public decimal AmountExternalHidden { get; set; }
        public decimal RoundingDifference { get; set; }
        public string Comment { get; set; }


    }


}