using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreİdentity.Web.Requirement
{
    public class ExchangeExpireRequirement : IAuthorizationRequirement
    {

    }

    public class ExchangeExpireRequirementHandler : AuthorizationHandler<ExchangeExpireRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ExchangeExpireRequirement requirement)
        {

            var hasExchangeExpireClaim = context.User.HasClaim(x => x.Type == "ExchangeExpireDate");

            if (hasExchangeExpireClaim!)
            {
                context.Fail();
            }

            var exchangeExpireDate = context.User.FindFirst("ExchangeExpireDate");

            if(DateTime.Now > Convert.ToDateTime(exchangeExpireDate.Value))
            {
                context.Fail();
            }
            else
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;

        }
    }
}
