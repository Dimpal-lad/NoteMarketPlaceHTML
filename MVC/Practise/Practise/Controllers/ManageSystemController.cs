using PagedList;
using Practise.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practise.Controllers
{
    public class ManageSystemController : Controller
    {
        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: ManageSystem
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ManageAdministrator(string searchTxt, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstSortParm = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.LastSortParm = String.IsNullOrEmpty(sortOrder) ? "last_desc" : "";
            ViewBag.EmailIDSortParm = String.IsNullOrEmpty(sortOrder) ? "emailId_desc" : "";
            ViewBag.CreatedDateParm = String.IsNullOrEmpty(sortOrder) ? "createddate_desc" : "";
            ViewBag.ActiveParm = String.IsNullOrEmpty(sortOrder) ? "active_desc" : "";

            var objuser = (dbObj.tblUsers.Where(x => x.tblUserRole.Name == "Admin" && (x.FirstName.Contains(searchTxt) ||
                                      searchTxt == null || x.LastName.Contains(searchTxt) || x.EmailID.ToString().Contains(searchTxt) ||
                                      x.tblUserProfiles.Where(m => m.UserID == m.tblUser.ID).Select(m => m.PhoneNumber).Contains(searchTxt) ||
                                      x.CreatedDate.ToString().Contains(searchTxt)))).AsQueryable();

            switch (sortOrder)
            {
                case "createddate_desc":
                    objuser = objuser.OrderBy(x => x.CreatedDate);
                    break;
                case "first_desc":
                    objuser = objuser.OrderBy(x => x.FirstName);
                    break;
                case "last_desc":
                    objuser = objuser.OrderBy(x => x.LastName);
                    break;
                case "emailId_desc":
                    objuser = objuser.OrderBy(x => x.EmailID);
                    break;
                case "active_desc":
                    objuser = objuser.OrderBy(x => x.IsActive);
                    break;
                default:
                    objuser = objuser.OrderByDescending(x => x.CreatedDate);
                    break;
            }
            
            return View(objuser.ToList().ToPagedList(page ?? 1, 5));
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult ManageCategory(int? page,string searchTxt,string sortOrder)
        {
            ViewBag.CategoryNameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DescriptionSortParm = String.IsNullOrEmpty(sortOrder) ? "description_desc" : "";
            ViewBag.CreatedDateParm = String.IsNullOrEmpty(sortOrder) ? "createddate_desc" : "";
            ViewBag.ActiveParm = String.IsNullOrEmpty(sortOrder) ? "active_desc" : "";
            ViewBag.AddedByParm = String.IsNullOrEmpty(sortOrder) ? "addedby_desc" : "";

            var noteCategory = dbObj.tblNoteCategories.Where(x=>x.Name.Contains(searchTxt) || x.Description.Contains(searchTxt) ||
                            x.CreatedDate.ToString().Contains(searchTxt) || x.tblUser.tblNoteCategories.Where(m=>m.CreatedBy ==m.tblUser.ID).Select(m=>m.tblUser.FirstName).Contains(searchTxt)
                            || searchTxt==null);

            switch (sortOrder)
            {
                case "createddate_desc":
                    noteCategory = noteCategory.OrderBy(x=>x.CreatedDate);
                    break;
                case "name_desc":
                    noteCategory = noteCategory.OrderBy(x => x.Name);
                    break;
                case "description_desc":
                    noteCategory = noteCategory.OrderBy(x => x.Description);
                    break;
                case "active_desc":
                    noteCategory = noteCategory.OrderBy(x => x.IsActive);
                    break;
                case "addedby_desc":
                    noteCategory = noteCategory.OrderBy(x => x.tblUser.FirstName);
                    break;
                default:
                    noteCategory = noteCategory.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            return View(noteCategory.ToList().ToPagedList(page ?? 1, 5));
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult ManageCountry(int? page, string searchTxt, string sortOrder)
        {
            ViewBag.CountryNameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CountryCodeSortParm = String.IsNullOrEmpty(sortOrder) ? "countrycode_desc" : "";
            ViewBag.CreatedDateParm = String.IsNullOrEmpty(sortOrder) ? "createddate_desc" : "";
            ViewBag.ActiveParm = String.IsNullOrEmpty(sortOrder) ? "active_desc" : "";
            ViewBag.AddedByParm = String.IsNullOrEmpty(sortOrder) ? "addedby_desc" : "";

            var country = dbObj.tblCountries.Where(x => x.Name.Contains(searchTxt) || x.CountryCode.Contains(searchTxt) ||
                       x.CreatedDate.ToString().Contains(searchTxt) || x.tblUser.tblCountries.Where(m => m.CreatedBy == m.tblUser.ID).Select(m => m.tblUser.FirstName).Contains(searchTxt) ||
                           searchTxt == null);

            switch (sortOrder)
            {
                case "createddate_desc":
                    country = country.OrderBy(x => x.CreatedDate);
                    break;
                case "name_desc":
                    country = country.OrderBy(x => x.Name);
                    break;
                case "countrycode_desc":
                    country = country.OrderBy(x => x.CountryCode);
                    break;
                case "addedby_desc":
                    country = country.OrderBy(x => x.tblUser.FirstName);
                    break;
                case "active_desc":
                    country = country.OrderBy(x => x.IsActive);
                    break;
                default:
                    country = country.OrderByDescending(x => x.CreatedDate);
                    break;
            }

            return View(country.ToList().ToPagedList(page ?? 1, 5));
        }

        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ManageSystemConfiguration()
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult ManageSystemConfiguration(ManageSystemConfigurationModel configurationModel)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            if (ModelState.IsValid)
            {

                tblSystemConfiguration systemConfiguration = new tblSystemConfiguration()
                {
                    Key = "Support email address",
                    Value = configurationModel.SupportEmailID,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };

                dbObj.tblSystemConfigurations.Add(systemConfiguration);
                dbObj.SaveChanges();

                tblSystemConfiguration systemConfiguration8 = new tblSystemConfiguration()
                {
                    Key = "Password",
                    Value = configurationModel.Password,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };

                dbObj.tblSystemConfigurations.Add(systemConfiguration8);
                dbObj.SaveChanges();

                tblSystemConfiguration systemConfiguration1 = new tblSystemConfiguration()
                {
                    Key = "Support phone number",
                    Value = configurationModel.PhoneNumber,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };

                dbObj.tblSystemConfigurations.Add(systemConfiguration1);
                dbObj.SaveChanges();

                tblSystemConfiguration systemConfiguration2 = new tblSystemConfiguration()
                {
                    Key = "Email Address(es)",
                    Value = configurationModel.EmailAddresses,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };

                dbObj.tblSystemConfigurations.Add(systemConfiguration2);
                dbObj.SaveChanges();

                tblSystemConfiguration systemConfiguration3 = new tblSystemConfiguration()
                {
                    Key = "Facebook URL",
                    Value = configurationModel.Facebook,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };

                dbObj.tblSystemConfigurations.Add(systemConfiguration3);
                dbObj.SaveChanges();

                tblSystemConfiguration systemConfiguration4 = new tblSystemConfiguration()
                {
                    Key = "Twitter URL",
                    Value = configurationModel.Twitter,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };

                dbObj.tblSystemConfigurations.Add(systemConfiguration4);
                dbObj.SaveChanges();

                tblSystemConfiguration systemConfiguration5 = new tblSystemConfiguration()
                {
                    Key = "Linkedin URL",
                    Value = configurationModel.LinkedIn,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };

                dbObj.tblSystemConfigurations.Add(systemConfiguration5);
                dbObj.SaveChanges();

                tblSystemConfiguration systemConfiguration6 = new tblSystemConfiguration()
                {
                    Key = "Default image for notes",
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };
                string storepath = Server.MapPath("/DefaultImage/");
                if (configurationModel.DisplayPicture != null && configurationModel.DisplayPicture.ContentLength > 0)
                {
                    string _FileName = Path.GetFileNameWithoutExtension(configurationModel.DisplayPicture.FileName);
                    string extension = Path.GetExtension(configurationModel.DisplayPicture.FileName);
                    _FileName = "DN_" + extension;
                    string finalp = Path.Combine(storepath, _FileName);
                    configurationModel.DisplayPicture.SaveAs(finalp);
                    systemConfiguration6.Value = Path.Combine(("/DefaultImage/"), _FileName);
                    dbObj.tblSystemConfigurations.Add(systemConfiguration6);
                    dbObj.SaveChanges();
                }
                

                tblSystemConfiguration systemConfiguration7 = new tblSystemConfiguration()
                {
                    Key = "Default profile picture",
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    ModifiedDate = DateTime.Now,
                    IsActive = true
                };

                string storepath1 = Server.MapPath("/DefaultImage/");
                if (configurationModel.ProfilePicture != null && configurationModel.ProfilePicture.ContentLength > 0)
                {
                    string _FileName = Path.GetFileNameWithoutExtension(configurationModel.ProfilePicture.FileName);
                    string extension = Path.GetExtension(configurationModel.ProfilePicture.FileName);
                    _FileName = "DP_" + extension;
                    string finalp = Path.Combine(storepath1, _FileName);
                    configurationModel.ProfilePicture.SaveAs(finalp);
                    systemConfiguration7.Value = Path.Combine(("/DefaultImage/"), _FileName);
                    dbObj.tblSystemConfigurations.Add(systemConfiguration7);
                    dbObj.SaveChanges();
                }
                
            }

            return RedirectToAction("AdminDashBoard", "Admin");
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult ManageType(int? page, string searchTxt, string sortOrder)
        {
            ViewBag.TypeNameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DescriptionSortParm = String.IsNullOrEmpty(sortOrder) ? "description_desc" : "";
            ViewBag.CreatedDateParm = String.IsNullOrEmpty(sortOrder) ? "createddate_desc" : "";
            ViewBag.ActiveParm = String.IsNullOrEmpty(sortOrder) ? "active_desc" : "";
            ViewBag.AddedByParm = String.IsNullOrEmpty(sortOrder) ? "addedby_desc" : "";

            var noteType = dbObj.tblNoteTypes.Where(x=>x.Name.Contains(searchTxt) || x.Description.Contains(searchTxt) ||
                        x.CreatedDate.ToString().Contains(searchTxt) || x.tblUser.tblNoteTypes.Where(m => m.CreatedBy == m.tblUser.ID).Select(m => m.tblUser.FirstName).Contains(searchTxt) ||
                            searchTxt==null);
            switch (sortOrder)
            {
                case "createddate_desc":
                    noteType = noteType.OrderBy(x => x.CreatedDate);
                    break;
                case "name_desc":
                    noteType = noteType.OrderBy(x => x.Name);
                    break;
                case "description_desc":
                    noteType = noteType.OrderBy(x => x.Description);
                    break;
                case "active_desc":
                    noteType = noteType.OrderBy(x => x.IsActive);
                    break;
                case "addedby_desc":
                    noteType = noteType.OrderBy(x => x.tblUser.FirstName);
                    break;
                default:
                    noteType = noteType.OrderByDescending(x => x.CreatedDate);
                    break;
            }
            
            return View(noteType.ToList().ToPagedList( page ?? 1,5 ));
        }
    }
}