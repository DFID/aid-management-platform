using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class RiskDocumentVM
    {
        public int RiskRegisterID { get; set; }
        public string ProjectID { get; set; }
        public string DocumentID { get; set; }
        public string Description { get; set; }
        public string DocSource { get; set; }
        public string DocumentLink { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }

    }
}