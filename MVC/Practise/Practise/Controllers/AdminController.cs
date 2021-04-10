using Practise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;
using System.IO;
using Ionic.Zip;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Practise.Controllers
{
    public class AdminController : Controller
    {
        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: Admin
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult AdminDashBoard(int? pagepublish, string searchNote, string sortOrder1)
        {
            ViewBag.CurrentSort1 = sortOrder1;
            ViewBag.TitleSortParm1 = String.IsNullOrEmpty(sortOrder1) ? "title_desc1" : "";
            ViewBag.AddedDateSortParm1 = String.IsNullOrEmpty(sortOrder1) ? "addedate_desc1" : "";
            ViewBag.CategorySortParm1 = String.IsNullOrEmpty(sortOrder1) ? "category_desc1" : "";
            ViewBag.SellTypeParm1 = String.IsNullOrEmpty(sortOrder1) ? "selltype_desc1" : "";
            ViewBag.PriceParm1 = String.IsNullOrEmpty(sortOrder1) ? "price_desc1" : "";
            ViewBag.PublishParm1 = String.IsNullOrEmpty(sortOrder1) ? "publish_desc1" : "";
            ViewBag.DownloadNoParm1 = String.IsNullOrEmpty(sortOrder1) ? "downloadno_desc1" : "";
            
            ViewBag.CurrentFilter1 = searchNote;
            List<tblSellerNote> tblSellers = dbObj.tblSellerNotes.Where(x => x.IsActive == true).ToList();
            List<tblNoteCategory> tblNotes = dbObj.tblNoteCategories.ToList();
            List<tblReferenceData> tblReferenceDatas = dbObj.tblReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value != "Rejected" && x.Value != "Removed").ToList();
            List<SelectListItem> Month = new List<SelectListItem>();
            for(int i = 0; i <= 5; i++)
            {
                var previousMonth = DateTime.Now.AddMonths(-i);
                Month.Add(new SelectListItem()
                {
                    Text=previousMonth.Date.ToString("MMMM"),
                    Value=previousMonth.Month.ToString()
                }
                    );
            }
            ViewBag.Month = Month.ToList();

            var multipletable = from s in tblSellers
                                join n in tblNotes on s.Category equals n.ID into table1
                                from n in table1.ToList()
                                join r in tblReferenceDatas on s.Status equals r.ID into table2
                                from r in table2.ToList()
                                where (r.Value == "Published")
                                select new DashboardModel
                                {
                                    sellernoteinfo = s,
                                    referenceDatainfo = r,
                                    noteCategoryinfo = n
                                };


            if (!String.IsNullOrEmpty(searchNote))
            {
                multipletable = multipletable.Where(x => x.noteCategoryinfo.Name.Contains(searchNote) || x.sellernoteinfo.Title.Contains(searchNote) || searchNote == null ||
                                                        x.sellernoteinfo.SellingPrice.ToString().Contains(searchNote) || x.sellernoteinfo.tblUser.FirstName.Contains(searchNote) ||
                                                        x.sellernoteinfo.PublishedDate.ToString().Contains(searchNote));
            }

            switch (sortOrder1)
            {
                case "addedate_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.PublishedDate);
                    break;
                case "title_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.Title);
                    break;
                case "category_desc1":
                    multipletable = multipletable.OrderBy(x => x.noteCategoryinfo.Name);
                    break;
                case "selltype_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.IsPaid);
                    break;
                case "price_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.SellingPrice);
                    break;
                case "publish_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.tblUser.FirstName);
                    break;
                case "downloadno_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.tblDownloads.Where(a => a.IsSellerHasAllowedDownload == true).Select(a => a.NoteID).Count());
                    break;
                default:
                    multipletable = multipletable.OrderByDescending(x => x.sellernoteinfo.CreatedDate);
                    break;
            }

            var today = DateTime.Now;
            var last7day = today.AddDays(-7);
            ViewBag.InReviewNotes = dbObj.tblSellerNotes.Where(x => x.tblReferenceData.Value == "In Review").Count();
            ViewBag.DownloadNotes = dbObj.tblDownloads.Where(x=>x.IsSellerHasAllowedDownload==true && x.CreatedDate>=last7day).Count();
            ViewBag.NewRegistration = dbObj.tblUsers.Where(x => x.IsEmailVarified == true && x.CreatedDate >= last7day && x.tblUserRole.Name=="Member").Count();
            return View(multipletable.ToPagedList(pagepublish ?? 1, 5));
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult AdminProfile()
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            tblUserProfile tblUserProfile = dbObj.tblUserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();

            ViewBag.CountryCode = dbObj.tblCountries.Where(x => x.IsActive == true);

            if (tblUserProfile != null)
            {
                AdminUpdateProfile userProfile1 = new AdminUpdateProfile()
                {
                    UserID = user.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID,
                    SecondaryEmailAddress = tblUserProfile.SecondaryEmailAddress,
                    CountryCode = tblUserProfile.CountryCode,
                    PhoneNumber = tblUserProfile.PhoneNumber,
                    CreatedDate = DateTime.Now
                };
                TempData["ProfilePicture"] = tblUserProfile.ProfilePicture;
                return View(userProfile1);
            }

            else
            {
                AdminUpdateProfile updateProfile = new AdminUpdateProfile()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailID = user.EmailID
                };
                return View(updateProfile);
            }

            return RedirectToAction("AdminDashBoard", "Admin");
        }

        [HttpPost]
        public ActionResult AdminProfile(AdminUpdateProfile updateProfile)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                tblUserProfile userProfile1 = dbObj.tblUserProfiles.Where(x => x.UserID == updateProfile.UserID).FirstOrDefault();

                if (userProfile1 != null)
                {
                    userProfile1.tblUser.FirstName = updateProfile.FirstName;
                    userProfile1.tblUser.LastName = updateProfile.LastName;
                    userProfile1.CountryCode = updateProfile.CountryCode;
                    userProfile1.PhoneNumber = updateProfile.PhoneNumber;
                    userProfile1.SecondaryEmailAddress = updateProfile.SecondaryEmailAddress;
                    userProfile1.ModifiedDate = DateTime.Now;
                    userProfile1.ModifiedBy = user.ID;

                    string storepath = Path.Combine(Server.MapPath("/Images/" + user.ID));

                    if (!Directory.Exists(storepath))
                    {
                        Directory.CreateDirectory(storepath);
                    }

                    //If user upload Display picture
                    if (updateProfile.ProfilePicture != null && updateProfile.ProfilePicture.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileNameWithoutExtension(updateProfile.ProfilePicture.FileName);
                        string extension = Path.GetExtension(updateProfile.ProfilePicture.FileName);
                        _FileName = _FileName + DateTime.Now.ToString("ddmmyyyy") + extension;
                        string finalp = Path.Combine(storepath, _FileName);
                        updateProfile.ProfilePicture.SaveAs(finalp);
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
                        CountryCode = updateProfile.CountryCode,
                        SecondaryEmailAddress = updateProfile.SecondaryEmailAddress,
                        PhoneNumber = updateProfile.PhoneNumber,
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.ID
                    };


                    string storepath = Path.Combine(Server.MapPath("/Images/" + user.ID));

                    if (!Directory.Exists(storepath))
                    {
                        Directory.CreateDirectory(storepath);
                    }

                    //If user upload Display picture
                    if (updateProfile.ProfilePicture != null && updateProfile.ProfilePicture.ContentLength > 0)
                    {
                        string _FileName = Path.GetFileNameWithoutExtension(updateProfile.ProfilePicture.FileName);
                        string extension = Path.GetExtension(updateProfile.ProfilePicture.FileName);
                        _FileName = DateTime.Now.ToString("ddmmyyyy") + extension;
                        string finalp = Path.Combine(storepath, _FileName);
                        updateProfile.ProfilePicture.SaveAs(finalp);
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

            ViewBag.CountryCode = dbObj.tblCountries.Where(x => x.IsActive == true);
            return RedirectToAction("AdminDashBoard", "Admin");

        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult AdminNoteDetails(int id)
        {
            tblSellerNote sellerNote = dbObj.tblSellerNotes.Find(id);

            return View(sellerNote);
        }

        public ActionResult Download(int id)
        {
            tblSellerNote sellerNote = dbObj.tblSellerNotes.Where(x => x.ID == id && x.tblReferenceData.Value == "Published").FirstOrDefault();
            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(Server.MapPath("~/Images/" + sellerNote.SellerID + "/" + id + "/" + "Attachment"));

                MemoryStream output = new MemoryStream();
                zip.Save(output);
                return File(output.ToArray(), "Attachment/zip", "Note.zip");
            }
        }

        public ActionResult DeleteReView(int id)
        {
            var delete = (from review in dbObj.tblSellerNotesReviews where review.NoteID == id select review).FirstOrDefault();
            dbObj.tblSellerNotesReviews.Remove(delete);
            dbObj.SaveChanges();
            return RedirectToAction("AdminDashBoard","Admin");
        }
    }
}