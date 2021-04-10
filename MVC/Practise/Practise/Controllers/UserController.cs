using Practise.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
                var v = dbObj.tblUsers.Where(a => a.EmailID == l.EmailID && a.IsActive==true).FirstOrDefault();
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
                            if (v.RoleID != 1)
                            {
                                return RedirectToAction("AdminDashBoard", "Admin");
                            }
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
            tblSystemConfiguration systemConfiguration = dbObj.tblSystemConfigurations.Where(x => x.Key.ToLower() == "Support email address").FirstOrDefault();
            tblSystemConfiguration systemConfiguration1 = dbObj.tblSystemConfigurations.Where(x => x.Key.ToLower() == "Password").FirstOrDefault();

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
                };
                dbObj.tblUsers.Add(objtblUser);
                dbObj.SaveChanges();

                //Send Email to user
                SendVerificationLinkEmail(user.EmailID.ToString(),systemConfiguration.Value,systemConfiguration1.Value);
                message = "Registration Successfully done.Account activation link" + "has been sent to your email id:" + user.EmailID;
                Status = true;
                
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
        public void SendVerificationLinkEmail(string emailID, string supportemailID, string password)
        {
            tblUser user = dbObj.tblUsers.Where(x => x.IsEmailVarified !=true && x.EmailID == emailID).FirstOrDefault();
            var verifyUrl = "/User/VerifyAccount/?emailID=" + emailID;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            var fromEmail = new MailAddress(supportemailID, "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = password; // Replace with actual password
            string subject = "Note Marketplace - Email Verification";
            string body = "Hello " + " " + user.FirstName + " " + ",<br/>"; 
            body += "<br/>Thank you for signing up with us.Please click on below link to verify your email address and to do login.<br/>";
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

        
        [HttpGet]
        //Verify Email Address
        public ActionResult VerifyAccount(string emailID)
        {
            
            {
                dbObj.Configuration.ValidateOnSaveEnabled = false;
                var v = dbObj.tblUsers.Where(a => a.EmailID == emailID).FirstOrDefault();
                if (v != null)
                {
                    v.IsEmailVarified = true;
                    ViewBag.firstname = v.FirstName;
                    dbObj.SaveChanges();
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
            tblSystemConfiguration systemConfiguration = dbObj.tblSystemConfigurations.Where(x => x.Key.ToLower() == "Support email address").FirstOrDefault();
            tblSystemConfiguration systemConfiguration1 = dbObj.tblSystemConfigurations.Where(x => x.Key.ToLower() == "Password").FirstOrDefault();

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
                    SendVerificationCode(forgot.EmailID.ToString(), newotp,systemConfiguration.Value,systemConfiguration1.Value);
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
        public void SendVerificationCode(string emailId, string otp, string supportemailID, string password)
        {

            var fromEmail = new MailAddress(supportemailID, "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(emailId);
            var fromEmailPassword = password; // Replace with actual password
            string subject = " New Temporary Password has been created for you";
            string body = "<br/><br/>Hello, <br/><br/>";
            body += "<br/>We have generated a new password for you<br/>";
            body += "<br/>Password: " + " "+ otp + " ";
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

        [Authorize(Roles = "Member")]
        public ActionResult UserProfile()
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            tblUserProfile tblUserProfile = dbObj.tblUserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();

            ViewBag.Gender = dbObj.tblReferenceDatas.Where(x => x.IsActive == true && x.RefCategory == "Gender");
            ViewBag.NotesCountry = dbObj.tblCountries.Where(x => x.IsActive == true);
            ViewBag.CountryCode = dbObj.tblCountries.Where(x => x.IsActive == true);
            
            if (tblUserProfile != null)
            {
                UserProfile userProfile1 = new UserProfile()
                {
                    UserID=user.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID,
                    DOB = tblUserProfile.DOB,
                    Gender = tblUserProfile.Gender,
                    CountryCode = tblUserProfile.CountryCode,
                    PhoneNumber = tblUserProfile.PhoneNumber,
                    AddressLine1 = tblUserProfile.AddressLine1,
                    AddressLine2 = tblUserProfile.AddressLine2,
                    City = tblUserProfile.City,
                    State = tblUserProfile.State,
                    ZipCode = tblUserProfile.ZipCode,
                    Country = tblUserProfile.Country,
                    University = tblUserProfile.University,
                    College = tblUserProfile.College,
                    CreatedDate = DateTime.Now
                };
                TempData["ProfilePicture"] = tblUserProfile.ProfilePicture;
                return View(userProfile1);
            }

            else
            {
                UserProfile userProfile = new UserProfile()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID
                };
               return View(userProfile);
            }
            
            return RedirectToAction("DashBoard", "SellerNotes");
        }

        [HttpPost]
        public ActionResult UserProfile(UserProfile userProfile)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                tblUserProfile userProfile1 = dbObj.tblUserProfiles.Where(x => x.UserID == userProfile.UserID).FirstOrDefault();

                if (userProfile1 != null)
                {
                    userProfile1.tblUser.FirstName = userProfile.FirstName;
                    userProfile1.tblUser.LastName = userProfile.LastName;
                    userProfile1.DOB = userProfile.DOB;
                    userProfile1.Gender = userProfile.Gender;
                    userProfile1.CountryCode = userProfile.CountryCode;
                    userProfile1.PhoneNumber = userProfile.PhoneNumber;
                    userProfile1.AddressLine1 = userProfile.AddressLine1;
                    userProfile1.AddressLine2 = userProfile.AddressLine2;
                    userProfile1.City = userProfile.City;
                    userProfile1.State = userProfile.State;
                    userProfile1.ZipCode = userProfile.ZipCode;
                    userProfile1.Country = userProfile.Country;
                    userProfile1.University = userProfile.University;
                    userProfile1.College = userProfile.College;
                    userProfile1.ModifiedDate = DateTime.Now;
                    userProfile1.ModifiedBy = user.ID;

                    string storepath = Path.Combine(Server.MapPath("/Images/" + user.ID));

                    if (!Directory.Exists(storepath))
                    {
                        Directory.CreateDirectory(storepath);
                    }

                    //If user upload Display picture
                    if (userProfile.ProfilePicture != null && userProfile.ProfilePicture.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileNameWithoutExtension(userProfile.ProfilePicture.FileName);
                        string extension = Path.GetExtension(userProfile.ProfilePicture.FileName);
                        _FileName = _FileName + DateTime.Now.ToString("ddmmyyyy") + extension;
                        string finalp = Path.Combine(storepath , _FileName);
                        userProfile.ProfilePicture.SaveAs(finalp);
                        userProfile1.ProfilePicture = Path.Combine(("/Images/" + user.ID + "/"), _FileName);
                        dbObj.SaveChanges();
                    }
                    //If user does not upload Display picture
                    else
                    {
                        userProfile1.ProfilePicture = "/DefaultImage/DP_.jpg";
                        dbObj.SaveChanges();
                    }

                    dbObj.Entry(userProfile1).State = EntityState.Modified;
                    dbObj.SaveChanges();
                }
                else
                {
                    tblUserProfile tblUserProfile = new tblUserProfile()
                    {
                        UserID = user.ID,
                        DOB = userProfile.DOB,
                        Gender = userProfile.Gender,
                        CountryCode = userProfile.CountryCode,
                        PhoneNumber = userProfile.PhoneNumber,
                        AddressLine1 = userProfile.AddressLine1,
                        AddressLine2 = userProfile.AddressLine2,
                        City = userProfile.City,
                        State = userProfile.State,
                        ZipCode = userProfile.ZipCode,
                        Country = userProfile.Country,
                        University = userProfile.University,
                        College = userProfile.College,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.ID
                    };


                    string storepath = Path.Combine(Server.MapPath("/Images/" + user.ID));

                    if (!Directory.Exists(storepath))
                    {
                        Directory.CreateDirectory(storepath);
                    }

                    //If user upload Display picture
                    if (userProfile.ProfilePicture != null && userProfile.ProfilePicture.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileNameWithoutExtension(userProfile.ProfilePicture.FileName);
                        string extension = Path.GetExtension(userProfile.ProfilePicture.FileName);
                        _FileName = DateTime.Now.ToString("ddmmyyyy") + extension;
                        string finalp = Path.Combine(storepath, _FileName);
                        userProfile.ProfilePicture.SaveAs(finalp);
                        tblUserProfile.ProfilePicture = Path.Combine(("/Images/" + user.ID + "/"), _FileName);
                        dbObj.SaveChanges();
                    }
                    //If user does not upload Display picture
                    else
                    {
                        tblUserProfile.ProfilePicture = "/DefaultImage/DP_.jpg";
                        dbObj.SaveChanges();
                    }

                    dbObj.tblUserProfiles.Add(tblUserProfile);
                    dbObj.SaveChanges();
                }
            }
            
            ViewBag.Gender = dbObj.tblReferenceDatas.Where(x => x.IsActive == true && x.RefCategory == "Gender");
            ViewBag.NotesCountry = dbObj.tblCountries.Where(x => x.IsActive == true);
            ViewBag.CountryCode = dbObj.tblCountries.Where(x => x.IsActive == true);
            return RedirectToAction("SearchNotes","SellerNotes");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult ChangePassWord()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassWord(ChangePassword changePassword)
        {
            if (ModelState.IsValid)
            {
                var EmailId = User.Identity.Name.ToString();
                tblUser objuser = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
                var Password = CryptoPassword.Hash(changePassword.OldPassword);

                if (objuser.Password == Password)
                {
                    objuser.Password = CryptoPassword.Hash(changePassword.ConfirmPassword);
                    dbObj.Entry(objuser).State = EntityState.Modified;
                    dbObj.SaveChanges();
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ViewBag.ErrorMessage = "Password does not match";
                }
            }
            
            return View(changePassword);
        }
    }
}