using System;
using System.Drawing;
using System.Windows.Forms;

namespace SmartAssignmentTracker
{
    public class MainForm : Form
    {
        private Panel navPanel = null!;
        private Panel contentPanel = null!;
        private Student student;
        private Button btnHome = null!;
        private Button btnCourses = null!;
        private Button btnAssignments = null!;
        private Button btnReminders = null!;
        private Button btnCalendar = null!;
        private Button btnProfile = null!;

        public MainForm(Student student)
        {
            this.student = student;

            Text = "Smart Assignment Tracker";
            Size = new Size(800, 550);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(700, 450);

            BuildNavBar();
            BuildContentPanel();

            // Start on Dashboard
            NavigateTo(new DashboardView(student, this));
            HighlightNavButton(btnHome);
            ShowAssignmentNotifications();
        }

        private void BuildNavBar()
        {
            navPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 45,
                BackColor = Color.FromArgb(45, 45, 48)
            };

            int btnWidth = 120;
            int x = 10;

            btnHome = CreateNavButton("Home", x); x += btnWidth + 5;
            btnCourses = CreateNavButton("Courses", x); x += btnWidth + 5;
            btnAssignments = CreateNavButton("Assignments", x); x += btnWidth + 5;
            btnReminders = CreateNavButton("Reminders", x); x += btnWidth + 5;
            btnCalendar = CreateNavButton("Calendar", x); x += btnWidth + 5;
            btnProfile = CreateNavButton("Profile", x);

            btnHome.Click += (s, e) => { NavigateTo(new DashboardView(student, this)); HighlightNavButton(btnHome); };
            btnCourses.Click += (s, e) => { NavigateTo(new CoursesView(student)); HighlightNavButton(btnCourses); };
            btnAssignments.Click += (s, e) => { NavigateTo(new AssignmentsView(student, this)); HighlightNavButton(btnAssignments); };
            btnReminders.Click += (s, e) => { NavigateTo(new RemindersView(student)); HighlightNavButton(btnReminders); };
            btnCalendar.Click += (s, e) => { NavigateTo(new CalendarView(student)); HighlightNavButton(btnCalendar); };
            btnProfile.Click += (s, e) => { NavigateTo(new ProfileView(student)); HighlightNavButton(btnProfile); };

            navPanel.Controls.AddRange(new Control[] { btnHome, btnCourses, btnAssignments, btnReminders, btnCalendar, btnProfile });
            Controls.Add(navPanel);
        }

        private Button CreateNavButton(string text, int x)
        {
            return new Button
            {
                Text = text,
                Location = new Point(x, 7),
                Size = new Size(120, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(45, 45, 48),
                Font = new Font("Segoe UI", 10),
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };
        }

        private void HighlightNavButton(Button active)
        {
            foreach (Control c in navPanel.Controls)
            {
                if (c is Button btn)
                {
                    btn.BackColor = Color.FromArgb(45, 45, 48);
                    btn.Font = new Font("Segoe UI", 10);
                }
            }
            active.BackColor = Color.FromArgb(0, 120, 215);
            active.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        private void BuildContentPanel()
        {
            contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };
            Controls.Add(contentPanel);
            contentPanel.BringToFront();
        }

        public void NavigateTo(UserControl view)
        {
            contentPanel.Controls.Clear();
            view.Dock = DockStyle.Fill;
            contentPanel.Controls.Add(view);
        }

        public void NavigateToAssignments()
        {
            NavigateTo(new AssignmentsView(student, this));
            HighlightNavButton(btnAssignments);
        }

        public void NavigateToDashboard()
        {
            NavigateTo(new DashboardView(student, this));
            HighlightNavButton(btnHome);
        }

        public void NavigateToAssignmentDetail(Assignment assignment, Course course)
        {
            NavigateTo(new AssignmentDetailView(assignment, course, this));
        }

        private void ShowAssignmentNotifications()
        {
            var importantAssignments = student.Courses
                .SelectMany(c => c.Assignments, (c, a) => new { Course = c, Assignment = a })
                .Where(x => x.Assignment.IsOverdue() || x.Assignment.IsDueToday())
                .ToList();

            if (importantAssignments.Count == 0)
            {
             return;   // No important assignments to notify
            }

            string message = "Important Assignments:\n\n";
            foreach (var item in importantAssignments)
            {
                if (item.Assignment.IsOverdue())
                {
                    message += $"[OVERDUE] {item.Assignment.Title} (Course: {item.Course.CourseName}, Due: {item.Assignment.DueDate:d})\n";
                }
                else if (item.Assignment.IsDueToday())
                {
                    message += $"[DUE TODAY] {item.Assignment.Title} (Course: {item.Course.CourseName}, Due: {item.Assignment.DueDate:d})\n";
                }
            }
            MessageBox.Show(message, "Assignment Notifications", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
