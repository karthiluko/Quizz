using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Quizz.Models;


namespace Quizz.ViewModel
{
   
    public class CategoryViewModel
    {
        [Display(Name ="Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage ="Category is required")]
        public int CategoryId { get; set; }
        [Display(Name = "Question")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Question is required")]
        public string QuestionName { get; set; }

        public string OptionName { get; set; }
        public string CategoryName { get; set; }
        public int QuestionId { get; set; }
        public IEnumerable<SelectListItem> ListOfCagtegory { get; set; }
        //public IEnumerable<Question> ListOfQuestion { get; set; }
        //public IEnumerable<Option> ListOfOptions { get; set; }
    }
}