using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AttentanceManagementSystem.Models
{
    public class Leave
    {
        [Key]
        public int Id { set; get; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public DateTime LeaveDate { get; set; } = DateTime.Now;

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}