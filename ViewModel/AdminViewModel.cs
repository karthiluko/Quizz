using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Quizz.ViewModel
{
    public class AdminViewModel
    {
        [Display(Name ="Username : ")]
        [Required(AllowEmptyStrings = false , ErrorMessage ="Username Required")]
        public string UserName { get; set; }

        [Display(Name = "Passworrd : ")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}