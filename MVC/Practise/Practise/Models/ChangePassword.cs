using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Practise.Models
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Please Enter Old Password")]
        [StringLength(24, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Please Enter New Password")]
        //[RegularExpression(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$")]
        [StringLength(24, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Confirm Password")]
        //[RegularExpression(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$")]
        [StringLength(24, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}