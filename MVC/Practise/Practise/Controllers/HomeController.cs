using Practise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Practise.Controllers
{
    public class HomeController : Controller
    {
        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();

        public ActionResult Home()
        {
            return View();
        }


        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult ContactUs()
        {

            if (User.Identity.IsAuthenticated)
            {
                var emailId = User.Identity.Name.ToString();
                tblUser user = dbObj.tblUsers.Where(x => x.EmailID == emailId).FirstOrDefault();

                ContactUsModel contactUs = new ContactUsModel()
                {
                    FullName = user.FirstName + " " + user.LastName,
                    EmailID = user.EmailID
                };
                return View(contactUs);

            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult ContactUs(ContactUsModel contactModel)
        {
            if (ModelState.IsValid)
            {
                var fromEmail = new MailAddress(contactModel.EmailID); //need system email
                var toEmail = new MailAddress("dnlad22@gmail.com");
                string subject = contactModel.FullName + " " + contactModel.Subject;
                string body = "<br/><br/>Hello,<br/><br/>";
                body += contactModel.Comment;
                body += "<br/><br/>Regards,<br/>";
                body += "Notes Marketplace";
                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("dnlad22@gmail.com", "jkqobpmshhmlgumw")
                };

                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                    smtp.Send(message);
            }
            return View();
        }


    }
}