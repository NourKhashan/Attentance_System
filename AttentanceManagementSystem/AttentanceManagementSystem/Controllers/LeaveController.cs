using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using AttentanceManagementSystem.Models;
using System.Data.Entity;

namespace AttentanceManagementSystem.Controllers
{
    public class LeaveController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Leave
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Leave()
        {
            var Id = User.Identity.GetUserId();
            db.Leave.Add(new Leave() { UserId = Id});
            db.SaveChanges();
            
            return PartialView("_Leave");
        }

        [HttpPost]
        public ActionResult CheckLeave()
        {
            var Id = User.Identity.GetUserId();

            dynamic result = db.Leave.SingleOrDefault(usr => usr.UserId == Id && usr.LeaveDate.Day == DateTime.Now.Day &&
            usr.LeaveDate.Month == DateTime.Now.Month && usr.LeaveDate.Year == DateTime.Now.Year);
            if(result == null)
            {
                result = "NoData";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllAttendenceToday()
        {
           var emps=  db.Attendence.Include(usr => usr.ApplicationUser).Where(usr => usr.AttendenceDate.Day == DateTime.Now.Day && usr.AttendenceDate.Month == DateTime.Now.Month && usr.AttendenceDate.Year == DateTime.Now.Year);
            return Json(emps, JsonRequestBehavior.AllowGet);

        }
    }
}