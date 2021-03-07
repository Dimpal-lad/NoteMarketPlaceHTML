using Practise.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Practise.Controllers
{
    public class AddNotesController : Controller
    {

        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: AddNote

        public ActionResult AddNotes()
        {
            ViewBag.NotesCategory = dbObj.tblNoteCategories.Where(x => x.IsActive == true);
            ViewBag.NotesCountry = dbObj.tblCountries.Where(x => x.IsActive == true);
            ViewBag.NotesType = dbObj.tblNoteTypes.Where(x => x.IsActive == true);
            return View();

        }

        [HttpPost]
        public ActionResult AddNotes(Addnote addnote)
        {

            if (ModelState.IsValid)
            {
                var EmailId = User.Identity.Name.ToString();

                tblUser user = dbObj.tblUsers.Where(x => x.EmailID == EmailId).FirstOrDefault();

                tblReferenceData referenceData = dbObj.tblReferenceDatas.Where(x => x.RefCategory.ToLower() == "Notes Status").FirstOrDefault();

                tblSellerNote sellerNote = new tblSellerNote()
                {
                    SellerID = user.ID,
                    Status = referenceData.ID,
                    Title = addnote.Title,
                    Category = addnote.Category,
                    NoteType = addnote.NoteType,
                    NumberofPages = addnote.NumberofPages,
                    Description = addnote.Description,
                    UniversityName = addnote.UniversityName,
                    Country = addnote.Country,
                    Course = addnote.Course,
                    CourseCode = addnote.CourseCode,
                    Professor = addnote.Professor,
                    IsPaid = addnote.IsPaid,
                    SellingPrice = addnote.SellingPrice,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };
                dbObj.tblSellerNotes.Add(sellerNote);
                dbObj.SaveChanges();

                var noteId = sellerNote.ID;

                string storepath = Path.Combine(Server.MapPath("~/Images/" + user.ID), noteId.ToString());

                if (!Directory.Exists(storepath))
                {
                    Directory.CreateDirectory(storepath);
                }

                //If user upload Display picture
                if (addnote.DisplayPicture != null && addnote.DisplayPicture.ContentLength > 0)
                {
                    string _FileName = Path.GetFileNameWithoutExtension(addnote.DisplayPicture.FileName);
                    string extension = Path.GetExtension(addnote.DisplayPicture.FileName);
                    _FileName = _FileName + DateTime.Now.ToString("ddmmyyyy") + extension;
                    addnote.FilePath = "~/Images/" + _FileName;
                    _FileName = Path.Combine(Server.MapPath("~/Images/" + user.ID + "/" + noteId), _FileName);
                    addnote.DisplayPicture.SaveAs(_FileName);
                    sellerNote.DisplayPicture = Path.Combine(("/Images/" + user.ID + "/" + noteId + "/"), _FileName);
                    dbObj.SaveChanges();
                }
                //If user does not upload Display picture
                else
                {
                    sellerNote.DisplayPicture = "D:/.Net/Practise/Practise/DefaultImage/4.jpg";
                    dbObj.SaveChanges();
                }

                //UPload Notes
                //SellerNote Attachment
                tblSellerNotesAttachment sellerNotesAttachment = new tblSellerNotesAttachment()
                {
                    NoteID = noteId,
                    IsActive = true,
                    CreatedBy = user.ID,
                    CreatedDate = DateTime.Now
                };

                string storeUploadpath = Path.Combine(storepath, "Attachment");
                //Check Directory for Upload Notes
                if (!Directory.Exists(storeUploadpath))
                {
                    Directory.CreateDirectory(storeUploadpath);
                }

                //Upload Notes
                int count = 1;
                var UploadFilePath = "";
                var UploadFileName = "";
                foreach (var file in addnote.UploadNotes)
                {
                    string _FileName = Path.GetFileNameWithoutExtension(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    _FileName = "Attachment" + count + "_" + DateTime.Now.ToString("ddmmyyyy") + extension;
                    string final = Path.Combine(storeUploadpath, _FileName);
                    file.SaveAs(final);
                    UploadFileName += _FileName + ";";
                    UploadFilePath += Path.Combine(("/Images/" + user.ID + "/" + noteId + "/Attachment/"), _FileName);
                    count++;
                }

                //Save Detail into Database
                sellerNotesAttachment.FileName = UploadFileName;
                sellerNotesAttachment.FilePath = UploadFilePath;
                dbObj.tblSellerNotesAttachments.Add(sellerNotesAttachment);
                dbObj.SaveChanges();


                //Notes Preview
                if (addnote.IsPaid == true)
                {
                    if (addnote.NotesPreview == null)
                    {
                        ViewBag.ErrorMessage = "You must have to upload a file";
                        ViewBag.NotesCategory = dbObj.tblNoteCategories.Where(x => x.IsActive == true);
                        ViewBag.NotesCountry = dbObj.tblCountries.Where(x => x.IsActive == true);
                        ViewBag.NotesType = dbObj.tblNoteTypes.Where(x => x.IsActive == true);

                        return View(addnote);
                    }
                }

                if (addnote.NotesPreview != null && addnote.NotesPreview.ContentLength > 0)
                {
                    string _FileName = Path.GetFileNameWithoutExtension(addnote.NotesPreview.FileName);
                    string extension = Path.GetExtension(addnote.NotesPreview.FileName);
                    _FileName = _FileName + DateTime.Now.ToString("ddmmyyyy") + extension;
                    addnote.FilePath = "~/Images/" + _FileName;
                    _FileName = Path.Combine(Server.MapPath("~/Images/" + user.ID + "/" + noteId), _FileName);
                    addnote.NotesPreview.SaveAs(_FileName);
                    sellerNote.NotesPreview = Path.Combine(("/Images/" + user.ID + "/" + noteId + "/"), _FileName);
                    dbObj.SaveChanges();
                }

                return RedirectToAction("DashBoard", "SellerNotes");

            }
            ViewBag.NotesCategory = dbObj.tblNoteCategories.Where(x => x.IsActive == true);
            ViewBag.NotesCountry = dbObj.tblCountries.Where(x => x.IsActive == true);
            ViewBag.NotesType = dbObj.tblNoteTypes.Where(x => x.IsActive == true);

            return View(addnote);
        }

        public ActionResult EditNotes()
        {
            ViewBag.NotesCategory = dbObj.tblNoteCategories.Where(x => x.IsActive == true);
            ViewBag.NotesCountry = dbObj.tblCountries.Where(x => x.IsActive == true);
            ViewBag.NotesType = dbObj.tblNoteTypes.Where(x => x.IsActive == true);

            return View();
        } 

        //[HttpPost]
        //public ActionResult EditNotes()
        //{
        //    ViewBag.NotesCategory = dbObj.tblNoteCategories.Where(x => x.IsActive == true);
        //    ViewBag.NotesCountry = dbObj.tblCountries.Where(x => x.IsActive == true);
        //    ViewBag.NotesType = dbObj.tblNoteTypes.Where(x => x.IsActive == true);

        //    return View();
        //}
    }
}