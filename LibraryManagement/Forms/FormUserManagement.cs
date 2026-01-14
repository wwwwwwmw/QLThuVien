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
        private TextBox txtSearch = null!;
        private ComboBox cboRole = null!;
        private DataGridView dgvUsers = null!;
        private Panel panelDetail = null!;

        private TextBox txtUsername = null!;
        private TextBox txtFullName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPhone = null!;
        private ComboBox cboUserRole = null!;
        private CheckBox chkActive = null!;
        private TextBox txtNewPassword = null!;

        private UserDAO userDAO = new UserDAO();
        private User? selectedUser;

        public FormUserManagement()
        {
            InitializeComponent();
            SetupForm();
            this.Load += FormUserManagement_Load;
        }

        private void FormUserManagement_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            LoadData();
        }


        private void SetupForm()
        {
            // Check permission
            if (CurrentUser.User?.Role != User.ROLE_ADMIN)
            {
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

            // Title
            var lblTitle = new Label
            {
                Text = "👥 QUẢN LÝ NGƯỜI DÙNG",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Search panel
            var panelSearch = new Panel
            {
                Location = new Point(20, 55),
                Size = new Size(750, 55),
                BackColor = Color.White
            };

            var lblSearch = new Label { Text = "Tìm kiếm:", Location = new Point(15, 17), AutoSize = true };
            txtSearch = new TextBox
            {
                Location = new Point(80, 14),
                Size = new Size(200, 28),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Tên đăng nhập, họ tên..."
            };
            txtSearch.TextChanged += (s, e) => SearchUsers();

            var lblRole = new Label { Text = "Vai trò:", Location = new Point(300, 17), AutoSize = true };
            cboRole = new ComboBox
            {
                Location = new Point(360, 14),
                Size = new Size(120, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboRole.Items.AddRange(new object[] { "-- Tất cả --", User.ROLE_ADMIN, User.ROLE_MANAGER, User.ROLE_STAFF });
            cboRole.SelectedIndex = 0;
            cboRole.SelectedIndexChanged += (s, e) => SearchUsers();

            var btnAdd = new Button
            {
                Text = "➕ Thêm mới",
                Location = new Point(500, 12),
                Size = new Size(100, 32),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => AddNew();

            var btnRefresh = new Button
            {
                Text = "🔄",
                Location = new Point(610, 12),
                Size = new Size(40, 32),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();

            panelSearch.Controls.AddRange(new Control[] { lblSearch, txtSearch, lblRole, cboRole, btnAdd, btnRefresh });
            this.Controls.Add(panelSearch);

            // DataGridView
            dgvUsers = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(750, 400),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvUsers.SelectionChanged += DgvUsers_SelectionChanged;
            dgvUsers.CellDoubleClick += (s, e) => EditUser();

            dgvUsers.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            dgvUsers.Columns.Add("UserID", "ID");
            dgvUsers.Columns.Add("Username", "Tên đăng nhập");
            dgvUsers.Columns.Add("FullName", "Họ tên");
            dgvUsers.Columns.Add("Email", "Email");
            dgvUsers.Columns.Add("Role", "Vai trò");
            dgvUsers.Columns.Add("Status", "Trạng thái");
            dgvUsers.Columns.Add("LastLogin", "Đăng nhập cuối");

            dgvUsers.Columns["UserID"]!.Visible = false;
            dgvUsers.Columns["Username"]!.Width = 120;
            dgvUsers.Columns["FullName"]!.Width = 180;
            dgvUsers.Columns["Email"]!.Width = 180;
            dgvUsers.Columns["Role"]!.Width = 80;
            dgvUsers.Columns["Status"]!.Width = 80;
            dgvUsers.Columns["LastLogin"]!.Width = 130;

            this.Controls.Add(dgvUsers);

            // Detail panel
            panelDetail = new Panel
            {
                Location = new Point(790, 55),
                Size = new Size(400, 470),
                BackColor = Color.White
            };

            var lblDetail = new Label
            {
                Text = "CHI TIẾT NGƯỜI DÙNG",
                Location = new Point(15, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            panelDetail.Controls.Add(lblDetail);

            // Form fields
            var lblUsername = new Label { Text = "Tên đăng nhập:", Location = new Point(20, 55), AutoSize = true };
            txtUsername = new TextBox
            {
                Location = new Point(140, 52),
                Size = new Size(230, 28),
                Font = new Font("Segoe UI", 10)
            };

            var lblFullName = new Label { Text = "Họ tên:", Location = new Point(20, 90), AutoSize = true };
            txtFullName = new TextBox
            {
                Location = new Point(140, 87),
                Size = new Size(230, 28),
                Font = new Font("Segoe UI", 10)
            };

            var lblEmail2 = new Label { Text = "Email:", Location = new Point(20, 125), AutoSize = true };
            txtEmail = new TextBox
            {
                Location = new Point(140, 122),
                Size = new Size(230, 28),
                Font = new Font("Segoe UI", 10)
            };

            var lblPhone2 = new Label { Text = "Điện thoại:", Location = new Point(20, 160), AutoSize = true };
            txtPhone = new TextBox
            {
                Location = new Point(140, 157),
                Size = new Size(230, 28),
                Font = new Font("Segoe UI", 10)
            };

            var lblUserRole = new Label { Text = "Vai trò:", Location = new Point(20, 195), AutoSize = true };
            cboUserRole = new ComboBox
            {
                Location = new Point(140, 192),
                Size = new Size(230, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboUserRole.Items.AddRange(new object[] { User.ROLE_ADMIN, User.ROLE_MANAGER, User.ROLE_STAFF });
            cboUserRole.SelectedIndex = 2;

            chkActive = new CheckBox
            {
                Text = "Kích hoạt tài khoản",
                Location = new Point(140, 230),
                AutoSize = true,
                Checked = true
            };

            var lblPassword = new Label { Text = "Mật khẩu mới:", Location = new Point(20, 265), AutoSize = true };
            txtNewPassword = new TextBox
            {
                Location = new Point(140, 262),
                Size = new Size(230, 28),
                Font = new Font("Segoe UI", 10),
                UseSystemPasswordChar = true,
                PlaceholderText = "Để trống nếu không đổi"
            };

            panelDetail.Controls.AddRange(new Control[] {
                lblUsername, txtUsername, lblFullName, txtFullName,
                lblEmail2, txtEmail, lblPhone2, txtPhone,
                lblUserRole, cboUserRole, chkActive,
                lblPassword, txtNewPassword
            });

            // Buttons
            var btnSave = new Button
            {
                Text = "Lưu",
                Location = new Point(20, 310),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            var btnCancel = new Button
            {
                Text = "Hủy",
                Location = new Point(110, 310),
                Size = new Size(70, 35),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => CancelEdit();

            var btnDelete = new Button
            {
                Text = "Xóa",
                Location = new Point(190, 310),
                Size = new Size(70, 35),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += BtnDelete_Click;

            var btnResetPwd = new Button
            {
                Text = "Reset mật khẩu",
                Location = new Point(20, 355),
                Size = new Size(130, 32),
                BackColor = Color.FromArgb(243, 156, 18),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnResetPwd.FlatAppearance.BorderSize = 0;
            btnResetPwd.Click += BtnResetPassword_Click;

            panelDetail.Controls.AddRange(new Control[] { btnSave, btnCancel, btnDelete, btnResetPwd });
            this.Controls.Add(panelDetail);

            SetFormEnabled(false);
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

                    MessageBox.Show("Xóa thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                    ClearForm();
                    selectedUser = null;
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

            var result = MessageBox.Show($"Reset mật khẩu cho '{selectedUser.Username}' về '123456'?",
                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                var (success, message) = userDAO.ChangePassword(selectedUser.UserID, "123456");

                if (success)
                {
                    var logDAO = new ActivityLogDAO();
                    logDAO.Log($"Reset mật khẩu: {selectedUser.Username}", "Users", selectedUser.UserID);

                    MessageBox.Show("Reset mật khẩu thành công!\nMật khẩu mới: 123456", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
