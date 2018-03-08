using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AMP.Models;

namespace AMP.Services
{
    public interface IPersonService: IDisposable
    {
        Task<IEnumerable<PersonDetails>> GetPeopleDetails(IEnumerable<String> empNoList);
        Task<IEnumerable<PersonDetails>> GetAllCurrentStaff();
        Task<PersonDetails> GetPersonDetails(string empNo);
        Task<string> GetUserSectionCode(string empNo);
    }
}