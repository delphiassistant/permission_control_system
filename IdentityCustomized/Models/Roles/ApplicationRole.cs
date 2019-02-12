using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.Owin.Security;

namespace IdentityCustomized.Models
{
    [MetadataType(typeof(ApplicationRoleMetadata))]
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name, string roleNameLocalized) : base(name)
        {
            this.RoleNameLocalized = roleNameLocalized;
        }
        public virtual string RoleNameLocalized { get; set; }
    }

    public class ApplicationRoleMetadata
    {
        [Key]
        public string Id { get; set; }

        [Display(Name = "نام گروه کاربری")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Name { get; set; }

        [Display(Name="نام گروه کاربری (بومی)")]
        public virtual string RoleNameLocalized { get; set; }
    }
}