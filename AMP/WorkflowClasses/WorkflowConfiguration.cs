using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Antlr.Runtime.Misc;

namespace AMP.WorkflowClasses
{
    public class WorkflowConfiguration
    {
        public bool RequiresDelegatedAuthorityApproval(Int32 taskId)
        {
            switch (taskId)
            {
                case (Int32) WorkflowType.ReApproveProject:
                    return true;


                case (Int32)WorkflowType.ApproveProject:
                    return true;

                case (Int32)WorkflowType.CloseProject:
                    return false;

                case (Int32)WorkflowType.AandD:
                    return true;

                case (Int32)WorkflowType.FastTrack:
                    return true;

                case (Int32)WorkflowType.ReOpen:
                    return false;

                case (Int32)WorkflowType.Archive:
                    return false;

                default:
                    return false;
            }

        }

        public bool DocumentRequired(Int32 taskId, string projectStage)
        {
            switch (taskId)
            {
                //case (Int32)WorkflowType.ReApproveProject:
                //    if (projectStage == "3")
                //    {
                //        return true;
                //    }
                //    return false;
                case (Int32)WorkflowType.ApproveProject:
                    return true;

                case (Int32)WorkflowType.PlannedEndDate:
                    return true;

                case (Int32)WorkflowType.ReApproveProject:
                    return false;

                case (Int32)WorkflowType.CloseProject:
                    return false;

                case (Int32)WorkflowType.AandD:
                    return true;

                case (Int32)WorkflowType.FastTrack:
                    return true;

                case (Int32)WorkflowType.ReOpen:
                    return false;

                case (Int32)WorkflowType.Archive:
                    return true;
                default:
                    return false;
            }
        }

    }
}