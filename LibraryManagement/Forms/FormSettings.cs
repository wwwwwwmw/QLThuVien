using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form cài đặt hệ thống
    /// </summary>
    public partial class FormSettings : Form
    {
        private TextBox txtLibraryName = null!;
        private TextBox txtAddress = null!;
        private TextBox txtPhone = null!;
        private TextBox txtEmail = null!;
        private NumericUpDown numBorrowDays = null!;
        private NumericUpDown numMaxBooks = null!;
        private NumericUpDown numFinePerDay = null!;
        private DataGridView dgvLogs = null!;

        private SystemSettingDAO settingDAO = new SystemSettingDAO();

        public FormSettings()
        {
            InitializeComponent();
            SetupForm();
            this.Load += FormSettings_Load;
        }

        private void FormSettings_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            LoadSettings();
            LoadLogs();
        }


        private void SetupForm()
        {
            // Title
            var lblTitle = new Label
            {
                Text = "⚙️ CÀI ĐẶT HỆ THỐNG",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Settings panel
            var panelSettings = new Panel
            {
                Location = new Point(20, 55),
                Size = new Size(550, 480),
                BackColor = Color.White
            };

            // Library Info Section
            var lblLibInfo = new Label
            {
                Text = "📚 THÔNG TIN THƯ VIỆN",
                Location = new Point(20, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            panelSettings.Controls.Add(lblLibInfo);

            var lblName = new Label { Text = "Tên thư viện:", Location = new Point(20, 50), AutoSize = true };
            txtLibraryName = new TextBox
            {
                Location = new Point(140, 47),
                Size = new Size(380, 28),
                Font = new Font("Segoe UI", 10)
            };

            var lblAddress = new Label { Text = "Địa chỉ:", Location = new Point(20, 85), AutoSize = true };
            txtAddress = new TextBox
            {
                Location = new Point(140, 82),
                Size = new Size(380, 28),
                Font = new Font("Segoe UI", 10)
            };

            var lblPhone = new Label { Text = "Điện thoại:", Location = new Point(20, 120), AutoSize = true };
            txtPhone = new TextBox
            {
                Location = new Point(140, 117),
                Size = new Size(180, 28),
                Font = new Font("Segoe UI", 10)
            };

            var lblEmail = new Label { Text = "Email:", Location = new Point(340, 120), AutoSize = true };
            txtEmail = new TextBox
            {
                Location = new Point(390, 117),
                Size = new Size(130, 28),
                Font = new Font("Segoe UI", 10)
            };

            panelSettings.Controls.AddRange(new Control[] {
                lblName, txtLibraryName, lblAddress, txtAddress, lblPhone, txtPhone, lblEmail, txtEmail
            });

            // Rules Section
            var lblRules = new Label
            {
                Text = "QUY ĐỊNH MƯỢN TRẢ",
                Location = new Point(20, 170),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            panelSettings.Controls.Add(lblRules);

            var lblBorrowDays = new Label { Text = "Số ngày mượn tối đa:", Location = new Point(20, 205), AutoSize = true };
            numBorrowDays = new NumericUpDown
            {
                Location = new Point(180, 202),
                Size = new Size(80, 28),
                Minimum = 1,
                Maximum = 60,
                Value = 14
            };
            var lblDaysUnit = new Label { Text = "ngày", Location = new Point(270, 205), AutoSize = true };

            var lblMaxBooks = new Label { Text = "Số sách mượn tối đa:", Location = new Point(20, 240), AutoSize = true };
            numMaxBooks = new NumericUpDown
            {
                Location = new Point(180, 237),
                Size = new Size(80, 28),
                Minimum = 1,
                Maximum = 20,
                Value = 5
            };
            var lblBooksUnit = new Label { Text = "quyển/lần", Location = new Point(270, 240), AutoSize = true };

            var lblFine = new Label { Text = "Tiền phạt quá hạn:", Location = new Point(20, 275), AutoSize = true };
            numFinePerDay = new NumericUpDown
            {
                Location = new Point(180, 272),
                Size = new Size(100, 28),
                Minimum = 0,
                Maximum = 100000,
                Increment = 1000,
                Value = 5000
            };
            var lblFineUnit = new Label { Text = "VNĐ/ngày", Location = new Point(290, 275), AutoSize = true };

            panelSettings.Controls.AddRange(new Control[] {
                lblBorrowDays, numBorrowDays, lblDaysUnit,
                lblMaxBooks, numMaxBooks, lblBooksUnit,
                lblFine, numFinePerDay, lblFineUnit
            });

            // Connection Section
            var lblConnection = new Label
            {
                Text = "KẾT NỐI MẠNG LAN",
                Location = new Point(20, 330),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            panelSettings.Controls.Add(lblConnection);

            var connString = DatabaseConnection.ConnectionString;
            var lblConnInfo = new Label
            {
                Text = $"Connection String:\n{(connString.Length > 60 ? connString.Substring(0, 60) + "..." : connString)}",
                Location = new Point(20, 360),
                Size = new Size(500, 40),
                Font = new Font("Segoe UI", 9)
            };
            panelSettings.Controls.Add(lblConnInfo);

            var btnTestConn = new Button
            {
                Text = "Test kết nối",
                Location = new Point(20, 405),
                Size = new Size(110, 32),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnTestConn.FlatAppearance.BorderSize = 0;
            btnTestConn.Click += (s, e) =>
            {
                try
                {
                    bool success = DatabaseConnection.TestConnection(out string message);
                    MessageBox.Show(success ? "Kết nối thành công!" : message, success ? "Thành công" : "Lỗi",
                        MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            panelSettings.Controls.Add(btnTestConn);

            // Save button
            var btnSave = new Button
            {
                Text = "Lưu cài đặt",
                Location = new Point(20, 440),
                Size = new Size(130, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            panelSettings.Controls.Add(btnSave);

            this.Controls.Add(panelSettings);

            // Activity Logs Panel
            var panelLogs = new Panel
            {
                Location = new Point(590, 55),
                Size = new Size(600, 480),
                BackColor = Color.White
            };

            var lblLogs = new Label
            {
                Text = "NHẬT KÝ HOẠT ĐỘNG",
                Location = new Point(15, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            panelLogs.Controls.Add(lblLogs);

            dgvLogs = new DataGridView
            {
                Location = new Point(15, 50),
                Size = new Size(570, 380),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgvLogs.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            dgvLogs.Columns.Add("LogTime", "Thời gian");
            dgvLogs.Columns.Add("Username", "Người dùng");
            dgvLogs.Columns.Add("Action", "Hoạt động");

            dgvLogs.Columns["LogTime"]!.Width = 130;
            dgvLogs.Columns["Username"]!.Width = 100;
            dgvLogs.Columns["Action"]!.Width = 320;

            panelLogs.Controls.Add(dgvLogs);

            var btnClearLogs = new Button
            {
                Text = "🗑 Xóa nhật ký cũ",
                Location = new Point(15, 440),
                Size = new Size(130, 30),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClearLogs.FlatAppearance.BorderSize = 0;
            btnClearLogs.Click += (s, e) =>
            {
                if (CurrentUser.User?.Role != User.ROLE_ADMIN)
                {
                    MessageBox.Show("Chỉ Admin mới có quyền xóa nhật ký!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Xóa tất cả nhật ký hoạt động cũ hơn 30 ngày?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    var logDAO = new ActivityLogDAO();
                    logDAO.ClearOldLogs(30);
                    LoadLogs();
                    MessageBox.Show("Đã xóa nhật ký cũ!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
            panelLogs.Controls.Add(btnClearLogs);

            var btnRefreshLogs = new Button
            {
                Text = "🔄 Làm mới",
                Location = new Point(155, 440),
                Size = new Size(100, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefreshLogs.FlatAppearance.BorderSize = 0;
            btnRefreshLogs.Click += (s, e) => LoadLogs();
            panelLogs.Controls.Add(btnRefreshLogs);

            this.Controls.Add(panelLogs);
        }

        private void LoadSettings()
        {
            try
            {
                txtLibraryName.Text = settingDAO.GetValue(SystemSetting.KEY_LIBRARY_NAME, "Thư viện");
                txtAddress.Text = settingDAO.GetValue(SystemSetting.KEY_LIBRARY_ADDRESS, "");
                txtPhone.Text = settingDAO.GetValue(SystemSetting.KEY_LIBRARY_PHONE, "");
                txtEmail.Text = settingDAO.GetValue(SystemSetting.KEY_LIBRARY_EMAIL, "");

                numBorrowDays.Value = settingDAO.GetIntValue(SystemSetting.KEY_MAX_BORROW_DAYS, 14);
                numMaxBooks.Value = settingDAO.GetIntValue(SystemSetting.KEY_MAX_BOOKS_PER_BORROW, 5);
                numFinePerDay.Value = settingDAO.GetDecimalValue(SystemSetting.KEY_FINE_PER_DAY, 5000);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải cài đặt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLogs()
        {
            try
            {
                var logDAO = new ActivityLogDAO();
                var logs = logDAO.GetRecentLogs(100);

                dgvLogs.Rows.Clear();
                foreach (var log in logs)
                {
                    dgvLogs.Rows.Add(
                        log.LogTime.ToString("dd/MM HH:mm:ss"),
                        log.Username,
                        log.Action
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải nhật ký: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLibraryName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên thư viện!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtLibraryName.Focus();
                return;
            }

            try
            {
                settingDAO.SaveValue(SystemSetting.KEY_LIBRARY_NAME, txtLibraryName.Text.Trim());
                settingDAO.SaveValue(SystemSetting.KEY_LIBRARY_ADDRESS, txtAddress.Text.Trim());
                settingDAO.SaveValue(SystemSetting.KEY_LIBRARY_PHONE, txtPhone.Text.Trim());
                settingDAO.SaveValue(SystemSetting.KEY_LIBRARY_EMAIL, txtEmail.Text.Trim());

                settingDAO.SaveValue(SystemSetting.KEY_MAX_BORROW_DAYS, numBorrowDays.Value.ToString());
                settingDAO.SaveValue(SystemSetting.KEY_MAX_BOOKS_PER_BORROW, numMaxBooks.Value.ToString());
                settingDAO.SaveValue(SystemSetting.KEY_FINE_PER_DAY, numFinePerDay.Value.ToString());

                // Log
                var logDAO = new ActivityLogDAO();
                logDAO.Log("Cập nhật cài đặt hệ thống", "SystemSettings", 0);

                MessageBox.Show("Lưu cài đặt thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu cài đặt: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
