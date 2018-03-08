using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class CompletedWorkflowVM
    {
        public int TaskID { get; set; }
        public string TaskDescription { get; set; }
        public string DocumentID { get; set; }
        public string DocumentDescription { get; set; }
        public string DocSource { get; set; }
        public string DocumentLink { get; set; }
        public WorkflowMasterVM WorkflowRequest { get; set; }
        public WorkflowMasterVM WorkflowResponse { get; set; }

    }
}