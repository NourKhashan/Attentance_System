using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AttentanceManagementSystem.Models;

namespace AttentanceManagementSystem.Controllers
{
    public class AttendenceController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Attendence
        public ActionResult Index()
        {
            return View();
        }

      
        public ActionResult AssignAttendece()
        {
            var id = User.Identity.GetUserId();
            db.Attendence.Add(new Attendence() { UserId = id });
            var time = DateTime.Now;
            db.SaveChanges();
            ViewBag.Assign = "true";
            dynamic min = time.Minute;
            min = (min <= 9) ? "0" + min.ToString() : min.ToString();
            dynamic hour = time.Hour;
            hour = (hour <= 9) ? "0" + hour.ToString() : hour.ToString();

            ViewBag.time = $"{hour} : {min}";
            var user = db.Users.SingleOrDefault(us => us.Id == id);
            ViewBag.Name = $"{user.FirstName} {user.LastName}";
            return PartialView("_Attendence");
        }

        [HttpPost]
        public JsonResult CheckAttendenceToday()
        {
            var id = User.Identity.GetUserId();
            dynamic result = db.Attendence.SingleOrDefault(att => att.UserId == id && att.AttendenceDate.Day == DateTime.Now.Day && att.AttendenceDate.Month == DateTime.Now.Month && DateTime.Now.Year == att.AttendenceDate.Year);
            if(result == null)
            {
                result = "NoData";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Settings()
        {
            return View();
        }
    }
}