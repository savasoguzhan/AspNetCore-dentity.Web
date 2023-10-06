using AspNetCoreİdentity.Core.Models;
using AspNetCoreİdentity.   Repository.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreİdentity.Web.CustomValidations
{
    public class PasswordValidator : IPasswordValidator<UygulamaUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<UygulamaUser> manager, UygulamaUser user, string? password)
        {


            var errors = new List<IdentityError>();
           
            if(password!.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new() { Code = "PasswordContaionUserName", Description = "Sifre Kullanici Adi Iceremez" });
            }

            if(password!.ToLower().StartsWith("1234"))
            {
                errors.Add(new() { Code = "PasswordContaion1234", Description = "Sifre Alani Ardisik Sayi Iceremez" });
            }

            if(errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);



        }
    }
}
