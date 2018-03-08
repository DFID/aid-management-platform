using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class WorkFlowTaskViewModel
    {
        public int TaskID { get; set; }

        public int WorkflowTypeCode { get; set; }

        public int WorkflowIdentifier { get; set; }

        public int WorkflowStatus { get; set; }

        public int WorkflowStep { get; set; }

        public string WorkflowComments { get; set; }

        public string ProjectID { get; set; }

        public string ActionedBy { get; set; }

        public Nullable<System.DateTime> ActionedDate { get; set; }

        public string ReceivedBy { get; set; }

        public Nullable<System.DateTime> ReceivedDate { get; set; }

        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
        public string UserID { get; set; }

 




    }
}