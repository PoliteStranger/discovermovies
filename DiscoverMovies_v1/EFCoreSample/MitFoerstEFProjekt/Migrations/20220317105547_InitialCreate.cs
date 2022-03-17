using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MitFoerstEFProjekt.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    movieId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _releaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    _budget = table.Column<int>(type: "int", nullable: false),
                    _revenue = table.Column<int>(type: "int", nullable: false),
                    _popularity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.movieId);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    personId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _Personname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _Personpopularity = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.personId);
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

            migrationBuilder.CreateTable(
                name: "Employment",
                columns: table => new
                {
                    employmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    movieId1 = table.Column<int>(type: "int", nullable: false),
                    personId1 = table.Column<int>(type: "int", nullable: false),
                    _job = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _character = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employment", x => x.employmentId);
                    table.ForeignKey(
                        name: "FK_Employment_Movies_movieId1",
                        column: x => x.movieId1,
                        principalTable: "Movies",
                        principalColumn: "movieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employment_Person_personId1",
                        column: x => x.personId1,
                        principalTable: "Person",
                        principalColumn: "personId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employment_movieId1",
                table: "Employment",
                column: "movieId1");

            migrationBuilder.CreateIndex(
                name: "IX_Employment_personId1",
                table: "Employment",
                column: "personId1");

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
                name: "Person");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
