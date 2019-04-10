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
            var user = db.Users.SingleOrDefault(us=>us.Id == id);
            ViewBag.Name = $"{user.FirstName} {user.LastName}";
            return PartialView("_Attendence");
        }

        public ActionResult MonthlyReportForOneEmp()

        {
            
            return View();
        }
        [HttpPost]
        public ActionResult MonthlyReportForOneEmp(int Id)
        {
            int late = 0,
                attendence = 0,
                absence = 0,
                min, hour;
            List<DateTime> attendenceTime = db.Attendence.Select(m=>m.AttendenceDate).ToList<DateTime>();
            attendenceTime= attendenceTime.Where(m => m.Month == Id+1).ToList<DateTime>();
            foreach (DateTime item in attendenceTime)
            {
                min = item.Minute;
                hour = item.Hour;
                if (hour> 9 )
                {
                    late++;
                   
                }
                if(hour == 9 && !(min>= 0 && min <= 30))
                {
                    late++;
                }

                attendence++;
            }
            if (attendenceTime != null)
            {
                absence = 24 - attendence;
            }
            ViewBag.late = late;
            ViewBag.attendence = attendence;
            ViewBag.absence = absence;
            var data = new int[] { attendence, late, absence };
            return Json(data,  JsonRequestBehavior.AllowGet);
            //return Json(s, JsonRequestBehavior.AllowGet);
        }



        public ActionResult MonthlyReportForSpecifEmp()
        {
            List<AttendenceModelView> list = new List<AttendenceModelView>();
           List<ApplicationUser> users =  db.Users.Where(user=>user.EmailConfirmed == true).Where(user=>user.UserName != "Admin").ToList<ApplicationUser>();
            foreach (ApplicationUser item in users)
            {
               var fullName = item.FirstName + " " + item.LastName;
                int late = 0,
                    attendence = 0,
                    absence = 0,
                    min, hour;
                List<DateTime> attendenceTime = db.Attendence.Where(att=> att.UserId == item.Id).Select(m => m.AttendenceDate).ToList<DateTime>();
                foreach (DateTime itemDate in attendenceTime)
                {
                    min = itemDate.Minute;
                    hour = itemDate.Hour;
                    if (hour > 9)
                    {
                        late++;

                    }
                    if (hour == 9 && !(min >= 0 && min <= 30))
                    {
                        late++;
                    }

                    attendence++;
                }
                if (attendenceTime != null)
                {
                    absence = 24 - attendence;
                }
                list.Add(new AttendenceModelView() { UserId = fullName, Absence=absence, Attendence = attendence, Late=late});
            }// For each For EveryUser
            return View(list);
        }


        [HttpPost]
        public ActionResult MonthlyReportForSpecifEmpByMonth(int Id)
        {
            List<AttendenceModelView> list = new List<AttendenceModelView>();
            List<ApplicationUser> users = db.Users.Where(user => user.EmailConfirmed == true).Where(user => user.UserName != "Admin").ToList<ApplicationUser>();
            foreach (ApplicationUser item in users)
            {
                var fullName = item.FirstName + " " + item.LastName;
                int late = 0,
                    attendence = 0,
                    absence = 0,
                    min, hour;
                List<DateTime> attendenceTime = db.Attendence.Where(att => att.UserId == item.Id).Select(m => m.AttendenceDate).ToList<DateTime>();

                attendenceTime = attendenceTime.Where(m => m.Month == Id + 1).ToList<DateTime>();
                foreach (DateTime itemDate in attendenceTime)
                {
                    min = itemDate.Minute;
                    hour = itemDate.Hour;
                    if (hour > 9)
                    {
                        late++;

                    }
                    if (hour == 9 && !(min >= 0 && min <= 30))
                    {
                        late++;
                    }

                    attendence++;
                }
                if ( attendenceTime.Count()!=0)
                {
                    absence = 24 - attendence;
                }
                list.Add(new AttendenceModelView() { UserId = fullName, Absence = absence, Attendence = attendence, Late = late });
            }// For each For EveryUser
            return Json(list, JsonRequestBehavior.AllowGet);
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