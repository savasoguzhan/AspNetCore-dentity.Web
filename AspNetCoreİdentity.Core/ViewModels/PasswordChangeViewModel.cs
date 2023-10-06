using System.ComponentModel.DataAnnotations;

namespace AspNetCoreİdentity.Core.ViewModels
{
    public class PasswordChangeViewModel
    {
        public string PasswordOld { get; set; }



        [Required(ErrorMessage = "Sifre Ad Alani Bos Birakilamaz")]
        [Display(Name = "Sifre:")]
        public string PasswordNew { get; set; }


        public string PasswordNewConfirm { get; set; }
    }
}
