using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AMP.EmailFactory
{
    public abstract class EmailCreator
    {
        private Email email;

        public async Task<Email> CreateEmail(string workflowTaskId, string projectId, string comments, string recipient, string user, string pageUrl)
        {
            email =  await ProcessEmail(workflowTaskId, projectId, comments, recipient, user, pageUrl);

            return email;
        }

        protected abstract Task<Email> ProcessEmail(string workflowTaskId, string projectId, string comments, string recipient, string user, string pageUrl);
 


    }
}