using PagedList;
using Practise.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practise.Controllers
{
    public class AdminMemberController : Controller
    {
        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: AdminMember
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult Members(string searchTxt,int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstSortParm = String.IsNullOrEmpty(sortOrder) ? "first_desc" : "";
            ViewBag.LastSortParm = String.IsNullOrEmpty(sortOrder) ? "last_desc" : "";
            ViewBag.EmailIDSortParm = String.IsNullOrEmpty(sortOrder) ? "emailId_desc" : "";
            ViewBag.CreatedDateParm = String.IsNullOrEmpty(sortOrder) ? "createddate_desc" : "";
            ViewBag.UnderReviewNoteSortParm = String.IsNullOrEmpty(sortOrder) ? "underreviewnote_desc" : "";
            ViewBag.PublishNoteSortParm = String.IsNullOrEmpty(sortOrder) ? "publishnote_desc" : "";
            ViewBag.DownloadNoteSortParm = String.IsNullOrEmpty(sortOrder) ? "downloadnote_desc" : "";
            ViewBag.TotalExpenseSortParm = String.IsNullOrEmpty(sortOrder) ? "totalexpense_desc" : "";
            ViewBag.TotalEarningSortParm = String.IsNullOrEmpty(sortOrder) ? "totalearning_desc" : "";

            var objUser = (dbObj.tblUsers.Where(x => x.tblUserRole.Name.ToLower() == "member" && x.IsEmailVarified==true && (x.FirstName.Contains(searchTxt) ||
                                    x.LastName.Contains(searchTxt) || x.EmailID.ToString().Contains(searchTxt) || searchTxt==null ||
                                    x.tblDownloads.Where(m=>m.IsSellerHasAllowedDownload==true && m.Seller==m.tblUser.ID).Sum(m=>m.PurchasedPrice).ToString().Contains(searchTxt) ||
                                    x.tblDownloads1.Where(m=>m.IsSellerHasAllowedDownload==true && m.Downloader==x.ID).Sum(m=>m.PurchasedPrice).ToString().Contains(searchTxt))).ToList()).AsQueryable();

            switch (sortOrder)
            {
                case "createddate_desc":
                    objUser = objUser.OrderBy(x => x.CreatedDate);
                    break;
                case "first_desc":
                    objUser = objUser.OrderBy(x => x.FirstName);
                    break;
                case "last_desc":
                    objUser = objUser.OrderBy(x => x.LastName);
                    break;
                case "emailId_desc":
                    objUser = objUser.OrderBy(x => x.EmailID);
                    break;
                case "underreviewnote_desc":
                    objUser = objUser.OrderBy(x => x.tblSellerNotes1.Where(m => m.tblReferenceData.RefCategory == "Notes Status" && (m.tblReferenceData.Value == "Submitted For Review" || m.tblReferenceData.Value == "In Review")).Select(m => m.ID).Count());
                    break;
                case "publishnote_desc":
                    objUser = objUser.OrderBy(x=>x.tblSellerNotes1.Where(m => m.tblReferenceData.RefCategory == "Notes Status" && m.tblReferenceData.Value == "Published").Select(m => m.ID).Count());
                    break;
                case "downloadnote_desc":
                    objUser = objUser.OrderBy(x=>x.tblDownloads1.Where(m => m.Seller == m.tblUser.ID && m.IsSellerHasAllowedDownload == true).Count());
                    break;
                case "totalexpense_desc":
                    objUser = objUser.OrderBy(x=>x.tblDownloads1.Where(m => m.Downloader == x.ID && m.IsSellerHasAllowedDownload == true).Sum(m => m.PurchasedPrice));
                    break;
                case "totalearning_desc":
                    objUser = objUser.OrderBy(x=>x.tblDownloads1.Where(m => m.Seller == m.tblUser.ID && m.IsSellerHasAllowedDownload == true).Sum(m => m.PurchasedPrice));
                    break;
                default:
                    objUser = objUser.OrderByDescending(x => x.CreatedDate);
                    break;
            }


            return View(objUser.ToList().ToPagedList(page ?? 1, 5));
        }

        public ActionResult Deactivate(int id)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();
            tblUser user1 = dbObj.tblUsers.Where(x => x.ID == id).FirstOrDefault();
            user1.IsActive = false;
            dbObj.Entry(user1).State = EntityState.Modified;
            dbObj.SaveChanges();

            var sellerNote = dbObj.tblSellerNotes.Where(x => x.SellerID == id && x.tblReferenceData.Value.ToLower() == "Published").ToList();
            foreach(var item in sellerNote)
            {
                item.Status = dbObj.tblReferenceDatas.Where(x => x.RefCategory.ToLower() == "Notes Status" && x.Value.ToLower() == "Removed").Select(x => x.ID).FirstOrDefault();
                item.ModifiedBy = user.ID;
                item.ActionedBy = user.ID;
                item.ModifiedDate = DateTime.Now;
                dbObj.Entry(item).State = EntityState.Modified;
            }
            dbObj.SaveChanges();
            return RedirectToAction("AdminDashBoard", "Admin");
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult MembersDetails(int id,int? page, string sortOrder)
        {
            var objUser = dbObj.tblUsers.Where(x => x.ID == id);
            var objsellernote = dbObj.tblSellerNotes.Where(x=>x.SellerID == id && x.tblReferenceData.RefCategory == "Notes Status" && x.tblReferenceData.Value != "Draft");

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.AddedDateSortParm = String.IsNullOrEmpty(sortOrder) ? "addedate_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "";
            ViewBag.StatusParm = String.IsNullOrEmpty(sortOrder) ? "status_desc" : "";
            ViewBag.PublishDateSortParm = String.IsNullOrEmpty(sortOrder) ? "publishdate_desc" : "";
            ViewBag.TotalEarningSortParm = String.IsNullOrEmpty(sortOrder) ? "totalearning_desc" : "";

            switch (sortOrder)
            {
                case "addedate_desc":
                    objsellernote = objsellernote.OrderBy(x => x.CreatedDate);
                    break;
                case "title_desc":
                    objsellernote = objsellernote.OrderBy(x => x.Title);
                    break;
                case "category_desc":
                    objsellernote = objsellernote.OrderBy(x => x.tblNoteCategory.Name);
                    break;
                case "status_desc":
                    objsellernote = objsellernote.OrderBy(x => x.tblReferenceData.Value);
                    break;
                case "publishdate_desc":
                    objsellernote = objsellernote.OrderBy(x => x.PublishedDate);
                    break;
                case "totalearning_desc":
                    objsellernote = objsellernote.OrderBy(x => x.tblDownloads.Where(m => m.Seller == m.tblUser.ID && m.IsSellerHasAllowedDownload == true).Sum(m => m.PurchasedPrice));
                    break;
                default:
                    objsellernote = objsellernote.OrderByDescending(x => x.CreatedDate);
                    break;
            }
            ViewBag.objUser = objUser.ToList();
            ViewBag.objsellernote = objsellernote.ToList().ToPagedList(page ?? 1, 5);
            return View();
        }
    }
}