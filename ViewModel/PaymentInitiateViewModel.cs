using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Quizz.ViewModel
{
    public class PaymentInitiateViewModel
    {
        [Required(AllowEmptyStrings =false, ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ConfirmPassword is required")]
        [CompareAttribute("Password", ErrorMessage = "Password doesn't match.")]
        public string ConfirmPassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ContactNumber is required")]
        public string ContactNumber { get; set; }
        public int Amount { get; set; }
    }
}