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
    
    public partial class ReviewDocument
    {
        public int ReviewDocumentsID { get; set; }
        public string ProjectID { get; set; }
        public int ReviewID { get; set; }
        public string DocumentID { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> LastUpdate { get; set; }
        public string UserID { get; set; }
        public string DocSource { get; set; }
    
        public virtual ReviewMaster ReviewMaster { get; set; }
    }
}
