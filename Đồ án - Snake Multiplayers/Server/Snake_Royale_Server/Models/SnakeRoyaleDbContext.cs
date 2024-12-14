using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


namespace Snake_Royale_Server.Models
{
    public class SnakeRoyaleDbContext : DbContext
    {
        public SnakeRoyaleDbContext()
        {
        }

        public DbSet<Account> Accounts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=SnakeRoyaleDB.db;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
