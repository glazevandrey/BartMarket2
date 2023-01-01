using Microsoft.EntityFrameworkCore;

namespace BartMarket.Data
{
    public class UserContext : DbContext
    {
        public DbSet<WarehouseModel> Warehouses { get; set; }
        public DbSet<WarehouseSetting> WarehouseSettings { get; set; }
        public DbSet<LinkModel> LinkModels { get; set; }
        public DbSet<UploadedOzonId> UploadedOzonIds{ get; set; }

        public UserContext()
        {
            Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //  optionsBuilder.UseMySql(@"server=mysql.j48317898.myjino.ru;user=j48317898;password=448agiAoi;database=j48317898;");
            //optionsBuilder.UseSqlite("Filename=Data.db");
            optionsBuilder.UseMySql("server=localhost:3306;user=root;password=448agiAoi;database=newdb;");
            //  optionsBuilder.UseMySql("server=n50jp.spectrum.myjino.ru;user=1231402_wp1;password=WDD$,p|h3/r8e;database=1231402_wp1;");
            //optionsBuilder.UseSqlite("Filename=Data.db");
        }
    }
}
