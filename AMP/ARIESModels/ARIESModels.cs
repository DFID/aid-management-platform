using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.ARIESModels
{
    public class ProjectApprovedBudget
    {
        public String ProjectID { get; set; }
        public Decimal ApprovedBudget { get; set; }

    }

    //Define more models here:
    public class ProjectFinance
    {
        public String Project { get; set; }
        public String work_order { get; set; }
        public string Year { get; set; }
        public Nullable<decimal> Spend { get; set; }
        public Nullable<decimal> Committed_Amount { get; set; }
        public Nullable<decimal> PrePipeline_Budget { get; set; }
        public Nullable<decimal> Pipeline_Budget { get; set; }
        public Nullable<decimal> Original_Budget { get; set; }
        public Nullable<decimal> Budget_Adjustments { get; set; }
        public Nullable<decimal> Revised_Budget { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<decimal> ForecastOutturn { get; set; }
        public Nullable<decimal> OutturnVariance { get; set; }

    }

    public class ComponentFinance
    {
        public String work_order { get; set; }
        public string Year { get; set; }
        public Nullable<decimal> Spend { get; set; }
        public Nullable<decimal> Committed_Amount { get; set; }
        public Nullable<decimal> PrePipeline_Budget { get; set; }
        public Nullable<decimal> Pipeline_Budget { get; set; }
        public Nullable<decimal> Original_Budget { get; set; }
        public Nullable<decimal> Budget_Adjustments { get; set; }
        public Nullable<decimal> Revised_Budget { get; set; }
        public Nullable<decimal> Balance { get; set; }
        public Nullable<decimal> ForecastOutturn { get; set; }
        public Nullable<decimal> OutturnVariance { get; set; }

    }

    public class ProjectProcurement
    {
        public long order_id { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCombind { get; set; }
        public string ComponentID { get; set; }
        public string Project { get; set; }
        public Nullable<decimal> OrderedAmount { get; set; }
        public Nullable<decimal> ReceiptedAmount { get; set; }
        public Nullable<decimal> Invoiced { get; set; }
        public System.DateTime Date { get; set; }
    }

    public class ProjectDocument
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

    public class Supplier
    {
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string CountryCode { get; set; }
        public string Status { get; set; }

    }
      public class ProjectWFCheck
    {
          public bool Status { get; set; }
    }

      public class Currency
      {
          public string CurrencyCode { get; set; }
          public string CurrencyDescription { get; set; }
      }
    public class PartnersTier1
    {
        public string ComponentID { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCombind { get; set; }
        public string Status { get; set; }
    }


}
