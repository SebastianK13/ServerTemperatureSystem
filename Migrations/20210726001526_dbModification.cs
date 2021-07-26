using Microsoft.EntityFrameworkCore.Migrations;

namespace ServerTemperatureSystem.Migrations
{
    public partial class dbModification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemperatureDetails_Mobo_MotherboardId",
                table: "TemperatureDetails");

            migrationBuilder.RenameColumn(
                name: "MotherboardId",
                table: "TemperatureDetails",
                newName: "MoboId");

            migrationBuilder.RenameIndex(
                name: "IX_TemperatureDetails_MotherboardId",
                table: "TemperatureDetails",
                newName: "IX_TemperatureDetails_MoboId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemperatureDetails_Mobo_MoboId",
                table: "TemperatureDetails",
                column: "MoboId",
                principalTable: "Mobo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemperatureDetails_Mobo_MoboId",
                table: "TemperatureDetails");

            migrationBuilder.RenameColumn(
                name: "MoboId",
                table: "TemperatureDetails",
                newName: "MotherboardId");

            migrationBuilder.RenameIndex(
                name: "IX_TemperatureDetails_MoboId",
                table: "TemperatureDetails",
                newName: "IX_TemperatureDetails_MotherboardId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemperatureDetails_Mobo_MotherboardId",
                table: "TemperatureDetails",
                column: "MotherboardId",
                principalTable: "Mobo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
