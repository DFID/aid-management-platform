using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AMP.ARIESModels;
using System.Net;
using System.Text;
using AMP.Models;
using AMP.Utilities;
using AMP.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AMP.ARIESModels
{
    public class ARIESService : IARIESService, IDisposable
    {

        //New Instance of the ErrorEngine
        Utilities.ErrorEngine errorengine = new Utilities.ErrorEngine();

        #region GET Methods

        public async Task<IEnumerable<ProjectApprovedBudget>> GetProjectApprovedBudgetsAsync(string user) //Rename this
        {


            List<ProjectApprovedBudget> projectApprovedBudgets = new List<ProjectApprovedBudget>();
            int i = 0;

            while (i < 5)
            {

                ProjectApprovedBudget projectApprovedBudget = new ProjectApprovedBudget();
                projectApprovedBudget.ProjectID = "30000" + i;
                projectApprovedBudget.ApprovedBudget = 200000;
           

                projectApprovedBudgets.Add(projectApprovedBudget);
                i = i + 1;
            }


            return projectApprovedBudgets;

        }

        public async Task<IEnumerable<ProjectApprovedBudget>> GetProjectApprovedBudgetsByPortfolioAsync(IEnumerable<String> projectList, string user) //Rename this
        {


            List<ProjectApprovedBudget> projectApprovedBudgets = new List<ProjectApprovedBudget>();
            int i = 0;

            while (i < 5)
            {

                ProjectApprovedBudget projectApprovedBudget = new ProjectApprovedBudget();
                projectApprovedBudget.ProjectID = "30000" + i;
                projectApprovedBudget.ApprovedBudget = 200000;

                projectApprovedBudgets.Add(projectApprovedBudget);
                i = i + 1;
            }


            return projectApprovedBudgets;


        }

        public async Task<ProjectApprovedBudget> GetProjectApprovedBudgetAsync(string projectID, string user) //Rename this
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AMPUtilities.FinanceWebServiceUrl());// We should store this in the web.config file
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));// This probably removes the need to have the config in the API

                try
                {
                    // HTTP GET
                    HttpResponseMessage response = await client.GetAsync("Projects/ProjectApprovedBudgets/" + projectID);// This URI shouldbe in the web.config file or passed via the AMP Service Layer.

                    if (response.IsSuccessStatusCode)
                    {
                        ProjectApprovedBudget projectApprovedBudget = Newtonsoft.Json.JsonConvert.DeserializeObject<ProjectApprovedBudget>(response.Content.ReadAsStringAsync().Result);

                        return projectApprovedBudget;
                    }
                    else
                    {
                        Exception ex = new Exception(response.StatusCode.ToString() + " - " + response.ReasonPhrase.ToString());
                        // Executes the error engine (ProjectID is optional, exception)
                        errorengine.LogError(projectID, ex, user);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    // Executes the error engine (ProjectID is optional, exception)
                    errorengine.LogError(projectID, ex, user);
                    throw ex;
                }
            }

        }

        public async Task<IEnumerable<ProjectFinanceRecordVM>> GetProjectFinancialsAsync(string projectID, string user) //Rename this
        {
            //Demo data
            List<ProjectFinanceRecordVM> projectFinanceRecordVms = new List<ProjectFinanceRecordVM>();

            ProjectFinanceRecordVM P = new ProjectFinanceRecordVM();

            P.Project = projectID;
            P.work_order = projectID + "-101";
            P.Year = "2016m";
            P.PrePipeline_Budget = 0;
            P.Pipeline_Budget = 100000;
            P.Revised_Budget = 250000;
            P.Spend = 150000;
            P.Committed_Amount = 10000;
            P.Balance = 100000;
            P.ForecastOutturn = 250000;
            P.OutturnVariance = 0;

            projectFinanceRecordVms.Add(P);

            return projectFinanceRecordVms;

        }

        public async Task<IEnumerable<ComponentFinanceRecordVM>> GetComponentFinancialsAsync(string componentid, string user) //Rename this
        {
            //Demo data
            List<ComponentFinanceRecordVM> projectFinanceRecordVms = new List<ComponentFinanceRecordVM>();

            ComponentFinanceRecordVM P = new ComponentFinanceRecordVM();

            P.Project = componentid.Substring(0, 6);
            P.work_order = componentid + "-101";
            P.Year = "2016";
            P.PrePipeline_Budget = 0;
            P.Pipeline_Budget = 100000;
            P.Revised_Budget = 250000;
            P.Spend = 150000;
            P.Committed_Amount = 10000;
            P.Balance = 100000;
            P.ForecastOutturn = 250000;
            P.OutturnVariance = 0;

            projectFinanceRecordVms.Add(P);

            return projectFinanceRecordVms;

        }

        public async Task<IEnumerable<ProcurementRecordVM>> GetProjectProcurementAsync(string projectid, string user) //Rename this
        {
            List<ProcurementRecordVM> Procurement = new List<ProcurementRecordVM>();
            ProcurementRecordVM proc = new ProcurementRecordVM();

            proc.Project = projectid;
            proc.ComponentID = projectid + "-101";
            proc.order_id = 400;
            proc.Date = DateTime.Today;
            proc.Invoiced = 300;
            proc.OrderedAmount = 1000;
            proc.ReceiptedAmount = 500;
            proc.SupplierID = "12345";
            proc.SupplierName = "Company A";
            proc.SupplierCombind = "12345 - Company A";

            Procurement.Add(proc);

            return Procurement;

        }

        public async Task<IEnumerable<DocumentRecordVM>> GetProjectDocumentsAsync(string projectid, string user) //Rename this
        {
            List<DocumentRecordVM> documentRecordList = new List<DocumentRecordVM>();

            DocumentRecordVM BusinessCaseVm = new DocumentRecordVM
            {
                ContentType = "Business Case",
                CreatedDate = "30/12/2014",
                CreatedDateTime = null,
                DocumentID = "1234567",
                DocumentTitle = "Approved Business Case for project " + projectid,
                FileExtension = "docx",
                LastUpdatedDate = DateTime.Now.ToShortDateString(),
                ProjectID = projectid,
                QuestIcon = ""
            };

            documentRecordList.Add(BusinessCaseVm);

            DocumentRecordVM LogFrameVm = new DocumentRecordVM
            {
                ContentType = "Logical framework",
                CreatedDate = "25/09/2014",
                CreatedDateTime = null,
                DocumentID = "5778932",
                DocumentTitle = "Logical Framework for project - version 2",
                FileExtension = "xlsx",
                LastUpdatedDate = DateTime.Now.ToShortDateString(),
                ProjectID = projectid,
                QuestIcon = ""
            };

            documentRecordList.Add(LogFrameVm);

            DocumentRecordVM AnnualReviewVm = new DocumentRecordVM
            {
                ContentType = "Annual Review",
                CreatedDate = "30/12/2015",
                CreatedDateTime = null,
                DocumentID = "1668902",
                DocumentTitle = "Annual Review No 1 for project " + projectid,
                FileExtension = "docx",
                LastUpdatedDate = DateTime.Now.ToShortDateString(),
                ProjectID = projectid,
                QuestIcon = ""
            };

            documentRecordList.Add(AnnualReviewVm);

            return documentRecordList;

        }

        public async Task<IEnumerable<SupplierVM>> GetSuppliers(IEnumerable<String> supplierList, string user) //Rename this
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AMPUtilities.FinanceWebServiceUrl());// We should store this in the web.config file
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));// This probably removes the need to have the config in the API

                try
                {
                    // string logonName = empNo.Substring(5);
                    string queryString = BuildQueryStringFromList(supplierList.ToList());


                    // HTTP GET 

                    HttpResponseMessage response = await client.GetAsync("API/ProjectAPI/GetSuppliers/?" + queryString);// This URI shouldbe in the web.config file or passed via the AMP Service Layer.



                    if (response.IsSuccessStatusCode)
                    {
                        IEnumerable<SupplierVM> suppliers = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<SupplierVM>>(response.Content.ReadAsStringAsync().Result);

                        return suppliers;
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        //This user has no projects to get budgets for.
                        return null;
                    }
                    else
                    {
                        Exception ex =
                            new Exception(response.StatusCode.ToString() + " - " + response.ReasonPhrase.ToString());
                        // Executes the error engine (ProjectID is optional, exception)
                        errorengine.LogError(ex, user);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    // Executes the error engine (ProjectID is optional, exception)
                    errorengine.LogError(ex, user);
                    throw ex;
                }
            }

        }

        public async Task<IEnumerable<SupplierVM>> GetAllSuppliers(string user)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AMPUtilities.FinanceWebServiceUrl());// We should store this in the web.config file
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));// This probably removes the need to have the config in the API

                try
                {
                    // HTTP GET 
                    HttpResponseMessage response = await client.GetAsync("Projects/GetAllSuppliers");// This URI shouldbe in the web.config file or passed via the AMP Service Layer.



                    if (response.IsSuccessStatusCode)
                    {
                        IEnumerable<SupplierVM> suppliers = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<SupplierVM>>(response.Content.ReadAsStringAsync().Result);

                        return suppliers;
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        //This user has no projects to get budgets for.
                        return null;
                    }
                    else
                    {
                        Exception ex =
                            new Exception(response.StatusCode.ToString() + " - " + response.ReasonPhrase.ToString());
                        // Executes the error engine (ProjectID is optional, exception)
                        errorengine.LogError(ex, user);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    // Executes the error engine (ProjectID is optional, exception)
                    errorengine.LogError(ex, user);
                    throw ex;
                }
            }

        }

        public async Task <string> GetSupplierName(string supplierId)
         { using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AMPUtilities.FinanceWebServiceUrl());// We should store this in the web.config file
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));// This probably removes the need to have the config in the API

                try
                {
                  
                 // HTTP GET 

                 HttpResponseMessage response = await client.GetAsync("API/ProjectAPI/GetSupplierName/" + supplierId);// This URI shouldbe in the web.config file or passed via the AMP Service Layer.


                    if (response.IsSuccessStatusCode)
                    {
                        string suppliers = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);

                        return suppliers;
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        //This user has no projects to get budgets for.
                        return null;
                    }
                    else
                    {
                        Exception ex =
                            new Exception(response.StatusCode.ToString() + " - " + response.ReasonPhrase.ToString());
                    // Executes the error engine (ProjectID is optional, exception)
                    errorengine.LogError(ex, "");
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    // Executes the error engine (ProjectID is optional, exception)
                    errorengine.LogError(ex, "");
                    throw ex;
                }
            }
        }
        public async Task<IEnumerable<SupplierVM>> GetSearchSuppliers(IEnumerable<String> searchString, string user)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AMPUtilities.FinanceWebServiceUrl());// We should store this in the web.config file
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));// This probably removes the need to have the config in the API

                try
                {
                    // string logonName = empNo.Substring(5);
                    string queryString = BuildQueryStringFromList(searchString.ToList());


                    // HTTP GET 

                    HttpResponseMessage response = await client.GetAsync("API/ProjectAPI/GetSearchSuppliers/?" + queryString);// This URI shouldbe in the web.config file or passed via the AMP Service Layer.



                    if (response.IsSuccessStatusCode)
                    {
                        IEnumerable<SupplierVM> suppliers = JsonConvert.DeserializeObject<IEnumerable<SupplierVM>>(response.Content.ReadAsStringAsync().Result);

                        return suppliers;
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        //This user has no projects to get budgets for.
                        return null;
                    }
                    else
                    {
                        Exception ex =
                            new Exception(response.StatusCode.ToString() + " - " + response.ReasonPhrase.ToString());
                        // Executes the error engine (ProjectID is optional, exception)
                        errorengine.LogError(ex, user);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    // Executes the error engine (ProjectID is optional, exception)
                    errorengine.LogError(ex, user);
                    throw ex;
                }
            }

        }


        public async Task<ProjectWFCheckVM> ProjectWFCheck(string ProjectID, string user)
        {

            ProjectWFCheckVM projectWfCheck = new ProjectWFCheckVM();

            projectWfCheck.Status = false;
            projectWfCheck.WorkFlowDescription = "ARIES";

            return projectWfCheck;

        }

        public async Task<bool> DoesComponentHaveApprovedBudget(String ComponentID, String user)
        {
            return false;
        }

        public async Task<bool> DoesProjectHaveBudget(string projectId, string user)
        {
            return false;

        }

        public async Task<List<CurrencyVM>> GetCurrency()
        {
            string user = "";

            List<CurrencyVM> currency = new List<CurrencyVM>();

            currency.Add(
                new CurrencyVM
                {
                    CurrencyCode = "FRF",
                    CurrencyCombined = "FRF - French Franc",
                    CurrencyDescription = "French Franc"
                });

            currency.Add(
                new CurrencyVM
                {
                    CurrencyCode = "GBP",
                    CurrencyCombined = "GBP - GBP Pound Sterling",
                    CurrencyDescription = "GBP Pound Sterling"
                });

            currency.Add(
                new CurrencyVM
                {
                    CurrencyCode = "INR",
                    CurrencyCombined = "INR - Indian Rupee",
                    CurrencyDescription = "Indian Rupee"
                });
            currency.Add(
                new CurrencyVM
                {
                    CurrencyCode = "UGX",
                    CurrencyCombined = "UGX - Uganda Shilling",
                    CurrencyDescription = "Uganda Shilling"
                });
            currency.Add(
                new CurrencyVM
                {
                    CurrencyCode = "USD",
                    CurrencyCombined = "USD - US Dollar",
                    CurrencyDescription = "US Dollar"
                });

            return currency;
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(AMPUtilities.FinanceWebServiceUrl());// We should store this in the web.config file
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));// This probably removes the need to have the config in the API



            //    try
            //    {
            //        // HTTP GET 

            //        HttpResponseMessage response = await client.GetAsync("Projects/Currency/");// This URI shouldbe in the web.config file or passed via the AMP Service Layer.

            //        if (response.IsSuccessStatusCode)
            //        {
            //            List<CurrencyVM> currency = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CurrencyVM>>(response.Content.ReadAsStringAsync().Result);

            //            foreach (CurrencyVM curr in currency)
            //            {
            //                curr.CurrencyCombined = curr.CurrencyCode + " - " + curr.CurrencyDescription;
            //            }
            //            return currency;
            //        }
            //        else
            //        {
            //            Exception ex =
            //                new Exception(response.StatusCode.ToString() + " - " + response.ReasonPhrase.ToString());
            //            // Executes the error engine (ProjectID is optional, exception)
            //            errorengine.LogError(ex, user);
            //            throw ex;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        // Executes the error engine (ProjectID is optional, exception)
            //        errorengine.LogError(ex, user);
            //        throw ex;
            //    }
            //}

        }

        public async Task<List<PartnersTier1>> GetTier1Partners(string ComponentId)
        {
            string user = "";


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AMPUtilities.FinanceWebServiceUrl());// We should store this in the web.config file
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));// This probably removes the need to have the config in the API

                try
                {
                    // HTTP GET 

                    HttpResponseMessage response = await client.GetAsync("Projects/ComponentPartnerFirstTier/" + ComponentId);// This URI shouldbe in the web.config file or passed via the AMP Service Layer.

                    if (response.IsSuccessStatusCode)
                    {
                        List<PartnersTier1> tier1Partners = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PartnersTier1>>(response.Content.ReadAsStringAsync().Result);

                        return tier1Partners;
                    }
                    else
                    {
                        Exception ex =
                            new Exception(response.StatusCode.ToString() + " - " + response.ReasonPhrase.ToString());
                        // Executes the error engine (ProjectID is optional, exception)
                        errorengine.LogError(ex, user);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    // Executes the error engine (ProjectID is optional, exception)
                    errorengine.LogError(ex, user);
                    throw ex;
                }
            }

        }



        public async Task<Decimal> GetProjBBudget(string projectId)
        {
            string user = "";


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AMPUtilities.FinanceWebServiceUrl());// We should store this in the web.config file
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));// This probably removes the need to have the config in the API
                
                try
                {
                    // HTTP GET 

                    HttpResponseMessage response = await client.GetAsync("Projects/projBBudget/" + projectId);// This URI shouldbe in the web.config file or passed via the AMP Service Layer.

                    if (response.IsSuccessStatusCode)
                    {
                        decimal budgetValue = Newtonsoft.Json.JsonConvert.DeserializeObject<Decimal>(response.Content.ReadAsStringAsync().Result);

                        return budgetValue;
                    }
                    else
                    {
                        Exception ex =
                            new Exception(response.StatusCode.ToString() + " - " + response.ReasonPhrase.ToString());
                        // Executes the error engine (ProjectID is optional, exception)
                        errorengine.LogError(ex, user);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    // Executes the error engine (ProjectID is optional, exception)
                    errorengine.LogError(ex, user);
                    throw ex;
                }
            }

        }


     

        private string BuildQueryStringFromList(List<string> paramList)
        {
            string queryString = "";

            for (int i = 0; i < paramList.Count(); i++)
            {
                queryString = queryString + "id=";
                queryString = queryString + paramList[i].Trim();

                if (i != paramList.Count() - 1)
                {
                    queryString = queryString + "&";
                }
            }

            return queryString;
        }

        #endregion

        #region POST Methods

        public async Task BudgetMovement(String ProjectID, String ComponentID, String From, String To, String user, int AD)
        {
           


        }

        #endregion

        #region Disposal Methods
        // Dispose Methods

        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ARIESService()
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
