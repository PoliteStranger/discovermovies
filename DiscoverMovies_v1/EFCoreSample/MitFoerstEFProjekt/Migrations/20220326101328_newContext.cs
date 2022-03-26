using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MitFoerstEFProjekt.Migrations
{
    public partial class newContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    movieId = table.Column<int>(type: "int", nullable: false),
                    _title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _releaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _budget = table.Column<int>(type: "int", nullable: false),
                    _revenue = table.Column<int>(type: "int", nullable: false),
                    _popularity = table.Column<double>(type: "float", nullable: false),
                    _runtime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.movieId);
                });

            migrationBuilder.CreateTable(
                name: "Employment",
                columns: table => new
                {
                    employmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    movieId = table.Column<int>(type: "int", nullable: false),
                    personId = table.Column<int>(type: "int", nullable: false),
                    _job = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _character = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employment", x => x.employmentId);
                    table.ForeignKey(
                        name: "FK_Employment_Movies_movieId",
                        column: x => x.movieId,
                        principalTable: "Movies",
                        principalColumn: "movieId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    genreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _Genrename = table.Column<int>(type: "int", nullable: false),
                    movieId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.genreId);
                    table.ForeignKey(
                        name: "FK_Genre_Movies_movieId",
                        column: x => x.movieId,
                        principalTable: "Movies",
                        principalColumn: "movieId");
                });

            migrationBuilder.CreateTable(
                name: "ProdCompany",
                columns: table => new
                {
                    prodCompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _ProdCompanyname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _ProdCompanycountry = table.Column<string>(type: "nvarchar(max)", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Employment_movieId",
                table: "Employment",
                column: "movieId");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_movieId",
                table: "Genre",
                column: "movieId");

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
                name: "Movies");
        }
    }
}
