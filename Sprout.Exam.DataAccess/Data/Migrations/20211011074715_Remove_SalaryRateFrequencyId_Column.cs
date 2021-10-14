using Microsoft.EntityFrameworkCore.Migrations;

namespace Sprout.Exam.DataAccess.Data.Migrations
{
    public partial class Remove_SalaryRateFrequencyId_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalary_SalaryRateFrequency_SalaryRateFrequencyId",
                table: "EmployeeSalary");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalary_SalaryRateFrequencyId",
                table: "EmployeeSalary");

            migrationBuilder.DropColumn(
                name: "SalaryRateFrequencyId",
                table: "EmployeeSalary");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalary_FrequencyId",
                table: "EmployeeSalary",
                column: "FrequencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalary_SalaryRateFrequency_FrequencyId",
                table: "EmployeeSalary",
                column: "FrequencyId",
                principalTable: "SalaryRateFrequency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalary_SalaryRateFrequency_FrequencyId",
                table: "EmployeeSalary");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalary_FrequencyId",
                table: "EmployeeSalary");

            migrationBuilder.AddColumn<int>(
                name: "SalaryRateFrequencyId",
                table: "EmployeeSalary",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalary_SalaryRateFrequencyId",
                table: "EmployeeSalary",
                column: "SalaryRateFrequencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalary_SalaryRateFrequency_SalaryRateFrequencyId",
                table: "EmployeeSalary",
                column: "SalaryRateFrequencyId",
                principalTable: "SalaryRateFrequency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
