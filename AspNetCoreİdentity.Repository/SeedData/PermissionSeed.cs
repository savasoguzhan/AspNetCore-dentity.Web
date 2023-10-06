using AspNetCoreİdentity.Core.Models;
using AspNetCoreİdentity.Repository.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using System.Data;
using System.Security.Claims;

namespace AspNetCoreİdentity.Repository.SeedData
{
    public class PermissionSeed
    {

        public static async Task Seed(RoleManager<UygulamaRole> roleManager)
        {
            var hasBasicRole = await roleManager.RoleExistsAsync("BasicRole");

            if(!hasBasicRole)
            {
                await roleManager.CreateAsync(new UygulamaRole() { Name = "BasicRole" });

                var role = await  roleManager.FindByIdAsync("BasicRole");
                //await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Permissions.Stock.Read));

                //await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Permissions.Order.Read));

                //await roleManager.AddClaimAsync(role, new Claim("Permission", Permission.Permissions.Catalog.Read));

            }



           
            
                
        }
    }
}
