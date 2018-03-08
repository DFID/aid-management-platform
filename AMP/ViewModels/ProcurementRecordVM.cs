using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AMP.ViewModels
{
    public class ProcurementRecordVM
    {
        public long order_id { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCombind { get; set; }
        public string ComponentID { get; set; }
        public string Project { get; set; }
                [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public Nullable<decimal> OrderedAmount { get; set; }
                [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public Nullable<decimal> ReceiptedAmount { get; set; }
                [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
        public Nullable<decimal> Invoiced { get; set; }
        public System.DateTime Date { get; set; }
    }
}