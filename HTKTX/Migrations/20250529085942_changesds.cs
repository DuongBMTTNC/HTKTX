using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTKTX.Migrations
{
    /// <inheritdoc />
    public partial class changesds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalBills_RoomTypes_RoomTypeId1",
                table: "RentalBills");

            migrationBuilder.DropIndex(
                name: "IX_RentalBills_RoomTypeId1",
                table: "RentalBills");

            migrationBuilder.DropColumn(
                name: "RoomTypeId1",
                table: "RentalBills");

            migrationBuilder.AlterColumn<string>(
                name: "RoomTypeId",
                table: "RentalBills",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_RentalBills_RoomTypeId",
                table: "RentalBills",
                column: "RoomTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalBills_RoomTypes_RoomTypeId",
                table: "RentalBills",
                column: "RoomTypeId",
                principalTable: "RoomTypes",
                principalColumn: "RoomTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalBills_RoomTypes_RoomTypeId",
                table: "RentalBills");

            migrationBuilder.DropIndex(
                name: "IX_RentalBills_RoomTypeId",
                table: "RentalBills");

            migrationBuilder.AlterColumn<int>(
                name: "RoomTypeId",
                table: "RentalBills",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "RoomTypeId1",
                table: "RentalBills",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalBills_RoomTypeId1",
                table: "RentalBills",
                column: "RoomTypeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalBills_RoomTypes_RoomTypeId1",
                table: "RentalBills",
                column: "RoomTypeId1",
                principalTable: "RoomTypes",
                principalColumn: "RoomTypeId");
        }
    }
}
