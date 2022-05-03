using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcquireDB_EFcore.Migrations
{
    public partial class newContext_v4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProducedBy",
                columns: table => new
                {
                    producedById = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    _movieId = table.Column<int>(type: "int", nullable: false),
                    prodCompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProducedBy", x => x.producedById);
                    table.ForeignKey(
                        name: "FK_ProducedBy_Movies__movieId",
                        column: x => x._movieId,
                        principalTable: "Movies",
                        principalColumn: "movieId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProducedBy_ProdCompanies_prodCompanyId",
                        column: x => x.prodCompanyId,
                        principalTable: "ProdCompanies",
                        principalColumn: "ProdCompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProducedBy__movieId",
                table: "ProducedBy",
                column: "_movieId");

            migrationBuilder.CreateIndex(
                name: "IX_ProducedBy_prodCompanyId",
                table: "ProducedBy",
                column: "prodCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProducedBy");
        }
    }
}
