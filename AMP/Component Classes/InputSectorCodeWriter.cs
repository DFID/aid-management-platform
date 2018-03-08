using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AMP.Models;

namespace AMP.Component_Classes
{
    public class InputSectorCodeWriter
    {
        protected IAMPRepository _ampRepository;

        public InputSectorCodeWriter(IAMPRepository ampRepository)
        {
            _ampRepository = ampRepository;
        }

        public void DeleteInputSectorCodes(List<InputSectorCode> inputSectorCodes)
        {
            _ampRepository.DeleteSectors(inputSectorCodes);
        }

        public void InsertInputSectorCodes(List<InputSectorCode> inputSectorCodes)
        {
            _ampRepository.InsertSectors(inputSectorCodes);
        }

        public void Commit()
        {
            _ampRepository.Save();
        }
    }
}