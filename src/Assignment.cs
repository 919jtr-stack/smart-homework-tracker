using System;
using System.Collections.Generic;

namespace SmartAssignmentTracker
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public int CourseID { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }

        public List<Reminder> Reminders { get; set; }

        public Assignment()
        {
            AssignmentID = 0;
            CourseID = 0;
            Title = "";
            DueDate = DateTime.Now;
            Status = "In Progress";
            Reminders = new List<Reminder>();
        }

        public Assignment(int assignmentID, int courseID, string title, DateTime dueDate, string status)
        {
            AssignmentID = assignmentID;
            CourseID = courseID;
            Title = title;
            DueDate = dueDate;
            Status = status;
            Reminders = new List<Reminder>();
        }

        public void AddReminder(Reminder reminder)
        {
            Reminders.Add(reminder);
        }

        public void MarkComplete()
        {
            Status = "Complete";
        }

        public bool IsOverdue()
        {
            return Status != "Complete" && DueDate.Date < DateTime.Now.Date;
        }

        public bool IsDueToday()
        {
            return Status != "Complete" && DueDate.Date == DateTime.Now.Date;
        }

        public void EditAssignment(string newTitle, DateTime newDueDate, string newStatus)
        {
            Title = newTitle;
            DueDate = newDueDate;
            Status = newStatus;
        }

        public void DeleteReminder(int reminderId)
        {
            Reminders.RemoveAll(r => r.ReminderID == reminderId);
        }
    }
}