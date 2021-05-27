using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.ViewModel
{
    public class QuestionAnswerViewModel
    {
        public bool IsLast { get; set; }
        public int OptionId { get; set; }
        public int CategoryId { get; set; }
        public int QuestionId { get; set; }
        public string QuestionName { get; set; }
        public List<QuizOption> ListOfOptions { get; set; }

    }
    public class QuizOption
    {
        public int OptionId { get; set; }
        public string OptionName { get; set; }

    }
}