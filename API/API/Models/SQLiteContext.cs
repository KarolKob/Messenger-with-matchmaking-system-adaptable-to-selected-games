﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Models
{
    public class SQLiteContext : DbContext
    {
        //Tabela PlayersDB o kolumnach zawartych w modelu Player
        public DbSet<Player> PlayersDB { get; set; }
        public DbSet<Game> GamesDB { get; set; }
        //public SQLiteContext(DbContextOptions<SQLiteContext> opt) : base(opt)
        //{

        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "SQLitePlayerBase.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection, options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
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
            modelBuilder.Entity<Game>().ToTable("Games", "test");
            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.GameId);
                entity.Property(e => e.GameDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            base.OnModelCreating(modelBuilder);
        }

    }
}