using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;

namespace AMP.WorkflowClasses
{
    public class ResponseBuilder:WorkflowBuilderBase
    {
        public ResponseBuilder(WorkflowMaster workflowMaster, IAMPRepository ampRepository, string userId)
        {
            _ampRepository = ampRepository;
            _userId = userId;
            _workflow = workflowMaster;
        }

        public void BuildApprovalResponse()
        {
            SetStageApproved();
            SetResponseStepId();
            SetActionBy();
            SetActionDate();
            SetStatusComplete();
            SetAuditData();
        }
    }
}