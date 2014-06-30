using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtusMobileAPI.Models
{
     

    public class AppointmentActionData
    {

        public int PriorityId { get; set; }
        public string UserName { get; set; }
        public string ToPersons { get; set; }
        public string ToOrganizations { get; set; }
        public string Subject { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentFrom { get; set; }
        public DateTime AppointmentUntil { get; set; }
        public string Location { get; set; }
        public string Message { get; set; }
        public bool CannotModify { get; set; }
        public bool PrivateAppointment { get; set; }
        //public int AppointmentId { get; set; }
        public bool IsNeedReminder { get; set; }
        public int ReminderDuration { get; set; }
        public bool IsDone { get; set; }
        public bool IsReOccur { get; set; }
        public int OccurrenceSeriesId { get; set; }
        public bool IsOpenSeries { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsAllDay { get; set; }
        public string EntryId { get; set; }
        public DateTime? LastModificationTime { get; set; }


    }
}