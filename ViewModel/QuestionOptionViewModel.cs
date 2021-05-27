using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quizz.Models;

namespace Quizz.ViewModel
{
    public class QuestionOptionViewModel
    {
        public int CategoryId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public List<string> ListOfOptions { get; set; }
        public string AnswerText { get; set; }

    }
}