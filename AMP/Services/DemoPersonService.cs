using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AMP.Models;
using AMP.Utilities;

namespace AMP.Services
{
    public class DemoPersonService : IPersonService
    {
        //New Instance of the ErrorEngine
        ErrorEngine _errorengine = new Utilities.ErrorEngine();

        private PersonDetails returnPersonDetails(string empNo)
        {
            PersonDetails personDetails;

            switch (empNo)
            {
                case "111111":
                    personDetails = new PersonDetails
                    {
                        DisplayName = "A Inputter",
                        EmpNo = empNo,
                        Forename = "A",
                        Surname = "Inputter",
                        Email = "A-Inputter@AMPDemo.com",
                        IsEmployed = "T"

                    };
                    return personDetails;
                case "222222":
                    personDetails = new PersonDetails
                    {
                        DisplayName = "An SRO",
                        EmpNo = empNo,
                        Forename = "A",
                        Surname = "SRO",
                        Email = "A-SRO@AMPDemo.com",
                        IsEmployed = "T"
                    };
                    return personDetails;
                case "333333":
                    personDetails = new PersonDetails
                    {
                        DisplayName = "An Adviser",
                        EmpNo = empNo,
                        Forename = "A",
                        Surname = "Adviser",
                        Email = "A-Adviser@AMPDemo.com",
                        IsEmployed = "T"
                    };
                    return personDetails;
                case "444444":
                    personDetails = new PersonDetails
                    {
                        DisplayName = "A TeamMember",
                        EmpNo = empNo,
                        Forename = "A",
                        Surname = "TeamMember",
                        Email = "A-TeamMember@AMPDemo.com",
                        IsEmployed = "T"
                    };
                    return personDetails;
                case "555555":
                    personDetails = new PersonDetails
                    {
                        DisplayName = "A OfficeHead",
                        EmpNo = empNo,
                        Forename = "A",
                        Surname = "OfficeHead",
                        Email = "A-Officehead@AMPDemo.com",
                        IsEmployed = "T"
                    };
                    return personDetails;
                default:
                    return null;
            }

        }

        public async Task<PersonDetails> GetPersonDetails(string empNo)
        {
            return returnPersonDetails(empNo);
        }




        public async Task<IEnumerable<PersonDetails>> GetPeopleDetails(IEnumerable<String> empNoList)
        {
            List<PersonDetails> personDetails = new List<PersonDetails>();
            foreach (String empNo in empNoList)
            {
                personDetails.Add(returnPersonDetails(empNo));
            }

            return personDetails;
        }

        public async Task<IEnumerable<PersonDetails>> GetAllCurrentStaff()
        {
            return null;

        }

        public async Task<string> GetUserSectionCode(string empNo)
        {
            return null;

        }

        #region Disposal Methods
        // Dispose Methods

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DemoPersonService()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // free other managed objects that implement
                // IDisposable only
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        #endregion

    }
}