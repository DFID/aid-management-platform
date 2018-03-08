using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class ProjectDocumentsVM
    {
        public IEnumerable<DocumentRecordVM> ProjectDocument { get; set; }

        public string WebServiceMessage { get; set; }

        public ProjectHeaderVM ProjectHeader { get; set; }
    }
}