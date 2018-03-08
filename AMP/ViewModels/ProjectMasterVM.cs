using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using AMP.Models;

namespace AMP.ViewModels
{
    public class ProjectMasterVM
    {
        public string ProjectID { get; set; }
        //[Required(ErrorMessage = "You must enter a project title")]
        //[MinLength(20, ErrorMessage = "Your project title must contain at least 20 characters")]
        public string Title { get; set; }
        
        //[Required(ErrorMessage = "You must enter a project description")]
        //[MinLength(40, ErrorMessage = "Your project description must contain at least 40 characters")]
        public string Description { get; set; }
        public string BudgetCentreID { get; set; }
        public string Stage { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }

    }
}