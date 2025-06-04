using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTKTX.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalBills_Rooms_RoomNumber",
                table: "RentalBills");

            migrationBuilder.DropIndex(
                name: "IX_RentalBills_RoomNumber",
                table: "RentalBills");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "RentalBills");

            migrationBuilder.AlterColumn<string>(
                name: "RoomNumber",
                table: "RentalBills",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "RoomNumber1",
                table: "RentalBills",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_RentalBills_RoomNumber1",
                table: "RentalBills",
                column: "RoomNumber1");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalBills_Rooms_RoomNumber1",
                table: "RentalBills",
                column: "RoomNumber1",
                principalTable: "Rooms",
                principalColumn: "RoomNumber",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalBills_Rooms_RoomNumber1",
                table: "RentalBills");

            migrationBuilder.DropIndex(
                name: "IX_RentalBills_RoomNumber1",
                table: "RentalBills");

            migrationBuilder.DropColumn(
                name: "RoomNumber1",
                table: "RentalBills");

            migrationBuilder.AlterColumn<string>(
                name: "RoomNumber",
                table: "RentalBills",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "RentalBills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RentalBills_RoomNumber",
                table: "RentalBills",
                column: "RoomNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalBills_Rooms_RoomNumber",
                table: "RentalBills",
                column: "RoomNumber",
                principalTable: "Rooms",
                principalColumn: "RoomNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
