using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AttentanceManagementSystem.Models
{
    
    public class ApplicationUserMetadata
    {
        [Remote("","", ErrorMessage ="UserName is found Before")]
        public String UserName { get; set; }
    }
}