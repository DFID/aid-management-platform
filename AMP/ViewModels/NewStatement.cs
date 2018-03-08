using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace AMP.ViewModels
{
    public class NewStatement
    {
        public string ProjectID { get; set; }
        public int StatementID { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
      

        public Nullable<System.DateTime> PromptDate { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }

        public Nullable<int> ReceivedDate_Day { get; set; }
        public Nullable<int> ReceivedDate_Month { get; set; }
        public Nullable<int> ReceivedDate_Year { get; set; }

        public Nullable<System.DateTime> PeriodFrom { get; set; }

        public Nullable<int> PeriodFrom_Day { get; set; }
        public Nullable<int> PeriodFrom_Month { get; set; }
        public Nullable<int> PeriodFrom_Year { get; set; }
        public Nullable<System.DateTime> PeriodTo { get; set; }

        public Nullable<int> PeriodTo_Day { get; set; }
        public Nullable<int> PeriodTo_Month { get; set; }
        public Nullable<int> PeriodTo_Year { get; set; }
        public Nullable<decimal> Value { get; set; }
        public string Currency { get; set; }
        public string StatementType { get; set; }
        public string reason_action { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }
        public string DocumentID { get; set; }
    }
}