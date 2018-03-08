using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using AMP.Models;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class ProjectVM
    {
        public string ProjectID { get; set; }
        
        //[Required(ErrorMessage = "You must enter a project title")]
        //[MinLength(20, ErrorMessage = "Your project title must contain at least 20 characters")]
        public string Title { get; set; }

        //[Required(ErrorMessage = "You must enter a project description")]
        //[MinLength(40, ErrorMessage = "Your project description must contain at least 40 characters")]
        public string Description { get; set; }

        //[Required(ErrorMessage = "You must enter a budget centre")]
        public string BudgetCentreID { get; set; }
        public string Stage { get; set; }
        public string BudgetCentreDescription { get; set; }
        public string StageDescription { get; set; }

        public string OpStatus { get; set; }
        public string RiskAtApproval { get; set; }
        public ProjectDateVM ProjectDates { get; set; }
        public ProjectTeamMemberVM ProjectSRO { get; set; }
        public ProjectHeaderVM ProjectHeader { get; set; }
        public WorkflowMasterVM CloseProjectRequest { get; set; }
        public WorkflowMasterVM CloseProjectResponse { get; set; }
        public string CloseProjectRole { get; set; }
        public WorkflowMasterVM ApproveProjectRequest { get; set; }
        public WorkflowMasterVM ApproveProjectResponse { get; set; }
        public string ApproveProjectRole { get; set; }
        public bool CanSendForClosure { get; set; }
        public bool CanSendForApproval { get; set; }
        public ProjectWFCheckVM WFCheck { get; set; }
        public IEnumerable<Risk> RiskLookups { get; set; }
        public bool IsProjectTeam { get; set; }

    }
}