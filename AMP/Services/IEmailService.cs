using System.Net.Mail;
using AMP.EmailFactory;

namespace AMP.Services
{
    public interface IEmailService
    {
        void SendEmail(Email email);

        void SendEmail(string aTo, string aCc, string aBcc, string aFrom, string aSubject, string aBody,MailPriority aPriority);
    }
}