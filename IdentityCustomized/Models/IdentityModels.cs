using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace IdentityCustomized.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    [MetadataType(typeof(ApplicationUserMetadata))]
    public class ApplicationUser : IdentityUser
    {
        //public virtual ICollection<UserLog> UserLogs { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        #region Adding Additional Properties to the ApplicationUser class
        public string NationalCode { get; set; }
        
        public string LandLinePhoneNumber { get; set; }
        
        public string Address { get; set; }
        
        public string Fullname { get; set; }

        public bool IsSuperAdmin { get; set; }
        
        public string OtpCode { get; set; }

        public DateTime? OtpCodeExpiry { get; set; }
        #endregion
    }

    public class ApplicationUserMetadata
    {
        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string NationalCode { get; set; }

        [Display(Name = "شماره تلفن ثابت")]
        public string LandLinePhoneNumber { get; set; }

        [Display(Name = "آدرس پستی")]
        public string Address { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Fullname { get; set; }

        [Display(Name="شماره موبایل")]
        public string PhoneNumber { get; set; }

        [Display(Name="شماره موبایل تایید شده؟")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name="آدرس ایمیل")]
        public string Email { get; set; }

        [Display(Name="آدرس ایمیل تایید شده؟")]
        public bool EmailConfirmed { get; set; }

        [Display(Name="ورود 2 مرحله ای")]
        public bool TwoFactorEnabled { get; set; }

        [Display(Name="تاریخ پایان بلوک بودن اکانت")]
        public DateTime? LockoutEndDateUtc { get; set; }

        [Display(Name="اکانت بلوک است؟")]
        public bool LockoutEnabled { get; set; }

        [Display(Name="دفعات تلاش ناموفق برای ورود")]
        public int AccessFailedCount { get; set; }

        [Display(Name="نام کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UserName { get; set; }

        [Display(Name = "دارای دسترسی Super Admin")]
        [Required]
        public bool IsSuperAdmin { get; set; }        
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<PermissionGroup> PermissionGroups { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>()
            .HasMany(u => u.Roles)
            .WithOptional()
            .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<IdentityUser>()
                        .HasMany(u => u.Logins)
                        .WithOptional()
                        .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<IdentityUser>()
                        .HasMany(u => u.Claims)
                        .WithOptional()
                        .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<IdentityUser>().ToTable("Users").Property(p => p.Id).HasColumnName("UserID");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users").Property(p => p.Id).HasColumnName("UserID");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles").Property(p => p.Id).HasColumnName("RoleID");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles").Property(p => p.RoleId).HasColumnName("RoleID");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles").Property(p => p.UserId).HasColumnName("UserID");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles").Property(p => p.Name).HasColumnName("RoleName");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims").Property(p => p.Id).HasColumnName("UserClaimID");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims").Property(p => p.UserId).HasColumnName("UserID");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins").Property(p => p.UserId).HasColumnName("UserID");

            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");

            // Change these from IdentityRole to ApplicationRole:
            EntityTypeConfiguration<ApplicationRole> entityTypeConfiguration1 =
                modelBuilder.Entity<ApplicationRole>().ToTable("Roles");
            entityTypeConfiguration1.Property((ApplicationRole r) => r.Name).IsRequired();

        }

        public System.Data.Entity.DbSet<IdentityCustomized.Models.Person> People { get; set; }
    }
   
}