﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Taxes.Data.Ef;

namespace Taxes.Data.Migrations
{
    [DbContext(typeof(TaxesDbContext))]
    [Migration("20201220224537_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Taxes.Data.Entities.Municipality", b =>
                {
                    b.Property<int>("MunicipalityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MunicipalityId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Municipality");
                });

            modelBuilder.Entity("Taxes.Data.Entities.TaxScheduler", b =>
                {
                    b.Property<int>("TaxSchedulerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime?>("Date")
                        .HasColumnType("Datetime");

                    b.Property<int?>("Month")
                        .HasColumnType("int");

                    b.Property<int>("MunicipalityId")
                        .HasColumnType("int");

                    b.Property<int>("TaxTypeId")
                        .HasColumnType("int");

                    b.Property<decimal>("TaxValue")
                        .HasPrecision(2, 2)
                        .HasColumnType("decimal(2,2)");

                    b.Property<int?>("Week")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("TaxSchedulerId");

                    b.HasIndex("MunicipalityId");

                    b.HasIndex("TaxTypeId");

                    b.ToTable("TaxScheduler");
                });

            modelBuilder.Entity("Taxes.Data.Entities.TaxType", b =>
                {
                    b.Property<int>("TaxTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("TaxTypeId");

                    b.HasIndex("TypeName")
                        .IsUnique();

                    b.ToTable("TaxType");
                });

            modelBuilder.Entity("Taxes.Data.Entities.TaxScheduler", b =>
                {
                    b.HasOne("Taxes.Data.Entities.Municipality", "Municipality")
                        .WithMany("TaxesSchedulers")
                        .HasForeignKey("MunicipalityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Taxes.Data.Entities.TaxType", "TaxType")
                        .WithMany("TaxesSchedulers")
                        .HasForeignKey("TaxTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Municipality");

                    b.Navigation("TaxType");
                });

            modelBuilder.Entity("Taxes.Data.Entities.Municipality", b =>
                {
                    b.Navigation("TaxesSchedulers");
                });

            modelBuilder.Entity("Taxes.Data.Entities.TaxType", b =>
                {
                    b.Navigation("TaxesSchedulers");
                });
#pragma warning restore 612, 618
        }
    }
}