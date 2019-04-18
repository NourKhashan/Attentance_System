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
            var result = db.Users.Where(user => user.EmailConfirmed == true).Where(usr => usr.UserName != "Admin").ToList();
            Session["Emp"] = result;
            return Json(result, JsonRequestBehavior.AllowGet);
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




       
       
   
       
    }
}