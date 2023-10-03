using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreİdentity.Web.Models
{
    public class UygulamaDbContext : IdentityDbContext<UygulamaUser, UygulamaRole, string>
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options): base(options)
        {
            
        }
    }
}
