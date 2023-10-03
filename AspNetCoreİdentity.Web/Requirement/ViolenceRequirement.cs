using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AspNetCoreİdentity.Web.Requirement
{
    public class ViolenceRequirement : IAuthorizationRequirement
    {
        public int ThresholdAge { get; set; }
    }

    public class ViolenceRequirementHandler : AuthorizationHandler<ViolenceRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ViolenceRequirement requirement)
        {
           if(!context.User.HasClaim(x =>x.Type=="birthdate"))
            {
                context.Fail();
                return Task.CompletedTask;
            }


            Claim brithdate = context.User.FindFirst("birthdate")!;


            var today = DateTime.Now;
            
            var age = today.Year - Convert.ToDateTime(brithdate.Value).Year;

            if (requirement.ThresholdAge > age)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
