﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ServiceAbonents.Data;

#nullable disable

namespace ServiceAbonents.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ServiceAbonents.Models.Abonent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<string>("DateForDeduct")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Pasport")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Status")
                        .HasColumnType("boolean");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("TariffCost")
                        .HasColumnType("numeric");

                    b.Property<string>("TarriffId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Abonents");
                });

            modelBuilder.Entity("ServiceAbonents.Models.Remain", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ClientId")
                        .HasColumnType("uuid");

                    b.Property<bool>("LongDistanceCall")
                        .HasColumnType("boolean");

                    b.Property<short>("RemainGb")
                        .HasColumnType("smallint");

                    b.Property<short>("RemainMin")
                        .HasColumnType("smallint");

                    b.Property<short>("RemainSMS")
                        .HasColumnType("smallint");

                    b.Property<bool>("UnlimMusic")
                        .HasColumnType("boolean");

                    b.Property<bool>("UnlimSocials")
                        .HasColumnType("boolean");

                    b.Property<bool>("UnlimVideo")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("Remains");
                });
#pragma warning restore 612, 618
        }
    }
}
