using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Razor;
using AMP.Models;

namespace AMP.ViewModels
{
    public class ExistingSectorCodeMappingVM
    {

        public string InputSectorCodeID { get; set; }

        public InputSector InputSector { get; set; }

        public IEnumerable<FundingMechToSector> FundingMechToSectorEntries { get; set; }

        public ICollection<FundingMech> FundingMechs { get; set; }



    }
}