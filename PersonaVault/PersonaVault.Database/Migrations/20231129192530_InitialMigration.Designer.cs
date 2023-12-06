﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonaVault.Database;

#nullable disable

namespace PersonaVault.Database.Migrations
{
    [DbContext(typeof(PersonaVaultContext))]
    [Migration("20231129192530_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PersonaVault.Database.Models.AddressDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("ApartamentNumber")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("City")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Country")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("HouseNumber")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Street")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.ToTable("AddressDetails");
                });

            modelBuilder.Entity("PersonaVault.Database.Models.PersonalDetails", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AddressDetailsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PersonalCode")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("Picture")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressDetailsId")
                        .IsUnique()
                        .HasFilter("[AddressDetailsId] IS NOT NULL");

                    b.ToTable("PersonalDetails");
                });

            modelBuilder.Entity("PersonaVault.Database.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid?>("PersonalDetailsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PersonalDetailsId")
                        .IsUnique()
                        .HasFilter("[PersonalDetailsId] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PersonaVault.Database.Models.PersonalDetails", b =>
                {
                    b.HasOne("PersonaVault.Database.Models.AddressDetails", "AddressDetails")
                        .WithOne()
                        .HasForeignKey("PersonaVault.Database.Models.PersonalDetails", "AddressDetailsId");

                    b.Navigation("AddressDetails");
                });

            modelBuilder.Entity("PersonaVault.Database.Models.User", b =>
                {
                    b.HasOne("PersonaVault.Database.Models.PersonalDetails", "PersonalDetails")
                        .WithOne()
                        .HasForeignKey("PersonaVault.Database.Models.User", "PersonalDetailsId");

                    b.Navigation("PersonalDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
