//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Quizz.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Option
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public string OptionName { get; set; }
        public Nullable<int> CategoryId { get; set; }
    }
}
