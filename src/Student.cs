using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SmartAssignmentTracker
{
    public class Student
    {
        public int StudentID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Course> Courses { get; set; }

        public Student()
        {
            StudentID = 0;
            Name = "";
            Email = "";
            Courses = new List<Course>();
        }

        public Student(int studentID, string name, string email)
        {
            StudentID = studentID;
            Name = name;
            Email = email;
            Courses = new List<Course>();
        }

        public void AddCourse(Course course)
        {
            Courses.Add(course);
        }
    }
}