using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net.Mail;
using AMP.EmailFactory;
using AMP.Utilities;

namespace AMP.Services
{
   

    public class EmailService : IEmailService
    {

        public void SendEmail(Email email)
        {
            SendEmail(email.To, email.Cc, email.Bcc, email.From, email.Subject, email.Body, email.Priority);
        }

        public  void SendEmail(AMPUtilities.AMPEmail emailContainer)
        {
            SendEmail(emailContainer.To, emailContainer.Cc, emailContainer.Bcc, emailContainer.From, emailContainer.Subject, emailContainer.Body, emailContainer.Priority);
        }

        public void SendEmail(string aTo, string aCc, string aBcc, string aFrom, string aSubject, string aBody, MailPriority aPriority)
        {
            string from = aFrom, body = aBody.Replace('\r', ' ').Replace('\n', ' ');
            aSubject = aSubject.Replace('\r', ' ').Replace('\n', ' ');
            MailMessage mail = new MailMessage(from, aTo, aSubject, aBody);

            try
            {
                if (!String.IsNullOrEmpty(aCc))
                {
                    string[] CCId = aCc.Split(',');

                    foreach (string CCEmail in CCId)
                    {
                        mail.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                    }
                }
                   

                if (!String.IsNullOrEmpty(aBcc))
                    mail.Bcc.Add(new MailAddress(aBcc));
                    

                if (mail.To.Count == 0 && mail.CC.Count == 0)
                {
                    body = string.Format("No recipients were specified for the below email. Investigation is required.<br /><br />{0}", body);
                    body = body + "</span>";
                    mail = new MailMessage(from, aTo, aSubject, body);
                }
                else
                    mail.Bcc.Add(new MailAddress(aTo, from));
                
                if (AMPUtilities.AppMode() == "DEV")
                {
                    body = string.Format("In LIVE environment the email would have been sent to {0} <br /><br />CC:{1}<br /><br />{2}", aTo,aCc,body);
                    mail = new MailMessage(from, AMPUtilities.TestEmail(), aSubject, body);                   
                }
                
                mail.Priority = aPriority;
                mail.IsBodyHtml = true;

                //Copy AMPFeedback in for all email comms.
                mail.Bcc.Add(new MailAddress(ConfigurationManager.AppSettings["AMPFeedback"].ToString()));

                SmtpClient client = new SmtpClient(AMPUtilities.SMTPClient()); 
                client.UseDefaultCredentials = true;

                client.Send(mail);

           
            }
            catch (Exception ex)
            {
                string exc = ex.ToString();
                
              
            }   
        }
    }
}