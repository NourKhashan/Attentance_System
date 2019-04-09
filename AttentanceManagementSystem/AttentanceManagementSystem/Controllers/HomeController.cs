using AttentanceManagementSystem.Common;
using AttentanceManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace AttentanceManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }


        [Authorize(Roles ="Admin")]
        public ActionResult GetAllEmployees()
        {
            return View(db.Users.Where(user => user.EmailConfirmed == true).ToList());
        }


        [Authorize(Roles = "Admin")]

        public ActionResult GetNewEmployees()
        {
            return View(db.Users.Where(user => user.EmailConfirmed == false).ToList());
        }


        [Authorize(Roles = "Admin")]

        [HttpPost]
        public ActionResult Confirm(string Id)
        {
           var user= db.Users.SingleOrDefault(usr => usr.Id == Id);
            user.EmailConfirmed = true;
            db.SaveChanges();
            StringBuilder msg = new StringBuilder();
            
            msg.AppendFormat($"Your Configration \n UserName {user.UserName} \n Password: itiN@123");
            Users.SendEmail(user.Email, "Admin", msg.ToString());
            return PartialView("_Confirm", db.Users.Where(usr => usr.EmailConfirmed == false).ToList());
        }




        public ActionResult Attendence()
        {

            var id = User.Identity.GetUserId();
            db.Attendence.Add(new Attendence() { UserId=id});
            var time = DateTime.Now;
            db.SaveChanges();
            ViewBag.Assign = "true";
            dynamic min = time.Minute;
            min =  (min <= 9)? "0" + min.ToString() : min.ToString();
            dynamic hour = time.Hour;
            hour = (hour <= 9) ? "0" + hour.ToString() : hour.ToString();

            ViewBag.time = $"{hour} : {min}";

            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}