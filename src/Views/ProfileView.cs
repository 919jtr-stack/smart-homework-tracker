using System;
using System.Drawing;
using System.Windows.Forms;

namespace SmartAssignmentTracker
{
    public class ProfileView : UserControl
    {
        private Student student;
        private Label lblNameValue = null!;
        private Label lblEmailValue = null!;
        private TextBox txtName = null!;
        private TextBox txtEmail = null!;
        private Button btnEdit = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;

        public ProfileView(Student student)
        {
            this.student = student;
            BuildUI();
        }

        private void BuildUI()
        {
            var lblTitle = new Label
            {
                Text = "PROFILE",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var lblName = new Label
            {
                Text = "Name:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 60),
                AutoSize = true
            };

            lblNameValue = new Label
            {
                Text = student.Name,
                Font = new Font("Segoe UI", 12),
                Location = new Point(120, 60),
                AutoSize = true
            };

            txtName = new TextBox
            {
                Text = student.Name,
                Font = new Font("Segoe UI", 12),
                Location = new Point(120, 57),
                Size = new Size(250, 30),
                Visible = false
            };

            var lblEmail = new Label
            {
                Text = "Email:",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(10, 100),
                AutoSize = true
            };

            lblEmailValue = new Label
            {
                Text = student.Email,
                Font = new Font("Segoe UI", 12),
                Location = new Point(120, 100),
                AutoSize = true
            };

            txtEmail = new TextBox
            {
                Text = student.Email,
                Font = new Font("Segoe UI", 12),
                Location = new Point(120, 97),
                Size = new Size(250, 30),
                Visible = false
            };

            btnEdit = new Button
            {
                Text = "Edit Profile",
                Location = new Point(10, 160),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnEdit.Click += BtnEdit_Click;

            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(10, 160),
                Size = new Size(100, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Visible = false
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(120, 160),
                Size = new Size(100, 35),
                Font = new Font("Segoe UI", 10),
                FlatStyle = FlatStyle.Flat,
                Visible = false
            };
            btnCancel.Click += BtnCancel_Click;

            Controls.AddRange(new Control[]
            {
                lblTitle, lblName, lblNameValue, txtName,
                lblEmail, lblEmailValue, txtEmail,
                btnEdit, btnSave, btnCancel
            });
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            txtName.Text = student.Name;
            txtEmail.Text = student.Email;
            lblNameValue.Visible = false;
            lblEmailValue.Visible = false;
            txtName.Visible = true;
            txtEmail.Visible = true;
            btnEdit.Visible = false;
            btnSave.Visible = true;
            btnCancel.Visible = true;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Name and email cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            student.Name = txtName.Text.Trim();
            student.Email = txtEmail.Text.Trim();
            lblNameValue.Text = student.Name;
            lblEmailValue.Text = student.Email;
            ExitEditMode();
            MessageBox.Show("Profile updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            ExitEditMode();
        }

        private void ExitEditMode()
        {
            lblNameValue.Visible = true;
            lblEmailValue.Visible = true;
            txtName.Visible = false;
            txtEmail.Visible = false;
            btnEdit.Visible = true;
            btnSave.Visible = false;
            btnCancel.Visible = false;
        }
    }
}
