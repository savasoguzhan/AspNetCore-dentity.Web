﻿using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreİdentity.Web.ViewModels
{
    public class SignInViewModel
    {


        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Email Alani Bos Birakilamaz")]
        [Display(Name = "Kullanici Adi:")]
        public string Email { get; set; }

        public string Password { get; set; }


        public bool RememberMe { get; set; }

    }
}
