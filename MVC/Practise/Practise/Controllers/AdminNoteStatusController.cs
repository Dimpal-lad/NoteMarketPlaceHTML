using PagedList;
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
    public class AdminNoteStatusController : Controller
    {

        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: AdminNoteStatus
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult NotesUnderReview(int? pagepublish, string searchNote, string sortOrder1, string Seller)
        {
            ViewBag.CurrentSort1 = sortOrder1;
            ViewBag.TitleSortParm1 = String.IsNullOrEmpty(sortOrder1) ? "title_desc1" : "";
            ViewBag.AddedDateSortParm1 = String.IsNullOrEmpty(sortOrder1) ? "addedate_desc1" : "";
            ViewBag.CategorySortParm1 = String.IsNullOrEmpty(sortOrder1) ? "category_desc1" : "";
            ViewBag.SellerParm2 = String.IsNullOrEmpty(sortOrder1) ? "seller_desc1" : "";
            ViewBag.StatusParm1 = String.IsNullOrEmpty(sortOrder1) ? "status_desc1" : "";

            ViewBag.CurrentFilter1 = searchNote;
            List<tblSellerNote> tblSellers = dbObj.tblSellerNotes.Where(x => x.IsActive == true).ToList();
            List<tblNoteCategory> tblNotes = dbObj.tblNoteCategories.ToList();
            List<tblReferenceData> tblReferenceDatas = dbObj.tblReferenceDatas.Where(x => x.RefCategory == "Notes Status" && (x.Value == "Submitted For Review" || x.Value == "In Review")).ToList();

            var multipletable = from s in tblSellers
                                join n in tblNotes on s.Category equals n.ID into table1
                                from n in table1.ToList()
                                join r in tblReferenceDatas on s.Status equals r.ID into table2
                                from r in table2.ToList()
                                where (r.Value == "Submitted For Review"|| r.Value == "In Review" && (s.tblUser.ID.ToString() == Seller || String.IsNullOrEmpty(Seller)))
                                select new DashboardModel
                                {
                                    sellernoteinfo = s,
                                    referenceDatainfo = r,
                                    noteCategoryinfo = n
                                };

            if (!String.IsNullOrEmpty(searchNote))
            {
                multipletable = multipletable.Where(x => x.noteCategoryinfo.Name.Contains(searchNote) || x.sellernoteinfo.Title.Contains(searchNote) || searchNote == null ||
                                                        x.sellernoteinfo.tblUser.FirstName.Contains(searchNote) || x.sellernoteinfo.CreatedDate.ToString().Contains(searchNote));
            }

            switch (sortOrder1)
            {
                case "addedate_desc1":
                    multipletable = multipletable.OrderByDescending(x => x.sellernoteinfo.CreatedDate);
                    break;
                case "title_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.Title);
                    break;
                case "category_desc1":
                    multipletable = multipletable.OrderBy(x => x.noteCategoryinfo.Name);
                    break;
                case "seller_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.tblUser.FirstName);
                    break;
                case "status_desc1":
                    multipletable = multipletable.OrderBy(x => x.referenceDatainfo.Value);
                    break;
                default:
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.CreatedDate);
                    break;
            }

            ViewBag.Seller = dbObj.tblUsers.Where(x => x.IsEmailVarified == true && x.RoleID == dbObj.tblUserRoles.Where(a => a.Name.ToLower() == "member").Select(a => a.ID).FirstOrDefault());
            return View(multipletable.ToPagedList(pagepublish ?? 1, 5));

        }


        public ActionResult InReview(int id)
        {
            tblSellerNote sellerNote = dbObj.tblSellerNotes.Find(id);
            if (sellerNote != null && sellerNote.tblReferenceData.Value == "Submitted For Review")
            {
                sellerNote.Status = dbObj.tblReferenceDatas.Where(x => x.RefCategory.ToLower() == "Notes Status" && x.Value.ToLower() == "In Review").Select(x => x.ID).FirstOrDefault();
                dbObj.Entry(sellerNote).State = EntityState.Modified;
                dbObj.SaveChanges();
            }
            return RedirectToAction("AdminDashBoard", "Admin");
        }

        public ActionResult Approve(int id)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            tblSellerNote sellerNote = dbObj.tblSellerNotes.Find(id);
            if (sellerNote != null && (sellerNote.tblReferenceData.Value == "Submitted For Review" || sellerNote.tblReferenceData.Value == "In Review" || sellerNote.tblReferenceData.Value == "Rejected"))
            {
                sellerNote.Status = dbObj.tblReferenceDatas.Where(x => x.RefCategory.ToLower() == "Notes Status" && x.Value.ToLower() == "Published").Select(x => x.ID).FirstOrDefault();
                sellerNote.ActionedBy = user.ID;
                sellerNote.PublishedDate = DateTime.Now;
                dbObj.Entry(sellerNote).State = EntityState.Modified;
                dbObj.SaveChanges();
            }
            return RedirectToAction("AdminDashBoard", "Admin");
        }

        [HttpPost]
        public ActionResult Reject(int id, string remark)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            tblSellerNote sellerNote = dbObj.tblSellerNotes.Find(id);
            if (sellerNote != null && (sellerNote.tblReferenceData.Value == "Submitted For Review" || sellerNote.tblReferenceData.Value == "In Review"))
            {
                sellerNote.Status = dbObj.tblReferenceDatas.Where(x => x.RefCategory.ToLower() == "Notes Status" && x.Value.ToLower() == "Rejected").Select(x => x.ID).FirstOrDefault();
                sellerNote.ActionedBy = user.ID;
                sellerNote.AdminRemarks = remark;
                sellerNote.ModifiedDate = DateTime.Now;
                dbObj.Entry(sellerNote).State = EntityState.Modified;
                dbObj.SaveChanges();
            }
            return RedirectToAction("AdminDashBoard", "Admin");
        }

        [HttpPost]
        public ActionResult Unpublish(int id, string remark)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            tblSystemConfiguration systemConfiguration = dbObj.tblSystemConfigurations.Where(x => x.Key.ToLower() == "Support email address").FirstOrDefault();
            tblSystemConfiguration systemConfiguration1 = dbObj.tblSystemConfigurations.Where(x => x.Key.ToLower() == "Password").FirstOrDefault();
            tblSellerNote sellerNote = dbObj.tblSellerNotes.Find(id);
            if (sellerNote != null && sellerNote.tblReferenceData.Value == "Published")
            {
                tblUser user1 = dbObj.tblUsers.Where(x => x.ID == sellerNote.SellerID).FirstOrDefault();
                sellerNote.Status = dbObj.tblReferenceDatas.Where(x => x.RefCategory.ToLower() == "Notes Status" && x.Value.ToLower() == "Removed").Select(x => x.ID).FirstOrDefault();
                sellerNote.ActionedBy = user.ID;
                sellerNote.AdminRemarks = remark;
                sellerNote.ModifiedDate = DateTime.Now;
                SendEmailtoSeller(user1.EmailID.ToString(), id,systemConfiguration.Value,systemConfiguration1.Value);
                dbObj.Entry(sellerNote).State = EntityState.Modified;
                dbObj.SaveChanges();
            }
            return RedirectToAction("AdminDashBoard", "Admin");
        }

        [NonAction]
        public void SendEmailtoSeller(string emailID, int id, string supportemailID, string password)
        {
            tblUser user1 = dbObj.tblUsers.Where(x => x.EmailID == emailID).FirstOrDefault();
            tblSellerNote sellerNote = dbObj.tblSellerNotes.Where(x => x.SellerID == user1.ID && x.ID == id).FirstOrDefault();
            var fromEmail = new MailAddress(supportemailID, "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = password; // Replace with actual password
            string subject = " Sorry! We need to remove your notes from our portal.";
            string body = "<br/>Hello" + " " + user1.FirstName + " ,<br/>";
            body += "We would like to inform you that,your note" + " " + sellerNote.Title + " " + "has been removed from the portal.<br/>" + "Please find our remarks as below -<br/>" + sellerNote.AdminRemarks;
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


        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult PublishNotes(string searchNote, int? pagepublish, string sortOrder1, string Seller)
        {
            ViewBag.CurrentSort1 = sortOrder1;
            ViewBag.TitleSortParm1 = String.IsNullOrEmpty(sortOrder1) ? "title_desc1" : "";
            ViewBag.PublishDateSortParm1 = String.IsNullOrEmpty(sortOrder1) ? "publishdate_desc1" : "";
            ViewBag.CategorySortParm1 = String.IsNullOrEmpty(sortOrder1) ? "category_desc1" : "";
            ViewBag.SellTypeParm1 = String.IsNullOrEmpty(sortOrder1) ? "selltype_desc1" : "";
            ViewBag.PriceParm1 = String.IsNullOrEmpty(sortOrder1) ? "price_desc1" : "";
            ViewBag.SellerParm1 = String.IsNullOrEmpty(sortOrder1) ? "seller_desc1" : "";
            ViewBag.DownloadNoParm1 = String.IsNullOrEmpty(sortOrder1) ? "downloadno_desc1" : "";

            ViewBag.CurrentFilter1 = searchNote;
            List<tblSellerNote> tblSellers = dbObj.tblSellerNotes.Where(x => x.IsActive == true).ToList();
            List<tblNoteCategory> tblNotes = dbObj.tblNoteCategories.ToList();
            List<tblReferenceData> tblReferenceDatas = dbObj.tblReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value != "Rejected" && x.Value != "Removed").ToList();

            var multipletable = from s in tblSellers
                                join n in tblNotes on s.Category equals n.ID into table1
                                from n in table1.ToList()
                                join r in tblReferenceDatas on s.Status equals r.ID into table2
                                from r in table2.ToList()
                                where (r.Value == "Published" && (s.tblUser.ID.ToString() == Seller || String.IsNullOrEmpty(Seller)))
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
                case "publishdate_desc1":
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

            ViewBag.Seller = dbObj.tblUsers.Where(x => x.IsEmailVarified == true && x.RoleID == dbObj.tblUserRoles.Where(a => a.Name.ToLower() == "member").Select(a => a.ID).FirstOrDefault());
            return View(multipletable.ToPagedList(pagepublish ?? 1, 5));
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        //DownloadNote
        public ActionResult DownloadNote(string searchTxt, int? page, string sortOrder, string Seller, string Note, string Buyer)
        {

            List<tblDownload> tblDownload = dbObj.tblDownloads.Where(x => x.IsSellerHasAllowedDownload == true).ToList();
            List<tblUser> tblUser = dbObj.tblUsers.ToList();
            List<tblUserProfile> tblUserProfile = dbObj.tblUserProfiles.ToList();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.AttachmentDateSortParm = String.IsNullOrEmpty(sortOrder) ? "attachmentdate_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "";
            ViewBag.SellTypeParm = String.IsNullOrEmpty(sortOrder) ? "selltype_desc" : "";
            ViewBag.PriceParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.BuyerParm = String.IsNullOrEmpty(sortOrder) ? "buyer_desc" : "";
            ViewBag.SellerParm = String.IsNullOrEmpty(sortOrder) ? "seller_desc" : "";

            var multiple = (from d in tblDownload
                            join u in tblUser on d.Downloader equals u.ID into table1
                            from u in table1.ToList()
                            join userpro in tblUserProfile on d.Downloader equals userpro.UserID into table2
                            from userpro in table2.ToList().DefaultIfEmpty()
                            where ((d.NoteTitle == Note || String.IsNullOrEmpty(Note)) &&
                                    (d.Seller.ToString() == Seller || String.IsNullOrEmpty(Seller)) &&
                                    (u.FirstName == Buyer || String.IsNullOrEmpty(Buyer)))
                            select new BuyerRequestModal
                            {
                                objdownload = d,
                                objuserProfile = userpro,
                                objuser = u
                            }).AsQueryable();

            if (!String.IsNullOrEmpty(searchTxt))
            {
                multiple = multiple.Where(x => x.objdownload.NoteTitle.Contains(searchTxt) || x.objdownload.NoteCategory.Contains(searchTxt) || x.objuser.LastName.Contains(searchTxt) ||
                                          x.objuser.FirstName.Contains(searchTxt) || searchTxt == null || x.objdownload.AttachmentDownloadedDate.ToString().Contains(searchTxt) || x.objdownload.PurchasedPrice.ToString().Contains(searchTxt));
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
                case "buyer_desc":
                    multiple = multiple.OrderBy(x => x.objdownload.tblUser1.FirstName);
                    break;
                case "seller_desc":
                    multiple = multiple.OrderBy(x => x.objdownload.tblUser.FirstName);
                    break;
                default:
                    multiple = multiple.OrderByDescending(x => x.objdownload.AttachmentDownloadedDate);
                    break;
            }

            ViewBag.Seller = dbObj.tblUsers.Where(x => x.IsEmailVarified == true && x.RoleID == dbObj.tblUserRoles.Where(a => a.Name.ToLower() == "member").Select(a => a.ID).FirstOrDefault());
            ViewBag.Buyer = dbObj.tblDownloads.Where(x => x.Downloader == x.tblUser.ID && x.tblUser.IsEmailVarified == true).Select(x => x.tblUser.FirstName).Distinct();
            ViewBag.Note = dbObj.tblDownloads.Where(x => x.NoteID == x.tblSellerNote.ID && x.IsSellerHasAllowedDownload == true).Select(x => x.NoteTitle).Distinct();
            return View(multiple.ToPagedList(page ?? 1, 5));
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult RejectedNotes(int? pagepublish, string searchNote, string sortOrder1, string Seller)
        {
            ViewBag.CurrentSort1 = sortOrder1;
            ViewBag.TitleSortParm1 = String.IsNullOrEmpty(sortOrder1) ? "title_desc1" : "";
            ViewBag.EditedDateSortParm1 = String.IsNullOrEmpty(sortOrder1) ? "editedate_desc1" : "";
            ViewBag.CategorySortParm1 = String.IsNullOrEmpty(sortOrder1) ? "category_desc1" : "";
            ViewBag.SellerParm1 = String.IsNullOrEmpty(sortOrder1) ? "seller_desc1" : "";
            ViewBag.RemarkParm = String.IsNullOrEmpty(sortOrder1) ? "remark_desc1" : "";
            ViewBag.RejectedByParm = String.IsNullOrEmpty(sortOrder1) ? "rejectedby_desc1" : "";

            ViewBag.CurrentFilter1 = searchNote;
            List<tblSellerNote> tblSellers = dbObj.tblSellerNotes.Where(x => x.IsActive == true).ToList();
            List<tblNoteCategory> tblNotes = dbObj.tblNoteCategories.ToList();
            List<tblReferenceData> tblReferenceDatas = dbObj.tblReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value == "Rejected").ToList();

            var multipletable = from s in tblSellers
                                join n in tblNotes on s.Category equals n.ID into table1
                                from n in table1.ToList()
                                join r in tblReferenceDatas on s.Status equals r.ID into table2
                                from r in table2.ToList()
                                where (r.Value != "Published" && (s.tblUser.ID.ToString() == Seller || String.IsNullOrEmpty(Seller)))
                                select new DashboardModel
                                {
                                    sellernoteinfo = s,
                                    referenceDatainfo = r,
                                    noteCategoryinfo = n
                                };

            if (!String.IsNullOrEmpty(searchNote))
            {
                multipletable = multipletable.Where(x => x.noteCategoryinfo.Name.Contains(searchNote) || x.sellernoteinfo.Title.Contains(searchNote) || searchNote == null ||
                                                        x.sellernoteinfo.tblUser.FirstName.Contains(searchNote) || x.sellernoteinfo.ModifiedDate.ToString().Contains(searchNote) ||
                                                        x.sellernoteinfo.tblUser1.FirstName.Contains(searchNote));
            }

            switch (sortOrder1)
            {
                case "editedate_desc1":
                    multipletable = multipletable.OrderByDescending(x => x.sellernoteinfo.ModifiedDate);
                    break;
                case "title_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.Title);
                    break;
                case "category_desc1":
                    multipletable = multipletable.OrderBy(x => x.noteCategoryinfo.Name);
                    break;
                case "seller_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.tblUser.FirstName);
                    break;
                case "remark_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.AdminRemarks);
                    break;
                case "rejectedby_desc1":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.tblUser1.FirstName);
                    break;
                default:
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.ModifiedDate);
                    break;
            }

            ViewBag.Seller = dbObj.tblUsers.Where(x => x.IsEmailVarified == true && x.RoleID == dbObj.tblUserRoles.Where(a => a.Name.ToLower() == "member").Select(a => a.ID).FirstOrDefault());
            return View(multipletable.ToPagedList(pagepublish ?? 1, 5));

        }
    }
}