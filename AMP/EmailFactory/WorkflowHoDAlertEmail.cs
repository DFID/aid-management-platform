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
    public class WorkflowHoDAlertEmail :EmailCreator
    {
        private IPersonService _personService;
        private IAMPRepository _ampRepository;

        #region Constructor

        public WorkflowHoDAlertEmail()
        {
            _personService = new DemoPersonService();
            _ampRepository = new AMPRepository();
        }

        public WorkflowHoDAlertEmail(IPersonService personService, IAMPRepository ampRepository)
        {
            this._personService = personService;
            this._ampRepository = ampRepository;
        }

        #endregion

        #region Process Email

        protected override async Task<Email> ProcessEmail(string workflowTaskId, string projectId, string comments,
            string recipient, string user, string pageUrl)
        {
            Email email = new HoDAlertEmail();

            //Get the HoD alert List

            //Decide who gets the e-mail and who is on the cc list.
            await email.SetHoDAlertPeople(_ampRepository, _personService, user, projectId);

            //Get the project approved budget.

            email.SetSROText();
            email.AMPlink = email.AMPlink = String.Format(email.AMPlink, AMPUtilities.BaseUrl(), projectId, workflowTaskId);
            email.Subject = String.Format(email.Subject, projectId, _ampRepository.GetProject(projectId).Title);
            email.Body = String.Format(email.Body, email.Recipient.Forename, email.Sender.Forename+ ' ' + email.Sender.Surname, email.AMPlink, email.sroText);

            return email;
        }

        #endregion


    }
}