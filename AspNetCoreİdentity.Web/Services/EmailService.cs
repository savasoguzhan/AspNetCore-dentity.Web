using AspNetCoreİdentity.Web.OptionsModel;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetCoreİdentity.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSetting;


        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSetting = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetPasswordEmailLink, string toEmail)
        {
            var smptCleint = new SmtpClient();

            smptCleint.DeliveryMethod = SmtpDeliveryMethod.Network;
            smptCleint.UseDefaultCredentials = false;
            smptCleint.Port = 507;
            smptCleint.Credentials = new NetworkCredential(_emailSetting.Email, _emailSetting.Password);
            smptCleint.EnableSsl = true;
            smptCleint.Host = _emailSetting.Host;

            var mailMessage = new MailMessage();

            mailMessage.From=new MailAddress(_emailSetting.Email);
            mailMessage.To.Add(toEmail);
            mailMessage.Subject = "Local Host Sifre Sifirlama Linki";
            mailMessage.Body = @$"<h4>Sifrenizi Yenilemek Icin Asagidaki Linke Tiklayiniz</h4>
                              <p><a href='{resetPasswordEmailLink}'></a></p>";

            mailMessage.IsBodyHtml = true;

             await smptCleint.SendMailAsync(mailMessage);


        }
    }
}
