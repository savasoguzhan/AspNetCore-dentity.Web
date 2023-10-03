using AspNetCoreİdentity.Web.CustomValidations;
using AspNetCoreİdentity.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreİdentity.Web.Extension
{
    public static class StartUpExtension
    {

        public static void AddIdentiyWithEx(this IServiceCollection services)
        {

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(2);
            });
            services.AddIdentity<UygulamaUser, UygulamaRole>(options =>
            {


                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;



                //lockout islemlerini burada yapiyoruz 
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                options.Lockout.MaxFailedAccessAttempts = 3;





            }).AddUserValidator<UserValidator>()
                 .AddPasswordValidator<PasswordValidator>()
                  .AddEntityFrameworkStores<UygulamaDbContext>()
                   .AddDefaultTokenProviders();
        }
    }
}
