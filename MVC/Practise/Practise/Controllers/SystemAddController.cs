using Practise.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Practise.Controllers
{
    public class SystemAddController : Controller
    {
        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: SystemAdd
        public ActionResult AddAdministrator(int? id)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            tblUser user1 = dbObj.tblUsers.Find(id);
            tblUserProfile userProfile = dbObj.tblUserProfiles.Where(x => x.UserID == user1.ID).FirstOrDefault();
            if (user1 != null)
            {
                AdminUpdateProfile adminUpdate = new AdminUpdateProfile()
                {
                    FirstName = user1.FirstName,
                    LastName = user1.LastName,
                    EmailID = user1.EmailID,
                    PhoneNumber = userProfile.PhoneNumber,
                    CountryCode = userProfile.CountryCode
                };
                ViewBag.CountryCode = dbObj.tblCountries.Where(x => x.IsActive == true);
                return View(adminUpdate);
            }
            else
            {
                return View();
            }

           
        }

        [HttpPost]
        public ActionResult AddAdministrator(AdminUpdateProfile admin, int? id)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                tblUserProfile profile = dbObj.tblUserProfiles.Where(x => x.UserID == id).FirstOrDefault();
                if (profile != null)
                {
                    profile.tblUser.FirstName = admin.FirstName;
                    profile.tblUser.LastName = admin.LastName;
                    profile.PhoneNumber = admin.PhoneNumber;
                    profile.CountryCode = admin.CountryCode;
                    profile.ModifiedBy = user.ID;
                    profile.ModifiedDate = DateTime.Now;
                    profile.tblUser.ModifiedBy = user.ID;
                    profile.tblUser.ModifiedDate = DateTime.Now;

                    dbObj.Entry(profile).State = EntityState.Modified;
                    dbObj.SaveChanges();
                }
                else
                {
                    tblUser objUser = new tblUser()
                    {
                        FirstName = admin.FirstName,
                        LastName = admin.LastName,
                        EmailID = admin.EmailID,
                        Password = CryptoPassword.Hash("admin@abc"),
                        IsActive = true,
                        RoleID = 2,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.ID
                    };
                    dbObj.tblUsers.Add(objUser);
                    dbObj.SaveChanges();
                    tblUserProfile userProfile = new tblUserProfile()
                    {
                        UserID = objUser.ID,
                        PhoneNumber = admin.PhoneNumber,
                        CountryCode = admin.CountryCode,
                        CreatedBy = user.ID,
                        CreatedDate = DateTime.Now
                    };
                    dbObj.tblUserProfiles.Add(userProfile);
                    dbObj.SaveChanges();
                    //Send Email to user
                    SendVerificationLinkEmail(admin.EmailID.ToString());
                }
            }
            ViewBag.CountryCode = dbObj.tblCountries.Where(x => x.IsActive == true);
            return RedirectToAction("ManageAdministrator", "ManageSystem");
        }

        //Send Verification Mail
        [NonAction]
        public void SendVerificationLinkEmail(string emailID)
        {
            var verifyUrl = "User/VerifyAccount/?emailID=" + emailID;
            var link = Request.Url.AbsoluteUri + verifyUrl;
            var fromEmail = new MailAddress("dnlad22@gmail.com", "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "jkqobpmshhmlgumw"; // Replace with actual password
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

        //Delete Administrator
        public ActionResult DeleteAdministrator(int id)
        {
            tblUser user = dbObj.tblUsers.Find(id);
            user.IsActive = false;
            dbObj.Entry(user).State = EntityState.Modified;
            dbObj.SaveChanges();
            return RedirectToAction("ManageAdministrator", "ManageSystem");
        }

        public ActionResult AddCategory(int? id)
        {
            tblNoteCategory noteCategory = dbObj.tblNoteCategories.Find(id);
            if (noteCategory != null)
            {
                AddCategoryModel addCategory = new AddCategoryModel()
                {
                    Name = noteCategory.Name,
                    Description = noteCategory.Description
                };
                return View(addCategory);
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        public ActionResult AddCategory(AddCategoryModel categoryModel,int? id)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                tblNoteCategory category = dbObj.tblNoteCategories.Find(id);
                if (category != null)
                {
                    category.Name = categoryModel.Name;
                    category.Description = categoryModel.Description;
                    category.ModifiedBy = user.ID;
                    category.ModifiedDate = DateTime.Now;

                    dbObj.Entry(category).State = EntityState.Modified;
                    dbObj.SaveChanges();
                }
                else
                {
                    tblNoteCategory noteCategory = new tblNoteCategory()
                    {
                        Name = categoryModel.Name,
                        Description = categoryModel.Description,
                        CreatedBy = user.ID,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };
                    dbObj.tblNoteCategories.Add(noteCategory);
                    dbObj.SaveChanges();
                }
            }
            return RedirectToAction("ManageCategory", "ManageSystem");
        }

        //Delete Category
        public ActionResult DeleteCategory(int id)
        {
            tblNoteCategory category = dbObj.tblNoteCategories.Find(id);
            category.IsActive = false;
            dbObj.Entry(category).State = EntityState.Modified;
            dbObj.SaveChanges();
            return RedirectToAction("ManageCategory", "ManageSystem");
        }
        
        public ActionResult AddCountry(int? id)
        {
            tblCountry country = dbObj.tblCountries.Find(id);
            if (country != null)
            {
                AddCountryModel addCountry = new AddCountryModel()
                {
                    CountryName = country.Name,
                    CountryCode = country.CountryCode
                };
                return View(addCountry);
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult AddCountry(AddCountryModel countryModel, int? id)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                tblCountry country = dbObj.tblCountries.Find(id);
                if (country != null)
                {
                    country.Name = countryModel.CountryName;
                    country.CountryCode = countryModel.CountryCode;
                    country.ModifiedBy = user.ID;
                    country.ModifiedDate = DateTime.Now;

                    dbObj.Entry(country).State = EntityState.Modified;
                    dbObj.SaveChanges();
                }
                else
                {
                    tblCountry country1 = new tblCountry()
                    {
                        Name = countryModel.CountryName,
                        CountryCode = countryModel.CountryCode,
                        CreatedBy = user.ID,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };
                    dbObj.tblCountries.Add(country1);
                    dbObj.SaveChanges();
                }
            }
            return RedirectToAction("ManageCountry", "ManageSystem");
        }

        //Delete Country
        public ActionResult DeleteCountry(int id)
        {
            tblCountry country = dbObj.tblCountries.Find(id);
            country.IsActive = false;
            dbObj.Entry(country).State = EntityState.Modified;
            dbObj.SaveChanges();
            return RedirectToAction("ManageCountry", "ManageSystem");
        }

        public ActionResult AddType(int? id)
        {
            tblNoteType noteType = dbObj.tblNoteTypes.Find(id);
            if (noteType != null)
            {
                AddCategoryModel addType = new AddCategoryModel()
                {
                    Name = noteType.Name,
                    Description = noteType.Description
                };
                return View(addType);
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        public ActionResult AddType(AddCategoryModel typeModel, int? id)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                tblNoteType noteType = dbObj.tblNoteTypes.Find(id);
                if (noteType != null)
                {
                    noteType.Name = typeModel.Name;
                    noteType.Description = typeModel.Description;
                    noteType.ModifiedBy = user.ID;
                    noteType.ModifiedDate = DateTime.Now;

                    dbObj.Entry(noteType).State = EntityState.Modified;
                    dbObj.SaveChanges();
                }
                else
                {
                    tblNoteType noteType1 = new tblNoteType()
                    {
                        Name = typeModel.Name,
                        Description = typeModel.Description,
                        CreatedBy = user.ID,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };
                    dbObj.tblNoteTypes.Add(noteType1);
                    dbObj.SaveChanges();
                }
            }
            return RedirectToAction("ManageType", "ManageSystem");
        }

        //Delete Type
        public ActionResult DeleteType(int id)
        {
            tblNoteType noteType = dbObj.tblNoteTypes.Find(id);
            noteType.IsActive = false;
            dbObj.Entry(noteType).State = EntityState.Modified;
            dbObj.SaveChanges();
            return RedirectToAction("ManageType", "ManageSystem");
        }
    }
}