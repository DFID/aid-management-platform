using System;

namespace AMP.Utilities
{
    public class Constants
    {
        public static bool ComponentDescriptionIsRequired {get { return true; }}
        public static Int32 ComponentDescriptionMinLength {get { return 20; }}
        public static Int32 ComponentDescriptionMaxLength { get { return 250; }}

        #region Workflow Task ID's

        public static Int32 CloseProjectTaskId { get { return 1; }}
        public static Int32 ReApproveProjectTaskId { get { return 2; } }
        public static Int32 AnnualReviewTaskId { get { return 3; } }

        public static Int32 ApproveAD { get { return 7; } }

        public static Int32 ApproveProjectTask {get { return 11; }
        }

        public static Int32 FastTrack { get { return 8; } }
        public static Int32 PCRTaskId { get { return 4; } }
        public static Int32 ReviewDeferralTaskId { get { return 5; } }
        public static Int32 reviewExemptionTaskId { get { return 6; } }
        public static Int32 ArchiveProject { get { return 9; } }
        public static  Int32 ReOpenProject { get { return 10; } }
        public static Int32 CancelWorkflow { get { return 0; } }

        public static Int32 PlannedEndDate { get { return 12; } }

        public enum WorkflowTaskId
        {
            CancelWorkflow, CloseProject, ApproveProject,AnnualReview,PCR,ReviewDefferal,ReviewExemption,ADApproval,FastTrackApproval,ArchiveProject,ReOpenProject, PlannedEndDate

        };

        #endregion

        #region Workflow Stages

        public static Int32 WorkflowStageAwaitingApproval { get { return 1; } }
        public static Int32 WorkflowStageApproved { get { return 2; } }
        public static Int32 WorkflowStageRejected { get { return 3; } }


        #endregion

        #region Project Stages

        public static string PrePipelineStage { get { return "0"; }}
        public static string AppraisalandDesignStage { get { return "3"; } }
        public static string ImplementationStage { get { return "5"; } }
        public static string CompletionStage { get { return "7"; } }
        public static string ArchiveStage { get { return "8"; } }

        #endregion

        #region Status Description

        public static string StatusActive { get { return "Active"; } }
        public static string StatusClosed { get { return "Closed"; } }

        #endregion

        #region Budget Types
        public static Int32 PROJABudget { get { return 1; } }
        public static Int32 PROJBBudget { get { return 2; } }

        #endregion
    }
}