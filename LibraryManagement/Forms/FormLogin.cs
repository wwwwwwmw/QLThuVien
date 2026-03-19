using System;
using System.ComponentModel;
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
        private int loginAttempts = 0;
        private const int MAX_ATTEMPTS = 3;

        public FormLogin()
        {
            InitializeComponent();
            Load += FormLogin_Load;
        }

        private void SetupForm()
        {
            // Giữ lại để không ảnh hưởng cấu trúc cũ,
            // nhưng không gọi nữa vì giao diện đã chuyển sang Designer.
        }

        private void FormLogin_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo: " + ex.Message);
            }

            txtUsername.Focus();
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
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
                    CurrentUser.Login(user);

                    var logDAO = new ActivityLogDAO();
                    logDAO.Log("Đăng nhập hệ thống", "Users", user.UserID);

                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    loginAttempts++;

                    if (loginAttempts >= MAX_ATTEMPTS)
                    {
                        ShowError($"Đăng nhập thất bại {MAX_ATTEMPTS} lần. Ứng dụng sẽ đóng.");
                        Close();
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
                    FormLogin_Load(sender, e);
                }
            }
        }

        private void BtnExit_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private void TxtUsername_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                txtPassword.Focus();
        }

        private void TxtPassword_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                BtnLogin_Click(sender, e);
        }

        private void ShowError(string message)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = Color.Red;
        }
    }
}