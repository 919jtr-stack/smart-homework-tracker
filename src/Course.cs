using System.Collections.Generic;
namespace SmartAssignmentTracker
{
    public class Course
    {
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public string CourseName { get; set; }
        public string Instructor { get; set; }

        public List<Assignment> Assignments { get; set; }

        public Course()
        {
            CourseID = 0;
            StudentID = 0;
            CourseName = "";
            Instructor = "";
            Assignments = new List<Assignment>();
        }

        public Course(int courseID, int studentID, string courseName, string instructor)
        {
            CourseID = courseID;
            StudentID = studentID;
            CourseName = courseName;
            Instructor = instructor;
            Assignments = new List<Assignment>();
        }
        
        public void AddAssignment(Assignment assignment)
        {
            Assignments.Add(assignment);
        }

        public void DeleteAssignment(int assignmentId)
        {
            Assignments.RemoveAll(a => a.AssignmentID == assignmentId);
        }

        public Assignment GetAssignmentById(int assignmentId)
        {
            return Assignments.Find(a => a.AssignmentID == assignmentId);
        }
    }
}