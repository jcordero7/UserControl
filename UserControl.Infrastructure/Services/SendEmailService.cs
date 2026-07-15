using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Threading.Tasks;
using UserControl.Application.Interfaces;
using UserControl.Core.Enumerations;
using UserControl.Core.Interfaces;
using UserControl.Infrastructure.Constants;
using UserControl.Infrastructure.Options;

namespace UserControl.Infrastructure.Services
{
    public class SendEmailService : ISendEmailService
    {
        private SmtpClient Cliente { get; }
        private EmailSenderOptions Options { get; }

        public SendEmailService(IOptions<EmailSenderOptions> options)
        {
            Options = options.Value;

            Cliente = new SmtpClient("smtp.gmail.com")
            {
                // Host = Options.Host,
                EnableSsl = Options.EnableSsl,
                UseDefaultCredentials = false,
                Port = Options.Port,
                // DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(Options.Email, Options.Password),
            };
        }

      
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var correo = new MailMessage(from: Options.Email, to: email, subject: subject, body: message);
            correo.IsBodyHtml = true;
            return Cliente.SendMailAsync(correo);
        }


        ////public Task SendEmailAsync(string email, UserState state, string numRamdon = null)
        ////{
        ////    string cuerpo;
        ////    var message = EmailMessagesUserStatus.GetEmailMessage(state);

        ////    if (numRamdon != null)
        ////    {
        ////        cuerpo = string.Format(message.Item2, numRamdon);
        ////    }
        ////    else
        ////    {
        ////        cuerpo = message.Item2;
        ////    }


        ////    var correo = new MailMessage(from: Options.Email, to: email, subject: message.Item1, body: cuerpo);
        ////    correo.IsBodyHtml = true;
        ////    return Cliente.SendMailAsync(correo);
        ////}

    }
}
