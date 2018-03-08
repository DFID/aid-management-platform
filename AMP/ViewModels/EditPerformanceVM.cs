using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AMP.Models;

namespace AMP.ViewModels
{
    public class EditPerformanceVM
    {
        public string ProjectID { get; set; }
        public string ARRequired { get; set; }
        public string ARExemptJustification { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> ARDueDate { get; set; }
        public int ARDueDate_Day { get; set; }
        public int ARDueDate_Month { get; set; }
        public int ARDueDate_Year { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> ARPromptDate { get; set; }
        public int ARPromptDate_Day { get; set; }
        public int ARPromptDate_Month { get; set; }
        public int ARPromptDate_Year { get; set; }

        public string ARDefferal { get; set; }
        public string PCRRequired { get; set; }
        public string PCRExemptJustification { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> PCRDueDate { get; set; }
        public int PCRDueDate_Day { get; set; }
        public int PCRDueDate_Month { get; set; }
        public int PCRDueDate_Year { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> PCRPrompt { get; set; }
        public int PCRPrompt_Day { get; set; }
        public int PCRPrompt_Month { get; set; }
        public int PCRPrompt_Year { get; set; }
        public string PCRDefferal { get; set; }
        public string PCRDefferalJustification { get; set; }
        public string PCRAuthorised { get; set; }
        public string ARExcemptReason { get; set; }
        public string PCRExcemptReason { get; set; }
        public Nullable<int> DefferalTimeScale { get; set; }
        public string DeferralReason { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }

        public List<ExemptionReason> ExemptionReasons { get; set; }
        public ReviewExemptionVM ReviewExemptionAR { get; set; }
        public ReviewExemptionVM ReviewExemptionPCR { get; set; }

        public string HasAR { get; set; }
        public string HasPCR { get; set; }

    }
}