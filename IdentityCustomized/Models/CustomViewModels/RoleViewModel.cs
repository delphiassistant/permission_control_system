using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdentityCustomized.Models
{
    public class RoleViewModel
    {
        [Key]
        public string RoleID { get; set; }
        [Display(Name="نام گروه کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RoleName { get; set; }

        [Display(Name = "نام گروه کاربری (بومی)")]
        public string RoleNameLocalized { get; set; }
    }
}