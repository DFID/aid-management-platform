using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ViewModels
{
    public class PublishedDocumentsVM
    {
        public IEnumerable<PublishedDocumentVM> PublishedDocument { get; set; }

        public string WebServiceMessage { get; set; }

        public ProjectHeaderVM ProjectHeader { get; set; }

        public string ProjectID { get; set; }
    }
}