using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieReviewApp.Migrations
{
    /// <inheritdoc />
    public partial class addnewcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RazorpayOrderId",
                table: "bookingTickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RazorpayPaymentId",
                table: "bookingTickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RazorpayOrderId",
                table: "bookingTickets");

            migrationBuilder.DropColumn(
                name: "RazorpayPaymentId",
                table: "bookingTickets");
        }
    }
}
