using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;

namespace AMP.WorkflowClasses
{
    public class RequestModifier:WorkflowBuilderBase
    {
        public RequestModifier(WorkflowMaster workflowMaster)
        {
            _workflow = workflowMaster;
        }

        public void Modify()
        {
            SetStatusComplete();
        }

    }
}