using Microsoft.EntityFrameworkCore.Migrations;

namespace ExampleConsole.EFCore.Migrations
{
    public partial class StationsRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK__stations",
                table: "_stations");

            migrationBuilder.RenameTable(
                name: "_stations",
                newName: "Stations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stations",
                table: "Stations",
                column: "StationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Stations",
                table: "Stations");

            migrationBuilder.RenameTable(
                name: "Stations",
                newName: "_stations");

            migrationBuilder.AddPrimaryKey(
                name: "PK__stations",
                table: "_stations",
                column: "StationId");
        }
    }
}
