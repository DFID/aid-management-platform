using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class ProjectDocumentVM
    {
        public int ID { get; set; }
        public string ProjectID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentDescription { get; set; }
        public string Author { get; set; }
        public string Language { get; set; }
        public int VersionNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime LastUpdatedDate { get; set; }
        public string Approved { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }



    }
}