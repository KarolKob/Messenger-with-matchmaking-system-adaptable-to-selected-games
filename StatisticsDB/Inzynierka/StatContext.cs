using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Inzynierka
{
    class StatContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Games { get; set; }
        public DbSet<Team> Teams { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Statistics.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Map table names
            modelBuilder.Entity<Player>().ToTable("Players", "test");
            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.PlayerId);
                entity.HasIndex(e => e.NickName).IsUnique();
            });
            modelBuilder.Entity<Match>().ToTable("Matches", "test");
            modelBuilder.Entity<Match>(entity =>
            {
                entity.HasKey(e => e.GameId);
                entity.Property(e => e.GameDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<Team>().ToTable("Teams", "test");
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.TeamID);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
