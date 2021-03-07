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

            return View();
        }

        [HttpPost]
        public ActionResult ContactUs(ContactUsModel contactModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msz = new MailMessage();
                    msz.From = new MailAddress(contactModel.EmailID);//Email which you are getting 
                                                                     //from contact us page 
                    msz.To.Add("dnlad22@gmail.com");//Where mail will be sent 
                    msz.Subject = contactModel.Subject;
                    msz.Body = "<br/>Hello,<br/></br>";
                    msz.Body += contactModel.Comment;
                    msz.Body += "<br/><br/>Regards,<br/>";
                    msz.Body += contactModel.FullName;
                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.gmail.com";

                    smtp.Port = 587;

                    smtp.Credentials = new System.Net.NetworkCredential
                    ("dnlad22@gmail.com", "rishi0712");

                    smtp.EnableSsl = true;

                    smtp.Send(msz);
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Sorry we are facing Problem here {ex.Message}";
                }


            }
            return View();
        }
    }
}