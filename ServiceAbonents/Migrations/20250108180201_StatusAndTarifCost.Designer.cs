﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ServiceAbonents.Data;

#nullable disable

namespace ServiceAbonents.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250108180201_StatusAndTarifCost")]
    partial class StatusAndTarifCost
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ServiceAbonents.Models.Abonent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Balance")
                        .HasColumnType("numeric");

                    b.Property<string>("DateForDeduct")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasportData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TarrifId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Abonents");
                });

            modelBuilder.Entity("ServiceAbonents.Models.Remain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ClientId")
                        .HasColumnType("integer");

                    b.Property<short>("ReaminGb")
                        .HasColumnType("smallint");

                    b.Property<short>("RemainMin")
                        .HasColumnType("smallint");

                    b.Property<short>("RemainSMS")
                        .HasColumnType("smallint");

                    b.HasKey("Id");

                    b.ToTable("Remains");
                });
#pragma warning restore 612, 618
        }
    }
}
