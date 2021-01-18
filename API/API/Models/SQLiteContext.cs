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
        public DbSet<Player> PlayersDB { get; set; }
        public DbSet<MatchTeam> TeamGamesDB { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<MatchSolo> SoloGamesDB { get; set; }
        //public SQLiteContext(DbContextOptions<SQLiteContext> opt) : base(opt)
        //{

        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=SQLiteMatchmakingDataBase.db", options =>
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
            modelBuilder.Entity<MatchTeam>().ToTable("Team Matches", "test");
            modelBuilder.Entity<MatchTeam>(entity =>
            {
                entity.HasKey(e => e.GameId);
                entity.Property(e => e.GameDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<Team>().ToTable("Teams", "test");
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.TeamID);
            });
            modelBuilder.Entity<MatchSolo>().ToTable("Solo Matches", "test");
            modelBuilder.Entity<MatchSolo>(entity =>
            {
                entity.HasKey(e => e.GameId);
                entity.Property(e => e.GameDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            base.OnModelCreating(modelBuilder);
        }
    }

}
