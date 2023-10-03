using AspNetCoreİdentity.Web.Models;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AspNetCoreİdentity.Web.CustomValidations
{
    public class UserValidator : IUserValidator<UygulamaUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<UygulamaUser> manager, UygulamaUser user)
        {
            var erros = new List<IdentityError>();
            var isNumeric = int.TryParse(user!.UserName[0].ToString(), out _);

            if(isNumeric)
            {
                erros.Add(new IdentityError { Code="UserNameContainFirstLetterDigig",Description="Kullanici Adi Digit Iceremez"});


            }
            if (erros.Any())
            {
                return Task.FromResult(IdentityResult.Failed(erros.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
        }
    }
}
