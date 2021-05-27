using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Quizz.ViewModel
{
    public class CandidateAnswer
    {
        public int CategoryId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
    }
}