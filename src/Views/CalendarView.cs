using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SmartAssignmentTracker
{
    
    public class CalendarView : UserControl
    {
        private Student student;
        private MonthCalendar monthCalendar = null!;
        private ListBox lstAssignments = null!;

        public CalendarView(Student student)
        {
            this.student = student;
            BuildUI();
        }

        private void BuildUI()
        {
            var lblTitle = new Label
            {
                Text = "CALENDAR VIEW",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            monthCalendar = new MonthCalendar
            {
                Location = new Point(10, 50),
                MaxSelectionCount = 1
            };
            monthCalendar.DateSelected += MonthCalendar_DateSelected;

            var lblAssignments = new Label
            {
                Text = "Assignments Due:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 230),
                AutoSize = true
            };

            lstAssignments = new ListBox
            {
                Location = new Point(10, 260),
                Size = new Size(450, 150),
                Font = new Font("Segoe UI", 10)
            };

            Controls.AddRange(new Control[] 
            {
                lblTitle, 
                monthCalendar,
                lblAssignments,
                lstAssignments
            });
            LoadAssignmentsForDate(DateTime.Today);
        }

        private void MonthCalendar_DateSelected(object? sender, DateRangeEventArgs e)
        {
            LoadAssignmentsForDate(e.Start);
        }

        private void LoadAssignmentsForDate(DateTime date)
        {
            lstAssignments.Items.Clear();
            var assignmentsDue = student.Courses
                .SelectMany(c => c.Assignments, (c, a) => new { Course = c, Assignment = a })
                .Where(x => x.Assignment.DueDate.Date == date.Date)
                .ToList();

            if (assignmentsDue.Count == 0)
            {
                lstAssignments.Items.Add("No assignments due on this date.");
            }
            else
            {
                foreach (var item in assignmentsDue)
                {
                    string prefix = "";
                    if (item.Assignment.IsOverdue())
                    {
                        prefix = "[OVERDUE] ";
                    }
                    else if (item.Assignment.IsDueToday())
                    {
                        prefix = "[DUE TODAY] ";
                    }
                    lstAssignments.Items.Add(
                        $"{prefix}{item.Assignment.Title} - {item.Course.CourseName} - Due: {item.Assignment.DueDate.ToShortDateString()} [{item.Assignment.Status}]"
                    );
                }
            }
        }
    }
}
