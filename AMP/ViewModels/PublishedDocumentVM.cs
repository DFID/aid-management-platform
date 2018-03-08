using System;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class PublishedDocumentVM
    {
        public string QuestID { get; set; }

        public string VersionNo { get; set; }

        public string DocType { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> ExtractionDate { get; set; }

        public Int16 PublicationStatusID { get; set; }

        public string ProjectID { get; set; }

        public string VaultID { get; set; }

        public string Language { get; set; }

        public string DocExtension { get; set; }

        public string PublicationStatusDescription { get; set; }
    }
}