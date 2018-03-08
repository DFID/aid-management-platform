using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using AMP.ARIESModels;
using AMP.Models;

namespace AMP.ViewModels
{
    public class WorkflowVM
    {
        public ProjectHeaderVM ProjectHeaderVm { get; set; }
        public WorkflowMasterVM WorkflowRequest { get; set; }
        public WorkflowMasterVM WorkflowResponse { get; set; }

    
        public string TaskDescription { get; set; }
        public int TaskID { get; set; }
        public string UserRole { get; set; }
        public string CurrentAction { get; set; }
        public bool RequiresDelegatedAuthorityApproval { get; set; }
        public string DocumentID { get; set; }
        public string DocumentDescription { get; set; }
        public string DocSource { get; set; }
        public string DocumentLink { get; set; }
        public bool IsDocumentRequired { get; set; }
        public string WfMessage { get; set; }
        public decimal BudgetValueToBeApproved { get; set; }
       // public BudgetApprovalValue budgetApprovedByWorkflow { get; set; }
    }
}