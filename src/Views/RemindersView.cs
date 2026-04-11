using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SmartAssignmentTracker
{
    public class RemindersView : UserControl
    {
        private Student student;
        private ListBox lstReminders = null!;

        public RemindersView(Student student)
        {
            this.student = student;
            BuildUI();
        }

        private void BuildUI()
        {
            var lblTitle = new Label
            {
                Text = "REMINDERS",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lstReminders = new ListBox
            {
                Location = new Point(10, 50),
                Size = new Size(400, 280),
                Font = new Font("Segoe UI", 11)
            };
            RefreshReminderList();

            var btnAddReminder = new Button
            {
                Text = "Add Reminder",
                Location = new Point(10, 340),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddReminder.Click += BtnAddReminder_Click;

            Controls.AddRange(new Control[] { lblTitle, lstReminders, btnAddReminder });
        }

        private void RefreshReminderList()
        {
            lstReminders.Items.Clear();

            var allReminders = student.Courses
                .SelectMany(c => c.Assignments, (c, a) => new { Course = c, Assignment = a })
                .SelectMany(ca => ca.Assignment.Reminders, (ca, r) => new { ca.Course, ca.Assignment, Reminder = r })
                .ToList();

            if (allReminders.Count == 0)
            {
                lstReminders.Items.Add("No reminders added yet.");
            }
            else
            {
                foreach (var item in allReminders)
                {
                    lstReminders.Items.Add($"{item.Reminder.ReminderDate:yyyy-MM-dd} - {item.Assignment.Title} ({item.Reminder.NotificationType})");
                }
            }
        }

        private void BtnAddReminder_Click(object? sender, EventArgs e)
        {
            var allAssignments = student.Courses
                .SelectMany(c => c.Assignments, (c, a) => new { Course = c, Assignment = a })
                .ToList();

            if (allAssignments.Count == 0)
            {
                MessageBox.Show("No assignments available. Add an assignment first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dialog = new Form
            {
                Text = "Add Reminder",
                Size = new Size(380, 290),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblAssignment = new Label { Text = "Assignment:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 10) };
            var cmbAssignment = new ComboBox
            {
                Location = new Point(20, 45),
                Size = new Size(320, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var item in allAssignments)
            {
                cmbAssignment.Items.Add($"{item.Assignment.Title} ({item.Course.CourseName})");
            }
            if (cmbAssignment.Items.Count > 0) cmbAssignment.SelectedIndex = 0;

            var lblDate = new Label { Text = "Reminder Date:", Location = new Point(20, 80), AutoSize = true, Font = new Font("Segoe UI", 10) };
            var dtpDate = new DateTimePicker
            {
                Location = new Point(20, 105),
                Size = new Size(320, 25),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };

            var lblType = new Label { Text = "Notification Type:", Location = new Point(20, 140), AutoSize = true, Font = new Font("Segoe UI", 10) };
            var cmbType = new ComboBox
            {
                Location = new Point(20, 165),
                Size = new Size(320, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbType.Items.AddRange(new object[] { "Email", "SMS", "Push Notification" });
            cmbType.SelectedIndex = 0;

            var btnSave = new Button
            {
                Text = "Save",
                Location = new Point(20, 200),
                Size = new Size(320, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnSave.Click += (s, args) =>
            {
                var selected = allAssignments[cmbAssignment.SelectedIndex];
                var allReminders = student.Courses
                    .SelectMany(c => c.Assignments)
                    .SelectMany(a => a.Reminders)
                    .ToList();
                int reminderId = allReminders.Count > 0
                    ? allReminders.Max(r => r.ReminderID) + 1
                    : 1;
                var reminder = new Reminder(reminderId, selected.Assignment.AssignmentID, dtpDate.Value, cmbType.SelectedItem?.ToString() ?? "Email");
                selected.Assignment.AddReminder(reminder);
                dialog.Close();
            };

            dialog.Controls.AddRange(new Control[] { lblAssignment, cmbAssignment, lblDate, dtpDate, lblType, cmbType, btnSave });
            dialog.ShowDialog();
            RefreshReminderList();
        }
    }
}
