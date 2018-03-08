using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ViewModels;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;

namespace AMP.Validation_Classes
{
    public class ComponentValidator :IComponentValidator
    {
        protected IValidationDictionary _ValidationDictionary;

        public IValidationDictionary ValidationDictionary
        {
            get
            {
                return _ValidationDictionary;
            }

            set
            {
                _ValidationDictionary = value;
            }
        }

        public ComponentValidator()
        {

        }

        public bool EditInputSectorsVM_Sector_Codes_Are_Valid(EditInputSectorsVM editInputSectorsVm)
        {
            for (int i = 0; i < editInputSectorsVm.SectorCodesCodeVm.Length; i++)
            {
                //Check Codes
                if (editInputSectorsVm.SectorCodesCodeVm[i].ISCode.IsNullOrWhiteSpace())
                {
                    _ValidationDictionary.AddError("SectorCodesCodeVm_" + i + "__ISCode", "Sector cannot be blank");
                    return _ValidationDictionary.IsValid;
                }

                if (editInputSectorsVm.SectorCodesCodeVm[i].Percentage < 1 || editInputSectorsVm.SectorCodesCodeVm[i].Percentage > 100)
                {
                    _ValidationDictionary.AddError("SectorCodesCodeVm_" + i + "__Percentage",
                        "Percentage must be a whole number between 1 and 100");
                    return _ValidationDictionary.IsValid;
                }
            }

            //Sector percentages must total 100%
            if (editInputSectorsVm.SectorCodesCodeVm.Sum(x => x.Percentage) != 100)
            {
                _ValidationDictionary.AddError("SectorCodesCodeVm_0__ISCode","Sector Codes total must equal 100%");
                return _ValidationDictionary.IsValid;
            }

            //No more than 8 sectors.
            if (editInputSectorsVm.SectorCodesCodeVm.Length > 8)
            {
                _ValidationDictionary.AddError("SectorCodesCodeVm_0__ISCode",
                    "You can only have a maximum of 8 sectors");
                return _ValidationDictionary.IsValid;

            }

            //Sector Must be dominant.
            Int32 maxSectorPercentage = editInputSectorsVm.SectorCodesCodeVm.Max(x => x.Percentage);
            //List<SectorCodeVM> maxPercentageSectors =
            //    editInputSectorsVm.SectorCodesCodeVm.Where(x => x.Percentage == maxSectorPercentage).ToList();
            if (editInputSectorsVm.SectorCodesCodeVm.Count(x => x.Percentage == maxSectorPercentage) > 1)
            {
                _ValidationDictionary.AddError("SectorCodesCodeVm_0__ISCode", "Sector percentages are equal. One must be higher than the other to identify broad sector for DAC reporting.");
                return _ValidationDictionary.IsValid;
            }

            //Duplicate Sector Codes are no allowed.
            List<SectorCodeVM> duplicates = editInputSectorsVm.SectorCodesCodeVm.GroupBy(s => s.ISCode).SelectMany(grp => grp.Skip(1)).ToList();

            if (duplicates.Any())
            {
                _ValidationDictionary.AddError("SectorCodesCodeVm_0__ISCode", "Duplicate input sectors are not allowed");
                return _ValidationDictionary.IsValid;
            }


            return true;
        }
    }
}