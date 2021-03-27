using Practise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Net;
using System.IO;
using Ionic.Zip;
using System.Net.Mail;

namespace Practise.Controllers
{

    public class SellerNotesController : Controller
    {
        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: SellerNotes

        [Authorize]
        public ActionResult DashBoard(string searchTxt, int? page, string sortOrder, string currentFilter, string searchNote, int? pagepublish, string sortOrder1, string currentFilter1)
        {
            //For In Progress Notes

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.AddedDateSortParm = String.IsNullOrEmpty(sortOrder) ? "addedate_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "";
            ViewBag.StatusSortParm = String.IsNullOrEmpty(sortOrder) ? "status_desc" : "";

            if (searchTxt != null)
            {
                page = 1;
            }
            else
            {
                searchTxt = currentFilter;
            }

            ViewBag.CurrentFilter = searchTxt;

            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            List<tblSellerNote> tblSellers = dbObj.tblSellerNotes.Where(x => x.SellerID == user.ID && x.IsActive == true).ToList();
            List<tblNoteCategory> tblNotes = dbObj.tblNoteCategories.ToList();
            List<tblReferenceData> tblReferenceDatas = dbObj.tblReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value != "Rejected" && x.Value != "Removed").ToList();



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

            if (!String.IsNullOrEmpty(searchTxt))
            {
                multipletable = multipletable.Where(x => x.noteCategoryinfo.Name.Contains(searchTxt) || x.referenceDatainfo.Value.Contains(searchTxt) || x.sellernoteinfo.Title.Contains(searchTxt) || searchTxt == null);
            }

            switch (sortOrder)
            {
                case "addedate_desc":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.CreatedDate);
                    break;
                case "title_desc":
                    multipletable = multipletable.OrderBy(x => x.sellernoteinfo.Title);
                    break;
                case "category_desc":
                    multipletable = multipletable.OrderBy(x => x.noteCategoryinfo.Name);
                    break;
                case "status_desc":
                    multipletable = multipletable.OrderBy(x => x.referenceDatainfo.Value);
                    break;
                default:
                    multipletable = multipletable.OrderByDescending(x => x.sellernoteinfo.CreatedDate);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            ViewBag.multipletable = multipletable.ToList().ToPagedList(pageNumber, pageSize);


            //For Publish Notes

            ViewBag.CurrentSort1 = sortOrder1;
            ViewBag.TitleSortParm1 = String.IsNullOrEmpty(sortOrder1) ? "title_desc1" : "";
            ViewBag.AddedDateSortParm1 = String.IsNullOrEmpty(sortOrder1) ? "addedate_desc1" : "";
            ViewBag.CategorySortParm1 = String.IsNullOrEmpty(sortOrder1) ? "category_desc1" : "";
            ViewBag.SellTypeParm1 = String.IsNullOrEmpty(sortOrder1) ? "selltype_desc1" : "";
            ViewBag.PriceParm1 = String.IsNullOrEmpty(sortOrder1) ? "price_desc1" : "";

            if (searchNote != null)
            {
                page = 1;
            }
            else
            {
                searchNote = currentFilter1;
            }

            ViewBag.CurrentFilter1 = searchNote;

            List<tblSellerNote> tblSellers1 = dbObj.tblSellerNotes.Where(x => x.SellerID == user.ID && x.IsActive == true).ToList();
            List<tblNoteCategory> tblNotes1 = dbObj.tblNoteCategories.ToList();
            List<tblReferenceData> tblReferenceDatas1 = dbObj.tblReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value != "Rejected" && x.Value != "Removed").ToList();

            var multipletable1 = from s in tblSellers1
                                 join n in tblNotes1 on s.Category equals n.ID into table1
                                 from n in table1.ToList()
                                 join r in tblReferenceDatas1 on s.Status equals r.ID into table2
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
                multipletable1 = multipletable1.Where(x => x.noteCategoryinfo.Name.Contains(searchNote) || x.sellernoteinfo.Title.Contains(searchNote) || searchNote == null);
            }
            
            switch (sortOrder1)
            {
                case "addedate_desc1":
                    multipletable1 = multipletable1.OrderBy(x => x.sellernoteinfo.CreatedDate);
                    break;
                case "title_desc1":
                    multipletable1 = multipletable1.OrderBy(x => x.sellernoteinfo.Title);
                    break;
                case "category_desc1":
                    multipletable1 = multipletable1.OrderBy(x => x.noteCategoryinfo.Name);
                    break;
                case "selltype_desc1":
                    multipletable1 = multipletable1.OrderBy(x => x.sellernoteinfo.IsPaid);
                    break;
                case "price_desc1":
                    multipletable1 = multipletable1.OrderBy(x => x.sellernoteinfo.SellingPrice);
                    break;
                default:
                    multipletable1 = multipletable1.OrderByDescending(x => x.sellernoteinfo.CreatedDate);
                    break;
            }

            int pageSize1 = 5;
            int pageNumber1 = (pagepublish ?? 1);
            ViewBag.multipletable1 = multipletable1.ToList().ToPagedList(pageNumber1, pageSize1);
            return View();
        }


        //Delete Notes
        public ActionResult DeleteNote(int id)
        {
            var del = (from attachment in dbObj.tblSellerNotesAttachments where attachment.NoteID == id select attachment).FirstOrDefault();
            var delete = (from seller in dbObj.tblSellerNotes where seller.ID == id select seller).FirstOrDefault();
            dbObj.tblSellerNotes.Remove(delete);
            dbObj.tblSellerNotesAttachments.Remove(del);
            dbObj.SaveChanges();
            return RedirectToAction("DashBoard", "SellerNotes");
        }

        public ActionResult SearchNotes(string searchTxt, int? page, string NoteType, string Category, string UniversityName, string Course, string Country)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            List<tblSellerNote> tblSellers = dbObj.tblSellerNotes.OrderBy(x => x.Title).ToList();
            List<tblNoteCategory> tblNotes = dbObj.tblNoteCategories.ToList();
            List<tblReferenceData> tblReferenceDatas = dbObj.tblReferenceDatas.Where(x => x.RefCategory == "Notes Status" && x.Value == "Published").ToList();
            List<tblSellerNotesReview> tblSellerNotesReviews = dbObj.tblSellerNotesReviews.ToList();
            
            var multipletable = from s in tblSellers
                                join n in tblNotes on s.Category equals n.ID into table1
                                from n in table1.ToList()
                                join r in tblReferenceDatas on s.Status equals r.ID into table2
                                from r in table2.ToList()
                                //join nreview in tblSellerNotesReviews on s.ID equals nreview.NoteID into table3
                                //from nreview in table3.ToList()
                                where (r.Value == "Published" && ((s.NoteType.ToString() == NoteType || String.IsNullOrEmpty(NoteType)) &&
                                                                  (s.Category.ToString() == Category || String.IsNullOrEmpty(Category)) &&
                                                                  (s.UniversityName == UniversityName || String.IsNullOrEmpty(UniversityName)) &&
                                                                  (s.Course == Course || String.IsNullOrEmpty(Course)) &&
                                                                  (s.Country.ToString() == Country || String.IsNullOrEmpty(Country))
                                                                 ))
                                select new SearchNoteModel
                                {
                                    sellernoteinfo = s,
                                    referenceDatainfo = r,
                                    noteCategoryinfo = n,
                                    //notesReviewinfo = nreview
                                };

            if (!String.IsNullOrEmpty(searchTxt))
            {
                multipletable = multipletable.Where(x => x.sellernoteinfo.Title.Contains(searchTxt) || searchTxt == null).ToList();
            }

            //tblUserProfile userProfile = dbObj.tblUserProfiles.Where(x => x.UserID == user.ID).FirstOrDefault();
            //TempData["ProfilePicture"] = userProfile.ProfilePicture;
            ViewBag.Rate = dbObj.tblSellerNotesReviews.Where(x => x.IsActive == true);
            ViewBag.NotesType = dbObj.tblNoteTypes.Where(x => x.IsActive == true);
            ViewBag.NotesCategory = dbObj.tblNoteCategories.Where(x => x.IsActive == true);
            ViewBag.UniversityName = dbObj.tblSellerNotes.Where(x => x.UniversityName != null).Select(x => x.UniversityName).Distinct();
            ViewBag.Course = dbObj.tblSellerNotes.Where(x => x.Course != null).Select(x => x.Course).Distinct();
            ViewBag.NotesCountry = dbObj.tblCountries.Where(x => x.IsActive == true);
            int pageSize = 9;
            int pageNumber = (page ?? 1);

            ViewBag.totalCount = multipletable.Count();
            return View(multipletable.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult NoteDetails(int id)
        {

            tblSellerNote sellerNote = dbObj.tblSellerNotes.Find(id);

            return View(sellerNote);
        }

        [Authorize]
        public ActionResult Download(int id)
        {

            tblSellerNote sellerNote = dbObj.tblSellerNotes.Find(id);

            tblSellerNotesAttachment sellerNotesAttachment = dbObj.tblSellerNotesAttachments.Where(x=>x.NoteID==id).FirstOrDefault();

            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            tblNoteCategory noteCategory = dbObj.tblNoteCategories.Where(x => x.ID == sellerNote.Category).FirstOrDefault();
            if (sellerNote.IsPaid == false)
            {
                tblDownload download = new tblDownload()
                {
                    NoteID = sellerNote.ID,
                    Seller = sellerNote.SellerID,
                    Downloader = user.ID,
                    IsSellerHasAllowedDownload = true,
                    AttachmentPath = sellerNotesAttachment.FilePath,
                    IsAttachmentDownloaded = true,
                    AttachmentDownloadedDate = DateTime.Now,
                    IsPaid = sellerNote.IsPaid,
                    PurchasedPrice = sellerNote.SellingPrice,
                    NoteTitle = sellerNote.Title,
                    NoteCategory = noteCategory.Name,
                    CreatedDate=DateTime.Now,
                    CreatedBy=user.ID,
                    ModifiedBy=user.ID
                };
                dbObj.tblDownloads.Add(download);
                dbObj.SaveChanges();

                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(Server.MapPath("~/Images/" + sellerNote.SellerID + "/" + id + "/" + "Attachment"));

                    MemoryStream output = new MemoryStream();
                    zip.Save(output);
                    return File(output.ToArray(), "Attachment/zip", "Note.zip");
                }
            }
            else
            {
                tblUser user1 = dbObj.tblUsers.Where(x => x.ID == sellerNote.SellerID).FirstOrDefault();
                SendEmail(user1.EmailID.ToString());
                tblDownload download = new tblDownload()
                {
                    NoteID = sellerNote.ID,
                    Seller = sellerNote.SellerID,
                    Downloader = user.ID,
                    IsSellerHasAllowedDownload = false,
                    AttachmentDownloadedDate = DateTime.Now,
                    IsPaid = sellerNote.IsPaid,
                    PurchasedPrice = sellerNote.SellingPrice,
                    NoteTitle = sellerNote.Title,
                    NoteCategory = noteCategory.Name,
                    CreatedDate = DateTime.Now,
                    CreatedBy = user.ID,
                    ModifiedBy = user.ID
                };
                dbObj.tblDownloads.Add(download);
                dbObj.SaveChanges();
                
            }
            return RedirectToAction("SearchNotes", "SellerNotes");

        }


        [NonAction]
        public void SendEmail(string emailID)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            tblUser user1 = dbObj.tblUsers.Where(x => x.EmailID==emailID).FirstOrDefault();

            var fromEmail = new MailAddress("dnlad22@gmail.com", "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "jkqobpmshhmlgumw"; // Replace with actual password
            string subject = user.FirstName +" "+"wants to purchase your notes";
            string body = "<br/>Hello"+" "+user1.FirstName +" ,<br/>";
            body += "We would like to inform you that, "+ user.FirstName+" " + "wants to purchase your notes.Please see<br/> Buyer Requests tab and allow download access to Buyer if you have received the payment from him. ";
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

    }
}