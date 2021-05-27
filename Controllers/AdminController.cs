using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quizz.Models;
using Quizz.ViewModel;

namespace Quizz.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private QuizDBEntities quizDBEntities;
        public AdminController()
        {
            quizDBEntities = new QuizDBEntities();
        }
        // GET: Admin
        public ActionResult Index()
        {
            CategoryViewModel category = new CategoryViewModel();
            category.ListOfCagtegory = (from categoryobj in quizDBEntities.Categories
                                        select new SelectListItem()
                                        {
                                            Value = categoryobj.CategoryId.ToString(),
                                            Text = categoryobj.CategoryName
                                        }).ToList();

            return View(category);
        }

        [HttpPost]
        public JsonResult Index(QuestionOptionViewModel QuestionOption)
        {
            Question question = new Question();
            question.QuestionName = QuestionOption.QuestionName;
            question.CategoryId = QuestionOption.CategoryId;
            question.IsActive = true;
            question.IsMultiple = false;
            quizDBEntities.Questions.Add(question);
            quizDBEntities.SaveChanges();

            int questionId = question.QuestionId;
            int categId = question.CategoryId;
            Option option = new Option();
            foreach (var optionitem in QuestionOption.ListOfOptions)
            {                
                option.OptionName = optionitem;
                option.QuestionId = questionId;
                option.CategoryId = categId;
                quizDBEntities.Options.Add(option);
                quizDBEntities.SaveChanges();
            }

            Answer answer = new Answer();
            answer.AnswerText = QuestionOption.AnswerText;
            answer.QuestionId = questionId;
            answer.CategoryId = categId;
            quizDBEntities.Answers.Add(answer);
            quizDBEntities.SaveChanges();

            return Json(new { message = "Data Succssfully Added.", success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DisplayQuestion()
        {
            CategoryViewModel category = new CategoryViewModel();
            category.ListOfCagtegory = (from categoryobj in quizDBEntities.Categories
                                        select new SelectListItem()
                                        {
                                            Value = categoryobj.CategoryId.ToString(),
                                            Text = categoryobj.CategoryName
                                        }).ToList();

            return View(category);
        }
        [HttpPost]
        public ActionResult DisplayQuestion(int CategoryId)
        {
            List<EditViewModel> QuestionOptionList = new List<EditViewModel>();
            List<Question> ListOfQuestion = quizDBEntities.Questions.Where(model => model.CategoryId == CategoryId).ToList();
            foreach(var question in ListOfQuestion)
            {
                EditViewModel EditQuestionOption = new EditViewModel();

                EditQuestionOption.CategoryId = question.CategoryId;
                EditQuestionOption.QuestionId = question.QuestionId;
                EditQuestionOption.QuestionName = question.QuestionName;
                EditQuestionOption.ListOfOptions = quizDBEntities.Options.Where(model => model.QuestionId == question.QuestionId).ToList();
                EditQuestionOption.AnswerText = quizDBEntities.Answers.Where(model => model.QuestionId == question.QuestionId).FirstOrDefault();

                QuestionOptionList.Add(EditQuestionOption);
            }
            
            return View("EditPost",QuestionOptionList);
        }

        [HttpPost]
        public ActionResult Edit(EditViewModel EditQuestion)
        {
            if(ModelState.IsValid)
            {
                var UpdateQuestion = quizDBEntities.Questions.Where(model => model.QuestionId == EditQuestion.QuestionId && model.CategoryId == EditQuestion.CategoryId).FirstOrDefault();
                UpdateQuestion.QuestionName = EditQuestion.QuestionName;
                quizDBEntities.Entry(UpdateQuestion).State = EntityState.Modified;

                var Options = EditQuestion.ListOfOptions.ToList();
                var UpdateOption = quizDBEntities.Options.Where(model => model.QuestionId == EditQuestion.QuestionId && model.CategoryId == EditQuestion.CategoryId).ToList();
                
                for(int i = 0; i < Options.Count; i++)
                {
                    UpdateOption[i].OptionName = Options[i].OptionName;
                    quizDBEntities.Entry(UpdateOption[i]).State = EntityState.Modified;
                }
                

                var UpdateAnswer = quizDBEntities.Answers.Where(model => model.QuestionId == EditQuestion.QuestionId && model.CategoryId == EditQuestion.CategoryId).FirstOrDefault();
                UpdateAnswer.AnswerText = EditQuestion.AnswerText.AnswerText;
                quizDBEntities.Entry(UpdateAnswer).State = EntityState.Modified;

                quizDBEntities.SaveChanges();
            }
            
            //ModelState.Clear();
            return RedirectToAction("DisplayQuestion");
        }

        [HttpPost]
        public ActionResult Delete(EditViewModel DeleteQuestion)
        {
            var RemoveQuestion = quizDBEntities.Questions.Where(model => model.QuestionId == DeleteQuestion.QuestionId && model.CategoryId == DeleteQuestion.CategoryId).FirstOrDefault();
            quizDBEntities.Questions.Remove(RemoveQuestion);

            var UpdateOption = quizDBEntities.Options.Where(model => model.QuestionId == DeleteQuestion.QuestionId && model.CategoryId == DeleteQuestion.CategoryId).ToList();

            for (int i = 0; i < UpdateOption.Count; i++)
            {
                quizDBEntities.Options.Remove(UpdateOption[i]);
            }

            var UpdateAnswer = quizDBEntities.Answers.Where(model => model.QuestionId == DeleteQuestion.QuestionId && model.CategoryId == DeleteQuestion.CategoryId).FirstOrDefault();
            quizDBEntities.Answers.Remove(UpdateAnswer);

            quizDBEntities.SaveChanges();
            return RedirectToAction("DisplayQuestion");
        }
        public ActionResult UserDetail()
        {
            OrderDetail UserDetails = new OrderDetail();
            UserDetails.OrderDetailList = quizDBEntities.PremiumUsers.ToList();
            return View(UserDetails.OrderDetailList);
        }
    }
}