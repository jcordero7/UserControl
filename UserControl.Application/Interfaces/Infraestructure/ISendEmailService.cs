using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UserControl.Core.Enumerations;

namespace UserControl.Application.Interfaces
{
   public interface ISendEmailService
    {
        //void EnviarCorreo(string destinatario, string asunto, string mensaje, bool esHtlm = false);

        //void EnviarCorreo(MailMessage message);

        //Task EnviarCorreoAsync(MailMessage message);

        Task SendEmailAsync(string email, string subject, string message);


    }
}
