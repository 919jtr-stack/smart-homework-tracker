using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SmartAssignmentTracker
{
    public class DashboardView : UserControl
    {
        private Student student;
        private MainForm mainForm;

        public DashboardView(Student student, MainForm mainForm)
        {
            this.student = student;
            this.mainForm = mainForm;
            BuildUI();
        }

        private void BuildUI()
        {
            AutoScroll = true;

            var lblTitle = new Label
            {
                Text = "DASHBOARD",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var allAssignments = student.Courses.SelectMany(c => c.Assignments).ToList();

            // Overdue Assignments
            var overdue = allAssignments
                .Where(a => a.IsOverdue())
                .OrderBy(a => a.DueDate)
                .ToList();

            var lblOverdue = new Label
            {
                Text = "Overdue Assignments:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.Red,
                Location = new Point(10, 50),
                AutoSize = true
            };

            var lstOverdue = new ListBox
            {
                Location = new Point(10, 75),
                Size = new Size(350, 80),
                Font = new Font("Segoe UI", 10)
            };

            if (overdue.Count == 0)
            {
                lstOverdue.Items.Add("No overdue assignments");
            }
            else
            {
                foreach (var a in overdue)
                {
                    string courseName = student.Courses.FirstOrDefault(c => c.CourseID == a.CourseID)?.CourseName ?? "Unknown";
                    lstOverdue.Items.Add($"{a.Title} - {courseName} ({a.DueDate:yyyy-MM-dd})");
                }
            }

            // Upcoming Assignments (next 7 days, not complete)
            var upcoming = allAssignments
                .Where(a => a.Status != "Complete" && a.DueDate >= DateTime.Now && a.DueDate <= DateTime.Now.AddDays(7))
                .OrderBy(a => a.DueDate)
                .ToList();

            var lblUpcoming = new Label
            {
                Text = "Upcoming Assignments:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 160),
                AutoSize = true
            };

            var lstUpcoming = new ListBox
            {
                Location = new Point(10, 185),
                Size = new Size(350, 90),
                Font = new Font("Segoe UI", 10)
            };

            if (upcoming.Count == 0)
            {
                lstUpcoming.Items.Add("No upcoming assignments");
            }
            else
            {
                foreach (var a in upcoming)
                {
                    string courseName = student.Courses.FirstOrDefault(c => c.CourseID == a.CourseID)?.CourseName ?? "Unknown";
                    lstUpcoming.Items.Add($"{a.Title} - {courseName} ({a.DueDate:yyyy-MM-dd})");
                }
            }

            // Due Soon (next 2 days)
            var dueSoon = allAssignments
                .Where(a => a.Status != "Complete" && a.DueDate >= DateTime.Now && a.DueDate <= DateTime.Now.AddDays(2))
                .OrderBy(a => a.DueDate)
                .ToList();

            var lblDueSoon = new Label
            {
                Text = "Due Soon:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, 285),
                AutoSize = true
            };

            var lstDueSoon = new ListBox
            {
                Location = new Point(10, 310),
                Size = new Size(350, 80),
                Font = new Font("Segoe UI", 10)
            };

            if (dueSoon.Count == 0)
            {
                lstDueSoon.Items.Add("Nothing due soon");
            }
            else
            {
                foreach (var a in dueSoon)
                {
                    string courseName = student.Courses.FirstOrDefault(c => c.CourseID == a.CourseID)?.CourseName ?? "Unknown";
                    lstDueSoon.Items.Add($"{a.Title} - {courseName} ({a.DueDate:yyyy-MM-dd})");
                }
            }

            // Completed Tasks
            var completed = allAssignments.Where(a => a.Status == "Complete").ToList();

            var lblCompleted = new Label
            {
                Text = "Completed Tasks:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.Green,
                Location = new Point(10, 400),
                AutoSize = true
            };

            var lstCompleted = new ListBox
            {
                Location = new Point(10, 425),
                Size = new Size(350, 80),
                Font = new Font("Segoe UI", 10)
            };

            if (completed.Count == 0)
            {
                lstCompleted.Items.Add("No completed tasks");
            }
            else
            {
                foreach (var a in completed)
                {
                    lstCompleted.Items.Add($"{a.Title} (Done)");
                }
            }

            // Add Assignment button
            var btnAddAssignment = new Button
            {
                Text = "Add Assignment",
                Location = new Point(10, 520),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddAssignment.Click += (s, e) => mainForm.NavigateToAssignments();

            Controls.AddRange(new Control[]
            {
                lblTitle, 
                lblOverdue, lstOverdue,
                lblUpcoming, lstUpcoming,
                lblDueSoon, lstDueSoon,
                lblCompleted, lstCompleted,
                btnAddAssignment
            });
        }
    }
}
