using Microsoft.EntityFrameworkCore.Migrations;

namespace Sprout.Exam.DataAccess.Data.Migrations
{
    public partial class TableRenaming : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeTypes_TypeId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaries_Employees_EmployeeId",
                table: "EmployeeSalaries");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalaries_SalaryRateFrequencies_SalaryRateFrequencyId",
                table: "EmployeeSalaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalaryRateFrequencies",
                table: "SalaryRateFrequencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeTypes",
                table: "EmployeeTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeSalaries",
                table: "EmployeeSalaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employees",
                table: "Employees");

            migrationBuilder.RenameTable(
                name: "SalaryRateFrequencies",
                newName: "SalaryRateFrequency");

            migrationBuilder.RenameTable(
                name: "EmployeeTypes",
                newName: "EmployeeType");

            migrationBuilder.RenameTable(
                name: "EmployeeSalaries",
                newName: "EmployeeSalary");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employee");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSalaries_SalaryRateFrequencyId",
                table: "EmployeeSalary",
                newName: "IX_EmployeeSalary_SalaryRateFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSalaries_EmployeeId",
                table: "EmployeeSalary",
                newName: "IX_EmployeeSalary_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_TypeId",
                table: "Employee",
                newName: "IX_Employee_TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalaryRateFrequency",
                table: "SalaryRateFrequency",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeType",
                table: "EmployeeType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeSalary",
                table: "EmployeeSalary",
                column: "EmployeeSalaryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employee",
                table: "Employee",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_EmployeeType_TypeId",
                table: "Employee",
                column: "TypeId",
                principalTable: "EmployeeType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalary_Employee_EmployeeId",
                table: "EmployeeSalary",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalary_SalaryRateFrequency_SalaryRateFrequencyId",
                table: "EmployeeSalary",
                column: "SalaryRateFrequencyId",
                principalTable: "SalaryRateFrequency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_EmployeeType_TypeId",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalary_Employee_EmployeeId",
                table: "EmployeeSalary");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalary_SalaryRateFrequency_SalaryRateFrequencyId",
                table: "EmployeeSalary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalaryRateFrequency",
                table: "SalaryRateFrequency");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeType",
                table: "EmployeeType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmployeeSalary",
                table: "EmployeeSalary");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Employee",
                table: "Employee");

            migrationBuilder.RenameTable(
                name: "SalaryRateFrequency",
                newName: "SalaryRateFrequencies");

            migrationBuilder.RenameTable(
                name: "EmployeeType",
                newName: "EmployeeTypes");

            migrationBuilder.RenameTable(
                name: "EmployeeSalary",
                newName: "EmployeeSalaries");

            migrationBuilder.RenameTable(
                name: "Employee",
                newName: "Employees");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSalary_SalaryRateFrequencyId",
                table: "EmployeeSalaries",
                newName: "IX_EmployeeSalaries_SalaryRateFrequencyId");

            migrationBuilder.RenameIndex(
                name: "IX_EmployeeSalary_EmployeeId",
                table: "EmployeeSalaries",
                newName: "IX_EmployeeSalaries_EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_Employee_TypeId",
                table: "Employees",
                newName: "IX_Employees_TypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalaryRateFrequencies",
                table: "SalaryRateFrequencies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeTypes",
                table: "EmployeeTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmployeeSalaries",
                table: "EmployeeSalaries",
                column: "EmployeeSalaryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Employees",
                table: "Employees",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeTypes_TypeId",
                table: "Employees",
                column: "TypeId",
                principalTable: "EmployeeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaries_Employees_EmployeeId",
                table: "EmployeeSalaries",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalaries_SalaryRateFrequencies_SalaryRateFrequencyId",
                table: "EmployeeSalaries",
                column: "SalaryRateFrequencyId",
                principalTable: "SalaryRateFrequencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
