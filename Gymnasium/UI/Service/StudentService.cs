using System;
using System.Linq;
using Gymnasium.Data;
using Gymnasium.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Gymnasium.UI.Service
{
    internal class StudentService
    {
        public static void AddStudent(GymDbContext context)
        {
            //var firstName = InputHandler.GetValidString("Enter First Name:", 50);
            //var lastName = InputHandler.GetValidString("Enter Last Name:", 50);
            //var dateOfBirth = InputHandler.GetValidDate("Enter Date of Birth (yyyy-MM-dd):");
            //var classes = context.Classes.ToList();
            var firstName = "rikard";
            var lastName = "rikard";
            DateTime dateOfBirth = new DateTime(2025, 1, 13);
            var classes = context.Classes.ToList();
            ModelLists.GetClassList(context);
            Console.WriteLine("Enter the ID of the class you want to assign the student to");
            var classId = InputHandler.GetValidID(classes, s => s.ClassId);

            var newStudent = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                ClassId = classId
            };
            context.Students.Add(newStudent);
            context.SaveChanges();

            // Retrieve and display the selected class
            var selectedClass = classes.FirstOrDefault(c => c.ClassId == classId);
            Console.WriteLine($"{firstName} {lastName} was successfully added to {selectedClass.ClassName}.");
            Thread.Sleep(3000);
            return;
        }

        public static void UpdateGrade(GymDbContext context)
        {
            //Choose student
            ModelLists.GetStudentList(context);
            Thread.Sleep(1000);
            var students = context.Students
                .Select(s => new
                {
                    s.StudentId,
                    s.FirstName
                })
                .ToList();
            Console.WriteLine(value: "Enter the ID of the student you would want to change grade on.");
            var studentId = InputHandler.GetValidID(students, s => s.StudentId);

            //Choose subject 
            ModelLists.GetSubjectList(context, studentId);
            var subjects = context.Subjects
                .Select(subj => new
                {
                     subj.SubjectId,
                     subj.SubjectName,
                     Grade = context.Grades
                    .Where(g => g.StudentId == studentId && g.SubjectId == subj.SubjectId)
                    .Select(g => g.GradeValue)
                    .FirstOrDefault() // Get the grade if it exists
                })
                .ToList();
            Console.WriteLine("of subject you want to change for this student");
            var subjectId = InputHandler.GetValidID( subjects, s => s.SubjectId);

            // get data
            var selectedSubject = subjects.FirstOrDefault(s => s.SubjectId == subjectId);
            var subjectName = selectedSubject?.SubjectName ?? "Unknown";
            var selectedStudent = students.FirstOrDefault(s => s.StudentId == studentId);
            var studentFirstName = selectedStudent?.FirstName ?? "Unknown";

            //choose Grade
            Console.Clear();
            Console.WriteLine($"What is {studentFirstName}'s new grade for {subjectName}?");
            Console.WriteLine();


            var newGradeValue = InputHandler.GetValidGrade(
            "A+", "A", "A-",
            "B+", "B", "B-",
            "C+", "C", "C-",
            "D+", "D", "D-",
            "F"
            );

            // Step 5: Save the grade
            var grade = context.Grades
                .FirstOrDefault(g => g.StudentId == studentId && g.SubjectId == subjectId);

            if (grade == null)
            {
                // If no grade exists, create a new entry
                grade = new Grade
                {
                    StudentId = studentId,
                    SubjectId = subjectId,
                    GradeValue = newGradeValue
                };
                context.Grades.Add(grade);
            }
            else
            {
                // Update existing grade
                grade.GradeValue = newGradeValue;
            }

            context.SaveChanges();

            Console.WriteLine($"{studentFirstName}s Grade successfully updated to '{newGradeValue}' for subject '{subjectName}'.");
            Thread.Sleep(2000);
            return;
        }
    }
}
