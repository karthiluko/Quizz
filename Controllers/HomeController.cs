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
    public class HomeController : Controller
    {
        QuizDBEntities quizDBEntities = new QuizDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserLogin(AdminViewModel UserLog)
        {
            if (ModelState.IsValid)
            {
                PremiumUser User = quizDBEntities.PremiumUsers.SingleOrDefault(model => model.UserName == UserLog.UserName);
                if (User == null)
                {
                    ModelState.AddModelError(string.Empty, "Username is invalid");
                }
                else if (User.Password != UserLog.UserPassword)
                {
                    ModelState.AddModelError(string.Empty, "Password is incorrect");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(UserLog.UserName, false);
                    var authTicket = new FormsAuthenticationTicket(1, User.UserName, DateTime.Now, DateTime.Now.AddMinutes(120), false, "Admin");
                    string encryptTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptTicket);
                    HttpContext.Response.Cookies.Add(authCookie);

                    return RedirectToAction("UserTest", "Home");
                }
            }
            return View();
        }

        public ActionResult UserTest()
        {
            List<Category> CategoryList = quizDBEntities.Categories.ToList();
            return View(CategoryList);
        }

        [HttpPost]
        public ActionResult UserTest(Category CategorySelect)
        {
            if (ModelState.IsValid)
            {
                if (Session["UserId"] != null)
                {
                    Session.Clear();
                }
                User user = new User();
                user.UserName = User.Identity.Name;
                user.LoginTime = DateTime.Now;
                user.CategoryId = CategorySelect.CategoryId;
                quizDBEntities.Users.Add(user);
                quizDBEntities.SaveChanges();

                Session["UserName"] = User.Identity.Name;
                Session["CategoryId"] = CategorySelect.CategoryId;
                Session["UserId"] = user.UserId;
                return RedirectToAction("UserQuestionAnswer","Quiz");
                //return View("~/Views/Quiz/QuestionIndex.cshtml");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}