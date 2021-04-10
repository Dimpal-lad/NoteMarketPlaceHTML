using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Practise.Models
{
    public class AddCategoryModel
    {
        [Required]
        public string Name { get; set;}
        [Required]
        public string Description { get; set; }
    }
}