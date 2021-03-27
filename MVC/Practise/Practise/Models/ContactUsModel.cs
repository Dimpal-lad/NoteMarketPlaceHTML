using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Practise.Models
{
    public class ContactUsModel
    {
        [Required(ErrorMessage = "Full Name is Required")]
        [StringLength(20,MinimumLength =5)]
        //[RegularExpression(@"^[a-zA-Z\\s]*$", ErrorMessage = "Use letters only please")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please Enter Email Address")]
        [Display(Name = "Email ID")]
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please Enter Correct Email Address")]
        public string EmailID { get; set; }
       
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Comment { get; set; }

    }
}