using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AMP.Models;

namespace AMP.ViewModels
{
    public class NewFundingMechToSectorVM
    {

        public InputSector InputSector { get; set; }

        public IEnumerable<string> UnMappedOptions { get; set; }

        public string NewMapping { get; set; }
         

    }
}