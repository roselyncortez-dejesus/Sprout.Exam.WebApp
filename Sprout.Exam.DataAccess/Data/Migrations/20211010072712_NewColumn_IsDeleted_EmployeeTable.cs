using Microsoft.EntityFrameworkCore.Migrations;

namespace Sprout.Exam.DataAccess.Data.Migrations
{
    public partial class NewColumn_IsDeleted_EmployeeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Employee");
        }
    }
}
