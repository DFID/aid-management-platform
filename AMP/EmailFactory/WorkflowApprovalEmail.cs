using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AMP.Models;
using AMP.Services;
using AMP.Utilities;

namespace AMP.EmailFactory
{
    public class WorkflowApprovalEmail : EmailCreator
    {
        private IPersonService _personService;
        private IAMPRepository _ampRepository;

        #region Constructor

        public WorkflowApprovalEmail()
        {
            _personService = new DemoPersonService();
            _ampRepository = new AMPRepository();
        }

        public WorkflowApprovalEmail(IPersonService personService, IAMPRepository ampRepository)
        {
            this._personService = personService;
            this._ampRepository = ampRepository;
        }

        #endregion

        #region Process Email

        protected override async Task<Email> ProcessEmail(string workflowTaskId, string projectId, string comments,
            string recipient, string user,string pageUrl )
        {
            Email email;

            if (workflowTaskId == Constants.CloseProjectTaskId.ToString())
            {
                email = new ApproveCloseProjectEmail();
            }
            else if (workflowTaskId == Constants.ArchiveProject.ToString())
            {
                email = new ApproveArchiveProjectEmail();
            }
            else if (workflowTaskId == Constants.ApproveProjectTask.ToString())
            {
                email = new ApproveProjectImplementaionWorkflowEmail();
            }
            else if (workflowTaskId == Constants.PlannedEndDate.ToString())
            {
                email = new ApprovePlannedEndDateEmail();
            }
            else
            {
                email = new ApproveProjectWorkflowEmail();
            }

            await email.SetEmailPeople(_ampRepository, _personService, user, recipient, projectId);
            email.SetSROText();
            email.AMPlink = pageUrl;
            email.Subject = String.Format(email.Subject, projectId, _ampRepository.GetProject(projectId).Title);
            email.Body = String.Format(email.Body, email.Recipient.Forename, email.Sender.Forename, email.AMPlink, email.sroText);

            return email;
        }

        #endregion
    }
}