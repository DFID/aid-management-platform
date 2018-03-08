using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace AMP.WorkflowClasses
{
    public enum WorkflowType
    {
        [Description("Cancel Workflow")] Cancel,
        [Description("Close Project")] CloseProject,
        [Description("Re-Approve Project")] ReApproveProject,
        [Description("Annual Review")] AnnualReview,
        [Description("Project Completion Review")] PCR,
        [Description("Review Deferral")] ReviewDeferral,
        [Description("Review Exemption")] ReviewExemption,
        [Description("Appraisal and Design")] AandD,
        [Description("Admin or Rapid Response")] FastTrack,
        [Description("Archive Project")] Archive,
        [Description("Re-Open Project")] ReOpen,
        [Description("Approve Project")] ApproveProject,
        [Description("Planned End Date")] PlannedEndDate,
    }

}