using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityCustomized.Models
{
    public class PermissionGroup
    {
        public PermissionGroup()
        {
            Permissions = new List<Permission>();
        }

        [Key]
        [Display(Name="شناسه گروه")]        
        public string PermissionGroupID { get; set; }

        [Display(Name="عنوان گروه دسترسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PermissionGroupTitle { get; set; }

        public string PermissionGroupNamespace { get; set; }

        [Display(Name = "عنوان گروه دسترسی (بومی)")]
        public string PermissionGroupTitleLocalized { get; set; }

        public bool RequiresAuthorization { get; set; }

        // Navigation Properties
        public List<Permission> Permissions { get; set; }
    }
}