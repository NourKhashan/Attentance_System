using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AttentanceManagementSystem.Models
{
    public class Attendence
    {
        [Key]
        public int Id { get; set; }
        public DateTime AttendenceDate { get; set; } = DateTime.Now;

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}