using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Practise.Models
{
    public class AdminUpdateProfile
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        [Required(ErrorMessage = "First Name is Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [Display(Name = "Email ID")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please Enter Correct Email Address")]
        public string EmailID { get; set; }
        public string SecondaryEmailAddress { get; set; }
       
        public string CountryCode { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        public string PhoneNumber { get; set; }
        public HttpPostedFileBase ProfilePicture { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}