using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Quizz.Models;

namespace Quizz.ViewModel
{
    public class EditViewModel
    {
        public int CategoryId { get; set; }
        public int QuestionId { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="Question name is required.")]
        public string QuestionName { get; set; }
        public List<Option> ListOfOptions { get; set; }
        public Answer AnswerText { get; set; }
    }
    
}