using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Quizz.Models;
using Quizz.ViewModel;

namespace Quizz.Controllers
{
    public class AccountController : Controller
    {
        private QuizDBEntities quizDBEntities;
        public AccountController()
        {
            quizDBEntities = new QuizDBEntities();
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(AdminViewModel AdminUser)
        {
            if(ModelState.IsValid)
            {
                Admin admin = quizDBEntities.Admins.SingleOrDefault(model => model.UserName == AdminUser.UserName);
                if(admin == null)
                {
                    ModelState.AddModelError(string.Empty, "Username is invalid");
                }
                else if(admin.UserPassword != AdminUser.UserPassword)
                {
                    ModelState.AddModelError(string.Empty, "Password is incorrect");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(AdminUser.UserName, false);
                    var authTicket = new FormsAuthenticationTicket(1, admin.UserName, DateTime.Now, DateTime.Now.AddMinutes(120),false,"Admin");
                    string encryptTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptTicket);
                    HttpContext.Response.Cookies.Add(authCookie);

                    return RedirectToAction("Index", "Admin");
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}