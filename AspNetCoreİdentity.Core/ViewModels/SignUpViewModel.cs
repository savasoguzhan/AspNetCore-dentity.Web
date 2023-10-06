using System.ComponentModel.DataAnnotations;

namespace AspNetCoreİdentity.Core.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage ="Kullanici Ad Alani Bos Birakilamaz")]
        [Display(Name ="Kullanici Adi:")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Email Alani Bos Birakilamaz")]
        [Display(Name = "Email:")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Telefon Ad Alani Bos Birakilamaz")]
        [Display(Name = "Telefon:")]
        public string Phone { get; set; }


        [Required(ErrorMessage = "Sifre Ad Alani Bos Birakilamaz")]
        [Display(Name = "Sifre:")]
        public string Password { get; set; }


        [Compare(nameof(Password),ErrorMessage ="Sifre Ayni Degildir")]
        [Required(ErrorMessage = "Sifre Tekrar Ad Alani Bos Birakilamaz")]
        [Display(Name = "Sifre Tekrar:")]
        public string PasswordConfirm { get; set; }
    }
}
