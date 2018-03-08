using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using AMP.Models;
using AMP.Services;
using AMP.Utilities;

namespace AMP.EmailFactory
{
    public abstract class Email
    {
        #region Properties

        public string To;
        public string Cc;
        public string Bcc;
        public string From;
        public string Subject;
        public string Body;
        public string sroText;
        public string AMPlink;
        public MailPriority Priority;
        public PersonDetails Sender;
        public PersonDetails Recipient;
        public PersonDetails SRO;
        public IEnumerable<PersonDetails> projectTeam;



        #endregion

        #region Constructor

        public Email()
        { }

        #endregion

        #region Email methods

        public async Task SetSROText()
        {
            if (SRO!= null)
            {
                sroText = "The SRO is " + SRO.Forename + " " + SRO.Surname;
            }
        else
            {
                sroText = "This project has no SRO assigned and does not comply with Smart Rules";
            }

        }

        public async Task SetEmailPeople(IAMPRepository ampRepository,IPersonService personService, string sender, string recipient, string projectId)
        {
            
            if (string.IsNullOrWhiteSpace(sender))
            {
                throw  new ArgumentException("string is null or blank","sender");
            }

            if (string.IsNullOrWhiteSpace(recipient))
            {
                throw new ArgumentException("string is null or blank", "recipient");
            }

            IEnumerable<Team> projectTeam = ampRepository.GetTeam(projectId);
            List<string> empNoList = projectTeam.Select(x => x.TeamID).Distinct().ToList();

            await SetSenderEmail(personService, sender);

            await SetRecipientEmail(personService, recipient);

            await SetSRO(personService, projectTeam);

            await SetCCRecipients(personService, empNoList);
        }

        private async Task SetSRO(IPersonService personService, IEnumerable<Team> projectTeam)
        {
            Team sroTeam = projectTeam.FirstOrDefault(x => x.RoleID == "SRO");
            if (sroTeam != null)
            {
                if (!string.IsNullOrEmpty(sroTeam.TeamID))
                {
                    SRO = await personService.GetPersonDetails(sroTeam.TeamID);
                }
            }
        }

        private async Task SetCCRecipients(IPersonService personService, List<string> empNoList)
        {
            IEnumerable<PersonDetails> personDetailsList = await personService.GetPeopleDetails(empNoList);

            bool firstElement = true;
            foreach (PersonDetails personDetails in personDetailsList)
            {
                if (personDetails != null)
                {
                    if (!string.IsNullOrEmpty(personDetails.Email))
                    {
                        if (firstElement) //add email without a comma
                        {
                            Cc = personDetails.Email;
                            firstElement = false;
                        }
                        else
                        {
                            Cc = Cc + "," + personDetails.Email;
                        }
                    }
                }
            }
        }

        private async Task SetRecipientEmail(IPersonService personService, string recipient)
        {
            try
            {
                Recipient = await personService.GetPersonDetails(recipient);
            }
            catch (Exception exception)
            {
                throw new HttpRequestException("Person Service threw an error when retrieving details for recipient " +
                                               recipient + " with a message: " + exception.Message);
            }
            To = Recipient.Email;
        }

        private async Task SetSenderEmail(IPersonService personService, string sender)
        {
            try
            {
                Sender = await personService.GetPersonDetails(sender);
            }
            catch (Exception exception)
            {
                throw new HttpRequestException("Person Service threw an error when retrieving details for sender " + sender +
                                               " with a message: " + exception.Message);
            }
            From = AMPUtilities.SenderEmail();
        }

        public async Task SetHoDAlertPeople(IAMPRepository ampRepository, IPersonService personService, string sender, string projectId)
        {
            if (string.IsNullOrWhiteSpace(sender))
            {
                throw new ArgumentException("string is null or blank", "sender");
            }

            if (string.IsNullOrWhiteSpace(projectId))
            {
                throw new ArgumentException("string is null or blank", "projectId");
            }

            //Get the list of HoD approvers for the project
            IEnumerable<vHoDBudCentLookup> HoDAlertPeople = ampRepository.GetHoDAlertRecipients(projectId);

            if (HoDAlertPeople == null)
            {
                throw new NullReferenceException("SetHodAlertPeople. Null returned for project " + projectId);
            }

            List<vHoDBudCentLookup> HodAlertPeopleOrderedList = HoDAlertPeople.OrderBy(x=>x.GradeRank).ToList();

            if (HodAlertPeopleOrderedList.Any())
            {
                if (HodAlertPeopleOrderedList.Count() == 1)
                {
                    vHoDBudCentLookup HodAlertLookUp = HodAlertPeopleOrderedList.FirstOrDefault();
                    if (HodAlertLookUp != null)
                        await SetRecipientEmail(personService, HodAlertLookUp.EmpNo);
                }
                else
                {
                    //Highest grade gets to be recipient. Everyone else is on the cc list.
                    string recipientEmpNo = HodAlertPeopleOrderedList.First().EmpNo;

                    List<String> ccEmpNoList = HodAlertPeopleOrderedList.Skip(1).Select(x => x.EmpNo).Distinct().ToList();

                    await SetRecipientEmail(personService, recipientEmpNo);

                    await SetCCRecipients(personService, ccEmpNoList);
                }

            }
            else
            {
                throw new ArgumentOutOfRangeException("SetHodAlertPeople. No HoD alerts returned for project " + projectId);

            }
            IEnumerable<Team> projectTeam = ampRepository.GetTeam(projectId);

            await SetSRO(personService, projectTeam);


            await SetSenderEmail(personService, sender);
        }

        #endregion
    }
}