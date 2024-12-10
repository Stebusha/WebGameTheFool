﻿// <auto-generated />
using System;
using BlazorCardGame.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlazorCardGame.Migrations
{
    [DbContext(typeof(FoolGameContext))]
    [Migration("20241210100322_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("BlazorCardGame.DataMangerAPI.Entities.ApplicationUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("FoolGameId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAttack")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastEnterTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("PlayerType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FoolGameId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlazorCardGame.DataMangerAPI.Entities.CardInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CardType")
                        .HasColumnType("int");

                    b.Property<int>("FoolGameId")
                        .HasColumnType("int");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("FoolGameId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("BlazorCardGame.DataMangerAPI.Entities.FoolGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CountOfGames")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("BlazorCardGame.DataMangerAPI.Entities.FoolGameScores", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CountOfGames")
                        .HasColumnType("int");

                    b.Property<int?>("NumberOfLosses")
                        .HasColumnType("int");

                    b.Property<int?>("NumberOfWins")
                        .HasColumnType("int");

                    b.Property<int?>("NumerOfDraws")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("BlazorCardGame.DataMangerAPI.Entities.ApplicationUser", b =>
                {
                    b.HasOne("BlazorCardGame.DataMangerAPI.Entities.FoolGame", "FoolGame")
                        .WithMany("Players")
                        .HasForeignKey("FoolGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoolGame");
                });

            modelBuilder.Entity("BlazorCardGame.DataMangerAPI.Entities.CardInfo", b =>
                {
                    b.HasOne("BlazorCardGame.DataMangerAPI.Entities.FoolGame", "FoolGame")
                        .WithMany()
                        .HasForeignKey("FoolGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoolGame");
                });

            modelBuilder.Entity("BlazorCardGame.DataMangerAPI.Entities.FoolGameScores", b =>
                {
                    b.HasOne("BlazorCardGame.DataMangerAPI.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlazorCardGame.DataMangerAPI.Entities.FoolGame", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
