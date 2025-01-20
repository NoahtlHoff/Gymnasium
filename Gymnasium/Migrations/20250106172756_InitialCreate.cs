using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gymnasium.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Class",
                columns: table => new
                {
                    ClassID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Class__CB1927A0BF9205F6", x => x.ClassID);
                });

            migrationBuilder.CreateTable(
                name: "GradeScale",
                columns: table => new
                {
                    GradeValue = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: false),
                    GradeRank = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GradeSca__5E16159511AA8ED5", x => x.GradeValue);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    StaffID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Personal__2834371383E095A6", x => x.StaffID);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    ClassID = table.Column<int>(type: "int", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Students__32C52A79F7C8E7F9", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_Students_ClassID",
                        column: x => x.ClassID,
                        principalTable: "Class",
                        principalColumn: "ClassID");
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    SubjectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TeacherID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Subject__C92D7187D5FADC2F", x => x.SubjectID);
                    table.ForeignKey(
                        name: "FK__Subject__TeacherI__3B75D760",
                        column: x => x.TeacherID,
                        principalTable: "Staff",
                        principalColumn: "StaffID");
                });

            migrationBuilder.CreateTable(
                name: "Grade",
                columns: table => new
                {
                    GradeID = table.Column<int>(type: "int", nullable: false),
                    SubjectID = table.Column<int>(type: "int", nullable: true),
                    StudentID = table.Column<int>(type: "int", nullable: true),
                    GradeValue = table.Column<string>(type: "char(2)", unicode: false, fixedLength: true, maxLength: 2, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Grade__54F87A371D29C5B7", x => x.GradeID);
                    table.ForeignKey(
                        name: "FK__Grade__GradeValu__4316F928",
                        column: x => x.GradeValue,
                        principalTable: "GradeScale",
                        principalColumn: "GradeValue");
                    table.ForeignKey(
                        name: "FK__Grade__StudentID__412EB0B6",
                        column: x => x.StudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID");
                    table.ForeignKey(
                        name: "FK__Grade__SubjectID__403A8C7D",
                        column: x => x.SubjectID,
                        principalTable: "Subject",
                        principalColumn: "SubjectID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Grade_GradeValue",
                table: "Grade",
                column: "GradeValue");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_StudentID",
                table: "Grade",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_Grade_SubjectID",
                table: "Grade",
                column: "SubjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassID",
                table: "Students",
                column: "ClassID");

            migrationBuilder.CreateIndex(
                name: "IX_Subject_TeacherID",
                table: "Subject",
                column: "TeacherID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Grade");

            migrationBuilder.DropTable(
                name: "GradeScale");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Subject");

            migrationBuilder.DropTable(
                name: "Class");

            migrationBuilder.DropTable(
                name: "Staff");
        }
    }
}
