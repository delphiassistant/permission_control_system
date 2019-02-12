using System.ComponentModel.DataAnnotations;

namespace IdentityCustomized.Models
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "فرمت آدرس ایمیل صحیح نیست")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "طول {0} میبایست حداقل {2} کاراکتر باشد", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور (مجدد)")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Compare("Password", ErrorMessage = "دو کلمه عبور وارد شده یکسان نیستند")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "نام کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Username { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string NationalCode { get; set; }

        [Display(Name = "شماره موبایل")]        
        public string Phone { get; set; }

        [Display(Name = "شماره تلفن ثابت")]
        public string LandLinePhoneNumber { get; set; }

        [Display(Name = "آدرس پستی")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }

        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Fullname { get; set; }

        [Display(Name = "دارای دسترسی Super Admin")]
        public bool IsSuperAdmin { get; set; }
    }
}