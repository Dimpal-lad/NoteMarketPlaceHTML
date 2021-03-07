using Practise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
namespace Practise.Controllers
{
    
    public class SellerNotesController : Controller
    {
        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: SellerNotes
        [Authorize]
        
        public ActionResult DashBoard(string searchTxt)
        {
            var EmailId = User.Identity.Name.ToString();

            tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

            List<tblSellerNote> tblSellers = dbObj.tblSellerNotes.OrderByDescending(x => x.CreatedDate).Where(x => x.SellerID == user.ID && x.IsActive == true && (x.Title.Contains(searchTxt) || searchTxt == null)).ToList();
            List<tblNoteCategory> tblNotes = dbObj.tblNoteCategories.ToList();
            List<tblReferenceData> tblReferenceDatas = dbObj.tblReferenceDatas.ToList();

            var multipletable = from s in tblSellers
                                join n in tblNotes on s.Category equals n.ID into table1
                                from n in table1.ToList()
                                join r in tblReferenceDatas on s.Status equals r.ID into table2
                                from r in table2.ToList()
                                select new DashboardModel
                                {
                                    sellernoteinfo = s,
                                    referenceDatainfo=r,
                                    noteCategoryinfo=n
                                };
            return View(multipletable);
        }

       
        public ActionResult SearchNotes()
        {
            return View();
        }

        public ActionResult NoteDetails()
        {
            return View();
        }

    }
}