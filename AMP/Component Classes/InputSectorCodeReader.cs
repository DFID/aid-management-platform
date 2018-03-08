using System.Collections.Generic;
using System.Linq;
using AMP.Models;
using AMP.Services;

namespace AMP.Component_Classes
{
    public class InputSectorCodeReader
    {
        protected IAMPRepository _ampRepository;

        public InputSectorCodeReader(IAMPRepository ampRepository)
        {
            _ampRepository = ampRepository;
        }

        public List<InputSectorCode> GetInputSectorCodes(string componentId)
        {
            List<InputSectorCode> inputSectorCodes = new List<InputSectorCode>();
            inputSectorCodes = _ampRepository.GetInputSectors(componentId).ToList();

            return inputSectorCodes;
        }

    }
}