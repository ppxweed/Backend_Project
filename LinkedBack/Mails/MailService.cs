using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Mails
{
    public interface Emails
    {
        Task<Response> SendMail(List<string> mails, string subj, string body); 
    }
    public class MailService : Emails
    {
        public IConfiguration Config {get;}

        public MailService(IConfiguration configuration)
        {
            Config = configuration;
        }

        
        public async Task<Response> SendMail(List<string>  mails, string subj, string body)
        {
            return await ExecuteMail(Config["SENDGRIDKEY"], subj, body, mails);
        }

         public async Task<Response> ExecuteMail(string Key, string subj, string body, List<string> mails)
        {
            var user = new SendGridClient(Key);
            var sms = new SendGridMessage()
            {
                From = new EmailAddress("guillaume.boulben@epita.fr", "Guillaume B"),
                Subject = subj,
                PlainTextContent = body,
                HtmlContent = body
            };

            foreach(var mail in mails)
            {
                sms.AddTo(new EmailAddress(mail));
            }

            Response answer = await user.SendEmailAsync(sms);
            return answer;
        }

    }

   
   
}