using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form quản lý người dùng (chỉ dành cho Admin)
    /// </summary>
    public partial class FormUserManagement : Form
    {
        private UserDAO userDAO = new UserDAO();
        private User? selectedUser;

        public FormUserManagement()
        {
            InitializeComponent();
            this.Load += FormUserManagement_Load;
        }

        private void FormUserManagement_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            // Check permission at runtime
            if (CurrentUser.User?.Role != User.ROLE_ADMIN)
            {
                this.Controls.Clear();
                var lblNoAccess = new Label
                {
                    Text = "⛔ Bạn không có quyền truy cập chức năng này!\n\nChỉ Admin mới có thể quản lý người dùng.",
                    Location = new Point(300, 200),
                    AutoSize = true,
                    Font = new Font("Segoe UI", 14),
                    ForeColor = Color.FromArgb(231, 76, 60)
                };
                this.Controls.Add(lblNoAccess);
                return;
            }

            try
            {
                cboRole.SelectedIndex = 0;
                SetFormEnabled(false);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            SearchUsers();
        }

        private void CboRole_SelectedIndexChanged(object? sender, EventArgs e)
        {
            SearchUsers();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            AddNew();
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadData();
        }

        private void DgvUsers_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            EditUser();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            CancelEdit();
        }

        private void LoadData()
        {
            SearchUsers();
        }

        private void SearchUsers()
        {
            try
            {
                string? keyword = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim();
                string? role = cboRole.SelectedIndex > 0 ? cboRole.SelectedItem?.ToString() : null;

                var users = userDAO.Search(keyword, role);

                dgvUsers.Rows.Clear();
                foreach (var user in users)
                {
                    var row = dgvUsers.Rows.Add(
                        user.UserID,
                        user.Username,
                        user.FullName,
                        user.Email,
                        user.Role,
                        user.IsActive ? "Hoạt động" : "Khóa",
                        user.LastLogin?.ToString("dd/MM HH:mm") ?? "-"
                    );

                    if (!user.IsActive)
                    {
                        dgvUsers.Rows[row].DefaultCellStyle.ForeColor = Color.Gray;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvUsers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow == null)
            {
                selectedUser = null;
                ClearForm();
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.CurrentRow.Cells["UserID"].Value);
            selectedUser = userDAO.GetById(userId);

            if (selectedUser != null)
            {
                DisplayUser(selectedUser);
            }
        }

        private void DisplayUser(User user)
        {
            txtUsername.Text = user.Username;
            txtFullName.Text = user.FullName;
            txtEmail.Text = user.Email;
            txtPhone.Text = user.Phone;
            cboUserRole.SelectedItem = user.Role;
            chkActive.Checked = user.IsActive;
            txtNewPassword.Text = "";

            txtUsername.Enabled = false; // Can't change username
        }

        private void AddNew()
        {
            selectedUser = null;
            ClearForm();
            SetFormEnabled(true);
            txtUsername.Enabled = true;
            txtUsername.Focus();
        }

        private void EditUser()
        {
            if (selectedUser == null) return;

            SetFormEnabled(true);
            txtUsername.Enabled = false;
            txtFullName.Focus();
        }

        private void CancelEdit()
        {
            if (selectedUser != null)
            {
                DisplayUser(selectedUser);
            }
            else
            {
                ClearForm();
            }
            SetFormEnabled(false);
        }

        private void ClearForm()
        {
            txtUsername.Text = "";
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            cboUserRole.SelectedIndex = 2;
            chkActive.Checked = true;
            txtNewPassword.Text = "";
        }

        private void SetFormEnabled(bool enabled)
        {
            txtUsername.Enabled = enabled;
            txtFullName.Enabled = enabled;
            txtEmail.Enabled = enabled;
            txtPhone.Enabled = enabled;
            cboUserRole.Enabled = enabled;
            chkActive.Enabled = enabled;
            txtNewPassword.Enabled = enabled;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            if (selectedUser == null && string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu cho tài khoản mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNewPassword.Focus();
                return;
            }

            try
            {
                if (selectedUser == null)
                {
                    // Add new
                    var newUser = new User
                    {
                        Username = txtUsername.Text.Trim(),
                        FullName = txtFullName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Phone = txtPhone.Text.Trim(),
                        Role = cboUserRole.SelectedItem?.ToString() ?? User.ROLE_STAFF,
                        IsActive = chkActive.Checked
                    };

                    var (success, message) = userDAO.Add(newUser, txtNewPassword.Text);

                    if (success)
                    {
                        var logDAO = new ActivityLogDAO();
                        logDAO.Log($"Thêm người dùng mới: {newUser.Username}", "Users", 0);

                        MessageBox.Show("Thêm người dùng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        CancelEdit();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Update
                    selectedUser.FullName = txtFullName.Text.Trim();
                    selectedUser.Email = txtEmail.Text.Trim();
                    selectedUser.Phone = txtPhone.Text.Trim();
                    selectedUser.Role = cboUserRole.SelectedItem?.ToString() ?? User.ROLE_STAFF;
                    selectedUser.IsActive = chkActive.Checked;

                    var (success, message) = userDAO.Update(selectedUser);

                    if (success)
                    {
                        // Update password if provided
                        if (!string.IsNullOrWhiteSpace(txtNewPassword.Text))
                        {
                            userDAO.ChangePassword(selectedUser.UserID, txtNewPassword.Text);
                        }

                        var logDAO = new ActivityLogDAO();
                        logDAO.Log($"Cập nhật người dùng: {selectedUser.Username}", "Users", selectedUser.UserID);

                        MessageBox.Show("Cập nhật thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                        CancelEdit();
                    }
                    else
                    {
                        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedUser.UserID == CurrentUser.User?.UserID)
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!", "Từ chối", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc muốn xóa người dùng '{selectedUser.Username}'?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                var (success, message) = userDAO.Delete(selectedUser.UserID);

                if (success)
                {
                    var logDAO = new ActivityLogDAO();
                    logDAO.Log($"Xóa người dùng: {selectedUser.Username}", "Users", selectedUser.UserID);

                    MessageBox.Show("Xóa người dùng thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    CancelEdit();
                }
                else
                {
                    MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnResetPassword_Click(object? sender, EventArgs e)
        {
            if (selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Reset mật khẩu về mặc định (123456) cho user '{selectedUser.Username}'?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                userDAO.ChangePassword(selectedUser.UserID, "123456");
                MessageBox.Show("Đã reset mật khẩu thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
