﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Practise.Models
{
    public class DashboardModel
    {
        public tblSellerNote sellernoteinfo { get; set; }
        public tblReferenceData referenceDatainfo { get; set; }
        public tblNoteCategory noteCategoryinfo { get; set; }
    }
}