using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcquireDB_EFcore.Migrations
{
    public partial class newContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    _genreId = table.Column<int>(type: "int", nullable: false),
                    _Genrename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x._genreId);
                });

            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    movieId = table.Column<int>(type: "int", nullable: false),
                    _title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _posterUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _releaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    _budget = table.Column<int>(type: "int", nullable: true),
                    _revenue = table.Column<int>(type: "int", nullable: true),
                    _popularity = table.Column<double>(type: "float", nullable: true),
                    _runtime = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.movieId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    _personId = table.Column<int>(type: "int", nullable: false),
                    _Personname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _Personpopularity = table.Column<double>(type: "float", nullable: true),
                    _biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _gender = table.Column<int>(type: "int", nullable: true),
                    _imageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _dob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    _dod = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x._personId);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    genreKey = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _genreId = table.Column<int>(type: "int", nullable: false),
                    _movieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.genreKey);
                    table.ForeignKey(
                        name: "FK_Genre_Genres__genreId",
                        column: x => x._genreId,
                        principalTable: "Genres",
                        principalColumn: "_genreId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Genre_Movies__movieId",
                        column: x => x._movieId,
                        principalTable: "Movies",
                        principalColumn: "movieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProdCompany",
                columns: table => new
                {
                    prodCompanyId = table.Column<int>(type: "int", nullable: false),
                    _ProdCompanyname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _ProdCompanycountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    movieId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdCompany", x => x.prodCompanyId);
                    table.ForeignKey(
                        name: "FK_ProdCompany_Movies_movieId",
                        column: x => x.movieId,
                        principalTable: "Movies",
                        principalColumn: "movieId");
                });

            migrationBuilder.CreateTable(
                name: "Employment",
                columns: table => new
                {
                    employmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _movieId = table.Column<int>(type: "int", nullable: false),
                    _personId = table.Column<int>(type: "int", nullable: false),
                    _job = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _character = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employment", x => x.employmentId);
                    table.ForeignKey(
                        name: "FK_Employment_Movies__movieId",
                        column: x => x._movieId,
                        principalTable: "Movies",
                        principalColumn: "movieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employment_Persons__personId",
                        column: x => x._personId,
                        principalTable: "Persons",
                        principalColumn: "_personId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employment__movieId",
                table: "Employment",
                column: "_movieId");

            migrationBuilder.CreateIndex(
                name: "IX_Employment__personId",
                table: "Employment",
                column: "_personId");

            migrationBuilder.CreateIndex(
                name: "IX_Genre__genreId",
                table: "Genre",
                column: "_genreId");

            migrationBuilder.CreateIndex(
                name: "IX_Genre__movieId",
                table: "Genre",
                column: "_movieId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdCompany_movieId",
                table: "ProdCompany",
                column: "movieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employment");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "ProdCompany");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
