using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMP.Models;
using AMP.ARIESModels;

namespace AMP.ViewModels
{
    public class ComponentViewModel
    {
        public ComponentMasterVM ComponentMaster { get; set; }

        public ProjectMasterVM ProjectMaster { get; set; }

        public ComponentStaticData ComponentStaticData { get; set; }

        public ComponentDateVM ComponentDate { get; set; }

        public virtual IEnumerable<BudgetCentre> BudgetCentre { get; set; }

        public virtual User User { get; set; }

        public IEnumerable<FundingMech> FundingMechs { get; set; }

        public IEnumerable<InputSectorVM> InputSectors { get; set; }

        public InputSectorVM NewInputSector { get; set; }

        public IEnumerable<ComponentFinanceRecordVM> ComponentFinance { get; set; }

        public IEnumerable<SupplierVM> ImplementingOrganisation { get; set; }

        //Error message returned from Finance Web Service
        public string FinanceWebServiceMessage { get; set; }



    }
}