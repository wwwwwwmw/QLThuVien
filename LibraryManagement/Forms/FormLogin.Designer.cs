using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormLogin
    {
        private System.ComponentModel.IContainer components = null;

        private Panel mainPanel;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblUsername;
        private Label lblPassword;

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;
        private Button btnConfig;
        private Label lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            mainPanel = new Panel();
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblUsername = new Label();
            lblPassword = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnExit = new Button();
            btnConfig = new Button();
            lblStatus = new Label();
            mainPanel.SuspendLayout();
            SuspendLayout();

            // mainPanel
            mainPanel.BackColor = Color.FromArgb(240, 244, 247);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(lblTitle);
            mainPanel.Controls.Add(lblSubtitle);
            mainPanel.Controls.Add(lblUsername);
            mainPanel.Controls.Add(txtUsername);
            mainPanel.Controls.Add(lblPassword);
            mainPanel.Controls.Add(txtPassword);
            mainPanel.Controls.Add(lblStatus);
            mainPanel.Controls.Add(btnLogin);
            mainPanel.Controls.Add(btnExit);
            mainPanel.Controls.Add(btnConfig);
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(450, 350);
            mainPanel.TabIndex = 0;

            // lblTitle
            lblTitle.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(41, 128, 185);
            lblTitle.Location = new Point(0, 30);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(450, 50);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "📚 QUẢN LÝ THƯ VIỆN";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblSubtitle
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.Location = new Point(0, 80);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(450, 25);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Hệ thống quản lý thư viện đa máy LAN";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // lblUsername
            lblUsername.Font = new Font("Segoe UI", 10F);
            lblUsername.Location = new Point(75, 130);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(120, 25);
            lblUsername.TabIndex = 2;
            lblUsername.Text = "Tên đăng nhập:";

            // txtUsername
            txtUsername.Font = new Font("Segoe UI", 11F);
            txtUsername.Location = new Point(75, 155);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderText = "Nhập tên đăng nhập";
            txtUsername.Size = new Size(300, 32);
            txtUsername.TabIndex = 3;
            txtUsername.KeyPress += TxtUsername_KeyPress;

            // lblPassword
            lblPassword.Font = new Font("Segoe UI", 10F);
            lblPassword.Location = new Point(75, 195);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(120, 25);
            lblPassword.TabIndex = 4;
            lblPassword.Text = "Mật khẩu:";

            // txtPassword
            txtPassword.Font = new Font("Segoe UI", 11F);
            txtPassword.Location = new Point(75, 220);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.PlaceholderText = "Nhập mật khẩu";
            txtPassword.Size = new Size(300, 32);
            txtPassword.TabIndex = 5;
            txtPassword.KeyPress += TxtPassword_KeyPress;

            // lblStatus
            lblStatus.Font = new Font("Segoe UI", 9F);
            lblStatus.ForeColor = Color.Red;
            lblStatus.Location = new Point(75, 255);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(300, 25);
            lblStatus.TabIndex = 6;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;

            // btnLogin
            btnLogin.BackColor = Color.FromArgb(41, 128, 185);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(75, 285);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(145, 40);
            btnLogin.TabIndex = 7;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += BtnLogin_Click;

            // btnExit
            btnExit.BackColor = Color.FromArgb(149, 165, 166);
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Segoe UI", 11F);
            btnExit.ForeColor = Color.White;
            btnExit.Location = new Point(230, 285);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(145, 40);
            btnExit.TabIndex = 8;
            btnExit.Text = "Thoát";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += BtnExit_Click;

            // btnConfig
            btnConfig.BackColor = Color.Transparent;
            btnConfig.FlatAppearance.BorderSize = 0;
            btnConfig.FlatStyle = FlatStyle.Flat;
            btnConfig.Font = new Font("Segoe UI", 12F);
            btnConfig.ForeColor = Color.Gray;
            btnConfig.Location = new Point(410, 10);
            btnConfig.Name = "btnConfig";
            btnConfig.Size = new Size(30, 30);
            btnConfig.TabIndex = 9;
            btnConfig.Text = "⚙";
            btnConfig.UseVisualStyleBackColor = false;
            btnConfig.Click += BtnConfig_Click;

            // FormLogin
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(450, 350);
            Controls.Add(mainPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng nhập - Quản lý Thư viện";

            mainPanel.ResumeLayout(false);
            mainPanel.PerformLayout();
            ResumeLayout(false);
        }
    }
}