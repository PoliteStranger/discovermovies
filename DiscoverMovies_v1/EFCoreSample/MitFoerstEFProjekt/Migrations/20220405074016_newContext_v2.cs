using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcquireDB_EFcore.Migrations
{
    public partial class newContext_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employment_Movies__movieId",
                table: "Employment");

            migrationBuilder.DropForeignKey(
                name: "FK_Employment_Persons__personId",
                table: "Employment");

            migrationBuilder.DropForeignKey(
                name: "FK_Genre_Genres__genreId",
                table: "Genre");

            migrationBuilder.DropForeignKey(
                name: "FK_Genre_Movies__movieId",
                table: "Genre");

            migrationBuilder.DropTable(
                name: "ProdCompany");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genre",
                table: "Genre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employment",
                table: "Employment");

            migrationBuilder.RenameTable(
                name: "Genre",
                newName: "GenresAndMovies");

            migrationBuilder.RenameTable(
                name: "Employment",
                newName: "Employments");

            migrationBuilder.RenameIndex(
                name: "IX_Genre__movieId",
                table: "GenresAndMovies",
                newName: "IX_GenresAndMovies__movieId");

            migrationBuilder.RenameIndex(
                name: "IX_Genre__genreId",
                table: "GenresAndMovies",
                newName: "IX_GenresAndMovies__genreId");

            migrationBuilder.RenameIndex(
                name: "IX_Employment__personId",
                table: "Employments",
                newName: "IX_Employments__personId");

            migrationBuilder.RenameIndex(
                name: "IX_Employment__movieId",
                table: "Employments",
                newName: "IX_Employments__movieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenresAndMovies",
                table: "GenresAndMovies",
                column: "genreKey");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employments",
                table: "Employments",
                column: "employmentId");

            migrationBuilder.CreateTable(
                name: "ProdCompanies",
                columns: table => new
                {
                    ProdCompaniesId = table.Column<int>(type: "int", nullable: false),
                    _ProdCompaniesname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    _ProdCompaniescountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    movieId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdCompanies", x => x.ProdCompaniesId);
                    table.ForeignKey(
                        name: "FK_ProdCompanies_Movies_movieId",
                        column: x => x.movieId,
                        principalTable: "Movies",
                        principalColumn: "movieId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdCompanies_movieId",
                table: "ProdCompanies",
                column: "movieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employments_Movies__movieId",
                table: "Employments",
                column: "_movieId",
                principalTable: "Movies",
                principalColumn: "movieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employments_Persons__personId",
                table: "Employments",
                column: "_personId",
                principalTable: "Persons",
                principalColumn: "_personId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenresAndMovies_Genres__genreId",
                table: "GenresAndMovies",
                column: "_genreId",
                principalTable: "Genres",
                principalColumn: "_genreId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenresAndMovies_Movies__movieId",
                table: "GenresAndMovies",
                column: "_movieId",
                principalTable: "Movies",
                principalColumn: "movieId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employments_Movies__movieId",
                table: "Employments");

            migrationBuilder.DropForeignKey(
                name: "FK_Employments_Persons__personId",
                table: "Employments");

            migrationBuilder.DropForeignKey(
                name: "FK_GenresAndMovies_Genres__genreId",
                table: "GenresAndMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_GenresAndMovies_Movies__movieId",
                table: "GenresAndMovies");

            migrationBuilder.DropTable(
                name: "ProdCompanies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenresAndMovies",
                table: "GenresAndMovies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employments",
                table: "Employments");

            migrationBuilder.RenameTable(
                name: "GenresAndMovies",
                newName: "Genre");

            migrationBuilder.RenameTable(
                name: "Employments",
                newName: "Employment");

            migrationBuilder.RenameIndex(
                name: "IX_GenresAndMovies__movieId",
                table: "Genre",
                newName: "IX_Genre__movieId");

            migrationBuilder.RenameIndex(
                name: "IX_GenresAndMovies__genreId",
                table: "Genre",
                newName: "IX_Genre__genreId");

            migrationBuilder.RenameIndex(
                name: "IX_Employments__personId",
                table: "Employment",
                newName: "IX_Employment__personId");

            migrationBuilder.RenameIndex(
                name: "IX_Employments__movieId",
                table: "Employment",
                newName: "IX_Employment__movieId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genre",
                table: "Genre",
                column: "genreKey");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employment",
                table: "Employment",
                column: "employmentId");

            migrationBuilder.CreateTable(
                name: "ProdCompany",
                columns: table => new
                {
                    prodCompanyId = table.Column<int>(type: "int", nullable: false),
                    _ProdCompanycountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    _ProdCompanyname = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "IX_ProdCompany_movieId",
                table: "ProdCompany",
                column: "movieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employment_Movies__movieId",
                table: "Employment",
                column: "_movieId",
                principalTable: "Movies",
                principalColumn: "movieId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employment_Persons__personId",
                table: "Employment",
                column: "_personId",
                principalTable: "Persons",
                principalColumn: "_personId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Genre_Genres__genreId",
                table: "Genre",
                column: "_genreId",
                principalTable: "Genres",
                principalColumn: "_genreId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Genre_Movies__movieId",
                table: "Genre",
                column: "_movieId",
                principalTable: "Movies",
                principalColumn: "movieId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
