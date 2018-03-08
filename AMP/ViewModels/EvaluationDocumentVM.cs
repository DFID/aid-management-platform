using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class EvaluationDocumentVM
    {
        public int ID { get; set; }
        public int EvaluationID { get; set; }
        public string DocumentID { get; set; }
        public string Description { get; set; }

        public string DocSource { get; set; }
        public string DocumentLink { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }

    }
}