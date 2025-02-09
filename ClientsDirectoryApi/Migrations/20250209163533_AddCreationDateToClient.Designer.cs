﻿// <auto-generated />
using System;
using ClientsDirectoryApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClientsDirectoryApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250209163533_AddCreationDateToClient")]
    partial class AddCreationDateToClient
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("ClientsDirectoryApi.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ActualAddressCity")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ActualAddressCountry")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ActualAddressLine")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("DocumentId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Gender")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LegalAddressCity")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LegalAddressCountry")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LegalAddressLine")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProfileImageUrl")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });
#pragma warning restore 612, 618
        }
    }
}
