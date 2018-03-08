using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AMP.Models
{
    public class FolderModel
    {
        public string Name { get; set; }
        public Dictionary<string, string> MetaData { get; set; }
        public string BudgetCentreID { get; set; }
        public bool IsFolderCreated { get; set; }
        public int NewFolderID { get; set; }


    }
}