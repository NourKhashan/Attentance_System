using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AttentanceManagementSystem.Common
{
    public static class Users
    {
       public static string GenerateUserName()
        {
            char[] values = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghigklmnobqrstuvwxyz1234567890!@#$%^&*".ToCharArray();
            Random randNum = new Random();
            StringBuilder userName = new StringBuilder();
            Console.WriteLine(randNum.Next(0,values.Length));
            for (int i = 0; i < 6; i++)
            {
                userName.Append(values[randNum.Next(0, values.Length)]);

            }
            return userName.ToString();
        }




        public static bool SendEmail(string _emailTo, string _name, string _message)
        {

            string Email = "nour.khashan@gmail.com";
            string Password = "nogo9892ogleur";





            MailMessage mailmessage = new MailMessage(Email, _emailTo)
            {
                Subject = "Confirmation Mail Attendence Site " ,
                Body = _message,
                 IsBodyHtml = true,
            };
            


            SmtpClient smtpclient = new SmtpClient("smtp.gmail.com", 587) {
                EnableSsl = true,

            Credentials = new System.Net.NetworkCredential(Email, Password),
            };
            



            try
            {
                smtpclient.Send(mailmessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }


   
}