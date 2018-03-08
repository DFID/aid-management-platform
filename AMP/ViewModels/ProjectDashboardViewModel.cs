using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMP.Models;


namespace AMP.ViewModels
{

    public partial class ProjectDashboardViewModel
    {
        public int Id { get; set; }
        public string ProjectID { get; set; }
        public string ProjectDescription { get; set; }
        public string RiskAtApproval { get; set; }
        public string Stage { get; set; }
        public decimal ApprovedBudget { get; set; }

        //List of Project Alerts to be returned to the dashboard.
        public IEnumerable<ProjectAlert> ProjectAlerts { get; set; }

        //public virtual "Model" allows access to foreign key values
        public virtual Stage Stage1 { get; set; }
    }
}