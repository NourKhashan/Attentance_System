using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttentanceManagementSystem.Models;


namespace AttentanceManagementSystem.Controllers
{
    public class ReportController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Report
        public ActionResult Index()
        {
            return View();
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
            List<DateTime> attendenceTime = db.Attendence.Select(m => m.AttendenceDate).ToList<DateTime>();
            attendenceTime = attendenceTime.Where(m => m.Month == Id).ToList<DateTime>();
            foreach (DateTime item in attendenceTime)
            {
                min = item.Minute;
                hour = item.Hour;
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
            
            if (attendenceTime.Count() != 0)
            {
                absence = 24 - attendence;
            }
            ViewBag.late = late;
            ViewBag.attendence = attendence;
            ViewBag.absence = absence;
            var data = new int[] { attendence, late, absence };
            return Json(data, JsonRequestBehavior.AllowGet);
            //return Json(s, JsonRequestBehavior.AllowGet);
        }



        public ActionResult MonthlyReportForSpecifEmp()
        {

            var result = db.Users.Where(user => user.EmailConfirmed == true).Where(usr => usr.UserName != "Admin").ToList();
            Session["Emp"] = result;

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
                list.Add(new AttendenceModelView() { UserId = fullName, Absence = absence, Attendence = attendence, Late = late });
            }// For each For EveryUser
            return View(list);
        }


        //[HttpPost]
        public ActionResult MonthlyReportForSpecifEmpByMonth(int monthId, string empId)
        {
            List<AttendenceModelView> list = new List<AttendenceModelView>();
            List<ApplicationUser> users = new List<ApplicationUser>();
            List<DateTime> attendenceTime = new List<DateTime>();
            if (empId != "0")
            {
                users = db.Users.Where(user => user.EmailConfirmed == true && user.Id == empId).Where(user => user.UserName != "Admin").ToList<ApplicationUser>();
                var fullName = users.Select(m => m.FirstName + " " + m.LastName).ToList()[0].ToString();

                if (monthId != 0)
                {
                    
                    attendenceTime = db.Attendence.Where(att => att.UserId == empId && att.AttendenceDate.Month == monthId && att.AttendenceDate.Year == DateTime.Now.Year).Select(m => m.AttendenceDate).ToList<DateTime>();
                    CalculateStatus(attendenceTime, out int late, out int absence, out int attendence);
                    list.Add(new AttendenceModelView() { UserId = fullName, Absence = absence, Attendence = attendence, Late = late });
                }
                else
                {
                    attendenceTime = db.Attendence.Where(att => att.UserId == empId).Select(m => m.AttendenceDate).ToList<DateTime>();
                    list = AddEmpToListByMonth( attendenceTime,   monthId,  empId);
                }

            }
            else// All USers
            {
                users = db.Users.Where(user => user.EmailConfirmed == true).Where(user => user.UserName != "Admin").ToList<ApplicationUser>();

                if (monthId != 0)
                {
                   
                    attendenceTime = db.Attendence.Where(att => att.AttendenceDate.Month == monthId && att.AttendenceDate.Year == DateTime.Now.Year).Select(m => m.AttendenceDate).ToList<DateTime>();

                    
                }
                else
                {
                    attendenceTime = db.Attendence.Select(m => m.AttendenceDate).ToList<DateTime>();
                }
               list= AddEmpToList(users, monthId, empId);

            }




            return Json(list, JsonRequestBehavior.AllowGet);
        }// Report

        public List<AttendenceModelView> AddEmpToListByMonth(List<DateTime> attendenceTime, int monthId, string empId)
        {
            List<AttendenceModelView> list = new List<AttendenceModelView>();
            string[] month = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };



            foreach (var item in month)
            {

                var monthName = item;
                monthId = Array.IndexOf(month, item);
                attendenceTime = db.Attendence.Where(att => att.UserId == empId).Select(m => m.AttendenceDate).ToList<DateTime>();

                attendenceTime = attendenceTime.Where(m => m.Month == monthId+1).ToList<DateTime>();
                CalculateStatus(attendenceTime, out int late, out int absence, out int attendence);
                list.Add(new AttendenceModelView() { UserId = monthName, Absence = absence, Attendence = attendence, Late = late });
            }// For each For EveryUser
            return list;
        }

        public List<AttendenceModelView>  AddEmpToList(List<ApplicationUser> users, int monthId, string empId)
        {
            List<DateTime> attendenceTime = new List<DateTime>();
            List<AttendenceModelView> list = new List<AttendenceModelView>();
            
            foreach (ApplicationUser item in users)
            {
               
                var fullName = item.FirstName + " " + item.LastName;
               
                attendenceTime = db.Attendence.Where(att => att.UserId == item.Id).Select(m => m.AttendenceDate).ToList<DateTime>();

                attendenceTime = attendenceTime.Where(m => m.Month == monthId).ToList<DateTime>();
                CalculateStatus(attendenceTime, out int late, out int absence, out int  attendence);
                list.Add(new AttendenceModelView() { UserId = fullName, Absence = absence, Attendence = attendence, Late = late });
            }// For each For EveryUser
            return list;
        }

        public void CalculateStatus(List<DateTime> attendenceTime , out int late , out int absence, out int  attendence)
        {
            late = 0;
            absence = 0;
            attendence = 0;
            int min, hour;
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
            if (attendenceTime.Count() != 0)
            {
                absence = 24 - attendence;
            }
        }

    }
}