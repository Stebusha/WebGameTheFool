﻿// <auto-generated />
using System;
using BlazorCardGame.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlazorCardGame.Migrations
{
    [DbContext(typeof(FoolGameContext))]
    partial class FoolGameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("BlazorCardGame.Entities.ApplicationUser", b =>
                {
                    b.Property<string>("Login")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int?>("FoolGameId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAttack")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.HasKey("Login");

                    b.HasIndex("FoolGameId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlazorCardGame.Entities.CardInfo", b =>
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

            modelBuilder.Entity("BlazorCardGame.Entities.FoolGame", b =>
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

            modelBuilder.Entity("BlazorCardGame.Entities.FoolGameScore", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CountOfGames")
                        .HasColumnType("int");

                    b.Property<int?>("NumberOfDraws")
                        .HasColumnType("int");

                    b.Property<int?>("NumberOfLosses")
                        .HasColumnType("int");

                    b.Property<int?>("NumberOfWins")
                        .HasColumnType("int");

                    b.Property<string>("UserLogin")
                        .IsRequired()
                        .HasColumnType("varchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("UserLogin");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("BlazorCardGame.Entities.ApplicationUser", b =>
                {
                    b.HasOne("BlazorCardGame.Entities.FoolGame", "FoolGame")
                        .WithMany("Players")
                        .HasForeignKey("FoolGameId");

                    b.Navigation("FoolGame");
                });

            modelBuilder.Entity("BlazorCardGame.Entities.CardInfo", b =>
                {
                    b.HasOne("BlazorCardGame.Entities.FoolGame", "FoolGame")
                        .WithMany()
                        .HasForeignKey("FoolGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FoolGame");
                });

            modelBuilder.Entity("BlazorCardGame.Entities.FoolGameScore", b =>
                {
                    b.HasOne("BlazorCardGame.Entities.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserLogin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlazorCardGame.Entities.FoolGame", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
