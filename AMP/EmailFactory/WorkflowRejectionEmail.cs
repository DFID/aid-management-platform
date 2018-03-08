using AMP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AMP.Models;
using AMP.Utilities;

namespace AMP.EmailFactory
{
    public class WorkflowRejectionEmail : EmailCreator
    {
        private IPersonService _personService;
        private IAMPRepository _ampRepository;

        #region Constructor

        public WorkflowRejectionEmail()
        {
            _personService = new DemoPersonService();
            _ampRepository = new AMPRepository();
        }

        public WorkflowRejectionEmail(IPersonService personService, IAMPRepository ampRepository)
        {
            this._personService = personService;
            this._ampRepository = ampRepository;
        }

        #endregion

        #region  Process Email

        protected async override Task<Email> ProcessEmail(string workflowTaskId, string projectId, string comments, string recipient, string user, string pageUrl)
        {
            Email email;

            if (workflowTaskId == Constants.CloseProjectTaskId.ToString())
            {
                email = new RejectCloseProjectEmail();
            }
            else if (workflowTaskId == Constants.ArchiveProject.ToString())
            {
                email = new RejectArchiveProjectEmail();
            }
            else
            {
                email = new RejectProjectWorkflowEmail();
            }

            await email.SetEmailPeople(_ampRepository, _personService, user, recipient, projectId);
            email.SetSROText();
            email.Subject = String.Format(email.Subject, projectId, _ampRepository.GetProject(projectId).Title);
            email.AMPlink = String.Format(email.AMPlink, AMPUtilities.BaseUrl(), projectId, workflowTaskId);
            email.Body = String.Format(email.Body, email.Recipient.Forename, email.Sender.Forename, email.AMPlink, email.sroText);

            return email;

        }
        #endregion
    }
}