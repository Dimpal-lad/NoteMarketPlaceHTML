using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Practise.Models
{
    public class ForgotPsw
    {
        [Required(ErrorMessage = "Please Enter Email Address")]
        public string EmailID { get; set; }
    }
}