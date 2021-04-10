using Ionic.Zip;
using PagedList;
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

namespace Practise.Controllers
{
    public class UserActivityStatusController : Controller
    {
        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: UserActivityStatus
        [Authorize(Roles = "Member")]
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

        [HttpPost]
        public ActionResult ReportAnIssue(int id, string remark)
        {
            tblDownload download = dbObj.tblDownloads.Where(x => x.NoteID == id).FirstOrDefault();
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            tblSystemConfiguration systemConfiguration = dbObj.tblSystemConfigurations.Where(x => x.Key.ToLower() == "Support email address").FirstOrDefault();
            tblSystemConfiguration systemConfiguration1 = dbObj.tblSystemConfigurations.Where(x => x.Key.ToLower() == "Password").FirstOrDefault();
            tblSystemConfiguration systemConfiguration2 = dbObj.tblSystemConfigurations.Where(x => x.Key.ToLower() == "Email Address(es)").FirstOrDefault();

            tblSellerNote sellerNote = dbObj.tblSellerNotes.Where(x => x.ID == id).FirstOrDefault();
            tblSellerNotesReportedIssue sellerNotesReportedIssue = dbObj.tblSellerNotesReportedIssues.Where(x => x.NoteID == sellerNote.ID && x.ReportedByID == user.ID).FirstOrDefault();
            if (sellerNotesReportedIssue != null)
            {
                sellerNotesReportedIssue.Remarks = remark;
                sellerNotesReportedIssue.ModifiedDate = DateTime.Now;
                sellerNotesReportedIssue.ModifiedBy = user.ID;

                dbObj.Entry(sellerNotesReportedIssue).State = EntityState.Modified;
                dbObj.SaveChanges();
            }
            else
            {
                tblSellerNotesReportedIssue reportedIssue = new tblSellerNotesReportedIssue()
                {
                    NoteID = sellerNote.ID,
                    ReportedByID = user.ID,
                    Remarks = remark,
                    AgainstDownloadID = download.ID,
                    CreatedDate = DateTime.Now,
                    CreatedBy = download.Downloader
                };
                dbObj.tblSellerNotesReportedIssues.Add(reportedIssue);
                dbObj.SaveChanges();
            }
            SendEmailToAdmin(systemConfiguration.Value, systemConfiguration1.Value, systemConfiguration2.Value, id);
            return RedirectToAction("DashBoard", "SellerNotes");

        }

        [NonAction]
        public void SendEmailToAdmin(string supportemailID, string password, string emailID, int id)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            tblSellerNote sellerNote = dbObj.tblSellerNotes.Where(x => x.ID == id).FirstOrDefault();
            tblUser user1 = dbObj.tblUsers.Where(x => x.ID == sellerNote.SellerID).FirstOrDefault();

            var fromEmail = new MailAddress(supportemailID, "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = password; // Replace with actual password
            string subject = user.FirstName + " " + "Reported an issue for" + " " + sellerNote.Title;
            string body = "<br/>Hello Admin,<br/>";
            body += "We want to inform you that," + " " + user.FirstName + " " + "Reported an issue for" + " " + user1.FirstName + "’s   Note with<br/>";
            body+= "title"+" "+sellerNote.Title+" " + ". Please look at the notes and take required actions. ";
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


        public ActionResult AddReview()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddReview(int id, int rate, string comment)
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
                    CreatedBy = download.Downloader,
                    IsActive = true
                };
                dbObj.tblSellerNotesReviews.Add(sellerNotesReview);
                dbObj.SaveChanges();
            }
            return RedirectToAction("DashBoard", "SellerNotes");
        }

        [Authorize(Roles = "Member")]
        public ActionResult MyRejectedNotes(int? pagepublish, string searchNote, string sortOrder1)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            ViewBag.TitleSortParm1 = String.IsNullOrEmpty(sortOrder1) ? "title_desc1" : "";
            ViewBag.EditedDateSortParm1 = String.IsNullOrEmpty(sortOrder1) ? "editedate_desc1" : "";
            ViewBag.CategorySortParm1 = String.IsNullOrEmpty(sortOrder1) ? "category_desc1" : "";
            ViewBag.RemarkParm = String.IsNullOrEmpty(sortOrder1) ? "remark_desc1" : "";


            ViewBag.CurrentFilter1 = searchNote;
            List<tblSellerNote> tblSellers = dbObj.tblSellerNotes.Where(x => x.SellerID == user.ID && x.IsActive == true).ToList();
            List<tblNoteCategory> tblNotes = dbObj.tblNoteCategories.ToList();
            List<tblReferenceData> tblReferenceDatas = dbObj.tblReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value == "Rejected").ToList();

            var multipletable = from s in tblSellers
                                join n in tblNotes on s.Category equals n.ID into table1
                                from n in table1.ToList()
                                join r in tblReferenceDatas on s.Status equals r.ID into table2
                                from r in table2.ToList()
                                where (r.Value != "Published")
                                select new DashboardModel
                                {
                                    sellernoteinfo = s,
                                    referenceDatainfo = r,
                                    noteCategoryinfo = n
                                };
            if (!String.IsNullOrEmpty(searchNote))
            {
                multipletable = multipletable.Where(x => x.noteCategoryinfo.Name.Contains(searchNote) || x.sellernoteinfo.Title.Contains(searchNote) || searchNote == null ||
                                                        x.sellernoteinfo.AdminRemarks.Contains(searchNote) || x.sellernoteinfo.ModifiedDate.ToString().Contains(searchNote));
            }

            switch (sortOrder1)
            {
                case "editedate_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.ModifiedDate);
                    break;
                case "title_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.Title);
                    break;
                case "category_desc1":
                    multipletable = multipletable.OrderBy(x => x.noteCategoryinfo.Name);
                    break;
                case "remark_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.AdminRemarks);
                    break;
                default:
                    multipletable = multipletable.OrderByDescending(x => x.sellernoteinfo.CreatedDate);
                    break;
            }


            return View(multipletable.ToPagedList(pagepublish ?? 1, 5));
        }

        public ActionResult CloneNote(int id)
        {
            tblSellerNote sellerNote = dbObj.tblSellerNotes.Find(id);
            if (sellerNote != null && sellerNote.tblReferenceData.Value == "Rejected")
            {
                sellerNote.Status = dbObj.tblReferenceDatas.Where(x => x.RefCategory.ToLower() == "Notes Status" && x.Value.ToLower() == "Draft").Select(x => x.ID).FirstOrDefault();
                dbObj.Entry(sellerNote).State = EntityState.Modified;
                dbObj.SaveChanges();
            }
            return RedirectToAction("DashBoard", "SellerNotes");
        }

        [Authorize(Roles = "Member")]
        public ActionResult MySoldNotes(string searchTxt, int? page, string sortOrder)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            List<tblDownload> tblDownload = dbObj.tblDownloads.Where(x => x.Seller == user.ID && x.IsSellerHasAllowedDownload == true).ToList();
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