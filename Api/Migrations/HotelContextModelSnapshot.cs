﻿// <auto-generated />
using System;
using DDPS.Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DDPS.Api.Migrations
{
    [DbContext(typeof(HotelContext))]
    partial class HotelContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true);

            modelBuilder.Entity("ApartamentsFacilities", b =>
                {
                    b.Property<int>("ApartamentsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FacilitiesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ApartamentsId", "FacilitiesId");

                    b.HasIndex("FacilitiesId");

                    b.ToTable("ApartamentsFacilities");
                });

            modelBuilder.Entity("ApartamentsServices", b =>
                {
                    b.Property<int>("ApartamentsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ServicesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ApartamentsId", "ServicesId");

                    b.HasIndex("ServicesId");

                    b.ToTable("ApartamentsServices");
                });

            modelBuilder.Entity("BookingsServices", b =>
                {
                    b.Property<int>("BookingsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ServicesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("BookingsId", "ServicesId");

                    b.HasIndex("ServicesId");

                    b.ToTable("BookingsServices");
                });

            modelBuilder.Entity("DDPS.Api.Entities.Apartaments", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Area")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Photo")
                        .HasColumnType("TEXT");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Reservation")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TariffId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TariffId");

                    b.ToTable("Apartaments");
                });

            modelBuilder.Entity("DDPS.Api.Entities.Bookings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ApartamentId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<int?>("TotalPrice")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ApartamentId");

                    b.HasIndex("ClientId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("DDPS.Api.Entities.Clients", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("DDPS.Api.Entities.Facilities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Facilities");
                });

            modelBuilder.Entity("DDPS.Api.Entities.Services", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("DDPS.Api.Entities.Tariffs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Tariffs");
                });

            modelBuilder.Entity("ApartamentsFacilities", b =>
                {
                    b.HasOne("DDPS.Api.Entities.Apartaments", null)
                        .WithMany()
                        .HasForeignKey("ApartamentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DDPS.Api.Entities.Facilities", null)
                        .WithMany()
                        .HasForeignKey("FacilitiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApartamentsServices", b =>
                {
                    b.HasOne("DDPS.Api.Entities.Apartaments", null)
                        .WithMany()
                        .HasForeignKey("ApartamentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DDPS.Api.Entities.Services", null)
                        .WithMany()
                        .HasForeignKey("ServicesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookingsServices", b =>
                {
                    b.HasOne("DDPS.Api.Entities.Bookings", null)
                        .WithMany()
                        .HasForeignKey("BookingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DDPS.Api.Entities.Services", null)
                        .WithMany()
                        .HasForeignKey("ServicesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DDPS.Api.Entities.Apartaments", b =>
                {
                    b.HasOne("DDPS.Api.Entities.Tariffs", "Tariff")
                        .WithMany()
                        .HasForeignKey("TariffId");

                    b.Navigation("Tariff");
                });

            modelBuilder.Entity("DDPS.Api.Entities.Bookings", b =>
                {
                    b.HasOne("DDPS.Api.Entities.Apartaments", "Apartament")
                        .WithMany()
                        .HasForeignKey("ApartamentId");

                    b.HasOne("DDPS.Api.Entities.Clients", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.Navigation("Apartament");

                    b.Navigation("Client");
                });
#pragma warning restore 612, 618
        }
    }
}
