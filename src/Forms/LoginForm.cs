using System;
using System.Drawing;
using System.Windows.Forms;

namespace SmartAssignmentTracker
{
    public class LoginForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;

        public LoginForm()
        {
            Text = "Smart Assignment Tracker - Login";
            Size = new Size(400, 300);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            var lblTitle = new Label
            {
                Text = "LOGIN",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 50
            };

            var lblEmail = new Label
            {
                Text = "Email",
                Location = new Point(80, 70),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            txtEmail = new TextBox
            {
                Location = new Point(80, 95),
                Size = new Size(220, 30),
                Font = new Font("Segoe UI", 10)
            };

            var lblPassword = new Label
            {
                Text = "Password",
                Location = new Point(80, 130),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            txtPassword = new TextBox
            {
                Location = new Point(80, 155),
                Size = new Size(220, 30),
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true
            };

            btnLogin = new Button
            {
                Text = "Login",
                Location = new Point(80, 200),
                Size = new Size(105, 35),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnLogin.Click += BtnLogin_Click;

            btnRegister = new Button
            {
                Text = "Register",
                Location = new Point(195, 200),
                Size = new Size(105, 35),
                Font = new Font("Segoe UI", 10),
                FlatStyle = FlatStyle.Flat
            };
            btnRegister.Click += BtnRegister_Click;

            Controls.AddRange(new Control[] { lblTitle, lblEmail, txtEmail, lblPassword, txtPassword, btnLogin, btnRegister });
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter both email and password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Student student = new Student(1, "Ron Swanson", txtEmail.Text);
            var mainForm = new MainForm(student);
            mainForm.FormClosed += (s, args) => Close();
            mainForm.Show();
            Hide();
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Registration is coming soon.", "Register", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
