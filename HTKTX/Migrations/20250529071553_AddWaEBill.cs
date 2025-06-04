using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTKTX.Migrations
{
    /// <inheritdoc />
    public partial class AddWaEBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaterElectricBills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ElectricityStart = table.Column<int>(type: "int", nullable: false),
                    ElectricityEnd = table.Column<int>(type: "int", nullable: false),
                    WaterStart = table.Column<int>(type: "int", nullable: false),
                    WaterEnd = table.Column<int>(type: "int", nullable: false),
                    ElectricityUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WaterUnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoomNumber1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterElectricBills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterElectricBills_Rooms_RoomNumber1",
                        column: x => x.RoomNumber1,
                        principalTable: "Rooms",
                        principalColumn: "RoomNumber");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WaterElectricBills_RoomNumber1",
                table: "WaterElectricBills",
                column: "RoomNumber1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaterElectricBills");
        }
    }
}
