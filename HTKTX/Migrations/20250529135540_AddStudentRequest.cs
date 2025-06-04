using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTKTX.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrentRoomNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DesiredRoomId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsRejected = table.Column<bool>(type: "bit", nullable: false),
                    CurrentRoomRoomNumber = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentRequests_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentRequests_Rooms_CurrentRoomRoomNumber",
                        column: x => x.CurrentRoomRoomNumber,
                        principalTable: "Rooms",
                        principalColumn: "RoomNumber");
                    table.ForeignKey(
                        name: "FK_StudentRequests_Rooms_DesiredRoomId",
                        column: x => x.DesiredRoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentRequests_CurrentRoomRoomNumber",
                table: "StudentRequests",
                column: "CurrentRoomRoomNumber");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRequests_DesiredRoomId",
                table: "StudentRequests",
                column: "DesiredRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRequests_UserId",
                table: "StudentRequests",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentRequests");
        }
    }
}
