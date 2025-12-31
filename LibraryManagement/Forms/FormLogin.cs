using System;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form đăng nhập hệ thống
    /// </summary>
    public partial class FormLogin : Form
    {
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;
        private Button btnExit = null!;
        private Button btnConfig = null!;
        private Label lblStatus = null!;
        private int loginAttempts = 0;
        private const int MAX_ATTEMPTS = 3;

        public FormLogin()
        {
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(450, 350);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập - Quản lý Thư viện";
            this.ResumeLayout(false);
        }

        private void SetupForm()
        {
            // Panel chính
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 244, 247)
            };

            // Logo/Title
            Label lblTitle = new Label
            {
                Text = "📚 QUẢN LÝ THƯ VIỆN",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 30),
                Size = new Size(450, 50),
                AutoSize = false
            };

            Label lblSubtitle = new Label
            {
                Text = "Hệ thống quản lý thư viện đa máy LAN",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 80),
                Size = new Size(450, 25),
                AutoSize = false
            };

            // Username
            Label lblUsername = new Label
            {
                Text = "Tên đăng nhập:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(75, 130),
                Size = new Size(120, 25)
            };

            txtUsername = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(75, 155),
                Size = new Size(300, 30),
                PlaceholderText = "Nhập tên đăng nhập"
            };

            // Password
            Label lblPassword = new Label
            {
                Text = "Mật khẩu:",
                Font = new Font("Segoe UI", 10),
                Location = new Point(75, 195),
                Size = new Size(120, 25)
            };

            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Location = new Point(75, 220),
                Size = new Size(300, 30),
                PasswordChar = '●',
                PlaceholderText = "Nhập mật khẩu"
            };

            // Status label
            lblStatus = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Red,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(75, 255),
                Size = new Size(300, 25),
                AutoSize = false
            };

            // Buttons
            btnLogin = new Button
            {
                Text = "Đăng nhập",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(75, 285),
                Size = new Size(145, 40),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            btnExit = new Button
            {
                Text = "Thoát",
                Font = new Font("Segoe UI", 11),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(230, 285),
                Size = new Size(145, 40),
                Cursor = Cursors.Hand
            };
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Click += (s, e) => this.Close();

            // Config button (nhỏ góc phải)
            btnConfig = new Button
            {
                Text = "⚙",
                Font = new Font("Segoe UI", 12),
                BackColor = Color.Transparent,
                ForeColor = Color.Gray,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(410, 10),
                Size = new Size(30, 30),
                Cursor = Cursors.Hand
            };
            btnConfig.FlatAppearance.BorderSize = 0;
            btnConfig.Click += BtnConfig_Click;

            // Add controls
            mainPanel.Controls.AddRange(new Control[] {
                lblTitle, lblSubtitle, lblUsername, txtUsername,
                lblPassword, txtPassword, lblStatus,
                btnLogin, btnExit, btnConfig
            });

            this.Controls.Add(mainPanel);

            // Key events
            txtPassword.KeyPress += (s, e) => { if (e.KeyChar == (char)Keys.Enter) BtnLogin_Click(s, e); };
            txtUsername.KeyPress += (s, e) => { if (e.KeyChar == (char)Keys.Enter) txtPassword.Focus(); };

            // Test connection on load
            this.Load += FormLogin_Load;
        }

        private void FormLogin_Load(object? sender, EventArgs e)
        {
            // Test database connection
            if (!DatabaseConnection.TestConnection(out string error))
            {
                lblStatus.Text = "⚠ Không thể kết nối CSDL. Kiểm tra cấu hình.";
                lblStatus.ForeColor = Color.Orange;
            }
            else
            {
                lblStatus.Text = "✓ Đã kết nối đến máy chủ";
                lblStatus.ForeColor = Color.Green;
            }

            txtUsername.Focus();
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                ShowError("Vui lòng nhập tên đăng nhập!");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                ShowError("Vui lòng nhập mật khẩu!");
                txtPassword.Focus();
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                lblStatus.Text = "Đang đăng nhập...";
                lblStatus.ForeColor = Color.Blue;
                Application.DoEvents();

                var userDAO = new UserDAO();
                var user = userDAO.Login(txtUsername.Text.Trim(), txtPassword.Text);

                if (user != null)
                {
                    // Login success
                    CurrentUser.Login(user);

                    // Log activity
                    var logDAO = new ActivityLogDAO();
                    logDAO.Log("Đăng nhập hệ thống", "Users", user.UserID);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    loginAttempts++;

                    if (loginAttempts >= MAX_ATTEMPTS)
                    {
                        ShowError($"Đăng nhập thất bại {MAX_ATTEMPTS} lần. Ứng dụng sẽ đóng.");
                        this.Close();
                    }
                    else
                    {
                        ShowError($"Sai tên đăng nhập hoặc mật khẩu! (Còn {MAX_ATTEMPTS - loginAttempts} lần thử)");
                        txtPassword.Clear();
                        txtPassword.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi kết nối: {ex.Message}");
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        private void BtnConfig_Click(object? sender, EventArgs e)
        {
            using (var configForm = new FormConnectionConfig())
            {
                if (configForm.ShowDialog() == DialogResult.OK)
                {
                    // Reload connection status
                    FormLogin_Load(sender, e);
                }
            }
        }

        private void ShowError(string message)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.Red;
        }
    }

    /// <summary>
    /// Form cấu hình kết nối SQL Server
    /// </summary>
    public class FormConnectionConfig : Form
    {
        private TextBox txtServer = null!;
        private TextBox txtDatabase = null!;
        private RadioButton rbWindowsAuth = null!;
        private RadioButton rbSqlAuth = null!;
        private TextBox txtUserId = null!;
        private TextBox txtPassword = null!;
        private Button btnTest = null!;
        private Button btnSave = null!;
        private Label lblStatus = null!;

        public FormConnectionConfig()
        {
            InitializeComponent();
            SetupForm();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(450, 380);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Cấu hình kết nối SQL Server";
            this.ResumeLayout(false);
        }

        private void SetupForm()
        {
            int y = 20;
            int labelWidth = 150;
            int inputWidth = 250;
            int leftMargin = 20;

            // Server
            AddLabel("Máy chủ SQL Server:", leftMargin, y);
            txtServer = AddTextBox(leftMargin + labelWidth, y, inputWidth);
            txtServer.Text = ".\\SQLEXPRESS";
            txtServer.PlaceholderText = "Ví dụ: 192.168.1.100\\SQLEXPRESS";
            y += 40;

            // Database
            AddLabel("Tên Database:", leftMargin, y);
            txtDatabase = AddTextBox(leftMargin + labelWidth, y, inputWidth);
            txtDatabase.Text = "LibraryManagement";
            y += 40;

            // Authentication type
            AddLabel("Phương thức xác thực:", leftMargin, y);
            y += 25;

            rbWindowsAuth = new RadioButton
            {
                Text = "Windows Authentication (Dùng tài khoản Windows)",
                Location = new Point(leftMargin + 20, y),
                Size = new Size(400, 25),
                Checked = true
            };
            rbWindowsAuth.CheckedChanged += AuthType_Changed;
            this.Controls.Add(rbWindowsAuth);
            y += 30;

            rbSqlAuth = new RadioButton
            {
                Text = "SQL Server Authentication (Dùng User/Password)",
                Location = new Point(leftMargin + 20, y),
                Size = new Size(400, 25)
            };
            rbSqlAuth.CheckedChanged += AuthType_Changed;
            this.Controls.Add(rbSqlAuth);
            y += 40;

            // SQL Auth credentials
            AddLabel("User ID:", leftMargin, y);
            txtUserId = AddTextBox(leftMargin + labelWidth, y, inputWidth);
            txtUserId.Text = "sa";
            txtUserId.Enabled = false;
            y += 40;

            AddLabel("Password:", leftMargin, y);
            txtPassword = AddTextBox(leftMargin + labelWidth, y, inputWidth);
            txtPassword.PasswordChar = '●';
            txtPassword.Enabled = false;
            y += 50;

            // Status
            lblStatus = new Label
            {
                Location = new Point(leftMargin, y),
                Size = new Size(400, 25),
                ForeColor = Color.Gray
            };
            this.Controls.Add(lblStatus);
            y += 35;

            // Buttons
            btnTest = new Button
            {
                Text = "Kiểm tra kết nối",
                Location = new Point(leftMargin, y),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTest.FlatAppearance.BorderSize = 0;
            btnTest.Click += BtnTest_Click;
            this.Controls.Add(btnTest);

            btnSave = new Button
            {
                Text = "Lưu & Đóng",
                Location = new Point(leftMargin + 140, y),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Enabled = false
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            var btnCancel = new Button
            {
                Text = "Hủy",
                Location = new Point(leftMargin + 280, y),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);

            // Info label
            var lblInfo = new Label
            {
                Text = "💡 Đối với mạng LAN, nhập địa chỉ IP của máy chủ SQL Server\n" +
                       "Ví dụ: 192.168.1.100 hoặc 192.168.1.100\\SQLEXPRESS",
                Location = new Point(leftMargin, y + 50),
                Size = new Size(400, 50),
                ForeColor = Color.FromArgb(41, 128, 185),
                Font = new Font("Segoe UI", 9)
            };
            this.Controls.Add(lblInfo);
        }

        private Label AddLabel(string text, int x, int y)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(x, y + 3),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(label);
            return label;
        }

        private TextBox AddTextBox(int x, int y, int width)
        {
            var textBox = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 28),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(textBox);
            return textBox;
        }

        private void AuthType_Changed(object? sender, EventArgs e)
        {
            bool sqlAuth = rbSqlAuth.Checked;
            txtUserId.Enabled = sqlAuth;
            txtPassword.Enabled = sqlAuth;
            btnSave.Enabled = false;
        }

        private void BtnTest_Click(object? sender, EventArgs e)
        {
            try
            {
                lblStatus.Text = "Đang kiểm tra kết nối...";
                lblStatus.ForeColor = Color.Blue;
                Application.DoEvents();

                string connStr = BuildConnectionString();

                if (DatabaseConnection.TestConnection(connStr, out string error))
                {
                    lblStatus.Text = "✓ Kết nối thành công!";
                    lblStatus.ForeColor = Color.Green;
                    btnSave.Enabled = true;
                }
                else
                {
                    lblStatus.Text = $"✗ Lỗi: {error}";
                    lblStatus.ForeColor = Color.Red;
                    btnSave.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"✗ Lỗi: {ex.Message}";
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            DatabaseConnection.UpdateConnectionString(
                txtServer.Text.Trim(),
                txtDatabase.Text.Trim(),
                rbWindowsAuth.Checked,
                txtUserId.Text.Trim(),
                txtPassword.Text
            );

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private string BuildConnectionString()
        {
            var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder
            {
                DataSource = txtServer.Text.Trim(),
                InitialCatalog = txtDatabase.Text.Trim(),
                IntegratedSecurity = rbWindowsAuth.Checked,
                TrustServerCertificate = true,
                MultipleActiveResultSets = true
            };

            if (rbSqlAuth.Checked)
            {
                builder.UserID = txtUserId.Text.Trim();
                builder.Password = txtPassword.Text;
            }

            return builder.ConnectionString;
        }
    }
}
