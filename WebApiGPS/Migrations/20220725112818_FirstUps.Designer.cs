﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiGPS.Database;

#nullable disable

namespace WebApiGPS.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20220725112818_FirstUps")]
    partial class FirstUps
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebApiGPS.Models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Mark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RegNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VIN")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("WebApiGPS.Models.Charge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsCharging")
                        .HasColumnType("bit");

                    b.Property<double>("Power")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Charges");
                });

            modelBuilder.Entity("WebApiGPS.Models.Geoposition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<int>("TrackerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TrackerId");

                    b.ToTable("Geopositions");
                });

            modelBuilder.Entity("WebApiGPS.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsResponsible")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("WebApiGPS.Models.Tracker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("CarId")
                        .HasColumnType("int");

                    b.Property<int?>("ChargeId")
                        .HasColumnType("int");

                    b.Property<string>("IMEI")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ResponsibleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("ChargeId");

                    b.HasIndex("PersonId");

                    b.HasIndex("ResponsibleId");

                    b.ToTable("Trackers");
                });

            modelBuilder.Entity("WebApiGPS.Models.Geoposition", b =>
                {
                    b.HasOne("WebApiGPS.Models.Tracker", "Tracker")
                        .WithMany("Geopositions")
                        .HasForeignKey("TrackerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tracker");
                });

            modelBuilder.Entity("WebApiGPS.Models.Tracker", b =>
                {
                    b.HasOne("WebApiGPS.Models.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId");

                    b.HasOne("WebApiGPS.Models.Charge", "Charge")
                        .WithMany()
                        .HasForeignKey("ChargeId");

                    b.HasOne("WebApiGPS.Models.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId");

                    b.HasOne("WebApiGPS.Models.Person", "Responsible")
                        .WithMany()
                        .HasForeignKey("ResponsibleId");

                    b.Navigation("Car");

                    b.Navigation("Charge");

                    b.Navigation("Person");

                    b.Navigation("Responsible");
                });

            modelBuilder.Entity("WebApiGPS.Models.Tracker", b =>
                {
                    b.Navigation("Geopositions");
                });
#pragma warning restore 612, 618
        }
    }
}
