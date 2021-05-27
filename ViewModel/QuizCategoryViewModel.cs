using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Quizz.ViewModel
{
    public class QuizCategoryViewModel
    {
        [Display(Name = "Category")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Category field required")]
        public int CategoryId { get; set; }
        [Display(Name = "Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name field required")]
        public string Name { get; set; }
        public IEnumerable<SelectListItem> ListOfCategory { get; set; }
    }
}