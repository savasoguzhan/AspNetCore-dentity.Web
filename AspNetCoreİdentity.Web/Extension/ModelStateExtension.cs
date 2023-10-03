using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AspNetCoreİdentity.Web.Extension
{
    public static class ModelStateExtension 
    {
        public static void AddModelErrorList(this ModelStateDictionary modelState, List<string> error)
        {
            error.ForEach(x =>
            {
                modelState.AddModelError(string.Empty, x);
            });
        }


        public static void AddModelErrorList(this ModelStateDictionary modelState, IEnumerable<IdentityError> errors)
        {
            errors.ToList().ForEach(x =>
            {
                modelState.AddModelError(string.Empty, x.Description);
            });
        }
    }
}
