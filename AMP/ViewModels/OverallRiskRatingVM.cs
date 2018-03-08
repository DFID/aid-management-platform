using System;
using System.Collections.Generic;
using AMP.Models;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class OverallRiskRatingVM
    {
        public int OverallRiskRatingId { get; set; }
        public string ProjectID { get; set; }
        [Required(ErrorMessage = "Comments can not be empty")]
        [StringLength(1000, MinimumLength = 20, ErrorMessage = "Comments must be at least 20 characters.")]
        public string Comments { get; set; }
        [Required(ErrorMessage = "Select a risk score")]
        public string RiskScore { get; set; }
        public string UserID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> LastUpdated { get; set; }

        public ProjectHeaderVM ProjectHeader { get; set; }

    }
}