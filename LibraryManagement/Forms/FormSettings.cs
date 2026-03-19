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
        private SystemSettingDAO settingDAO = new SystemSettingDAO();

        public FormSettings()
        {
            InitializeComponent();
            this.Load += FormSettings_Load;
        }

        private void FormSettings_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                UpdateConnectionInfo();
                LoadSettings();
                LoadLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void UpdateConnectionInfo()
        {
            string connString;
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                connString = "Design Mode Connection String...";
            }
            else
            {
                connString = DatabaseConnection.ConnectionString;
            }

            lblConnInfo.Text = $"Connection String:\n{(connString.Length > 60 ? connString.Substring(0, 60) + "..." : connString)}";
        }

        private void BtnTestConn_Click(object? sender, EventArgs e)
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
        }

        private void BtnClearLogs_Click(object? sender, EventArgs e)
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
        }

        private void BtnRefreshLogs_Click(object? sender, EventArgs e)
        {
            LoadLogs();
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
