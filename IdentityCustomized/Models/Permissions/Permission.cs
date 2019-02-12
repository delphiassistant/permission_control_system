using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityCustomized.Models
{
    public class Permission
    {
        [Key]
        [Display(Name = "شناسه دسترسی")]
        public string PermissionID { get; set; }

        [Display(Name="گروه دسترسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PermissionGroupID { get; set; }

        [Display(Name="عنوان دسترسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PermissionTitle { get; set; }

        [Display(Name="عنوان دسترسی (بومی)")]
        public string PermissionTitleLocalized { get; set; }

        [Display(Name = "آیکُن")]
        public string PermissionIcon { get; set; }

        [Display(Name = "دسترسی آزاد برای همه")]
        public bool AllowAnonymous { get; set; }

        [Display(Name = "دسترسی نیازمند لاگین است")]
        public bool RequiresAuthorization { get; set; }        

        [Display(Name = "نام کامل اکشن")]
        public string ActionFullName { get; set; }

        // Navigation Properties
        [ForeignKey("PermissionGroupID")]
        public PermissionGroup PermissionGroup { get; set; }

    }
}