using Microsoft.EntityFrameworkCore;

namespace BartMarket.Data
{
    public class UserContext : DbContext
    {
        public DbSet<WarehouseModel> Warehouses { get; set; }
        public DbSet<WarehouseSetting> WarehouseSettings { get; set; }
        public DbSet<LinkModel> LinkModels { get; set; }

        public UserContext()
        {
            Database.Migrate();
            //Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Data.db");
        }
    }
}
