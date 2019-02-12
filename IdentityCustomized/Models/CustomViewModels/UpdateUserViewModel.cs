using System.ComponentModel.DataAnnotations;

namespace IdentityCustomized.Models
{
    public class UpdateUserViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "فرمت آدرس ایمیل صحیح نیست")]
        public string Email { get; set; }

        [Display(Name = "نام کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Username { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string NationalCode { get; set; }

        [Display(Name = "شماره موبایل")]
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
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