using AMP.Models;
using AMP.ViewModels;

namespace AMP.Validation_Classes
{


    public interface IComponentValidator
    {
        IValidationDictionary ValidationDictionary { get; set; }
        bool EditInputSectorsVM_Sector_Codes_Are_Valid(EditInputSectorsVM editInputSectorsVm);

    }
}