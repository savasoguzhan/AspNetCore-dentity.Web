using AspNetCoreİdentity.Core.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreİdentity.Core.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Kullanici Ad Alani Bos Birakilamaz")]
        [Display(Name = "Kullanici Adi:")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Email Alani Bos Birakilamaz")]
        [Display(Name = "Email:")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Telefon Ad Alani Bos Birakilamaz")]
        [Display(Name = "Telefon:")]
        public string Phone { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDay { get; set; }

        public string? City { get; set; }


        public IFormFile? Picture { get; set; }

        public Gender Gender { get; set; }





    }
}
