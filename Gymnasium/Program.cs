using Gymnasium.Models;
using Microsoft.EntityFrameworkCore;

namespace Gymnasium
{
    class Program
    {
        static void Main(string[] args)
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
                Console.WriteLine("5. Fetch Courses with Average, Highest, and Lowest Grades");
                Console.WriteLine("6. Add New Student");
                Console.WriteLine("7. Add New Staff");
                Console.WriteLine("8. Exit");

                var choice = Console.ReadKey().KeyChar;
                Console.ReadLine(); 

                switch (choice)
                {
                    case '1':
                        GetStaff();
                        break;
                    case '2':
                        GetAllStudents();
                        break;
                    case '3':
                        GetStudentsInClass();
                        break;
                    case '4':
                        GetGradesFromLastMonth();
                        break;
                    case '5':
                        GetCoursesWithGrades();
                        break;
                    case '6':
                        AddNewStudent();
                        break;
                    case '7':
                        AddNewStaff();
                        break;
                    case '8':
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

        // Method 1: Fetch Staff
        private static void GetStaff()
        {
            Console.Clear();
            using (var context = new GymDbContext())
            {
                Console.WriteLine("Do you want to see all staff or filter by category?");
                Console.WriteLine("1. All Staff");
                Console.WriteLine("2. Teachers");
                var choice = Console.ReadKey().KeyChar;

                Console.ReadLine();

                Console.Clear();
                IQueryable<Staff> query = context.Staff;

                if (choice == '2')
                {
                    query = query.Where(staff => staff.Position.Contains("Teacher"));
                }

                Console.WriteLine("List of staff:");

                var staffList = query.ToList();

                foreach (var staff in staffList)
                {
                    Console.WriteLine($"ID: {staff.StaffId}, Name: {staff.FirstName} {staff.LastName}, Position: {staff.Position}");
                }

                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
            }
        }


        // Method 2: Fetch All Students
        private static void GetAllStudents()
        {
            Console.Clear();
            using (var context = new GymDbContext())
            {
                Console.WriteLine("Do you want to sort by first name or last name?");
                Console.WriteLine("1. First Name");
                Console.WriteLine("2. Last Name");
                var nameChoice = Console.ReadKey().KeyChar;

                Console.ReadLine();

                Console.WriteLine("\nDo you want ascending or descending order?");
                Console.WriteLine("1. Ascending");
                Console.WriteLine("2. Descending");
                var orderChoice = Console.ReadKey().KeyChar;

                Console.ReadLine();

                IQueryable<Student> studentsQuery = context.Students;

                Console.Clear();

                if (nameChoice == '1')
                {
                    studentsQuery = orderChoice == '1'
                        ? studentsQuery.OrderBy(student => student.FirstName)
                        : studentsQuery.OrderByDescending(student => student.FirstName);
                }
                else
                {
                    studentsQuery = orderChoice == '1'
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
        }


        // Method 3: Fetch All Students in a Specific Class
        private static void GetStudentsInClass()
        {
            Console.Clear();

            using (var context = new GymDbContext())
            {
                var classes = context.Classes.ToList();
                Console.WriteLine("Select a class:");
                for (int i = 0; i < classes.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {classes[i].ClassName}");
                }

                int classChoice = int.Parse(Console.ReadLine());
                var selectedClass = classes[classChoice - 1];

                var studentsInClass = context.Students.Where(s => s.ClassId == selectedClass.ClassId).ToList();
                
                Console.Clear();

                foreach (var student in studentsInClass)
                {
                    Console.WriteLine($"ID: {student.StudentId}, Name: {student.FirstName} {student.LastName}");
                }
                Console.ReadLine();
            }
        }

        // Method 4: Fetch All Grades Given in the Last Month
        private static void GetGradesFromLastMonth()
        {
            Console.Clear();
            using (var context = new GymDbContext())
            {
                var lastMonth = DateTime.Now.AddMonths(-1);

                var recentGrades = context.Grades
                    .Where(grade => grade.Date.HasValue
                                 && grade.Date.Value >= lastMonth 
                                 && !string.IsNullOrEmpty(grade.GradeValue)) 
                    .Include(grade => grade.Student) 
                    .Include(grade => grade.Course)  
                    .ToList();

                foreach (var grade in recentGrades)
                {
                    var studentName = grade.Student != null
                        ? $"{grade.Student.FirstName} {grade.Student.LastName}"
                        : "Unknown Student";

                    var courseName = grade.Course != null
                        ? grade.Course.CourseName
                        : "Unknown Course";
                    Console.WriteLine($"Date: {grade.Date}, Grade: {grade.GradeValue}, Student: {studentName}, Course: {courseName}.");
                }
                Console.ReadLine();
            }
        }



        // Method 5: Fetch Courses with Average, Highest, and Lowest Grades
        private static void GetCoursesWithGrades()
        {
            Console.Clear();
            using (var context = new GymDbContext())
            {
                // Fetch GradeScale once as a lookup dictionary for average rank conversion
                var gradeScaleLookup = context.GradeScales
                    .OrderBy(gradeScale => gradeScale.GradeRank)
                    .ToDictionary(gradeScale => gradeScale.GradeRank, gradeScale => gradeScale.GradeValue);


                var courseStatistics = context.Courses
                    .Select(course => new
                    {
                        CourseName = course.CourseName,

                        AverageGradeRank = context.Grades
                            .Where(grade => grade.CourseId == course.CourseId && grade.GradeValue != null)
                            .Join(context.GradeScales,
                                  grade => grade.GradeValue,
                                  gradeScale => gradeScale.GradeValue,
                                  (grade, gradeScale) => gradeScale.GradeRank)
                            .Average(),

                        BestGrade = context.Grades
                            .Where(grade => grade.CourseId == course.CourseId && grade.GradeValue != null)
                            .Join(context.GradeScales,
                                  grade => grade.GradeValue,
                                  gradeScale => gradeScale.GradeValue,
                                  (grade, gradeScale) => new { grade.GradeValue, gradeScale.GradeRank })
                            .OrderBy(result => result.GradeRank)
                            .Select(result => result.GradeValue)
                            .FirstOrDefault(),

                        WorstGrade = context.Grades
                            .Where(grade => grade.CourseId == course.CourseId && grade.GradeValue != null)
                            .Join(context.GradeScales,
                                  grade => grade.GradeValue,
                                  gradeScale => gradeScale.GradeValue,
                                  (grade, gradeScale) => new { grade.GradeValue, gradeScale.GradeRank })
                            .OrderByDescending(result => result.GradeRank)
                            .Select(result => result.GradeValue)
                            .FirstOrDefault()
                    })
                    .ToList();

                Console.WriteLine("Course Statistics:\n");
                foreach (var stat in courseStatistics)
                {
                    var averageGradeLetter = gradeScaleLookup
                        .OrderBy(entry => Math.Abs(entry.Key - stat.AverageGradeRank)) 
                        .First().Value;

                    Console.WriteLine($"Course: {stat.CourseName}");
                    Console.WriteLine($"  Average Grade: {averageGradeLetter}");
                    Console.WriteLine($"  Best Grade: {stat.BestGrade}");
                    Console.WriteLine($"  Worst Grade: {stat.WorstGrade}");
                    Console.WriteLine("");
                }

                Console.ReadLine();
            }
        }

        // Method 6: Add New Student
        private static void AddNewStudent()
        {
            using (var context = new GymDbContext())
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
            }
        }

        // Method 7: Add New Staff
        private static void AddNewStaff()
        {
            using (var context = new GymDbContext())
            {
                Console.WriteLine("Enter First Name:");
                var firstName = Console.ReadLine();
                Console.WriteLine("Enter Last Name:");
                var lastName = Console.ReadLine();
                Console.WriteLine("Enter Position:");
                var position = Console.ReadLine();

                var newStaff = new Staff
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Position = position
                };

                context.Staff.Add(newStaff);
                context.SaveChanges();

                Console.WriteLine("New staff member added!");
                Console.ReadLine();
            }
        }
    }

}
