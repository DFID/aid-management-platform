using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class WorkflowsVM
    {
        public ProjectHeaderVM ProjectHeaderVm { get; set; }
        public List<CompletedWorkflowVM> workflows { get; set; }
    }
}