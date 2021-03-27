using Ionic.Zip;
using PagedList;
using Practise.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practise.Controllers
{
    public class UserActivityStatusController : Controller
    {
        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: UserActivityStatus
        public ActionResult MyDownload(string searchTxt, int? page, string sortOrder)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            List<tblDownload> tblDownload = dbObj.tblDownloads.Where(x => x.Downloader == user.ID && x.IsSellerHasAllowedDownload == true).ToList();
            List<tblUser> tblUser = dbObj.tblUsers.ToList();
            List<tblUserProfile> tblUserProfile = dbObj.tblUserProfiles.ToList();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.AttachmentDateSortParm = String.IsNullOrEmpty(sortOrder) ? "attachmentdate_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "";
            ViewBag.SellTypeParm = String.IsNullOrEmpty(sortOrder) ? "selltype_desc" : "";
            ViewBag.PriceParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            var multiple = (from d in tblDownload
                            join u in tblUser on d.Downloader equals u.ID into table1
                            from u in table1.ToList()
                            join userpro in tblUserProfile on d.Downloader equals userpro.UserID into table2
                            from userpro in table2.ToList().DefaultIfEmpty()
                            select new BuyerRequestModal
                            {
                                objdownload = d,
                                objuserProfile = userpro,
                                objuser = u
                            }).AsQueryable();

            if (!String.IsNullOrEmpty(searchTxt))
            {
                multiple = multiple.Where(x => x.objdownload.NoteTitle.Contains(searchTxt) || x.objdownload.NoteCategory.Contains(searchTxt) ||
                                          x.objuser.EmailID.Contains(searchTxt) || searchTxt == null || x.objuserProfile.PhoneNumber.Contains(searchTxt) || x.objdownload.PurchasedPrice.ToString().Contains(searchTxt));
            }

            switch (sortOrder)
            {
                case "attachmentdate_desc":
                    multiple = multiple.OrderBy(x => x.objdownload.AttachmentDownloadedDate);
                    break;
                case "title_desc":
                    multiple = multiple.OrderBy(x => x.objdownload.NoteTitle);
                    break;
                case "category_desc":
                    multiple = multiple.OrderBy(x => x.objdownload.NoteCategory);
                    break;
                case "selltype_desc":
                    multiple = multiple.OrderBy(x => x.objdownload.IsPaid);
                    break;
                case "price_desc":
                    multiple = multiple.OrderBy(x => x.objdownload.PurchasedPrice);
                    break;
                default:
                    multiple = multiple.OrderByDescending(x => x.objdownload.AttachmentDownloadedDate);
                    break;
            }


            return View(multiple.ToPagedList(page ?? 1, 5));
        }

        [Authorize]
        public ActionResult DownloadNote(int id)
        {

            tblSellerNote sellerNote = dbObj.tblSellerNotes.Find(id);

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(Server.MapPath("~/Images/" + sellerNote.SellerID + "/" + id + "/" + "Attachment"));

                    MemoryStream output = new MemoryStream();
                    zip.Save(output);
                    return File(output.ToArray(), "Attachment/zip", "Note.zip");
                }
            
        }

        public ActionResult AddReview()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddReview(int id,int rate, string comment)
        {
            tblDownload download = dbObj.tblDownloads.Where(x => x.NoteID == id).FirstOrDefault();
            var EmailId = User.Identity.Name.ToString();
            
            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            tblSellerNote sellerNote = dbObj.tblSellerNotes.Where(x => x.ID == id).FirstOrDefault();

            tblSellerNotesReview sellerNotesReview1 = dbObj.tblSellerNotesReviews.Where(x => x.NoteID == sellerNote.ID && x.ReviewedByID == user.ID).FirstOrDefault();
            if (sellerNotesReview1 != null)
            {
                sellerNotesReview1.Ratings = rate;
                sellerNotesReview1.Comments = comment;
                sellerNotesReview1.ModifiedBy = user.ID;
                sellerNotesReview1.ModifiedDate = DateTime.Now;
                dbObj.Entry(sellerNotesReview1).State = EntityState.Modified;
                dbObj.SaveChanges();
            }

            else
            {
                tblSellerNotesReview sellerNotesReview = new tblSellerNotesReview()
                {
                    NoteID = sellerNote.ID,
                    Ratings = rate,
                    Comments = comment,
                    ReviewedByID = user.ID,
                    AgainstDownloadsID = download.ID,
                    CreatedDate = DateTime.Now,
                    CreatedBy=download.Downloader,
                    IsActive = true
                };
                dbObj.tblSellerNotesReviews.Add(sellerNotesReview);
                dbObj.SaveChanges();
            }
            return RedirectToAction("DashBoard", "SellerNotes");
        }

        public ActionResult MyRejectedNotes()
        {
            return View();
        }

        public ActionResult MySoldNotes(string searchTxt, int? page, string sortOrder)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            List<tblDownload> tblDownload = dbObj.tblDownloads.Where(x => x.Seller == user.ID && x.IsSellerHasAllowedDownload==true).ToList();
            List<tblUser> tblUser = dbObj.tblUsers.ToList();
            List<tblUserProfile> tblUserProfile = dbObj.tblUserProfiles.ToList();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.AttachmentDateSortParm = String.IsNullOrEmpty(sortOrder) ? "attachmentdate_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "";
            ViewBag.SellTypeParm = String.IsNullOrEmpty(sortOrder) ? "selltype_desc" : "";
            ViewBag.PriceParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";

            var multiple = (from d in tblDownload
                            join u in tblUser on d.Downloader equals u.ID into table1
                            from u in table1.ToList()
                            join userpro in tblUserProfile on d.Downloader equals userpro.UserID into table2
                            from userpro in table2.ToList().DefaultIfEmpty()
                            select new BuyerRequestModal
                            {
                                objdownload = d,
                                objuserProfile = userpro,
                                objuser = u
                            }).AsQueryable();

            if (!String.IsNullOrEmpty(searchTxt))
            {
                multiple = multiple.Where(x => x.objdownload.NoteTitle.Contains(searchTxt) || x.objdownload.NoteCategory.Contains(searchTxt) ||
                                          x.objuser.EmailID.Contains(searchTxt) || searchTxt == null || x.objuserProfile.PhoneNumber.Contains(searchTxt) || x.objdownload.PurchasedPrice.ToString().Contains(searchTxt));
            }

            switch (sortOrder)
            {
                case "attachmentdate_desc":
                    multiple = multiple.OrderBy(x => x.objdownload.AttachmentDownloadedDate);
                    break;
                case "title_desc":
                    multiple = multiple.OrderBy(x => x.objdownload.NoteTitle);
                    break;
                case "category_desc":
                    multiple = multiple.OrderBy(x => x.objdownload.NoteCategory);
                    break;
                case "selltype_desc":
                    multiple = multiple.OrderBy(x => x.objdownload.IsPaid);
                    break;
                case "price_desc":
                    multiple = multiple.OrderBy(x => x.objdownload.PurchasedPrice);
                    break;
                default:
                    multiple = multiple.OrderByDescending(x => x.objdownload.AttachmentDownloadedDate);
                    break;
            }


            return View(multiple.ToPagedList(page ?? 1, 5));
        }
    }
}