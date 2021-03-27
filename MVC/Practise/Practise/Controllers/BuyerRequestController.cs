using Practise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Net.Mail;
using System.Net;

namespace Practise.Controllers
{
    public class BuyerRequestController : Controller
    {
        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: BuyerRequest
        //[Authorize]
        public ActionResult BuyerRequest(string searchTxt,int? page,string sortOrder)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            List<tblDownload> tblDownload = dbObj.tblDownloads.Where(x=>x.Seller==user.ID && x.IsSellerHasAllowedDownload==false && x.IsPaid==true).ToList();
            List<tblUser> tblUser = dbObj.tblUsers.ToList();
            List<tblUserProfile> tblUserProfile = dbObj.tblUserProfiles.ToList();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.AttachmentDateSortParm = String.IsNullOrEmpty(sortOrder) ? "attachmentdate_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "";
            ViewBag.SellTypeParm = String.IsNullOrEmpty(sortOrder) ? "selltype_desc" : "";
            ViewBag.PriceParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            
            var multiple =(from d in tblDownload
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


            return View(multiple.ToPagedList(page ?? 1,5));
        }

        public ActionResult AllowDownload(int id,int buyerID)
        {
            var v = dbObj.tblDownloads.Where(x =>x.NoteID == id && x.IsSellerHasAllowedDownload==false && x.Downloader==buyerID).FirstOrDefault();
            tblSellerNotesAttachment sellerNotesAttachment = dbObj.tblSellerNotesAttachments.Where(x => x.NoteID == id).FirstOrDefault();
            tblSellerNote sellerNote = dbObj.tblSellerNotes.Where(x => x.ID == id).FirstOrDefault();
            if (v != null)
            {
                v.IsSellerHasAllowedDownload = true;
                v.AttachmentPath = sellerNotesAttachment.FilePath;
                v.ModifiedBy = sellerNote.SellerID;
                dbObj.SaveChanges();
            }

            tblUser user = dbObj.tblUsers.Where(x => x.ID == v.Downloader).FirstOrDefault();
            SendEmailtoBuyer(user.EmailID.ToString(),v.NoteID);
            return RedirectToAction("SearchNotes", "SellerNotes");
        }


        [NonAction]
        public void SendEmailtoBuyer(string emailID,int noteId)
        {
            
            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == emailID).FirstOrDefault();

            tblSellerNote sellerNote = dbObj.tblSellerNotes.Where(x=>x.ID==noteId).FirstOrDefault();

            tblUser user1 = dbObj.tblUsers.Where(x => x.ID == sellerNote.SellerID).FirstOrDefault();

            var fromEmail = new MailAddress("dnlad22@gmail.com", "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "jkqobpmshhmlgumw"; // Replace with actual password
            string subject = user1.FirstName + " " + "Allows you to download a note ";
            string body = "<br/>Hello" + " " + user.FirstName + " ,<br/>";
            body += "We would like to inform you that, " + user1.FirstName + " " + "Allows you to downnload a note.<br/> Please login and see My Download tabs to download particular note ";
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