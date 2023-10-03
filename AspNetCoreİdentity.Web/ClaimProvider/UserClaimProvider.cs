using AspNetCoreİdentity.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AspNetCoreİdentity.Web.ClaimProvider
{
    public class UserClaimProvider : IClaimsTransformation
    {
        private readonly UserManager<UygulamaUser> _userManager;

        public UserClaimProvider(UserManager<UygulamaUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {

            var identityUser=principal.Identity as ClaimsIdentity;

            var currentUser = await _userManager.FindByNameAsync(identityUser.Name);

            if(currentUser.City!=null)
            { 
                if(principal.HasClaim(x => x.Type != "city"))
                {
                    Claim cityClaim = new Claim("city",currentUser.City);

                    identityUser.AddClaim(cityClaim);
                }
            
            
            }

            return principal;


        }
    }
}
