using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentityCustomized.Models
{
    public class Person
    {
        [Key]
        public int PersonID { get; set; }

        [Display(Name = "نام شخص")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PersonName { get; set; }

        [Display(Name = "سن شخص")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int PersonAge { get; set; }
    }
}