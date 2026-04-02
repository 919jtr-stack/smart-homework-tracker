using System;

namespace SmartAssignmentTracker
{
    public class Reminder
    {
        public int ReminderID { get; set; }
        public int AssignmentID { get; set; }
        public DateTime ReminderDate {get; set; }
        public string NotificationType { get; set; }

        public Reminder()
        {
            ReminderID = 0;
            AssignmentID = 0;
            ReminderDate = DateTime.Now;
            NotificationType = "";
        }

        public Reminder(int reminderID, int assignmentID, DateTime reminderDate, string notificationType)
        {
            ReminderID = reminderID;
            AssignmentID = assignmentID;
            ReminderDate = reminderDate;
            NotificationType = notificationType;
        }
    }
}