using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMP.Models;
using System.ComponentModel.DataAnnotations;


namespace AMP.ViewModels
{

    public partial class UserProjectsViewModel
    {
        public int Id { get; set; }
        public string ProjectID { get; set; }
        public string Title { get; set; }

        public string TitleShort { get; set; }
        public string RiskAtApproval { get; set; }
        public string Stage { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> NextReview { get; set; }

        public Boolean Portfolio { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public decimal ApprovedBudget { get; set; }

        //List of Project Alerts to be returned to the dashboard.
        public IEnumerable<ProjectAlert> ProjectAlerts { get; set; }

        //public virtual "Model" allows access to foreign key values
        public virtual Stage Stage1 { get; set; }

    }
}