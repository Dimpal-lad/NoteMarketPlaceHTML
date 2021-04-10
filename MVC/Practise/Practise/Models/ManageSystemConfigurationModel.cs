using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Practise.Models
{
    public class ManageSystemConfigurationModel
    {
        
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please Enter Correct Email Address")]
        public string SupportEmailID { get; set; }
        [RegularExpression(".+@.+\\..+", ErrorMessage = "Please Enter Correct Email Address")]
        public string EmailAddresses { get; set; }
        
        public string Password { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        public string PhoneNumber { get; set; }
        public HttpPostedFileBase ProfilePicture { get; set; }
        public HttpPostedFileBase DisplayPicture { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string FileName1 { get; set; }
        public string FilePath2 { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
       
    }
}