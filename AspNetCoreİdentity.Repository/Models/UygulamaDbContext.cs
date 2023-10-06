using AspNetCoreİdentity.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreİdentity.Repository.Models
{
    public class UygulamaDbContext : IdentityDbContext<UygulamaUser, UygulamaRole, string>
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options): base(options)
        {
            
        }
    }
}
