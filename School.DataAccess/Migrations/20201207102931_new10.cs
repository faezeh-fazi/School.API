using Microsoft.EntityFrameworkCore.Migrations;

namespace School.DataAccess.Migrations
{
    public partial class new10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GPA",
                table: "StudentCourse");

            migrationBuilder.AddColumn<double>(
                name: "GPA",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GPA",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<double>(
                name: "GPA",
                table: "StudentCourse",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
