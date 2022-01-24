using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeMeetingOrganizer.DataAccess.Migrations
{
    public partial class Organizer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeMeeting",
                columns: table => new
                {
                    EmployeesId = table.Column<int>(type: "int", nullable: false),
                    MeetingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeMeeting", x => new { x.EmployeesId, x.MeetingsId });
                    table.ForeignKey(
                        name: "FK_EmployeeMeeting_Employees_EmployeesId",
                        column: x => x.EmployeesId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeMeeting_Meetings_MeetingsId",
                        column: x => x.MeetingsId,
                        principalTable: "Meetings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhoneNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhoneNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhoneNumbers_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "HR" },
                    { 2, "Finance" },
                    { 3, "IT" }
                });

            migrationBuilder.InsertData(
                table: "Meetings",
                columns: new[] { "Id", "DateFrom", "DateTo", "Title" },
                values: new object[] { 1, new DateTime(2022, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2022, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sprint Planning" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DepartmentId", "Email", "FirstName", "LastName" },
                values: new object[] { 1, 3, "ozankomurcu@gmail.com", "Ozan", "Komurcu" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DepartmentId", "Email", "FirstName", "LastName" },
                values: new object[] { 2, 1, "john@doe.com", "John", "Doe" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "DepartmentId", "Email", "FirstName", "LastName" },
                values: new object[] { 3, 2, "jolene@doe.com", "Jolene", "Doe" });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeMeeting_MeetingsId",
                table: "EmployeeMeeting",
                column: "MeetingsId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PhoneNumbers_EmployeeId",
                table: "PhoneNumbers",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeMeeting");

            migrationBuilder.DropTable(
                name: "PhoneNumbers");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
