using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheSocialBaz.Model.Enteties;

namespace TheSocialBaz.Data
{
    // ENTITY FRAMEWORK PODESAVANJA BAZE!
    public class DBContext : DbContext
    {
        private readonly IConfiguration configuration;

        public DBContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("postgresDatabase"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //this is empty method
        }
    }
}
