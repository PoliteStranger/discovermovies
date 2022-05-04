﻿// <auto-generated />
using System;
using AcquireDB_EFcore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AcquireDB_EFcore.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AcquireDB_EFcore.Tables.Employment", b =>
                {
                    b.Property<int>("employmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("employmentId"), 1L, 1);

                    b.Property<string>("_character")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_job")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("_movieId")
                        .HasColumnType("int");

                    b.Property<int>("_personId")
                        .HasColumnType("int");

                    b.HasKey("employmentId");

                    b.HasIndex("_movieId");

                    b.HasIndex("_personId");

                    b.ToTable("Employments");
                });

            modelBuilder.Entity("AcquireDB_EFcore.Tables.Genre", b =>
                {
                    b.Property<int>("genreKey")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("genreKey"), 1L, 1);

                    b.Property<int>("_genreId")
                        .HasColumnType("int");

                    b.Property<int>("_movieId")
                        .HasColumnType("int");

                    b.HasKey("genreKey");

                    b.HasIndex("_genreId");

                    b.HasIndex("_movieId");

                    b.ToTable("GenresAndMovies");
                });

            modelBuilder.Entity("AcquireDB_EFcore.Tables.ProdCompany", b =>
                {
                    b.Property<int>("ProdCompanyId")
                        .HasColumnType("int");

                    b.Property<string>("_ProdCompanycountry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_ProdCompanyname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("movieId")
                        .HasColumnType("int");

                    b.HasKey("ProdCompanyId");

                    b.HasIndex("movieId");

                    b.ToTable("ProdCompanies");
                });

            modelBuilder.Entity("Genres", b =>
                {
                    b.Property<int>("_genreId")
                        .HasColumnType("int");

                    b.Property<string>("_Genrename")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("_genreId");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Movie", b =>
                {
                    b.Property<int>("movieId")
                        .HasColumnType("int");

                    b.Property<int?>("_budget")
                        .HasColumnType("int");

                    b.Property<string>("_description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("_popularity")
                        .HasColumnType("float");

                    b.Property<string>("_posterUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("_releaseDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("_revenue")
                        .HasColumnType("int");

                    b.Property<int?>("_runtime")
                        .HasColumnType("int");

                    b.Property<string>("_title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("movieId");

                    b.ToTable("Movies");
                });

            modelBuilder.Entity("Person", b =>
                {
                    b.Property<int>("_personId")
                        .HasColumnType("int");

                    b.Property<string>("_Personname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("_Personpopularity")
                        .HasColumnType("float");

                    b.Property<string>("_biography")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("_dob")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("_dod")
                        .HasColumnType("datetime2");

                    b.Property<int?>("_gender")
                        .HasColumnType("int");

                    b.Property<string>("_imageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("_personId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("AcquireDB_EFcore.Tables.Employment", b =>
                {
                    b.HasOne("Movie", "Movies")
                        .WithMany("_employmentList")
                        .HasForeignKey("_movieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Person", "Person")
                        .WithMany()
                        .HasForeignKey("_personId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Movies");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("AcquireDB_EFcore.Tables.Genre", b =>
                {
                    b.HasOne("Genres", "Genres")
                        .WithMany()
                        .HasForeignKey("_genreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Movie", "Movies")
                        .WithMany("_genreList")
                        .HasForeignKey("_movieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genres");

                    b.Navigation("Movies");
                });

            modelBuilder.Entity("AcquireDB_EFcore.Tables.ProdCompany", b =>
                {
                    b.HasOne("Movie", null)
                        .WithMany("_ProdCompaniesList")
                        .HasForeignKey("movieId");
                });

            modelBuilder.Entity("Movie", b =>
                {
                    b.Navigation("_ProdCompaniesList");

                    b.Navigation("_employmentList");

                    b.Navigation("_genreList");
                });
#pragma warning restore 612, 618
        }
    }
}
