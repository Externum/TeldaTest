using Microsoft.EntityFrameworkCore;
using TeldaTest.Models;

namespace TeldaTest.Db
{
    public class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions<BaseContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Region> Regions { get; set; }
    }
}
