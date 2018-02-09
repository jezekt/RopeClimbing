﻿// <auto-generated />
using JezekT.RopeClimbing.Server.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace JezekT.RopeClimbing.Server.Migrations.ConsoleApp.Migrations
{
    [DbContext(typeof(RopeClimbingDbContext))]
    partial class RopeClimbingDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("JezekT.RopeClimbing.Domain.Entities.TestAttempt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AttemptEndTime");

                    b.Property<string>("RacerName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("TimeInMiliseconds");

                    b.HasKey("Id");

                    b.ToTable("TestAttemptSet");
                });
#pragma warning restore 612, 618
        }
    }
}