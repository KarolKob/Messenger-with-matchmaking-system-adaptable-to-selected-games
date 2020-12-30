﻿// <auto-generated />
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Migrations.SQLite
{
    [DbContext(typeof(SQLiteContext))]
    [Migration("20201122221101_PlayerBase")]
    partial class PlayerBase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10");

            modelBuilder.Entity("API.Models.Player", b =>
                {
                    b.Property<string>("Nickname")
                        .HasColumnType("TEXT");

                    b.Property<string>("Adress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(250);

                    b.Property<string>("Localization")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Rank")
                        .HasColumnType("TEXT");

                    b.Property<string>("WinRate")
                        .HasColumnType("TEXT");

                    b.HasKey("Nickname");

                    b.ToTable("PlayersDB");
                });
#pragma warning restore 612, 618
        }
    }
}