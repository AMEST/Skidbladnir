﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Skidbkadnir.Repository.EntityFrameworkCore.Sample;

namespace Skidbkadnir.Repository.EntityFrameworkCore.Sample.Migrations
{
    [DbContext(typeof(SampleDbContext))]
    [Migration("20210809175700_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.17");

            modelBuilder.Entity("Skidbkadnir.Repository.EntityFrameworkCore.Sample.entities.SimpleGuid", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Guid")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("GuidStorage");
                });

            modelBuilder.Entity("Skidbkadnir.Repository.EntityFrameworkCore.Sample.entities.SimpleMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Timestamp")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
