using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SmartAssignmentTracker
{
    public class AssignmentsView : UserControl
    {
        private Student student;
        private MainForm mainForm;
        private ListBox lstAssignments = null!;

        public AssignmentsView(Student student, MainForm mainForm)
        {
            this.student = student;
            this.mainForm = mainForm;
            BuildUI();
        }

        private void BuildUI()
        {
            var lblTitle = new Label
            {
                Text = "ASSIGNMENTS",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lstAssignments = new ListBox
            {
                Location = new Point(10, 50),
                Size = new Size(400, 280),
                Font = new Font("Segoe UI", 11)
            };
            lstAssignments.DoubleClick += LstAssignments_DoubleClick;
            RefreshAssignmentList();

            var btnMarkComplete = new Button
            {
                Text = "Mark Complete",
                Location = new Point(10, 340),
                Size = new Size(130, 35),
                Font = new Font("Segoe UI", 10),
                FlatStyle = FlatStyle.Flat
            };
            btnMarkComplete.Click += BtnMarkComplete_Click;

            var btnAddAssignment = new Button
            {
                Text = "Add Assignment",
                Location = new Point(150, 340),
                Size = new Size(140, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddAssignment.Click += BtnAddAssignment_Click;

            var lblHint = new Label
            {
                Text = "Double-click an assignment to view details",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(10, 380),
                AutoSize = true
            };

            Controls.AddRange(new Control[] { lblTitle, lstAssignments, btnMarkComplete, btnAddAssignment, lblHint });
        }

        private void RefreshAssignmentList()
        {
            lstAssignments.Items.Clear();
            var allAssignments = student.Courses
                .SelectMany(c => c.Assignments, (c, a) => new { Course = c, Assignment = a })
                .ToList();

            if (allAssignments.Count == 0)
            {
                lstAssignments.Items.Add("No assignments added yet.");
            }
            else
            {
                foreach (var item in allAssignments)
                {
                    lstAssignments.Items.Add($"{item.Assignment.Title} - {item.Course.CourseName} [{item.Assignment.Status}]");
                }
            }
        }

        private void LstAssignments_DoubleClick(object? sender, EventArgs e)
        {
            var allAssignments = student.Courses
                .SelectMany(c => c.Assignments, (c, a) => new { Course = c, Assignment = a })
                .ToList();

            int index = lstAssignments.SelectedIndex;
            if (index < 0 || allAssignments.Count == 0) return;

            var selected = allAssignments[index];
            mainForm.NavigateToAssignmentDetail(selected.Assignment, selected.Course);
        }

        private void BtnMarkComplete_Click(object? sender, EventArgs e)
        {
            var allAssignments = student.Courses
                .SelectMany(c => c.Assignments, (c, a) => new { Course = c, Assignment = a })
                .ToList();

            int index = lstAssignments.SelectedIndex;
            if (index < 0 || allAssignments.Count == 0)
            {
                MessageBox.Show("Please select an assignment.", "Mark Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selected = allAssignments[index];
            selected.Assignment.MarkComplete();
            MessageBox.Show($"'{selected.Assignment.Title}' marked as complete.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshAssignmentList();
        }

        private void BtnAddAssignment_Click(object? sender, EventArgs e)
        {
            if (student.Courses.Count == 0)
            {
                MessageBox.Show("No courses available. Add a course first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var dialog = new Form
            {
                Text = "Add Assignment",
                Size = new Size(380, 300),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblCourse = new Label { Text = "Course:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 10) };
            var cmbCourse = new ComboBox
            {
                Location = new Point(20, 45),
                Size = new Size(320, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (var course in student.Courses)
            {
                cmbCourse.Items.Add(course.CourseName);
            }
            if (cmbCourse.Items.Count > 0) cmbCourse.SelectedIndex = 0;

            var lblTitle = new Label { Text = "Title:", Location = new Point(20, 80), AutoSize = true, Font = new Font("Segoe UI", 10) };
            var txtTitle = new TextBox { Location = new Point(20, 105), Size = new Size(320, 25), Font = new Font("Segoe UI", 10) };
            var lblDueDate = new Label { Text = "Due Date:", Location = new Point(20, 140), AutoSize = true, Font = new Font("Segoe UI", 10) };
            var dtpDueDate = new DateTimePicker
            {
                Location = new Point(20, 165),
                Size = new Size(320, 25),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Short
            };

            var btnSave = new Button
            {
                Text = "Save",
                Location = new Point(20, 205),
                Size = new Size(320, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnSave.Click += (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Please enter a title.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedCourse = student.Courses[cmbCourse.SelectedIndex];
                var allAssignmentsForId = student.Courses.SelectMany(c => c.Assignments).ToList();
                int assignmentId = allAssignmentsForId.Count > 0
                    ? allAssignmentsForId.Max(a => a.AssignmentID) + 1
                    : 1;
                var assignment = new Assignment(assignmentId, selectedCourse.CourseID, txtTitle.Text.Trim(), dtpDueDate.Value, "In Progress");
                selectedCourse.AddAssignment(assignment);
                dialog.Close();
            };

            dialog.Controls.AddRange(new Control[] { lblCourse, cmbCourse, lblTitle, txtTitle, lblDueDate, dtpDueDate, btnSave });
            dialog.ShowDialog();
            RefreshAssignmentList();
        }
    }
}
