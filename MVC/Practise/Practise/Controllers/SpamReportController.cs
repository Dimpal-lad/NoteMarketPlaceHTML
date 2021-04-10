using PagedList;
using Practise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practise.Controllers
{
    public class SpamReportController : Controller
    {
        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: SpamReport
        [Authorize(Roles = "Admin,SuperAdmin")]
        public ActionResult SpamReports(int? page, string searchTxt, string sortOrder)
        {
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.AddedDateSortParm = String.IsNullOrEmpty(sortOrder) ? "addedate_desc" : "";
            ViewBag.CategorySortParm = String.IsNullOrEmpty(sortOrder) ? "category_desc" : "";
            ViewBag.RemarkParm = String.IsNullOrEmpty(sortOrder) ? "remark_desc" : "";
            ViewBag.Name= String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var reportedIssue = dbObj.tblSellerNotesReportedIssues.Where(x => x.tblSellerNote.Title.Contains(searchTxt) || x.tblSellerNote.tblNoteCategory.Name.Contains(searchTxt) ||
                       x.CreatedDate.ToString().Contains(searchTxt) || x.tblUser.tblSellerNotesReportedIssues.Where(m => m.CreatedBy == m.tblUser.ID).Select(m => m.tblUser.FirstName).Contains(searchTxt) ||
                           searchTxt == null || x.Remarks.Contains(searchTxt));

            switch (sortOrder)
            {
                case "addedate_desc":
                    reportedIssue = reportedIssue.OrderBy(x => x.CreatedDate);
                    break;
                case "name_desc":
                    reportedIssue = reportedIssue.OrderBy(x => x.tblUser.FirstName);
                    break;
                case "title_desc":
                    reportedIssue = reportedIssue.OrderBy(x => x.tblSellerNote.Title);
                    break;
                case "category_desc":
                    reportedIssue = reportedIssue.OrderBy(x => x.tblSellerNote.tblNoteCategory.Name);
                    break;
                case "remark_desc":
                    reportedIssue = reportedIssue.OrderBy(x => x.Remarks);
                    break;
                default:
                    reportedIssue = reportedIssue.OrderByDescending(x => x.CreatedDate);
                    break;
            }


            return View(reportedIssue.ToList().ToPagedList(page ?? 1, 5));
        }

        //Delete SpamReport
        public ActionResult DeleteSpamReport(int id)
        {
            var delete = (from spamreport in dbObj.tblSellerNotesReportedIssues where spamreport.NoteID == id select spamreport).FirstOrDefault();
            dbObj.tblSellerNotesReportedIssues.Remove(delete);
            dbObj.SaveChanges();
            return RedirectToAction("SpamReports");
        }
    }
}