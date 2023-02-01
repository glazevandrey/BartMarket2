using Microsoft.EntityFrameworkCore;
using System;

namespace BartMarket.Data
{
    public class UserContext : DbContext
    {
        public DbSet<WarehouseModel> Warehouses { get; set; }
        public DbSet<WarehouseSetting> WarehouseSettings { get; set; }
        public DbSet<LinkModel> LinkModels { get; set; }
        public DbSet<UploadedOzonId> UploadedOzonIds { get; set; }

        public UserContext()
        {
            Database.SetCommandTimeout(300);
           // Database.Migrate();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
             //optionsBuilder.UseSqlite("Filename=Data.db");
            optionsBuilder.UseMySql("server=db4free.net;user=bartmarket;password=448agiAoi;database=bartmarket;", builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);

            });

            base.OnConfiguring(optionsBuilder);

            //optionsBuilder.UseSqlite("Filename=Data.db");
        }
    }
}
