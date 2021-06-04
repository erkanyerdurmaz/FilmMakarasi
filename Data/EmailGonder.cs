using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace FilmMakarasi.Data
{
    public class EmailGonder : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress("admin@optikanaliz.online", "Yönetim", System.Text.Encoding.UTF8),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            mail.To.Add(email);
            SmtpClient smp = new SmtpClient
            {
                Host = "mail.optikanaliz.online",
                Credentials = new NetworkCredential("admin@optikanaliz.online", "v_9mz7E0:V.E@L7p"),
                Port = 587,
                EnableSsl = false,
            };
            smp.Send(mail);

            return Task.CompletedTask;
        }
    }
}