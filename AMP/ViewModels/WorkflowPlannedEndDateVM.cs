using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class WorkflowPlannedEndDateVM
    {

        public ProjectHeaderVM ProjectHeaderVm { get; set; }
        // current planned end dates
        public DateTime ExistingPlannedEndDate { get; set; }
        // new planned end dates 
        public Nullable<System.DateTime> NewPlannedEndDate { get; set; }
       
      
        [Required(ErrorMessage = "You must enter a day")]
        public int NewPlannedEndDate_Day { get; set; }
        [Required(ErrorMessage = "You must enter a month")]
        public int NewPlannedEndDate_Month { get; set; }
        [Required(ErrorMessage = "You must enter a year")]
        public int NewPlannedEndDate_Year { get; set; }

        //the workflow status
        public ProjectWFCheckVM WFCheck { get; set; }
    }
}






