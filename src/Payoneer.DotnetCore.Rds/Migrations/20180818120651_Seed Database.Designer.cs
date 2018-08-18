﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Payoneer.DotnetCore.Rds;

namespace Payoneer.DotnetCore.Rds.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20180818120651_Seed Database")]
    partial class SeedDatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Payoneer.DotnetCore.Domain.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountHolderId");

                    b.Property<string>("AccountHolderName")
                        .HasMaxLength(100);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Currency")
                        .HasMaxLength(3);

                    b.Property<DateTime>("PaymentDate");

                    b.Property<string>("Reason")
                        .HasMaxLength(250);

                    b.Property<int>("Status");

                    b.Property<string>("StatusDescription")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Payments");

                    b.HasData(
                        new { Id = 832321, AccountHolderId = 15651, AccountHolderName = "Alex Dumsky", Amount = 445.12m, Currency = "EUR", PaymentDate = new DateTime(2015, 1, 23, 18, 25, 43, 511, DateTimeKind.Utc), Status = 0, StatusDescription = "Pending" },
                        new { Id = 806532, AccountHolderId = 46556, AccountHolderName = "Dudi Elias", Amount = 4511.12m, Currency = "EUR", PaymentDate = new DateTime(2015, 2, 10, 18, 25, 43, 511, DateTimeKind.Utc), Status = 0, StatusDescription = "Pending" },
                        new { Id = 7845431, AccountHolderId = 48481, AccountHolderName = "Niv Cohen", Amount = 10.99m, Currency = "USD", PaymentDate = new DateTime(2015, 4, 1, 18, 25, 43, 511, DateTimeKind.Utc), Reason = "Good Person", Status = 1, StatusDescription = "Approved" },
                        new { Id = 545341, AccountHolderId = 32131, AccountHolderName = "Alex Dumsky", Amount = 9952.48m, Currency = "EUR", PaymentDate = new DateTime(2016, 2, 21, 18, 25, 43, 511, DateTimeKind.Utc), Reason = "This is suspicious", Status = 99, StatusDescription = "Rejected" }
                    );
                });
#pragma warning restore 612, 618
        }
    }
}
