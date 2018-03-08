using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.Utilities;

namespace AMP.WorkflowClasses
{
    public class RequestBuilder:WorkflowBuilderBase
    {

        public RequestBuilder(WorkflowMaster workflowMaster, IAMPRepository ampRepository, string userId)
        {
            _ampRepository = ampRepository;
            _userId = userId;
            _workflow = workflowMaster;
        }

        public void BuildWorkflowMaster()
        {
            SetStageAwaitingApproval();
            SetRequestStepId();
            SetActionBy();
            SetActionDate();
            SetStatusActive();
            SetAuditData();
            SetWorkflowId();
        }


    }
}