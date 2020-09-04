﻿// <auto-generated />
using System;
using Husky.TwoFactor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Husky.TwoFactor.Data.Migrations
{
    [DbContext(typeof(TwoFactorDbContext))]
    [Migration("20190811112249_Init_TwoFactor")]
    partial class Init_TwoFactor
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Husky.TwoFactor.Data.TwoFactorCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code")
                        .HasColumnType("varchar(8)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("ErrorTimes")
                        .HasColumnType("int");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<string>("SentTo")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("UserIdString")
                        .HasColumnType("varchar(36)");

                    b.HasKey("Id");

                    b.ToTable("TwoFactorCodes");
                });
#pragma warning restore 612, 618
        }
    }
}
