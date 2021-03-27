using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Practise.Models
{
    public class UserProfile
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

        public Nullable<System.DateTime> DOB { get; set; }
        public Nullable<int> Gender { get; set; }
        public string SecondaryEmailAddress { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        public string PhoneNumber { get; set; }
        public HttpPostedFileBase ProfilePicture { get; set; }
        [Required(ErrorMessage = "AddressLine1 is Required")]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        [Required(ErrorMessage = "City is Required")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is Required")]
        public string State { get; set; }
        [Required(ErrorMessage = "ZipCode is Required")]
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string University { get; set; }
        public string College { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}