using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsBox.Models;

namespace NewsBox.Data
{
    public class NewsDbContext : IdentityDbContext<AppUser>
    {
        public NewsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Info> infos { get; set; }
    }
}
