using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class DocumentRecordVM
    {
        public string ContentType { get; set; }
        public string ProjectID { get; set; }
        public string FileExtension { get; set; }
        public string DocumentID { get; set; }
        public string DocumentTitle { get; set; }
        public string CreatedDate { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string QuestIcon { get; set; }
        public string LastUpdatedDate { get; set; }
    }
}