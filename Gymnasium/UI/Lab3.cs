using System;
using System.Linq;
using Gymnasium.Models;
using Gymnasium.Data;
using Microsoft.EntityFrameworkCore;
using Gymnasium.UI.Service;

namespace Gymnasium.UI
{
    internal class Lab3
    {
        public static void Lab3Menu(GymDbContext context)
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("Select an option:");
                Console.WriteLine("1. Fetch Staff");
                Console.WriteLine("2. Fetch All Students");
                Console.WriteLine("3. Fetch All Students in a Specific Class");
                Console.WriteLine("4. Fetch All Grades Given in the Last Month");
                Console.WriteLine("5. Fetch Subjects with Average, Highest, and Lowest Grades");
                Console.WriteLine("6. Add New Student");
                Console.WriteLine("7. Add New Staff");
                Console.WriteLine("8. Exit");

                var choice = InputHandler.GetMenuChoice("1", "2", "3", "4", "5", "6", "7", "8");

                switch (choice)
                {
                    case "1":
                        ModelLists.GetStaffList(context);
                        break;
                    case "2":
                        GetAllStudents(context);
                        break;
                    case "3":
                        GetStudentsInClass(context);
                        break;
                    case "4":
                        GetGradesFromLastMonth(context);
                        break;
                    case "5":
                        GetSubjectsWithGrades(context);
                        break;
                    case "6":
                        AddNewStudent(context);
                        break;
                    case "7":
                        AddNewStaff(context);
                        break;
                    case "8":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice, try again.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Method 1: Fetch All Students
        public static void GetAllStudents(GymDbContext context)
        {
            Console.WriteLine("Do you want to sort by first name or last name?");
            Console.WriteLine("1. First Name");
            Console.WriteLine("2. Last Name");
            var nameChoice = InputHandler.GetMenuChoice("1", "2");

            Console.WriteLine("Do you want ascending or descending order?");
            Console.WriteLine("1. Ascending");
            Console.WriteLine("2. Descending");
            var orderChoice = InputHandler.GetMenuChoice("1", "2");

            IQueryable<Student> studentsQuery = context.Students;

            if (nameChoice == "1")
            {
                studentsQuery = orderChoice == "1"
                    ? studentsQuery.OrderBy(student => student.FirstName)
                    : studentsQuery.OrderByDescending(student => student.FirstName);
            }
            else
            {
                studentsQuery = orderChoice == "1"
                    ? studentsQuery.OrderBy(student => student.LastName)
                    : studentsQuery.OrderByDescending(student => student.LastName);
            }

            var students = studentsQuery.ToList();

            foreach (var student in students)
            {
                Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName} {student.LastName}");
            }

            Console.ReadLine();
        }

        // Method 2: Fetch All Students in a Specific Class
        public static void GetStudentsInClass(GymDbContext context)
        {
            var classes = context.Classes.ToList();

            if (classes.Count == 0)
            {
                Console.WriteLine("No classes found.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Select a class:");
            for (int i = 0; i < classes.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {classes[i].ClassName}");
            }

            int choice = InputHandler.GetValidInteger(1, classes.Count);

            var selectedClass = classes[choice - 1];
            var studentsInClass = context.Students
                .Where(students => students.ClassId == selectedClass.ClassId)
                .ToList();

            foreach (var student in studentsInClass)
            {
                Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName} {student.LastName}");
            }

            Console.ReadLine();
        }

        // Method 3: Fetch All Grades Given in the Last Month
        public static void GetGradesFromLastMonth(GymDbContext context)
        {
            Console.Clear();
            var lastMonth = DateTime.Now.AddMonths(-1);

            var recentGrades = context.Grades
                .Where(grade => grade.Date.HasValue
                             && grade.Date.Value >= lastMonth
                             && !string.IsNullOrEmpty(grade.GradeValue))
                .Include(grade => grade.Student)
                .Include(grade => grade.Subject) // Updated from Course to Subject
                .ToList();

            foreach (var grade in recentGrades)
            {
                var studentName = grade.Student != null
                    ? $"{grade.Student.FirstName} {grade.Student.LastName}"
                    : "Unknown Student";

                var subjectName = grade.Subject != null
                    ? grade.Subject.SubjectName
                    : "Unknown Subject";

                Console.WriteLine($"Date: {grade.Date}, Grade: {grade.GradeValue}, Student: {studentName}, Subject: {subjectName}.");
            }
            Console.ReadLine();
        }

        // Method 4: Fetch Subjects with Average, Highest, and Lowest Grades
        public static void GetSubjectsWithGrades(GymDbContext context)
        {
            Console.Clear();
            var gradeScaleLookup = context.GradeScales
                .OrderBy(gradeScale => gradeScale.GradeRank)
                .ToDictionary(gradeScale => gradeScale.GradeRank, gradeScale => gradeScale.GradeValue);

            var subjectStatistics = context.Subjects
                .Select(subject => new
                {
                    SubjectName = subject.SubjectName,

                    AverageGradeRank = context.Grades
                        .Where(grade => grade.SubjectId == subject.SubjectId && grade.GradeValue != null)
                        .Join(context.GradeScales,
                              grade => grade.GradeValue,
                              gradeScale => gradeScale.GradeValue,
                              (grade, gradeScale) => gradeScale.GradeRank)
                        .Average(),

                    BestGrade = context.Grades
                        .Where(grade => grade.SubjectId == subject.SubjectId && grade.GradeValue != null)
                        .Join(context.GradeScales,
                              grade => grade.GradeValue,
                              gradeScale => gradeScale.GradeValue,
                              (grade, gradeScale) => new { grade.GradeValue, gradeScale.GradeRank })
                        .OrderBy(result => result.GradeRank)
                        .Select(result => result.GradeValue)
                        .FirstOrDefault(),

                    WorstGrade = context.Grades
                        .Where(grade => grade.SubjectId == subject.SubjectId && grade.GradeValue != null)
                        .Join(context.GradeScales,
                              grade => grade.GradeValue,
                              gradeScale => gradeScale.GradeValue,
                              (grade, gradeScale) => new { grade.GradeValue, gradeScale.GradeRank })
                        .OrderByDescending(result => result.GradeRank)
                        .Select(result => result.GradeValue)
                        .FirstOrDefault()
                })
                .ToList();

            Console.WriteLine("Subject Statistics:\n");
            foreach (var stat in subjectStatistics)
            {
                var averageGradeLetter = gradeScaleLookup
                    .OrderBy(entry => Math.Abs(entry.Key - stat.AverageGradeRank))
                    .First().Value;

                Console.WriteLine($"Subject: {stat.SubjectName}");
                Console.WriteLine($"  Average Grade: {averageGradeLetter}");
                Console.WriteLine($"  Best Grade: {stat.BestGrade}");
                Console.WriteLine($"  Worst Grade: {stat.WorstGrade}");
                Console.WriteLine("");
            }

            Console.ReadLine();
        }

        // Method 5: Add New Student
        public static void AddNewStudent(GymDbContext context)
        {
            Console.WriteLine("Enter First Name:");
            var firstName = Console.ReadLine();
            Console.WriteLine("Enter Last Name:");
            var lastName = Console.ReadLine();

            var newStudent = new Student
            {
                FirstName = firstName,
                LastName = lastName
            };

            context.Students.Add(newStudent);
            context.SaveChanges();

            Console.WriteLine("New student added!");
            Console.ReadLine();
        }

        // Method 6: Add New Staff
        public static void AddNewStaff(GymDbContext context)
        {
            Console.WriteLine("Enter First Name:");
            var firstName = Console.ReadLine();
            Console.WriteLine("Enter Last Name:");
            var lastName = Console.ReadLine();
            Console.WriteLine("Enter Position:");
            var role = Console.ReadLine();

            var newStaff = new Staff
            {
                FirstName = firstName,
                LastName = lastName,
                Role = role,
            };

            context.Staff.Add(newStaff);
            context.SaveChanges();

            Console.WriteLine("New staff member added!");
            Console.ReadLine();
        }
    }
}
