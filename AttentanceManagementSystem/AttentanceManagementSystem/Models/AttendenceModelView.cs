using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttentanceManagementSystem.Models
{
    public class AttendenceModelView
    {
        public string UserId { get; set; }
        public int Attendence { get; set; }
        public int Late { get; set; }
        public int Absence { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}