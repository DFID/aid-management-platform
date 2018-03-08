using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AMP.Utilities;
using AMP.ViewModels;

namespace AMP.Services
{
    public class IATIWebService : IIATIWebService, IDisposable
    {
        private IErrorEngine errorengine;
        private HttpClient _client;
        private IConfiguration _configuration;

        public IATIWebService()
        {
            //_client = new HttpClient(new HttpClientHandler() { UseDefaultCredentials = true });
            _configuration = new Configuration();
        }

        #region Get Service Methods

        public async Task<IEnumerable<PublishedDocumentVM>> GetPublishedDocumentListWithStatusAsync(string projectID, string user)
        {
            //ProjectMasterVM project = new ProjectMasterVM();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration.IATIWebServiceUrl);// We should store this in the web.config file
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));// This probably removes the need to have the config in the API

                try
                {
                    // HTTP GET
                    HttpResponseMessage response = await client.GetAsync("PublishedDocument/GetPublishedDocumentByProjectId/" + projectID);// This URI shouldbe in the web.config file or passed via the AMP Service Layer.
                    if (response.IsSuccessStatusCode)
                    {

                        List<PublishedDocumentVM> publishedDocumentVM = new List<PublishedDocumentVM>();
                        var publishedDoc = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<PublishedDocumentVM>>(response.Content.ReadAsStringAsync().Result);
                        foreach (var projectDoc in publishedDoc)
                        {
                            publishedDocumentVM.Add(projectDoc);
                        }

                        
                        return publishedDocumentVM;
                    }
                    else
                    {
                        Exception ex = new Exception(response.StatusCode + " - " + response.ReasonPhrase);
                        // Executes the error engine (ProjectID is optional, exception)
                        errorengine.LogError(ex, user);
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
        #endregion

        #region Disposal
        bool _disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~IATIWebService()
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