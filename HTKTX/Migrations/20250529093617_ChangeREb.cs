using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HTKTX.Migrations
{
    /// <inheritdoc />
    public partial class ChangeREb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "RentalBills",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "RentalBills");
        }
    }
}
