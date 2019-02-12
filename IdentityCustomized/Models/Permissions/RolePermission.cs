using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityCustomized.Models
{
    public class RolePermission
    {
        public RolePermission()
        {           
        }

        [Key]
        [Column(Order = 1)]
        public string RoleID { get; set; }

        [Key]
        [Column(Order = 2)]
        public string PermissionID { get; set; }

        // Navigation Properties
        
        [ForeignKey("RoleID")]
        public IdentityRole Role { get; set; }

        [ForeignKey("PermissionID")]
        public Permission Permission { get; set; }
        
    }
}