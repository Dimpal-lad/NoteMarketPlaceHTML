using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Practise.Models
{
    
    public class UsersMetadata
    {
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

        [Required(ErrorMessage = "Please Enter Password")]
        [StringLength(24, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public bool IsEmailVarified { get; set; }


    }

}