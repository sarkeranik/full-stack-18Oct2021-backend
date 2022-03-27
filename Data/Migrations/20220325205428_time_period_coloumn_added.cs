using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class time_period_coloumn_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TimePeriod",
                table: "RestaurantDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimePeriod",
                table: "RestaurantDetails");
        }
    }
}
