using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gymnasium.Models;
using Microsoft.EntityFrameworkCore;

namespace Gymnasium.Data
{
    public static class SampleData
    {
        public static void Add(GymDbContext context)
        {
            var random = new Random();

            var gradeScales = new List<GradeScale>
            {
                new GradeScale { GradeValue = "A+", GradeRank = 1 },
                new GradeScale { GradeValue = "A",  GradeRank = 2 },
                new GradeScale { GradeValue = "A-", GradeRank = 3 },
                new GradeScale { GradeValue = "B+", GradeRank = 4 },
                new GradeScale { GradeValue = "B",  GradeRank = 5 },
                new GradeScale { GradeValue = "B-", GradeRank = 6 },
                new GradeScale { GradeValue = "C+", GradeRank = 7 },
                new GradeScale { GradeValue = "C",  GradeRank = 8 },
                new GradeScale { GradeValue = "C-", GradeRank = 9 },
                new GradeScale { GradeValue = "D+", GradeRank = 10 },
                new GradeScale { GradeValue = "D",  GradeRank = 11 },
                new GradeScale { GradeValue = "D-", GradeRank = 12 },
                new GradeScale { GradeValue = "F",  GradeRank = 13 },
            };
            context.GradeScales.AddRange(gradeScales);
            context.SaveChanges();

            var classes = new List<Class>
            {
                new Class { ClassName = "Avengers Assemble" },
                new Class { ClassName = "Justice League" },
                new Class { ClassName = "Springfield Elementary" },
                new Class { ClassName = "Hogwarts Crew" },
            };
            context.Classes.AddRange(classes);
            context.SaveChanges();


            var staffList = new List<Staff>
            {
                // 10 Teachers
                new Staff { FirstName = "Homer",    LastName = "Simpson",   Role = "Teacher",   Salary = random.Next(40000, 80001) },
                new Staff { FirstName = "Marge",    LastName = "Simpson",   Role = "Teacher",   Salary = random.Next(40000, 80001) },
                new Staff { FirstName = "Tony",     LastName = "Stark",     Role = "Teacher",   Salary = random.Next(40000, 80001) },
                new Staff { FirstName = "Bruce",    LastName = "Wayne",     Role = "Teacher",   Salary = random.Next(40000, 80001) },
                new Staff { FirstName = "Lois",     LastName = "Lane",      Role = "Teacher",   Salary = random.Next(40000, 80001) },
                new Staff { FirstName = "Clark",    LastName = "Kent",      Role = "Teacher",   Salary = random.Next(40000, 80001) },
                new Staff { FirstName = "Peter",    LastName = "Parker",    Role = "Teacher",   Salary = random.Next(40000, 80001) },
                new Staff { FirstName = "Diana",    LastName = "Prince",    Role = "Teacher",   Salary = random.Next(40000, 80001) },
                new Staff { FirstName = "Thor",     LastName = "Odinson",   Role = "Teacher",   Salary = random.Next(40000, 80001) },
                new Staff { FirstName = "Natasha",  LastName = "Romanoff",  Role = "Teacher",   Salary = random.Next(40000, 80001) },

                // 1 Principal
                new Staff { FirstName = "Albus",    LastName = "Dumbledore",Role = "Principal", Salary = random.Next(60000, 100001) },

                // 1 Counselor
                new Staff { FirstName = "Minerva",  LastName = "McGonagall",Role = "Counselor", Salary = random.Next(50000, 90001) },

                // 2 Secretaries
                new Staff { FirstName = "Severus",  LastName = "Snape",      Role = "Secretary", Salary = random.Next(30000, 50001) },
                new Staff { FirstName = "Rubeus",   LastName = "Hagrid",     Role = "Secretary", Salary = random.Next(30000, 50001) },

                // 1 Janitor
                new Staff { FirstName = "Argus",    LastName = "Filch",      Role = "Janitor",   Salary = random.Next(20000, 40001) },
            };

            foreach (var s in staffList)
            {
                s.StartDate = DateTime.Now.AddYears(-random.Next(1, 16));
                s.DateOfBirth = new DateTime(random.Next(1960, 1990), random.Next(1, 13), random.Next(1, 28));
            }

            context.Staff.AddRange(staffList);
            context.SaveChanges();

            var teacherStaff = staffList
                .Where(s => s.Role == "Teacher")
                .ToList();

            var subjects = new List<Subject>
            {
                new Subject
                {
                    SubjectName = "Mathematics",
                    TeacherId = teacherStaff[random.Next(teacherStaff.Count)].StaffId
                },
                new Subject
                {
                    SubjectName = "History",
                    TeacherId = teacherStaff[random.Next(teacherStaff.Count)].StaffId
                },
                new Subject
                {
                    SubjectName = "Science",
                    TeacherId = teacherStaff[random.Next(teacherStaff.Count)].StaffId
                },
                new Subject
                {
                    SubjectName = "Art",
                    TeacherId = teacherStaff[random.Next(teacherStaff.Count)].StaffId
                }
            };
            context.Subjects.AddRange(subjects);
            context.SaveChanges();

            var firstNames = new List<string>
            {
                "Bart","Lisa","Maggie","Sandy","Squidward","Patrick","Spongebob","Tom","Jerry",
                "Lois","Peter","Stewie","Meg","Brian","Cleveland","Joe","Morty","Rick","Bruce",
                "Tony","Clark","Diana","Natasha","Goku","Vegeta","Naruto","Sasuke","Ash",
                "Misty","Brock","Donald","Mickey","Goofy","Minnie","Daisy","Pluto","Huey",
                "Dewey","Louie","Aladdin","Jasmine","Genie","Woody","Buzz","Simba","Nala",
                "Timon","Pumba","Pikachu","Luigi","Mario","Peach","Toad","Zelda","Link","Kirby",
            };

            var lastNames = new List<string>
            {
                "Simpson","Cheeks","Tentacles","Star","Squarepants","Cat","Mouse","Griffin",
                "McFly","Sanchez","Smith","Doe","Wayne","Stark","Kent","Prince","Romanoff",
                "Skywalker","Solo","Kenobi","Baggins","Potter","Weasley","Malfoy","Ranger",
                "Bond","Ketchum","Duck","Mouse","Lion","Wilde","Sparkle","Pig","Panther",
                "Hutt","Organa","Lestrange","Dursley","Utonium","Bubblegum","Vader","Spartan",
                "Koopa","Kong","Scar","Jetson","Flintstone","Quagmire","Swanson"
            };

            var studentList = new List<Student>();

            for (int i = 0; i < 120; i++)
            {
                var fn = firstNames[random.Next(firstNames.Count)];
                var ln = lastNames[random.Next(lastNames.Count)];

                var randomClass = classes[random.Next(classes.Count)];

                var dob = new DateTime(
                    random.Next(2005, 2015), // year
                    random.Next(1, 13), // month
                    random.Next(1, 28) // day
                );

                studentList.Add(new Student
                {
                    FirstName = fn,
                    LastName = ln,
                    ClassId = randomClass.ClassId,
                    DateOfBirth = dob
                });
            }

            context.Students.AddRange(studentList);
            context.SaveChanges();


            var allSubjects = context.Subjects.ToList();
            var allStudents = context.Students.ToList();

            var letterGrades = new List<string>
            {
                "A+", "A", "A-",
                "B+", "B", "B-",
                "C+", "C", "C-",
                "D+", "D", "D-",
                "F"
            };

            var gradeEntries = new List<Grade>();

            foreach (var student in allStudents)
            {
                foreach (var sub in allSubjects)
                {
                    var randomLetter = letterGrades[random.Next(letterGrades.Count)];

                    gradeEntries.Add(new Grade
                    {
                        StudentId = student.StudentId,
                        SubjectId = sub.SubjectId,
                        GradeValue = randomLetter,
                        Date = DateTime.Now.AddDays(-random.Next(1, 365))
                    });
                }
            }

            context.Grades.AddRange(gradeEntries);
            context.SaveChanges();
        }

        public static void RemoveAll(GymDbContext context)
        {
            context.Grades.RemoveRange(context.Grades);
            context.SaveChanges();

            context.GradeScales.RemoveRange(context.GradeScales);
            context.SaveChanges();

            context.Subjects.RemoveRange(context.Subjects);
            context.SaveChanges();

            context.Students.RemoveRange(context.Students);
            context.SaveChanges();

            context.Classes.RemoveRange(context.Classes);
            context.SaveChanges();

            context.Staff.RemoveRange(context.Staff);
            context.SaveChanges();
        }

    }
}

