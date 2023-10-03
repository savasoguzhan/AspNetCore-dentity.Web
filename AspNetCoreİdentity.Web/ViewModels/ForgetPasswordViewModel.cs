using System.ComponentModel.DataAnnotations;

namespace AspNetCoreİdentity.Web.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [Required(ErrorMessage = "Email Alani Bos Birakilamaz")]
        [Display(Name = "Email:")]
        public string Email { get; set; }
    }
}
