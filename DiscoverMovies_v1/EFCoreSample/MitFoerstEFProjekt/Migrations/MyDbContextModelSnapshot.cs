﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyFirstProject;

#nullable disable

namespace MitFoerstEFProjekt.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MitFoerstEFProjekt.Tables.Employment", b =>
                {
                    b.Property<int>("employmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("employmentId"), 1L, 1);

                    b.Property<string>("_character")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_job")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("movieId")
                        .HasColumnType("int");

                    b.Property<int>("personId")
                        .HasColumnType("int");

                    b.HasKey("employmentId");

                    b.HasIndex("movieId");

                    b.ToTable("Employment");
                });

            modelBuilder.Entity("MitFoerstEFProjekt.Tables.Genre", b =>
                {
                    b.Property<int>("genreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("genreId"), 1L, 1);

                    b.Property<int>("_Genrename")
                        .HasColumnType("int");

                    b.Property<int?>("movieId")
                        .HasColumnType("int");

                    b.HasKey("genreId");

                    b.HasIndex("movieId");

                    b.ToTable("Genre");
                });

            modelBuilder.Entity("MitFoerstEFProjekt.Tables.ProdCompany", b =>
                {
                    b.Property<int>("prodCompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("prodCompanyId"), 1L, 1);

                    b.Property<string>("_ProdCompanycountry")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_ProdCompanyname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("movieId")
                        .HasColumnType("int");

                    b.HasKey("prodCompanyId");

                    b.HasIndex("movieId");

                    b.ToTable("ProdCompany");
                });

            modelBuilder.Entity("Movie", b =>
                {
                    b.Property<int>("movieId")
                        .HasColumnType("int");

                    b.Property<int>("_budget")
                        .HasColumnType("int");

                    b.Property<double>("_popularity")
                        .HasColumnType("float");

                    b.Property<DateTime>("_releaseDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("_revenue")
                        .HasColumnType("int");

                    b.Property<int>("_runtime")
                        .HasColumnType("int");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("movieId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("MitFoerstEFProjekt.Tables.Employment", b =>
                {
                    b.HasOne("Movie", null)
                        .WithMany("_employmentList")
                        .HasForeignKey("movieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MitFoerstEFProjekt.Tables.Genre", b =>
                {
                    b.HasOne("Movie", null)
                        .WithMany("_genreList")
                        .HasForeignKey("movieId");
                });

            modelBuilder.Entity("MitFoerstEFProjekt.Tables.ProdCompany", b =>
                {
                    b.HasOne("Movie", null)
                        .WithMany("_prodCompanyList")
                        .HasForeignKey("movieId");
                });

            modelBuilder.Entity("Movie", b =>
                {
                    b.Navigation("_employmentList");

                    b.Navigation("_genreList");

                    b.Navigation("_prodCompanyList");
                });
#pragma warning restore 612, 618
        }
    }
}
