using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quizz.Models;
using Quizz.ViewModel;

namespace Quizz.Controllers
{
    public class QuizController : Controller
    {
        private QuizDBEntities quizDBEntities;
        public QuizController()
        {
            quizDBEntities = new QuizDBEntities();
        }
        // GET: Quiz
        public ActionResult Index()
        {
            QuizCategoryViewModel quizCategory = new QuizCategoryViewModel();
            quizCategory.ListOfCategory = (from categoryobj in quizDBEntities.Categories
                                           select new SelectListItem()
                                           {
                                               Value = categoryobj.CategoryId.ToString(),
                                               Text = categoryobj.CategoryName
                                           }).ToList();
            return View(quizCategory);
        }
        [HttpPost]
        public ActionResult Index(QuizCategoryViewModel quizCategory)
        {
            if(ModelState.IsValid)
            {
                if(quizCategory.CategoryId != 1)
                {
                    return RedirectToAction("Index", "Payment");
                }
                if(Session["UserId"] !=  null)
                {
                    Session.Clear();
                }
                User user = new User();
                user.UserName = quizCategory.Name;
                user.LoginTime = DateTime.Now;
                user.CategoryId = quizCategory.CategoryId;
                quizDBEntities.Users.Add(user);
                quizDBEntities.SaveChanges();

                Session["UserName"] = quizCategory.Name;
                Session["CategoryId"] = quizCategory.CategoryId;
                Session["UserId"] = user.UserId;
                return RedirectToAction("UserQuestionAnswer");
                //return View("QuestionIndex");
            }
            else
            {
                return View();
            }
           
        }
        public PartialViewResult UserQuestionAnswer(CandidateAnswer useranswer)
        {
            bool IsLast = false;
            if(useranswer.AnswerText != null)
            {
                List<CandidateAnswer> useranswers = Session["QuestionAnswer"] as List<CandidateAnswer>;
                if(useranswers == null)
                {
                    useranswers = new List<CandidateAnswer>();
                }

                useranswers.Add(useranswer);  
                Session["QuestionAnswer"] = useranswers;
            }
            
            int pageSize = 1;
            int pageNumber = 0;
            int CategoryId = Convert.ToInt32(Session["CategoryId"]);
            if(Session["QuestionAnswer"] == null)
            {
                pageNumber = pageNumber + 1;
            }
            else
            {
                List<CandidateAnswer> Answers = Session["QuestionAnswer"] as List<CandidateAnswer>;
                pageNumber = Answers.Count + 1;
            }
            List<Question> listOfQuestion = new List<Question>();
            listOfQuestion = quizDBEntities.Questions.Where(model => model.CategoryId == CategoryId).ToList();
            
            if(pageNumber == listOfQuestion.Count)
            {
                IsLast = true;
            }
            
            QuestionAnswerViewModel questionAnswer = new QuestionAnswerViewModel();
            Question question = new Question();
            question = listOfQuestion.Skip((pageNumber - 1) * pageSize).Take(pageSize).FirstOrDefault();

            questionAnswer.IsLast = IsLast;
            ModelState.Remove("QuestionId");
            questionAnswer.QuestionId = question.QuestionId;
            questionAnswer.CategoryId = question.CategoryId;
            questionAnswer.QuestionName = question.QuestionName;
            questionAnswer.ListOfOptions = (from option in quizDBEntities.Options
                                            where option.QuestionId == question.QuestionId && option.CategoryId == question.CategoryId
                                            select new QuizOption()
                                            {
                                                OptionName = option.OptionName,
                                                OptionId = option.OptionId
                                            }).ToList();

            return PartialView("QuizQuestionOption", questionAnswer);
        }

        public JsonResult SaveAnswer(CandidateAnswer candidateAnswer)
        {
            List<CandidateAnswer> UserAnswers = Session["QuestionAnswer"] as List<CandidateAnswer>;
            if(candidateAnswer.AnswerText != null)
            {
                List<CandidateAnswer> candidateAnswers = Session["QuestionAnswer"] as List<CandidateAnswer>;
                if(candidateAnswers == null)
                {
                    candidateAnswers = new List<CandidateAnswer>();
                }
                candidateAnswers.Add(candidateAnswer);
                Session["QuestionAnswer"] = candidateAnswers;
            }
            foreach(var item in UserAnswers)
            {
                Result result = new Result();
                result.AnswerText = item.AnswerText;
                result.QuestionId = item.QuestionId;
                result.CategoryId = item.CategoryId;
                result.UserId = Convert.ToInt32(Session["UserId"]);
                quizDBEntities.Results.Add(result);
                quizDBEntities.SaveChanges();
                
            }
            return Json(new { message = "Data Succeccfully Added.", success = true}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFinalResult()
        {
            List<CandidateAnswer> listOfQuestionAnswers;
            listOfQuestionAnswers = Session["QuestionAnswer"] as List<CandidateAnswer>;
            var UserResult = (from result in listOfQuestionAnswers
                              join answerDB in quizDBEntities.Answers on result.QuestionId equals answerDB.QuestionId
                              join questionDB in quizDBEntities.Questions on result.QuestionId equals questionDB.QuestionId

                              select new ResultModel()
                              {
                                  Question = questionDB.QuestionName,
                                  Answer = answerDB.AnswerText,
                                  AnswerByUser = result.AnswerText,
                                  Status = answerDB.AnswerText == result.AnswerText ? "Correct" : "Wrong"
                              }).ToList();
            //Session.Abandon();
            return View(UserResult);
        }
    }
}