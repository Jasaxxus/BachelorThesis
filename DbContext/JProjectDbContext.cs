using JProject;
using JProject.Models;
using Microsoft.EntityFrameworkCore;


    public class JprojectDbContext : DbContext
    {
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Settings> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=JProjectDataBase.sqlite");
        }
    }

