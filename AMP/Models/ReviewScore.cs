//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AMP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ReviewScore
    {
        public ReviewScore()
        {
            this.ReviewOutputs = new HashSet<ReviewOutput>();
        }
    
        public string OutputScore { get; set; }
        public string Definition { get; set; }
        public int Weight { get; set; }
    
        public virtual ICollection<ReviewOutput> ReviewOutputs { get; set; }
    }
}
