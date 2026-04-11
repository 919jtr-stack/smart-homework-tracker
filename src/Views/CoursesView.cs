using System;
using System.Drawing;
using System.Windows.Forms;

namespace SmartAssignmentTracker
{
    public class CoursesView : UserControl
    {
        private Student student;
        private ListBox lstCourses = null!;

        public CoursesView(Student student)
        {
            this.student = student;
            BuildUI();
        }

        private void BuildUI()
        {
            var lblTitle = new Label
            {
                Text = "COURSES",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            lstCourses = new ListBox
            {
                Location = new Point(10, 50),
                Size = new Size(350, 250),
                Font = new Font("Segoe UI", 11)
            };
            RefreshCourseList();

            var btnViewCourse = new Button
            {
                Text = "View Course",
                Location = new Point(10, 310),
                Size = new Size(130, 35),
                Font = new Font("Segoe UI", 10),
                FlatStyle = FlatStyle.Flat
            };
            btnViewCourse.Click += BtnViewCourse_Click;

            var btnAddCourse = new Button
            {
                Text = "Add Course",
                Location = new Point(150, 310),
                Size = new Size(130, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAddCourse.Click += BtnAddCourse_Click;

            Controls.AddRange(new Control[] { lblTitle, lstCourses, btnViewCourse, btnAddCourse });
        }

        private void RefreshCourseList()
        {
            lstCourses.Items.Clear();
            if (student.Courses.Count == 0)
            {
                lstCourses.Items.Add("No courses added yet.");
            }
            else
            {
                foreach (var course in student.Courses)
                {
                    lstCourses.Items.Add($"{course.CourseName} - {course.Instructor}");
                }
            }
        }

        private void BtnViewCourse_Click(object? sender, EventArgs e)
        {
            int index = lstCourses.SelectedIndex;
            if (index < 0 || student.Courses.Count == 0)
            {
                MessageBox.Show("Please select a course.", "View Course", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var course = student.Courses[index];
            string details = $"Course ID: {course.CourseID}\n" +
                             $"Course Name: {course.CourseName}\n" +
                             $"Instructor: {course.Instructor}\n" +
                             $"Assignments: {course.Assignments.Count}";

            MessageBox.Show(details, "Course Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnAddCourse_Click(object? sender, EventArgs e)
        {
            using var dialog = new Form
            {
                Text = "Add Course",
                Size = new Size(350, 240),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblName = new Label { Text = "Course Name:", Location = new Point(20, 20), AutoSize = true, Font = new Font("Segoe UI", 10) };
            var txtName = new TextBox { Location = new Point(20, 45), Size = new Size(290, 25), Font = new Font("Segoe UI", 10) };
            var lblInstructor = new Label { Text = "Instructor:", Location = new Point(20, 80), AutoSize = true, Font = new Font("Segoe UI", 10) };
            var txtInstructor = new TextBox { Location = new Point(20, 105), Size = new Size(290, 25), Font = new Font("Segoe UI", 10) };

            var btnSave = new Button
            {
                Text = "Save",
                Location = new Point(20, 150),
                Size = new Size(290, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            btnSave.Click += (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Please enter a course name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int courseId = student.Courses.Count > 0
                    ? student.Courses.Max(c => c.CourseID) + 1
                    : 1;
                var course = new Course(courseId, student.StudentID, txtName.Text.Trim(), txtInstructor.Text.Trim());
                student.AddCourse(course);
                dialog.Close();
            };

            dialog.Controls.AddRange(new Control[] { lblName, txtName, lblInstructor, txtInstructor, btnSave });
            dialog.ShowDialog();
            RefreshCourseList();
        }
    }
}
