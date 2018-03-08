using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace AMP.ViewModels
{
    public class ComponentFinanceVM
    {
        public IEnumerable<ComponentFinanceRecordVM> ComponentFinance { get; set; }

        //Error message returned from Finance Web Service
        public string FinanceWebServiceMessage { get; set; }

        public ComponentHeaderVM ComponentHeader { get; set; }

    }
}