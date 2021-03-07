using Practise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Practise.Controllers
{
    public class UserController : Controller
    {
        private NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();


        // GET: Welcome
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(VerifyLogin l, string ReturnUrl = "")
        {

            string message = "";

            {
                var v = dbObj.tblUsers.Where(a => a.EmailID == l.EmailID).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(CryptoPassword.Hash(l.Password), v.Password) == 0)
                    {
                        int timeout = l.RememberMe ? 525600 : 20; // 525600 min = 1 year
                        var ticket = new FormsAuthenticationTicket(l.EmailID, l.RememberMe, timeout);
                        string encrypted = FormsAuthentication.Encrypt(ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                        cookie.Expires = DateTime.Now.AddMinutes(timeout);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);


                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("DashBoard", "SellerNotes");
                        }

                    }
                    else
                    {
                        message = "Invalid credential provided";
                    }
                }
                else
                {
                    string errmsg = "Invalid EmailID or PassWord";
                    ViewBag.ErrorMessage = errmsg;
                    return View();
                }
            }
            ViewBag.Message = message;

            return View();
        }
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp([Bind(Exclude = "IsEmailVarified")] UsersMetadata user)
        {
            bool Status = false;
            string message = "";

            //Model Validation
            if (ModelState.IsValid)
            {
                //Email is already Exist
                var isExist = IsEmailExist(user.EmailID);
                if (isExist)
                {
                    ModelState.AddModelError("EmailExist", "Email already exist");
                    return View(user);
                }

                //Passrord Hashing
                user.Password = CryptoPassword.Hash(user.Password);
                user.ConfirmPassword = CryptoPassword.Hash(user.ConfirmPassword);
                user.IsEmailVarified = false;



                tblUserRole objRole = dbObj.tblUserRoles.Where(x => x.Name.ToLower() == "member").FirstOrDefault();
                tblUser objtblUser = new tblUser()
                {
                    RoleID = objRole.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID,
                    Password = user.Password,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    //IsEmailVarified = false
                };
                dbObj.tblUsers.Add(objtblUser);
                dbObj.SaveChanges();

                //Send Email to user
                SendVerificationLinkEmail(user.EmailID.ToString());
                message = "Registration Successfully done.Account activation link" + "has been sent to your email id:" + user.EmailID;
                Status = true;
                //return RedirectToAction("Login", "User");

            }

            ViewBag.Message = message;
            ViewBag.Status = Status;
            return View(user);
        }

        //To Check Is Email Already Exist
        [NonAction]
        public bool IsEmailExist(string emailID)
        {
            using (NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2())
            {
                var v = dbObj.tblUsers.Where(x => x.EmailID == emailID).FirstOrDefault();
                return v != null;
            }
        }



        //Send Verification Mail
        [NonAction]
        public void SendVerificationLinkEmail(string emailID)
        {
            var verifyUrl = "User/VerifyAccount/?emailID=" + emailID;
            var link = Request.Url.AbsoluteUri + verifyUrl;
            var fromEmail = new MailAddress("dnlad22@gmail.com", "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "rishi0712"; // Replace with actual password
            string subject = "Your account is successfully created";
            string body = "<br/>Thank you for signing up with us.Please click on below link to verify your email address and to do login.<br/>";
            body += "<a href='" + link + "'> Click To VerifyEmail</a>";
            body += "<br/><br/>Regards,<br/>";
            body += "Notes Marketplace";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        public ActionResult VerifyAccount()
        {
            return View();
        }

        [HttpGet]
        //Verify Email Address
        public ActionResult VerifyAccount(string emailID)
        {
            //bool Status = false;
            {
                dbObj.Configuration.ValidateOnSaveEnabled = false;
                var v = dbObj.tblUsers.Where(a => a.EmailID == emailID).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVarified = true;
                    ViewBag.firstname = v.FirstName;
                    dbObj.SaveChanges();
                    //Status = true;
                }
                else
                {
                    ViewBag.message("Email not varified");
                }
            }
            return View();

        }


        public ActionResult ForgotPassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(ForgotPsw forgot)
        {
            if (ModelState.IsValid)
            {
                bool isvalid = dbObj.tblUsers.Any(x => x.EmailID == forgot.EmailID);
                if (isvalid)
                {
                    tblUser objtblUser1 = dbObj.tblUsers.Where(x => x.EmailID == forgot.EmailID).FirstOrDefault();
                    //Random Password Generator
                    Random generator = new Random();
                    int otp = generator.Next(100000, 999999);
                    string newotp = otp.ToString("");
                    objtblUser1.Password = CryptoPassword.Hash(newotp);
                    dbObj.SaveChanges();
                    //Send Password to Email
                    SendVerificationCode(forgot.EmailID.ToString(), newotp);
                    dbObj.SaveChanges();
                    return RedirectToAction("Login", "User");
                }

                string msg = "Enter valid EmailID";
                ViewBag.ErrorMsg = msg;
                return View();

            }
            return View();
        }

        [NonAction]
        public void SendVerificationCode(string emailId, string otp)
        {

            var fromEmail = new MailAddress("dnlad22@gmail.com", "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(emailId);
            var fromEmailPassword = "rishi0712"; // Replace with actual password
            string subject = "Reset Password";
            string body = "<br/><br/>We got request for reset your account password.<br/> Please click on the below link to reset your password<br/>";
            body += "Your Password is" + otp;
            body += "Notes Marketplace";
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
        public ActionResult UserProfile()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}