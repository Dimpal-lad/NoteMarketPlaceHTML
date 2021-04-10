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
    public class AddNotesController : Controller
    {

        private readonly NotesMarketPlaceEntities2 dbObj = new NotesMarketPlaceEntities2();
        // GET: AddNote
        [Authorize(Roles = "Member")]
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

                string storepath = Path.Combine(Server.MapPath("/Images/" + user.ID), noteId.ToString());

                if (!Directory.Exists(storepath))
                {
                    Directory.CreateDirectory(storepath);
                }

                //If user upload Display picture
                if (addnote.DisplayPicture != null && addnote.DisplayPicture.ContentLength > 0)
                {
                    string _FileName = Path.GetFileNameWithoutExtension(addnote.DisplayPicture.FileName);
                    string extension = Path.GetExtension(addnote.DisplayPicture.FileName);
                    _FileName =  DateTime.Now.ToString("ddmmyyyy") + extension;
                    string finalp = Path.Combine(storepath , _FileName);
                    addnote.DisplayPicture.SaveAs(finalp);
                    sellerNote.DisplayPicture = Path.Combine(("/Images/" + user.ID + "/" + noteId + "/"), _FileName);
                    dbObj.SaveChanges();
                }
                //If user does not upload Display picture
                else
                {
                    sellerNote.DisplayPicture = "/DefaultImage/DN_.jpg";
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
                    _FileName = DateTime.Now.ToString("ddmmyyyy") + extension;
                    string finalp = Path.Combine(storepath, _FileName);
                    addnote.NotesPreview.SaveAs(finalp);
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

        [Authorize(Roles = "Member")]
        public ActionResult EditNotes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            tblSellerNote sellerNote = dbObj.tblSellerNotes.Find(id);
            tblSellerNotesAttachment sellerNotesAttachment = dbObj.tblSellerNotesAttachments.Where(x => x.NoteID == id).FirstOrDefault();
            Addnote addnote1 = new Addnote()
            {
                ID = sellerNote.ID,
                Title = sellerNote.Title,
                Category = sellerNote.Category,
                NoteType = sellerNote.NoteType,
                NumberofPages = sellerNote.NumberofPages,
                Description = sellerNote.Description,
                UniversityName = sellerNote.UniversityName,
                Country = sellerNote.Country,
                Course = sellerNote.Course,
                CourseCode = sellerNote.CourseCode,
                Professor = sellerNote.Professor,
                IsPaid = sellerNote.IsPaid,
                SellingPrice = sellerNote.SellingPrice,
                CreatedDate = sellerNote.CreatedDate
            };

            if (sellerNote == null)
            {
                return HttpNotFound();
            }
            ViewBag.NotesCategory = dbObj.tblNoteCategories.Where(x => x.IsActive == true);
            ViewBag.NotesCountry = dbObj.tblCountries.Where(x => x.IsActive == true);
            ViewBag.NotesType = dbObj.tblNoteTypes.Where(x => x.IsActive == true);

            return View(addnote1);
        }

        [HttpPost]
       
        public ActionResult EditNotes(int id, string save, Addnote addnote1)
        {
           
            tblSellerNote sellerNote = dbObj.tblSellerNotes.Find(id);
            tblSystemConfiguration systemConfiguration = dbObj.tblSystemConfigurations.Where(x => x.Key.ToLower() == "Support email address").FirstOrDefault();
            tblSystemConfiguration systemConfiguration1 = dbObj.tblSystemConfigurations.Where(x => x.Key.ToLower() == "Password").FirstOrDefault();
            tblSystemConfiguration systemConfiguration2 = dbObj.tblSystemConfigurations.Where(x => x.Key.ToLower() == "Email Address(es)").FirstOrDefault();


            sellerNote.ID = addnote1.ID;
            sellerNote.Title = addnote1.Title;
            sellerNote.Category = addnote1.Category;
            sellerNote.NoteType = addnote1.NoteType;
            sellerNote.NumberofPages = addnote1.NumberofPages;
            sellerNote.Description = addnote1.Description;
            sellerNote.UniversityName = addnote1.UniversityName;
            sellerNote.Country = addnote1.Country;
            sellerNote.Course = addnote1.Course;
            sellerNote.CourseCode = addnote1.CourseCode;
            sellerNote.Professor = addnote1.Professor;
            sellerNote.IsPaid = addnote1.IsPaid;
            sellerNote.SellingPrice = addnote1.SellingPrice;
            sellerNote.ModifiedDate = DateTime.Now;
           
            
            if (save == "Save")
            {
                sellerNote.Status = dbObj.tblReferenceDatas.Where(x => x.RefCategory.ToLower() == "Notes Status" && x.Value.ToLower() == "Draft").Select(x=>x.ID).FirstOrDefault(); 
            }
            else
            {
                sellerNote.Status = dbObj.tblReferenceDatas.Where(x => x.RefCategory.ToLower() == "Notes Status" && x.Value.ToLower() == "Submitted For Review").Select(x => x.ID).FirstOrDefault();
                SendEmailtoAdmin(systemConfiguration.Value, systemConfiguration1.Value, systemConfiguration2.Value, id);
            }

            dbObj.Entry(sellerNote).State = EntityState.Modified;
            dbObj.SaveChanges();

            if (sellerNote == null)
            {
                return HttpNotFound();
            }
            ViewBag.NotesCategory = dbObj.tblNoteCategories.Where(x => x.IsActive == true);
            ViewBag.NotesCountry = dbObj.tblCountries.Where(x => x.IsActive == true);
            ViewBag.NotesType = dbObj.tblNoteTypes.Where(x => x.IsActive == true);
            return RedirectToAction("DashBoard", "SellerNotes");
            return View(addnote1);
        }

        [NonAction]
        public void SendEmailtoAdmin(string supportemailID, string password, string emailID, int id)
        {
            tblSellerNote sellerNote = dbObj.tblSellerNotes.Where(x => x.ID == id).FirstOrDefault();
            tblUser user1 = dbObj.tblUsers.Where(x => x.ID == sellerNote.SellerID).FirstOrDefault();

            var fromEmail = new MailAddress(supportemailID, "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = password; // Replace with actual password
            string subject = user1.FirstName + " " + "sent his note for review";
            string body = "<br/>Hello Admin,<br/>";
            body += "We want to inform you that," + " " + user1.FirstName + " " + " sent his note <br/>";
            body += sellerNote.Title+" "+"for review.Please look at the notes and take required actions." ;
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