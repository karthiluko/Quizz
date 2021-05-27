using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quizz.Models;
using Quizz.ViewModel;

namespace Quizz.Controllers
{
    public class PaymentController : Controller
    {
        QuizDBEntities quizDBEntities = new QuizDBEntities();
        // GET: Payment
        public ActionResult Index()
        {
            PaymentInitiateViewModel PremiumUser = new PaymentInitiateViewModel();
            PremiumUser.Amount = 499;
            return View(PremiumUser);
        }
        [HttpPost]
        public ActionResult CreateOrder(PaymentInitiateViewModel RequestData)
        {
            if (ModelState.IsValid)
            {
                // Generate random receipt number for order
                // Key Id = rzp_test_ecW657tHPGfCqI
                // Key Secret = Ku6kgbfmmMyoj5x4DIODM7z1
                Random randomObj = new Random();
                string transactionId = randomObj.Next(10000000, 100000000).ToString();

                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_ecW657tHPGfCqI", "Ku6kgbfmmMyoj5x4DIODM7z1");
                Dictionary<string, object> options = new Dictionary<string, object>() {
                    {"amount",RequestData.Amount * 100},// Amount will in paise
                    {"receipt", transactionId },
                    {"currency", "INR" },
                    {"payment_capture", "0" }// 1 - automatic  , 0 - manual
                };                           //options.Add("notes", "-- You can put any notes here --"); 
                                                     
                Razorpay.Api.Order orderResponse = client.Order.Create(options);

                // Create order model for return on view
                PremiumUser orderModel = new PremiumUser
                {
                    OrderId = orderResponse.Attributes["id"],
                    RazorpayKey = "rzp_test_ecW657tHPGfCqI",
                    Amount = RequestData.Amount * 100,
                    Currency = "INR",
                    Name = RequestData.Name,
                    Email = RequestData.Email,
                    UserName = RequestData.UserName,
                    Password = RequestData.Password,
                    ContactNumber = RequestData.ContactNumber,
                    Description = "Testing description"
                };
                TempData["OrderDetails"] = orderModel;
                TempData.Keep();
                // Return on PaymentPage with Order data
                return View("PaymentPage", orderModel);
            }
            return View("Index", RequestData);
        }

        [HttpPost]
        public ActionResult Complete()
        {
            // Payment data comes in url so we have to get it from url

            // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
            string paymentId = Request.Params["rzp_paymentid"];

            // This is orderId
            string orderId = Request.Params["rzp_orderid"];

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_ecW657tHPGfCqI", "Ku6kgbfmmMyoj5x4DIODM7z1");

            Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

            // This code is for capture the payment 
            Dictionary<string, object> options = new Dictionary<string, object>(){ { "amount", payment.Attributes["amount"] } };
            Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
            string amt = paymentCaptured.Attributes["amount"];

            //// Check payment made successfully

            if (paymentCaptured.Attributes["status"] == "captured")
            {
                PremiumUser UserOrderDetail = TempData["OrderDetails"] as PremiumUser;
                UserOrderDetail.PaymentId = paymentId;
                UserOrderDetail.Amount = (UserOrderDetail.Amount / 100);

                quizDBEntities.PremiumUsers.Add(UserOrderDetail);
                quizDBEntities.SaveChanges();
                // Create these action method
                return RedirectToAction("Success");
            }
            else
            {
                return RedirectToAction("Failed");
            }
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Failed()
        {
            return View();
        }
    }
}