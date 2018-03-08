using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Component_Classes;
using AMP.Models;
using AMP.RiskClasses;
using AMP.Validation_Classes;
using AMP.ViewModels;

namespace AMP.Services
{
    public class SectorCodeService:ISectorCodeService   
    {
        private IAMPRepository _ampRepository;
        private IPersonService _personService;
        private IValidationDictionary _validationDictionary;
        private readonly InputSectorCodeReader _inputSectorCodeReader;
        private readonly InputSectorCodeWriter _inputSectorCodeWriter;
        private IComponentValidator _componentValidator;

        public SectorCodeService(IAMPRepository ampRepository, IValidationDictionary validationDictionary)
        {
            _ampRepository = ampRepository;
            _personService = new DemoPersonService();
            _inputSectorCodeReader = new InputSectorCodeReader(_ampRepository);
            _inputSectorCodeWriter = new InputSectorCodeWriter(_ampRepository);
            _validationDictionary = validationDictionary;
            _componentValidator = new ComponentValidator();
        }

        public SectorCodeService(IAMPRepository ampRepository, IValidationDictionary validationDictionary, IComponentValidator componentValidator)
        {
            _ampRepository = ampRepository;
            _personService = new DemoPersonService();
            _inputSectorCodeReader = new InputSectorCodeReader(_ampRepository);
            _inputSectorCodeWriter = new InputSectorCodeWriter(_ampRepository);
            _validationDictionary = validationDictionary;
            _componentValidator = componentValidator;

        }



        public bool ValidateSectorCodes(EditInputSectorsVM editInputSectorsVm)
        {
            _componentValidator.ValidationDictionary = _validationDictionary;
            if (_componentValidator.EditInputSectorsVM_Sector_Codes_Are_Valid(editInputSectorsVm))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateSectorCodes(EditInputSectorsVM editInputSectorsVm, string user)
        {
            if (ValidateSectorCodes(editInputSectorsVm))
            {
                //Get Existing Input SectorCodes
                List<InputSectorCode> inputSectorCodes =
                    _inputSectorCodeReader.GetInputSectorCodes(editInputSectorsVm.CompID);

                //Remove Existing Input SectorCodes
                _inputSectorCodeWriter.DeleteInputSectorCodes(inputSectorCodes);

                //Create InputSectors
                InputSectorCollectionBuilder inputSectorCollectionBuilder = new InputSectorCollectionBuilder();
                inputSectorCodes = inputSectorCollectionBuilder.ReturnInputSectorCodes(editInputSectorsVm, user);

                //Insert New InputSectorCodes
                _inputSectorCodeWriter.InsertInputSectorCodes(inputSectorCodes);

                //Commit
                _inputSectorCodeWriter.Commit();

                return true;
            }
            else
            {
                return false;
            }
        }


    }


}