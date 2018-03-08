using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using System.ComponentModel.DataAnnotations;


namespace AMP.ViewModels
{
    public class StatementRecordVM
    {
        public string ProjectID { get; set; }
        public int StatementID { get; set; }
           [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> DueDate { get; set; }
           [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> PromptDate { get; set; }
           [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> ReceivedDate { get; set; }
           [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> PeriodFrom { get; set; }
           [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> PeriodTo { get; set; }

                  
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:N}")]
        public Nullable<decimal> Value { get; set; }
        public string Currency { get; set; }
        public string StatementType { get; set; }
        public string reason_action { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }

        public string DocumentID { get; set; }

        public string DocSource { get; set; }

        public string DocumentLink { get; set; }

    }
}