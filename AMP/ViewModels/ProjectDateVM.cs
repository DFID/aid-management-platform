using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class ProjectDateVM
    {
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Created_date { get; set; }
        
        public int Created_Date_Day { get; set; }
        public int Created_Date_Month { get; set; }
        public int Created_Date_Year { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> FinancialStartDate { get; set; }
        public int FinancialStartDate_Day { get; set; }
        public int FinancialStartDate_Month { get; set; }
        public int FinancialStartDate_Year { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> FinancialEndDate { get; set; }

        public int FinancialEndDate_Day { get; set; }
        public int FinancialEndDate_Month { get; set; }
        public int FinancialEndDate_Year { get; set; }

        public Nullable<System.DateTime> OperationalStartDate { get; set; }

        [Required(ErrorMessage = "You must enter a day")]
        public int OperationalStartDate_Day { get; set; }
        [Required(ErrorMessage = "You must enter a month")]
        public int OperationalStartDate_Month { get; set; }
        [Required(ErrorMessage = "You must enter a year")]
        public int OperationalStartDate_Year { get; set; }


        public Nullable<System.DateTime> OperationalEndDate { get; set; }
        [Required(ErrorMessage = "You must enter a day")]
        public int OperationalEndDate_Day { get; set; }
        [Required(ErrorMessage = "You must enter a month")]
        public int OperationalEndDate_Month { get; set; }
        [Required(ErrorMessage = "You must enter a year")]
        public int OperationalEndDate_Year { get; set; }


        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode=true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> ActualStartDate { get; set; }

        //public int ActualStartDate_Day { get; set; }
        //public int ActualStartDate_Month { get; set; }
        //public int ActualStartDate_Year { get; set; }


        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> PromptCompletionDate { get; set; }

        public int PromptCompletionDate_Day { get; set; }
        public int PromptCompletionDate_Month { get; set; }
        public int PromptCompletionDate_Year { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> ActualEndDate { get; set; }

        public Nullable<System.DateTime> ESNApprovedDate { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public Nullable<System.DateTime> ArchiveDate { get; set; }

        public string UserID { get; set; }

    }
}