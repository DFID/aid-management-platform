using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.Utilities;

namespace AMP.WorkflowClasses
{
    public class WorkflowBuilderBase
    {
        protected IAMPRepository _ampRepository;
        protected string _userId;
        protected WorkflowMaster _workflow;

        public WorkflowMaster workflow
        {
            get
            {
                return _workflow;
            }

            set
            {
                _workflow = value;
            }
        }

        protected void SetWorkflowId()
        {
            _workflow.WorkFlowID = _ampRepository.NextWorkFlowId();
        }

        protected void SetStageAwaitingApproval()
        {
            _workflow.StageID = Constants.WorkflowStageAwaitingApproval;
        }

        protected void SetStageApproved()
        {
            _workflow.StageID = Constants.WorkflowStageApproved;
        }

        protected void SetStageRejected()
        {
            _workflow.StageID = Constants.WorkflowStageRejected;
        }

        protected void SetRequestStepId()
        {
            _workflow.WorkFlowStepID = 0;
        }

        protected void SetResponseStepId()
        {
            _workflow.WorkFlowStepID = 1;
        }


        protected void SetActionBy()
        {
            _workflow.ActionBy = _userId;
        }

        protected void SetActionDate()
        {
            _workflow.ActionDate = DateTime.Now;
        }

        protected void SetStatusActive()
        {
            _workflow.Status = "A";
        }

        protected void SetStatusComplete()
        {
            _workflow.Status = "C";
        }


        protected void SetAuditData()
        {
            _workflow.LastUpdate = DateTime.Now;
            _workflow.UserID = _userId;

        }
    }
}