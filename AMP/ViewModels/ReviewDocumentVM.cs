using System;
using System.ComponentModel.DataAnnotations;
using AMP.Models;

namespace AMP.ViewModels
{
    public class ReviewDocumentVM
    {
        public int ReviewDocumentsID { get; set; }
        public string ProjectID { get; set; }
        public int ReviewID { get; set; }
        [Required(ErrorMessage = "You must enter a document number.")]
        [Range(0, long.MaxValue, ErrorMessage = "Document number must be a number.")]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Document ID must be between 6 & 12 digits.")]
        public string DocumentID { get; set; }
        [Required(ErrorMessage = "Document description must be at least 6 characters.")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "Document description must be at least 6 characters.")]
        public string Description { get; set; }
        public string DocSource { get; set; }
        public string DocumentLink { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }
        public virtual ReviewMaster ReviewMaster { get; set; }

    }
}