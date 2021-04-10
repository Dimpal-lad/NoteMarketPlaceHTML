using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Practise.Models
{
    public class AddCountryModel
    {
        [Required]
        public string CountryName { get; set; }
        [Required]
        public string CountryCode { get; set; }
    }
}