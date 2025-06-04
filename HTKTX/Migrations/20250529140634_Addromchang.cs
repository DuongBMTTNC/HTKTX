using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTKTX.Migrations
{
    /// <inheritdoc />
    public partial class Addromchang : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoomChangeRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentCCCD = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentRoomNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DesiredRoomNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsRejected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomChangeRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomChangeRequests_Rooms_CurrentRoomNumber",
                        column: x => x.CurrentRoomNumber,
                        principalTable: "Rooms",
                        principalColumn: "RoomNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoomChangeRequests_Rooms_DesiredRoomNumber",
                        column: x => x.DesiredRoomNumber,
                        principalTable: "Rooms",
                        principalColumn: "RoomNumber",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoomChangeRequests_StudentProfiles_StudentCCCD",
                        column: x => x.StudentCCCD,
                        principalTable: "StudentProfiles",
                        principalColumn: "CCCD",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomChangeRequests_CurrentRoomNumber",
                table: "RoomChangeRequests",
                column: "CurrentRoomNumber");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChangeRequests_DesiredRoomNumber",
                table: "RoomChangeRequests",
                column: "DesiredRoomNumber");

            migrationBuilder.CreateIndex(
                name: "IX_RoomChangeRequests_StudentCCCD",
                table: "RoomChangeRequests",
                column: "StudentCCCD");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomChangeRequests");
        }
    }
}
