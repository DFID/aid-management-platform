using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class ComponentDateVM
    {
        public Nullable<System.DateTime> StartDate { get; set; }

        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> OperationalStartDate  { get; set; }

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
        public Nullable<System.DateTime> LastUpdate { get; set; }

        public string UserID { get; set; }
    }
}