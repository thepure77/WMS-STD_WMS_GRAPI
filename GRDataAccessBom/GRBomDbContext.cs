using BomDataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccessBom
{
    public class GRBomDbContext : DbContext
    {
        
        public virtual DbSet<im_BOM> im_BOM { get; set; }
        public virtual DbSet<im_BOMItem> im_BOMItem { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("BomConnection").ToString();

                optionsBuilder.UseSqlServer(connectionString);
            }

        }
    }

    

   
}
