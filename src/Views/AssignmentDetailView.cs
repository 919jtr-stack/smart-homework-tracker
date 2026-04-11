using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SmartAssignmentTracker
{
    public class AssignmentDetailView : UserControl
    {
        private Assignment assignment;
        private Course course;
        private MainForm mainForm;

        public AssignmentDetailView(Assignment assignment, Course course, MainForm mainForm)
        {
            this.assignment = assignment;
            this.course = course;
            this.mainForm = mainForm;
            BuildUI();
        }

        private void BuildUI()
        {
            var lblTitle = new Label
            {
                Text = "ASSIGNMENT DETAIL",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            int y = 55;
            int spacing = 35;

            var lblAssignTitle = CreateInfoLabel("Title:", assignment.Title, y); y += spacing;
            var lblDueDate = CreateInfoLabel("Due Date:", assignment.DueDate.ToString("yyyy-MM-dd"), y); y += spacing;
            var lblStatus = CreateInfoLabel("Status:", assignment.Status, y); y += spacing;
            var lblCourse = CreateInfoLabel("Linked Course:", course.CourseName, y); y += spacing;

            // Reminder Settings
            var lblReminders = new Label
            {
                Text = "Reminder Settings:",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, y),
                AutoSize = true
            };
            y += 25;

            var lstReminders = new ListBox
            {
                Location = new Point(10, y),
                Size = new Size(350, 80),
                Font = new Font("Segoe UI", 10)
            };

            if (assignment.Reminders.Count == 0)
            {
                lstReminders.Items.Add("No reminders set");
            }
            else
            {
                foreach (var r in assignment.Reminders)
                {
                    lstReminders.Items.Add($"{r.ReminderDate:yyyy-MM-dd} - {r.NotificationType}");
                }
            }
            y += 90;

            var btnEdit = new Button
            {
                Text = "Edit Assignment",
                Location = new Point(10, y),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 10),
                FlatStyle = FlatStyle.Flat
            };
            btnEdit.Click += (s, e) => MessageBox.Show("This feature is coming soon.", "Edit Assignment", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var btnBack = new Button
            {
                Text = "Back to Dashboard",
                Location = new Point(170, y),
                Size = new Size(160, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBack.Click += (s, e) => mainForm.NavigateToDashboard();

            Controls.AddRange(new Control[]
            {
                lblTitle,
                lblAssignTitle.label, lblAssignTitle.value,
                lblDueDate.label, lblDueDate.value,
                lblStatus.label, lblStatus.value,
                lblCourse.label, lblCourse.value,
                lblReminders, lstReminders,
                btnEdit, btnBack
            });
        }

        private (Label label, Label value) CreateInfoLabel(string labelText, string valueText, int y)
        {
            var lbl = new Label
            {
                Text = labelText,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(10, y),
                AutoSize = true
            };
            var val = new Label
            {
                Text = valueText,
                Font = new Font("Segoe UI", 11),
                Location = new Point(160, y),
                AutoSize = true
            };
            return (lbl, val);
        }
    }
}
