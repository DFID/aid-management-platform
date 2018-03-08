using System.Threading.Tasks;

namespace AMP.Services
{
    public interface IEDRMService
    {
        Task<string> CreateProjectFolder(string projectTitle, string budgetCentreId, string projectID);

    }
}