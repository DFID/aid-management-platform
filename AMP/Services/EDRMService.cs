using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using AMP.Models;
using AMP.Utilities;
using Newtonsoft.Json;

namespace AMP.Services
{
    public class EDRMService : IEDRMService
    {
        //private IHttpClient _client;

        private HttpClient _client;
        private IConfiguration _configuration;

        public EDRMService()
        {
            _client = new HttpClient(new HttpClientHandler() {UseDefaultCredentials = true});
            _configuration = new Configuration();
        }

        public EDRMService(HttpClient httpClient, IConfiguration configuration)
        {
            this._client = httpClient;
            this._configuration = configuration;
        }

        public async Task<string> CreateProjectFolder(string projectTitle, string budgetCentreId, string projectID)
        {
            string result = "";
            string userName = "";

            if (string.IsNullOrEmpty(projectTitle))
            {
                throw new ArgumentException("string is null or blank","projectTitle");
            }

            if (string.IsNullOrEmpty(budgetCentreId))
            {
                throw new ArgumentException("string is null or blank", "budgetCentreId");
            }

            //if (string.IsNullOrEmpty(userName))
            //{
            //    throw new ArgumentException("string is null or blank", "userName");
            //}
            userName = _configuration.EDRMUser;
            _client.BaseAddress = new Uri(_configuration.EDRMWebServiceUrl);
            _client.DefaultRequestHeaders.Accept.Clear();

            _client.DefaultRequestHeaders.Add("UserName", userName);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            Dictionary<string, string> metaData = new Dictionary<string, string>();
            metaData.Add("ProjectID",projectID);

            FolderModel folderModel = new FolderModel
            {
                Name = projectTitle,
                BudgetCentreID = budgetCentreId,
                MetaData = metaData

            };

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(folderModel);

            HttpResponseMessage responseMessage;

            // Construct an HttpContent from a StringContent
            HttpContent _Body = new StringContent(json);
            // and add the header to this object instance
            // optional: add a formatter option to it as well
            _Body.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            // synchronous request without the need for .ContinueWith() or await
            try
            {
                responseMessage = _client.PostAsync("api/documentservice/createfolder", _Body).Result;

                string content = responseMessage.Content.ReadAsStringAsync().Result;

                return content;

            }
            catch (Exception exception)
            {
                //Return zero when the web service call fails, regardless of why;
                throw new HttpRequestException("Create Vault Folder returned 0",exception.InnerException);
            }
            
        }



    }
}