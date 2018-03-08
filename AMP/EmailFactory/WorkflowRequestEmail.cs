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
    public class WorkflowRequestEmail : EmailCreator
    {
        private IPersonService _personService;
        private IAMPRepository _ampRepository;

        #region Constructor

        public WorkflowRequestEmail()
        {
            _personService = new DemoPersonService();
            _ampRepository = new AMPRepository();
        }

        public WorkflowRequestEmail(IPersonService personService, IAMPRepository ampRepository)
        {
            this._personService = personService;
            this._ampRepository = ampRepository;
        }

        #endregion

        #region  Process Email

        protected async override Task<Email> ProcessEmail(string workflowTaskId, string projectId, string comments, string recipient, string user, string pageUrl)
        {
            Email email;

            //switch (workflowTaskId)
            //{
            //    case Constants.WorkflowTaskId.ApproveProject:

            //    default:
            //}

            if (workflowTaskId == Constants.ArchiveProject.ToString())
            {
                email = new RequestArchiveEmail();
            }
            else if (workflowTaskId == Constants.ApproveProjectTask.ToString())
            {
                email = new RequestApprovalEmail();
            }
            else if (workflowTaskId == Constants.ReApproveProjectTaskId.ToString())
            {
                    email = new RequestReApprovalEmail();
            }
            else if (workflowTaskId == Constants.PlannedEndDate.ToString())
            {
                email = new RequestPlannedEndDateEmail();
            }
            else if (workflowTaskId == Constants.ApproveAD.ToString())
            {
                email = new RequestAandDApprovalEmail();
            }
            else if (workflowTaskId == Constants.FastTrack.ToString())
            {
                email = new RequestFastTrackApprovalEmail();
            }
            else if (workflowTaskId == Constants.CloseProjectTaskId.ToString())
            {
                email = new RequestCloseProjectEmail();
            }
            else if(workflowTaskId == Constants.CancelWorkflow.ToString())
            {
                email = new RequestCancelEmail();
            }
            else if (workflowTaskId == Constants.ReOpenProject.ToString())
            {
                email = new RequestReOpenProjectEmail();
            }
            else
            {
                return null;
            }

            await email.SetEmailPeople(_ampRepository, _personService, user, recipient, projectId);
            email.SetSROText();
            email.AMPlink = String.Format(email.AMPlink,AMPUtilities.BaseUrl(), projectId, workflowTaskId);
            email.Subject = String.Format(email.Subject, projectId,_ampRepository.GetProject(projectId).Title);
            email.Body = String.Format(email.Body, email.Recipient.Forename, email.Sender.Forename, comments,email.AMPlink, email.sroText);

            return email;

        }

        public void SendEmail(Email email)
        {
            //endEmail(email.To, email.Cc, email.Bcc, email.From, email.Subject, email.Body, email.Priority);
        }


        #endregion


        #region Create Email

       //protected override Email CreateEmail();


        #endregion

    }
}