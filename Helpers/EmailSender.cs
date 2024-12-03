using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;

namespace myportfolio.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //string fromMail = "alamgirkabir45789@gmail.com";
            //string fromPassword = "zobgyeyyhyirktps";
            //string fromMail = "reapon.test@gmail.com";
            //string fromPassword = "ktvtcqyqojqznrqh";

            var fromMail = _config.GetSection("EmailConfiguration:From").Value;
            var port = _config.GetSection("EmailConfiguration:Port").Value;
            var smtpServer = _config.GetSection("EmailConfiguration:SmtpServer").Value;
            var fromPassword = _config.GetSection("EmailConfiguration:Password").Value;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient(smtpServer)
            {
                Port = Convert.ToInt32(port),
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,
                UseDefaultCredentials = false
            };
            smtpClient.Send(message);
        }
    }
}
