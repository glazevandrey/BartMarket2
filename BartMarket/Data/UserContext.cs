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
            optionsBuilder.UseMySql("server=localhost;user=root;password=448agiAoi;database=mysql;");
            //  optionsBuilder.UseMySql("server=n50jp.spectrum.myjino.ru;user=1231402_wp1;password=WDD$,p|h3/r8e;database=1231402_wp1;");
            //optionsBuilder.UseSqlite("Filename=Data.db");
        }
    }
}
