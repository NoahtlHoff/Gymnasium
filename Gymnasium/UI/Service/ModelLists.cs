using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gymnasium.Data;
using Gymnasium.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Gymnasium.UI.Service
{
    internal class ModelLists
    {
        public static void GetClassList(GymDbContext context)
        {
            var classes = context.Classes
                .Select(c => new
                {
                    c.ClassId,
                    c.ClassName,
                    StudentCount = c.Students.Count,
                })
                .ToList();

            int idWidth = 5;
            int nameWidth = 25;
            int studentsWidth = 1;
            Console.WriteLine(
                $"{" ID".PadRight(idWidth)} "
                + $"{"Class Name".PadRight(nameWidth)} "
                + $"{"Current Size".PadRight(studentsWidth)}"
            );
            Console.WriteLine(new string('-', idWidth + nameWidth + studentsWidth + 15));

            foreach (var cls in classes)
            {
                Console.WriteLine(
                    $"| {cls.ClassId.ToString().PadRight(idWidth)}"
                    + $"{cls.ClassName.PadRight(nameWidth)} "
                    + $"{cls.StudentCount.ToString().PadRight(studentsWidth)} Students |"
                );
            }

            Console.WriteLine(new string('-', idWidth + nameWidth + studentsWidth + 15));
            return;
        }

        public static void GetGradeList(GymDbContext context)
        {

        }

        public static void GetStaffList(GymDbContext context)
        {
            var staffList = context.Staff
                .Include(s => s.Subjects)
                .Select(s => new
                {
                    s.StaffId,
                    s.FirstName,
                    s.LastName,
                    s.Role,
                    s.Salary,
                    s.StartDate,
                    YearsEmployed = DateTime.Now.Year - s.StartDate.Year
                                    - (DateTime.Now < s.StartDate.AddYears(DateTime.Now.Year - s.StartDate.Year) ? 1 : 0),
                    SubjectNames = s.Subjects.Select(sub => sub.SubjectName).ToList()
                })
                .ToList();

            int idWidth = 5;
            int fNameWidth = 12;
            int lNameWidth = 12;
            int roleWidth = 10;
            int subjWidth = 15;
            int salaryWidth = 15;
            int yearsWidth = 2;

            Console.WriteLine(
                $"{"ID".PadRight(idWidth)} "
                + $"{"FirstName".PadRight(fNameWidth)} "
                + $"{"LastName".PadRight(lNameWidth)} "
                + $"{"Role".PadRight(roleWidth)} "
                + $"{"Subjects".PadRight(subjWidth)} "
                + $"{"Salary".PadRight(salaryWidth)} "
                + $"{"Employed".PadRight(yearsWidth)}"
            );

            Console.WriteLine(new string('-',
                idWidth + fNameWidth + lNameWidth
                + roleWidth + subjWidth + salaryWidth + yearsWidth + 13));

            foreach (var staff in staffList)
            {
                var subjectsString = staff.SubjectNames.Any()
                    ? string.Join(", ", staff.SubjectNames)
                    : "";

                var salaryString = staff.Salary.ToString("C0");

                Console.WriteLine(
                    $"{staff.StaffId.ToString().PadRight(idWidth)} "
                    + $"{(staff.FirstName ?? "").PadRight(fNameWidth)} "
                    + $"{(staff.LastName ?? "").PadRight(lNameWidth)} "
                    + $"{staff.Role.PadRight(roleWidth)} "
                    + $"{subjectsString.PadRight(subjWidth)} "
                    + $"{salaryString.PadRight(salaryWidth)} "
                    + $"{staff.YearsEmployed.ToString().PadRight(yearsWidth)} Years"
                );
            }
            Console.WriteLine();
        }
        public static void GetStudentList(GymDbContext context, int? classID = null)
        {
            var query = context.Students
                .Include(s => s.Class)
                .AsQueryable();

            if (classID.HasValue)
            {
                query = query.Where(s => s.ClassId == classID.Value);
            }

            var studentList = query
                .Select(s => new
                {
                    s.StudentId,
                    s.FirstName,
                    s.LastName,
                    ClassName = s.Class != null ? s.Class.ClassName : "None",
                    s.DateOfBirth
                })
                .ToList();

            int idWidth = 5;
            int fNameWidth = 15;
            int lNameWidth = 15;
            int classWidth = 25;
            int dobWidth = 11;

            Console.WriteLine(
                $"  {"ID".PadRight(idWidth)} " +
                $"{"First Name".PadRight(fNameWidth)} " +
                $"{"Last Name".PadRight(lNameWidth)} " +
                $"{"Class".PadRight(classWidth)} " +
                $"{"Date of Birth".PadRight(dobWidth)}"
            );
            Console.WriteLine(new string('-', idWidth + fNameWidth + lNameWidth + dobWidth + 33));

            foreach (var student in studentList)
            {
                string dobString = student.DateOfBirth.ToString("MM/dd/yyyy");

                Console.WriteLine(
                    $"| {student.StudentId.ToString().PadRight(idWidth)} " +
                    $"{(student.FirstName ?? "").PadRight(fNameWidth)} " +
                    $"{(student.LastName ?? "").PadRight(lNameWidth)} " +
                    $"{student.ClassName.PadRight(classWidth)} " +
                    $"{dobString.PadRight(dobWidth)} |"
                );
            }

            Console.WriteLine(new string('-', idWidth + fNameWidth + lNameWidth + dobWidth + 33));
        }

        public static void GetSubjectList(GymDbContext context, int? studentId = null)
        {
            var subjects = context.Subjects
                .Select(subj => new
                {
                    subj.SubjectId,
                    subj.SubjectName,
                    Grade = studentId.HasValue
                        ? context.Grades
                            .Where(g => g.StudentId == studentId && g.SubjectId == subj.SubjectId)
                            .Select(g => g.GradeValue)
                            .FirstOrDefault()
                        : null
                })
                .ToList();

            int idWidth = 5;
            int nameWidth = 25;
            int gradeWidth = 10;

            Console.Clear();

            Console.WriteLine(
                $"  {"ID".PadRight(idWidth)}" +
                $"{"Subject".PadRight(nameWidth)}" +
                $"{"Grade".PadRight(gradeWidth)}"
            );
            Console.WriteLine(new string('-', idWidth + nameWidth + gradeWidth + 5));

            foreach (var subj in subjects)
            {
                var gradeString = subj.Grade?.ToString() ?? "None";

                Console.WriteLine(
                    $"| {subj.SubjectId.ToString().PadRight(idWidth)}" +
                    $"{subj.SubjectName.PadRight(nameWidth)}" +
                    $"{gradeString.PadRight(gradeWidth)} |"
                );
            }

            Console.WriteLine(new string('-', idWidth + nameWidth + gradeWidth + 5));
            Console.WriteLine();
            return;
        }
        public static void GetRoleList(GymDbContext context)
        {
            var roleList = context.Staff
                .GroupBy(s => s.Role)
                .Select(g => new
                {
                    Role = g.Key,
                    Count = g.Count(),
                    TotalSalary = g.Sum(s => s.Salary),
                    AverageSalary = g.Average(s => s.Salary)
                })
                .OrderByDescending(r => r.Count) // Optional: Order by count descending
                .ToList();

            int roleWidth = 20;
            int countWidth = 10;
            int totalSalaryWidth = 15;
            int averageSalaryWidth = 15;

            Console.WriteLine(
                $"{"Role".PadRight(roleWidth)} "
                + $"{"Count".PadRight(countWidth)} "
                + $"{"Total Salary".PadRight(totalSalaryWidth)}"
                + $"{"Avg Salary".PadRight(averageSalaryWidth)}"
            );
            Console.WriteLine(new string('-', roleWidth + countWidth + totalSalaryWidth + averageSalaryWidth +4));

            foreach (var role in roleList)
            {
                Console.WriteLine(
                    $"| {role.Role.PadRight(roleWidth)}"
                    + $"{role.Count.ToString().PadRight(countWidth)}"
                    + $"{role.TotalSalary.ToString("C0").PadRight(totalSalaryWidth)}"
                    + $"{role.AverageSalary.ToString("C0").PadRight(averageSalaryWidth)} |"
                );
            }

            Console.WriteLine(new string('-', roleWidth + countWidth + totalSalaryWidth + averageSalaryWidth + 4));
        }


    }
}
