using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTKTX.Migrations
{
    /// <inheritdoc />
    public partial class changeRB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalBills_AspNetUsers_UserId",
                table: "RentalBills");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalBills_Rooms_RoomNumber1",
                table: "RentalBills");

            migrationBuilder.DropIndex(
                name: "IX_RentalBills_RoomNumber1",
                table: "RentalBills");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RentalBills");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "RentalBills");

            migrationBuilder.DropColumn(
                name: "RoomNumber1",
                table: "RentalBills");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RentalBills",
                newName: "StudentCCCD");

            migrationBuilder.RenameColumn(
                name: "NumberOfMonths",
                table: "RentalBills",
                newName: "Year");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "RentalBills",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_RentalBills_UserId",
                table: "RentalBills",
                newName: "IX_RentalBills_StudentCCCD");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "RentalBills",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomTypeId",
                table: "RentalBills",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_RentalBills_StudentProfiles_StudentCCCD",
                table: "RentalBills",
                column: "StudentCCCD",
                principalTable: "StudentProfiles",
                principalColumn: "CCCD",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalBills_RoomTypes_RoomTypeId1",
                table: "RentalBills");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalBills_StudentProfiles_StudentCCCD",
                table: "RentalBills");

            migrationBuilder.DropIndex(
                name: "IX_RentalBills_RoomTypeId1",
                table: "RentalBills");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "RentalBills");

            migrationBuilder.DropColumn(
                name: "RoomTypeId",
                table: "RentalBills");

            migrationBuilder.DropColumn(
                name: "RoomTypeId1",
                table: "RentalBills");

            migrationBuilder.RenameColumn(
                name: "Year",
                table: "RentalBills",
                newName: "NumberOfMonths");

            migrationBuilder.RenameColumn(
                name: "StudentCCCD",
                table: "RentalBills",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "RentalBills",
                newName: "CreatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_RentalBills_StudentCCCD",
                table: "RentalBills",
                newName: "IX_RentalBills_UserId");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RentalBills",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "RentalBills",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                name: "FK_RentalBills_AspNetUsers_UserId",
                table: "RentalBills",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RentalBills_Rooms_RoomNumber1",
                table: "RentalBills",
                column: "RoomNumber1",
                principalTable: "Rooms",
                principalColumn: "RoomNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
