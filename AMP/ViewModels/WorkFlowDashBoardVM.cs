using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;


namespace AMP.ViewModels
{
    public class WorkFlowDashBoardVM
    {
        public IEnumerable<WorkflowMasterVM> GetAllWorkFlows { get; set; }
    }
}