using Microsoft.EntityFrameworkCore;
using RandoStore_Temp.Models;

namespace RandoStore_Temp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Category> Categories {get;set;}

    }
}
