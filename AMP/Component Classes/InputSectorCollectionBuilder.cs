using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;
using AMP.ViewModels;

namespace AMP.Component_Classes
{
    public class InputSectorCollectionBuilder
    {
        public List<InputSectorCode> ReturnInputSectorCodes(EditInputSectorsVM editInputSectorsVm, string user)
        {
           List<InputSectorCode> inputSectorCodes = new List<InputSectorCode>();

            for (int i = 0; i < editInputSectorsVm.SectorCodesCodeVm.Length; i++)
            {
                InputSectorCode inputSectorCode = new InputSectorCode
                {
                    ComponentID = editInputSectorsVm.CompID,
                    InputSectorCode1 = editInputSectorsVm.SectorCodesCodeVm[i].ISCode,
                    LastUpdate = DateTime.Now,
                    LineNo = i,
                    Percentage = editInputSectorsVm.SectorCodesCodeVm[i].Percentage,
                    UserID = user
                };
                inputSectorCodes.Add(inputSectorCode);
            }

           return inputSectorCodes;
        }
    }
}