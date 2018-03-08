using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.Utilities;
using MoreLinq;

namespace AMP.WorkflowClasses
{
    public class ExistingWorkflows
    {
        private IEnumerable<WorkflowMaster> _workflows;

        public ExistingWorkflows(IEnumerable<WorkflowMaster> existingWorkflows)
        {
            _workflows = existingWorkflows;
        }

        private WorkflowMaster rejectedWorkflow
        {
            get
            {
                    return _workflows.MaxBy(x => x.ActionDate);
            }
        }

        public bool HasWorkflows()
        {
            return _workflows.Any();
        }

        public bool HasActiveWorkflow()
        {
            if (_workflows != null && _workflows.Any(x => x.Status == "A"))
            {
                return true;
            }
            return false;
        }

        public bool LastWorkflowWasRejected()
        {
            if (_workflows != null && _workflows.Any() && (_workflows.MaxBy(x => x.ActionDate).StageID == (int)WorkflowStage.Rejected))
            {
                return true;
            }
            return false;

        }

        public WorkflowMaster ActiveWorkflowRequest()
        {
            return _workflows.FirstOrDefault(x => x.Status == "A");
        }

        public WorkflowMaster LastRejectedWorkflowResponse()
        {
            return _workflows.FirstOrDefault(x => x.WorkFlowID == rejectedWorkflow.WorkFlowID && x.StageID == (int)WorkflowStage.Rejected);
        }


        public WorkflowMaster LastRejectedWorkflowRequest()
        {
            return _workflows.FirstOrDefault(x => x.WorkFlowID == rejectedWorkflow.WorkFlowID && x.StageID == (int)WorkflowStage.InProgress);
        }

        public WorkflowMaster WorkflowRequest(Int32 workflowId)
        {
            return _workflows.FirstOrDefault(x => x.WorkFlowID == workflowId && x.WorkFlowStepID == 0);
        }

        public WorkflowMaster WorkflowResponse(Int32 workflowId)
        {
            return _workflows.FirstOrDefault(x => x.WorkFlowID == workflowId && x.WorkFlowStepID == 1);
        }

        public IEnumerable<WorkflowMaster> CompletedWorkflows()
        {
            return _workflows.Where(x => x.Status == "C");
        }

        public IEnumerable<WorkflowMaster> CompletedRequests()
        {
            return _workflows.Where(x => x.StageID == 1 && x.Status == "C").OrderByDescending(x => x.ActionDate);
        }

        public IEnumerable<WorkflowMaster> CompletedResponses()
        {
            return _workflows.Where(x => x.Status == "C" && ((x.StageID == 2 || x.StageID == 3)));
        }

    }
}