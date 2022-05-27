using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcquireDB_EFcore.Migrations
{
    public partial class newContext_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_ProdCompaniesname",
                table: "ProdCompanies",
                newName: "_ProdCompanyname");

            migrationBuilder.RenameColumn(
                name: "_ProdCompaniescountry",
                table: "ProdCompanies",
                newName: "_ProdCompanycountry");

            migrationBuilder.RenameColumn(
                name: "ProdCompaniesId",
                table: "ProdCompanies",
                newName: "ProdCompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "_ProdCompanyname",
                table: "ProdCompanies",
                newName: "_ProdCompaniesname");

            migrationBuilder.RenameColumn(
                name: "_ProdCompanycountry",
                table: "ProdCompanies",
                newName: "_ProdCompaniescountry");

            migrationBuilder.RenameColumn(
                name: "ProdCompanyId",
                table: "ProdCompanies",
                newName: "ProdCompaniesId");
        }
    }
}
