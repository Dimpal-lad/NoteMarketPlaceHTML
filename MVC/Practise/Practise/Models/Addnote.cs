using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Practise.Models
{
    public class Addnote {


        [Required(ErrorMessage = "Title is Required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Category  is Required")]
        public int Category { get; set; }
        public HttpPostedFileBase DisplayPicture { get; set; }
        [Required(ErrorMessage = "Description  is Required")]
        public List<HttpPostedFileBase> UploadNotes
        {
            get; set;
        }
        public int ID { get; set; }
        public Nullable<int> NoteType { get; set; }
        public Nullable<int> NumberofPages { get; set; }
        [Required(ErrorMessage = "Description  is Required")]
        public string Description { get; set; }
        public string UniversityName { get; set; }
        public Nullable<int> Country { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Professor { get; set; }
        [Required]
        public bool IsPaid { get; set; }
        
        public Nullable<decimal> SellingPrice { get; set; }
        public HttpPostedFileBase NotesPreview { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}