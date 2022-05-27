using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcquireDB_EFcore.Migrations
{
    public partial class newContext_v5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AlterColumn<long>(
                name: "_revenue",
                table: "Movies",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<int>(
                name: "_revenue",
                table: "Movies",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
