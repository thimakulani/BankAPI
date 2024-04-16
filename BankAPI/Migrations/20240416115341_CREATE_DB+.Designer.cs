﻿// <auto-generated />
using System;
using BankAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BankAPI.Migrations
{
    [DbContext(typeof(BankDbContext))]
    [Migration("20240416115341_CREATE_DB+")]
    partial class CREATE_DB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("BankAPI.Models.AccountHolder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext");

                    b.Property<string>("IdNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("AccountHolders");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            FirstName = "Thima",
                            LastName = "Sigauque"
                        });
                });

            modelBuilder.Entity("BankAPI.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountHolderId")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("longtext");

                    b.Property<string>("HouseNo")
                        .HasColumnType("longtext");

                    b.Property<string>("PostalCode")
                        .HasColumnType("longtext");

                    b.Property<string>("Street")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AccountHolderId")
                        .IsUnique();

                    b.ToTable("Address");
                });

            modelBuilder.Entity("BankAPI.Models.AuditLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .HasColumnType("longtext");

                    b.Property<string>("NewData")
                        .HasColumnType("longtext");

                    b.Property<string>("TableName")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("AuditLogs");
                });

            modelBuilder.Entity("BankAPI.Models.BankAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountHolderId")
                        .HasColumnType("int");

                    b.Property<string>("AccountNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("AccountType")
                        .HasColumnType("longtext");

                    b.Property<decimal>("AvailableBalance")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AccountHolderId");

                    b.ToTable("BankAccounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccountHolderId = 1,
                            AccountNumber = "55555",
                            AccountType = "Cheque",
                            AvailableBalance = 15000m,
                            Name = "My Account",
                            Status = "Active"
                        });
                });

            modelBuilder.Entity("BankAPI.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountHolderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Token")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AccountHolderId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("BankAPI.Models.Address", b =>
                {
                    b.HasOne("BankAPI.Models.AccountHolder", "AccountHolder")
                        .WithOne("Address")
                        .HasForeignKey("BankAPI.Models.Address", "AccountHolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountHolder");
                });

            modelBuilder.Entity("BankAPI.Models.BankAccount", b =>
                {
                    b.HasOne("BankAPI.Models.AccountHolder", "AccountHolder")
                        .WithMany()
                        .HasForeignKey("AccountHolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountHolder");
                });

            modelBuilder.Entity("BankAPI.Models.RefreshToken", b =>
                {
                    b.HasOne("BankAPI.Models.AccountHolder", "AccountHolder")
                        .WithMany()
                        .HasForeignKey("AccountHolderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountHolder");
                });

            modelBuilder.Entity("BankAPI.Models.AccountHolder", b =>
                {
                    b.Navigation("Address");
                });
#pragma warning restore 612, 618
        }
    }
}
