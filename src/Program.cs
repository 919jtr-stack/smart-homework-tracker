using System;
using System.Linq;

namespace SmartAssignmentTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Student student = new Student(1, "Ron Swanson", "Ron@email.com");
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n=== Smart Assignment Tracker ===");
                Console.WriteLine("1. Add Course");
                Console.WriteLine("2. View Courses");
                Console.WriteLine("3. Add Assignment");
                Console.WriteLine("4. View All Assignments");
                Console.WriteLine("5. Mark Assignment Complete");
                Console.WriteLine("6. Exit");
                Console.Write("Choose an option: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCourse(student);
                        break;
                    case "2":
                        ViewCourses(student);
                        break;
                    case "3":
                        AddAssignment(student);
                        break;
                    case "4":
                        ViewAssignments(student);
                        break;
                    case "5":
                        MarkAssignmentComplete(student);
                        break;
                    case "6":
                        running = false;
                        Console.WriteLine("Exiting program...");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        static void AddCourse(Student student)
        {
            Console.Write("Enter Course ID: ");
            if (!int.TryParse(Console.ReadLine(), out int courseID))
            {
                Console.WriteLine("Invalid Course ID.");
                return;
            }

            Console.Write("Enter Course Name: ");
            string? courseName = Console.ReadLine();

            Console.Write("Enter Instructor Name: ");
            string? instructor = Console.ReadLine();

            Course course = new Course(
                courseID,
                student.StudentID,
                courseName ?? "",
                instructor ?? ""
            );

            student.AddCourse(course);
            Console.WriteLine("Course added successfully.");
        }

        static void ViewCourses(Student student)
        {
            if (student.Courses.Count == 0)
            {
                Console.WriteLine("No courses added yet.");
                return;
            }

            Console.WriteLine("\n--- Courses ---");
            foreach (Course course in student.Courses)
            {
                Console.WriteLine($"Course ID: {course.CourseID}");
                Console.WriteLine($"Course Name: {course.CourseName}");
                Console.WriteLine($"Instructor: {course.Instructor}");
                Console.WriteLine();
            }
        }

        static void AddAssignment(Student student)
        {
            if (student.Courses.Count == 0)
            {
                Console.WriteLine("No courses available. Add a course first.");
                return;
            }

            Console.WriteLine("\nAvailable Courses:");
            foreach (Course course in student.Courses)
            {
                Console.WriteLine($"{course.CourseID} - {course.CourseName}");
            }

            Console.Write("Enter Course ID for this assignment: ");
            if (!int.TryParse(Console.ReadLine(), out int courseID))
            {
                Console.WriteLine("Invalid Course ID.");
                return;
            }

            Course? selectedCourse = student.Courses.FirstOrDefault(c => c.CourseID == courseID);

            if (selectedCourse == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            Console.Write("Enter Assignment ID: ");
            if (!int.TryParse(Console.ReadLine(), out int assignmentID))
            {
                Console.WriteLine("Invalid Assignment ID.");
                return;
            }

            Console.Write("Enter Assignment Title: ");
            string? title = Console.ReadLine();

            Console.Write("Enter Due Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime dueDate))
            {
                Console.WriteLine("Invalid date format.");
                return;
            }

            Assignment assignment = new Assignment(
                assignmentID,
                selectedCourse.CourseID,
                title ?? "",
                dueDate,
                "In Progress"
            );

            selectedCourse.AddAssignment(assignment);
            Console.WriteLine("Assignment added successfully.");
        }

        static void ViewAssignments(Student student)
        {
            if (student.Courses.Count == 0)
            {
                Console.WriteLine("No courses available.");
                return;
            }

            Console.WriteLine("\n--- Assignments ---");
            bool foundAny = false;

            foreach (Course course in student.Courses)
            {
                if (course.Assignments.Count > 0)
                {
                    Console.WriteLine($"\nCourse: {course.CourseName}");

                    foreach (Assignment assignment in course.Assignments)
                    {
                        Console.WriteLine($"Assignment ID: {assignment.AssignmentID}");
                        Console.WriteLine($"Title: {assignment.Title}");
                        Console.WriteLine($"Due Date: {assignment.DueDate:yyyy-MM-dd}");
                        Console.WriteLine($"Status: {assignment.Status}");
                        Console.WriteLine();
                        foundAny = true;
                    }
                }
            }

            if (!foundAny)
            {
                Console.WriteLine("No assignments added yet.");
            }
        }

        static void MarkAssignmentComplete(Student student)
        {
            Console.Write("Enter Assignment ID to mark complete: ");
            if (!int.TryParse(Console.ReadLine(), out int assignmentID))
            {
                Console.WriteLine("Invalid assignment ID.");
                return;
            }

            foreach (Course course in student.Courses)
            {
                Assignment? foundAssignment =
                    course.Assignments.FirstOrDefault(a => a.AssignmentID == assignmentID);

                if (foundAssignment != null)
                {
                    foundAssignment.MarkComplete();
                    Console.WriteLine("Assignment marked as complete.");
                    return;
                }
            }

            Console.WriteLine("Assignment not found.");
        }
    }
}