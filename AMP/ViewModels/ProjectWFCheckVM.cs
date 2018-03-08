using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class ProjectWFCheckVM
    {
        public bool Status { get; set; }
        public Int32 TaskId { get; set; }
        public string WorkFlowDescription { get; set; }
        public bool IsWorkFlowApprover { get; set; }
    }
}