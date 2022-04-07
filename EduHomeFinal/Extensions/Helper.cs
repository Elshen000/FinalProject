using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EduHomeFinal.Extensions
{
    public static class Helper
    {
        public static async Task SendMessage(string messageSubject, string messageBody, string mailTo)
        {

            SmtpClient client = new SmtpClient("smtp.yandex.com", 587);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("elshen.q@itbrains.az", "hxcdqwqoimwxnnub");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage message = new MailMessage("elshen.q@itbrains.az", mailTo);
            message.Subject = messageSubject;
            message.Body = messageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true;


            await client.SendMailAsync(message);


        }
        public enum Roles
        {
            Admin,
            Member,
            Moderator
        }
        //public static async Task SendMessage(string messageSubject, string messageBody, string mailTo)
        //{

        //    SmtpClient client = new SmtpClient("smtp.yandex.com", 587);
        //    client.UseDefaultCredentials = false;
        //    client.EnableSsl = true;
        //    client.Credentials = new NetworkCredential("elshen.q@itbrains.az", "iacsarxixmnbtroi");
        //    client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    MailMessage message = new MailMessage("elshen.q@itbrains.az", mailTo);
        //    message.Subject = messageSubject;
        //    message.Body = messageBody;
        //    message.BodyEncoding = System.Text.Encoding.UTF8;
        //    message.IsBodyHtml = true;


        //    await client.SendMailAsync(message);


        //}
    }
}
