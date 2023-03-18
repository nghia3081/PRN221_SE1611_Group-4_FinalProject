using SE1611_Group_4_Final_Project.Repository.Interfaces;
using SE1611_Group_4_Final_Project.Utils.AppSetting;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SE1611_Group_4_Final_Project.Repository
{
    public class EmailSender : IEmailSender
    {
        private readonly ApplicationSetting _configuration;
        private readonly string _from;
        private readonly string _smtpRemote;
        private readonly int _smtpPort;
        private readonly string _psw;
        public EmailSender()
        {
            _configuration = ApplicationSetting.Instance;
            _from = _configuration.MailServiceEmail;
            _smtpRemote = _configuration.MailServiceRemote;
            _smtpPort = _configuration.MailServicePort;
            _psw = _configuration.MailServicePassword;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            MailMessage mail = new()
            {
                From = new MailAddress(this._from)
            };
            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;
            mail.BodyEncoding = UTF8Encoding.UTF8;

            SmtpClient smtp = new SmtpClient(this._smtpRemote, this._smtpPort);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl =true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(this._from, this._psw);
            smtp.Timeout = 60000;
            await Task.Run(() => { smtp.Send(mail); });
        }
    }
}
