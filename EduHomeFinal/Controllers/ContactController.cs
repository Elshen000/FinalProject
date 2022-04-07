using EduHomeFinal.DAL;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace EduHomeFinal.Controllers
{
    public class ContactController : Controller
    {
        #region DbContext
        private readonly AppDbContext _db;
        public ContactController(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region ContactAdmin
        public ContentResult SendMessage(string message,string email,string subject,string name)
        {
            if (email==null)
            {
                return Content("Please Enter your Email Adress");
            }
            MimeMessage Message = new MimeMessage();
            Message.From.Add(new MailboxAddress("Admin", email));
            Message.To.Add(MailboxAddress.Parse("elwenquliyev513@gmail.com"));
            Message.Body = new TextPart("plain")
            {
                Text = message
            };
            Message.Subject = subject;
            string Email = "elwenquliyev513@gmail.com";
            string Password = "guliyev513";
            SmtpClient client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(Email, Password);
            client.Send(Message);

            return Content("Message sending successfully");
        }
        #endregion



    }
}
