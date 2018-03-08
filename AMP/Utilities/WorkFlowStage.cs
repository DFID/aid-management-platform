using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Utilities
{
    public class WorkFlowStage
    {
        // Summary:
        //     Specifies the stage of a workflow
        public enum ReviewApproval
        {
            // Summary:
            //     The email has normal priority.
            IN_PREPARATION = 0,
            //
            // Summary:
            //     The email has low priority.
            AWAITING_APPROVAL = 1,
            //
            // Summary:
            //     The email has high priority.
            APPROVED = 2,
            //
            // Summary:
            //     The email has high priority.
            REJECTED = 3,
        }
        public enum DefferalApproval
        {

        }
        public enum ExemptApproval
        {
           
        }
       
    }
}