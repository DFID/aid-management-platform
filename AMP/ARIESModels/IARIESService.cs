using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using AMP.ARIESModels;
using AMP.ViewModels;
using System.Threading.Tasks;

namespace AMP.ARIESModels
{
    public interface IARIESService : IDisposable
    {

        #region GET Methods
        Task<IEnumerable<ProjectApprovedBudget>> GetProjectApprovedBudgetsAsync(string user);

        Task<IEnumerable<ProjectApprovedBudget>> GetProjectApprovedBudgetsByPortfolioAsync(IEnumerable<String> projectList, string user);

        Task<ProjectApprovedBudget> GetProjectApprovedBudgetAsync(string projectID, string user);

        Task<IEnumerable<ProjectFinanceRecordVM>> GetProjectFinancialsAsync(string projectID, string user);

        Task<IEnumerable<ComponentFinanceRecordVM>> GetComponentFinancialsAsync(string componentid, string user);

        Task<IEnumerable<ProcurementRecordVM>> GetProjectProcurementAsync(string projectid, string user);

        Task<IEnumerable<DocumentRecordVM>> GetProjectDocumentsAsync(string projectid, string user);

        Task<IEnumerable<SupplierVM>> GetSuppliers(IEnumerable<String> suppliers, string user);
        Task<IEnumerable<SupplierVM>> GetSearchSuppliers(IEnumerable<String> seachString, string user);
        Task<IEnumerable<SupplierVM>> GetAllSuppliers(string user);
        Task<string>  GetSupplierName(string supplierId);


        Task<ProjectWFCheckVM> ProjectWFCheck(string ProjectID, string user);

        Task<bool> DoesComponentHaveApprovedBudget(String ComponentID, String User);

        Task<bool> DoesProjectHaveBudget(string projectId, string user);
        Task<List<CurrencyVM>> GetCurrency();

        Task<Decimal> GetProjBBudget(string projectId);

        Task<List<PartnersTier1>> GetTier1Partners(string ComponentId);

        #endregion

        #region POST Methods

        Task BudgetMovement(String ProjectID, String ComponentID, String From, String To, String user, int AD);

        #endregion
    }
}