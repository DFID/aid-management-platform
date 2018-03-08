using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AMP.ViewModels;

namespace AMP.Services
{
    public interface IIATIWebService
    {
        Task<IEnumerable<PublishedDocumentVM>> GetPublishedDocumentListWithStatusAsync(string projectID, string user);
    }
}