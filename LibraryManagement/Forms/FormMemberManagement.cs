using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form quản lý độc giả / thành viên
    /// </summary>
    public partial class FormMemberManagement : Form
    {
        private DataGridView dgvMembers = null!;
        private TextBox txtSearch = null!;
        private ComboBox cboMemberType = null!;
        private CheckBox chkActiveOnly = null!;

        // Detail fields
        private TextBox txtMemberCode = null!;
        private TextBox txtFullName = null!;
        private ComboBox cboGender = null!;
        private DateTimePicker dtpDateOfBirth = null!;
        private TextBox txtPhone = null!;
        private TextBox txtEmail = null!;
        private TextBox txtIdentityCard = null!;
        private TextBox txtAddress = null!;
        private ComboBox cboMemberTypeDetail = null!;
        private DateTimePicker dtpExpiryDate = null!;
        private TextBox txtNotes = null!;
        private Label lblTotalFine = null!;

        private MemberDAO memberDAO = new MemberDAO();
        private Member? currentMember;

        public FormMemberManagement()
        {
            InitializeComponent();
            SetupForm();
            this.Load += FormMemberManagement_Load;
        }

        private void FormMemberManagement_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            LoadData();
        }


        private void SetupForm()
        {
            // Title
            var lblTitle = new Label
            {
                Text = "👥 QUẢN LÝ ĐỘC GIẢ",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Search panel
            var panelSearch = new Panel
            {
                Location = new Point(20, 50),
                Size = new Size(800, 50),
                BackColor = Color.White
            };

            txtSearch = new TextBox
            {
                Location = new Point(10, 12),
                Size = new Size(250, 28),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Tìm theo mã, tên, SĐT..."
            };
            txtSearch.TextChanged += (s, e) => SearchMembers();

            var lblType = new Label { Text = "Loại thẻ:", Location = new Point(270, 15), AutoSize = true };
            cboMemberType = new ComboBox
            {
                Location = new Point(330, 12),
                Size = new Size(150, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboMemberType.Items.AddRange(new object[] { "-- Tất cả --", Member.TYPE_NORMAL, Member.TYPE_VIP, Member.TYPE_STUDENT, Member.TYPE_TEACHER });
            cboMemberType.SelectedIndex = 0;
            cboMemberType.SelectedIndexChanged += (s, e) => SearchMembers();

            chkActiveOnly = new CheckBox
            {
                Text = "Chỉ đang hoạt động",
                Location = new Point(500, 14),
                AutoSize = true,
                Checked = true
            };
            chkActiveOnly.CheckedChanged += (s, e) => SearchMembers();

            panelSearch.Controls.AddRange(new Control[] { txtSearch, lblType, cboMemberType, chkActiveOnly });
            this.Controls.Add(panelSearch);

            // DataGridView
            dgvMembers = new DataGridView
            {
                Location = new Point(20, 110),
                Size = new Size(800, 350),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvMembers.SelectionChanged += DgvMembers_SelectionChanged;
            dgvMembers.CellDoubleClick += (s, e) => EditMember();

            dgvMembers.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            dgvMembers.Columns.Add("MemberID", "ID");
            dgvMembers.Columns.Add("MemberCode", "Mã thẻ");
            dgvMembers.Columns.Add("FullName", "Họ tên");
            dgvMembers.Columns.Add("Gender", "Giới tính");
            dgvMembers.Columns.Add("Phone", "Điện thoại");
            dgvMembers.Columns.Add("MemberType", "Loại thẻ");
            dgvMembers.Columns.Add("ExpiryDate", "Hạn thẻ");
            dgvMembers.Columns.Add("TotalFine", "Nợ phạt");
            dgvMembers.Columns.Add("Status", "Trạng thái");

            dgvMembers.Columns["MemberID"]!.Visible = false;
            dgvMembers.Columns["MemberCode"]!.Width = 70;
            dgvMembers.Columns["FullName"]!.Width = 140;
            dgvMembers.Columns["Gender"]!.Width = 75;
            dgvMembers.Columns["Phone"]!.Width = 100;
            dgvMembers.Columns["MemberType"]!.Width = 85;
            dgvMembers.Columns["ExpiryDate"]!.Width = 95;
            dgvMembers.Columns["TotalFine"]!.Width = 80;
            dgvMembers.Columns["Status"]!.Width = 85;

            this.Controls.Add(dgvMembers);

            // Buttons
            int btnY = 470;

            var btnAdd = CreateButton("Thêm mới", 20, btnY, Color.FromArgb(46, 204, 113));
            btnAdd.Click += (s, e) => AddMember();
            this.Controls.Add(btnAdd);

            var btnEdit = CreateButton("Sửa", 130, btnY, Color.FromArgb(52, 152, 219));
            btnEdit.Click += (s, e) => EditMember();
            this.Controls.Add(btnEdit);

            var btnDelete = CreateButton("Xóa", 220, btnY, Color.FromArgb(231, 76, 60));
            btnDelete.Click += (s, e) => DeleteMember();
            this.Controls.Add(btnDelete);

            var btnHistory = CreateButton("Lịch sử mượn", 310, btnY, Color.FromArgb(155, 89, 182));
            btnHistory.Size = new Size(110, 35);
            btnHistory.Click += (s, e) => ShowBorrowHistory();
            this.Controls.Add(btnHistory);

            var btnPayFine = CreateButton("Đóng phạt", 430, btnY, Color.FromArgb(241, 196, 15));
            btnPayFine.Size = new Size(100, 35);
            btnPayFine.Click += (s, e) => PayFine();
            this.Controls.Add(btnPayFine);

            var btnRefresh = CreateButton("Làm mới", 540, btnY, Color.FromArgb(149, 165, 166));
            btnRefresh.Click += (s, e) => LoadData();
            this.Controls.Add(btnRefresh);

            // Detail panel
            var panelDetail = new Panel
            {
                Location = new Point(840, 50),
                Size = new Size(380, 480),
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            var lblDetailTitle = new Label
            {
                Text = "👤 Thông tin độc giả",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(15, 10),
                AutoSize = true
            };
            panelDetail.Controls.Add(lblDetailTitle);

            int detailY = 45;
            int labelWidth = 90;
            int inputWidth = 250;

            AddDetailLabel("Mã thẻ:", 15, detailY, panelDetail);
            txtMemberCode = AddDetailTextBox(labelWidth + 15, detailY, inputWidth, panelDetail);
            txtMemberCode.ReadOnly = true;
            txtMemberCode.BackColor = Color.FromArgb(245, 245, 245);
            detailY += 32;

            AddDetailLabel("Họ tên:", 15, detailY, panelDetail);
            txtFullName = AddDetailTextBox(labelWidth + 15, detailY, inputWidth, panelDetail);
            detailY += 32;

            AddDetailLabel("Giới tính:", 15, detailY, panelDetail);
            cboGender = new ComboBox
            {
                Location = new Point(labelWidth + 15, detailY),
                Size = new Size(100, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboGender.Items.AddRange(new object[] { Member.GENDER_MALE, Member.GENDER_FEMALE, Member.GENDER_OTHER });
            panelDetail.Controls.Add(cboGender);
            detailY += 32;

            AddDetailLabel("Ngày sinh:", 15, detailY, panelDetail);
            dtpDateOfBirth = new DateTimePicker
            {
                Location = new Point(labelWidth + 15, detailY),
                Size = new Size(150, 28),
                Format = DateTimePickerFormat.Short
            };
            panelDetail.Controls.Add(dtpDateOfBirth);
            detailY += 32;

            AddDetailLabel("Điện thoại:", 15, detailY, panelDetail);
            txtPhone = AddDetailTextBox(labelWidth + 15, detailY, 150, panelDetail);
            detailY += 32;

            AddDetailLabel("Email:", 15, detailY, panelDetail);
            txtEmail = AddDetailTextBox(labelWidth + 15, detailY, inputWidth, panelDetail);
            detailY += 32;

            AddDetailLabel("CCCD:", 15, detailY, panelDetail);
            txtIdentityCard = AddDetailTextBox(labelWidth + 15, detailY, 150, panelDetail);
            detailY += 32;

            AddDetailLabel("Địa chỉ:", 15, detailY, panelDetail);
            txtAddress = AddDetailTextBox(labelWidth + 15, detailY, inputWidth, panelDetail);
            detailY += 32;

            AddDetailLabel("Loại thẻ:", 15, detailY, panelDetail);
            cboMemberTypeDetail = new ComboBox
            {
                Location = new Point(labelWidth + 15, detailY),
                Size = new Size(150, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboMemberTypeDetail.Items.AddRange(new object[] { Member.TYPE_NORMAL, Member.TYPE_VIP, Member.TYPE_STUDENT, Member.TYPE_TEACHER });
            panelDetail.Controls.Add(cboMemberTypeDetail);
            detailY += 32;

            AddDetailLabel("Hạn thẻ:", 15, detailY, panelDetail);
            dtpExpiryDate = new DateTimePicker
            {
                Location = new Point(labelWidth + 15, detailY),
                Size = new Size(150, 28),
                Format = DateTimePickerFormat.Short
            };
            panelDetail.Controls.Add(dtpExpiryDate);
            detailY += 32;

            AddDetailLabel("Nợ phạt:", 15, detailY, panelDetail);
            lblTotalFine = new Label
            {
                Location = new Point(labelWidth + 15, detailY + 3),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(192, 57, 43),
                AutoSize = true
            };
            panelDetail.Controls.Add(lblTotalFine);
            detailY += 32;

            AddDetailLabel("Ghi chú:", 15, detailY, panelDetail);
            txtNotes = new TextBox
            {
                Location = new Point(labelWidth + 15, detailY),
                Size = new Size(inputWidth, 45),
                Multiline = true
            };
            panelDetail.Controls.Add(txtNotes);
            detailY += 55;

            // Save/Cancel buttons
            var btnSave = CreateButton("💾 Lưu", 15, detailY, Color.FromArgb(46, 204, 113));
            btnSave.Size = new Size(100, 35);
            btnSave.Click += BtnSave_Click;
            panelDetail.Controls.Add(btnSave);

            var btnCancel = CreateButton("❌ Hủy", 125, detailY, Color.FromArgb(149, 165, 166));
            btnCancel.Size = new Size(100, 35);
            btnCancel.Click += (s, e) => ClearDetailForm();
            panelDetail.Controls.Add(btnCancel);

            this.Controls.Add(panelDetail);
        }

        private void AddDetailLabel(string text, int x, int y, Panel parent)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(x, y + 3),
                AutoSize = true,
                Font = new Font("Segoe UI", 9)
            };
            parent.Controls.Add(label);
        }

        private TextBox AddDetailTextBox(int x, int y, int width, Panel parent)
        {
            var textBox = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(width, 26),
                Font = new Font("Segoe UI", 9)
            };
            parent.Controls.Add(textBox);
            return textBox;
        }

        private Button CreateButton(string text, int x, int y, Color color)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(110, 35),
                BackColor = color,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private void LoadData()
        {
            SearchMembers();
        }

        private void SearchMembers()
        {
            try
            {
                string? keyword = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim();
                string? memberType = cboMemberType.SelectedIndex > 0 ? cboMemberType.SelectedItem?.ToString() : null;

                var members = memberDAO.Search(keyword, memberType, chkActiveOnly.Checked);

                dgvMembers.Rows.Clear();
                foreach (var member in members)
                {
                    var row = dgvMembers.Rows.Add(
                        member.MemberID,
                        member.MemberCode,
                        member.FullName,
                        member.Gender,
                        member.Phone,
                        member.MemberType,
                        member.ExpiryDate?.ToString("dd/MM/yyyy"),
                        member.TotalFine.ToString("N0") + " đ",
                        member.StatusDisplay
                    );

                    // Highlight if has fine or expired
                    if (member.HasFine || member.IsExpired)
                    {
                        dgvMembers.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvMembers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvMembers.CurrentRow == null) return;

            int memberId = Convert.ToInt32(dgvMembers.CurrentRow.Cells["MemberID"].Value);
            currentMember = memberDAO.GetById(memberId);

            if (currentMember != null)
            {
                txtMemberCode.Text = currentMember.MemberCode;
                txtFullName.Text = currentMember.FullName;
                cboGender.SelectedItem = currentMember.Gender;
                if (currentMember.DateOfBirth.HasValue)
                    dtpDateOfBirth.Value = currentMember.DateOfBirth.Value;
                txtPhone.Text = currentMember.Phone;
                txtEmail.Text = currentMember.Email;
                txtIdentityCard.Text = currentMember.IdentityCard;
                txtAddress.Text = currentMember.Address;
                cboMemberTypeDetail.SelectedItem = currentMember.MemberType;
                if (currentMember.ExpiryDate.HasValue)
                    dtpExpiryDate.Value = currentMember.ExpiryDate.Value;
                txtNotes.Text = currentMember.Notes;
                lblTotalFine.Text = currentMember.TotalFine.ToString("N0") + " đ";
            }
        }

        private void AddMember()
        {
            currentMember = null;
            ClearDetailForm();

            // Generate new member code
            txtMemberCode.Text = memberDAO.GenerateMemberCode();
            dtpExpiryDate.Value = DateTime.Now.AddYears(1);
            cboMemberTypeDetail.SelectedIndex = 0;
            cboGender.SelectedIndex = 0;

            txtFullName.Focus();
        }

        private void EditMember()
        {
            if (dgvMembers.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtFullName.Focus();
        }

        private void DeleteMember()
        {
            if (dgvMembers.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc muốn xóa độc giả này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int memberId = Convert.ToInt32(dgvMembers.CurrentRow.Cells["MemberID"].Value);
                    if (memberDAO.Delete(memberId))
                    {
                        MessageBox.Show("Xóa độc giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SearchMembers();
                        ClearDetailForm();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowBorrowHistory()
        {
            if (currentMember == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var historyForm = new FormBorrowHistory(currentMember))
            {
                historyForm.ShowDialog();
            }
        }

        private void PayFine()
        {
            if (currentMember == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (currentMember.TotalFine <= 0)
            {
                MessageBox.Show("Độc giả này không có nợ phạt!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var payForm = new FormPayFine(currentMember))
            {
                if (payForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh data
                    currentMember = memberDAO.GetById(currentMember.MemberID);
                    lblTotalFine.Text = currentMember?.TotalFine.ToString("N0") + " đ";
                    SearchMembers();
                }
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtFullName.Focus();
                return;
            }

            try
            {
                var member = currentMember ?? new Member();
                member.MemberCode = txtMemberCode.Text.Trim();
                member.FullName = txtFullName.Text.Trim();
                member.Gender = cboGender.SelectedItem?.ToString();
                member.DateOfBirth = dtpDateOfBirth.Value;
                member.Phone = txtPhone.Text.Trim();
                member.Email = txtEmail.Text.Trim();
                member.IdentityCard = txtIdentityCard.Text.Trim();
                member.Address = txtAddress.Text.Trim();
                member.MemberType = cboMemberTypeDetail.SelectedItem?.ToString() ?? Member.TYPE_NORMAL;
                member.ExpiryDate = dtpExpiryDate.Value;
                member.Notes = txtNotes.Text.Trim();

                if (currentMember == null)
                {
                    memberDAO.Insert(member);
                    MessageBox.Show("Thêm độc giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    memberDAO.Update(member);
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                SearchMembers();
                ClearDetailForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearDetailForm()
        {
            currentMember = null;
            txtMemberCode.Clear();
            txtFullName.Clear();
            cboGender.SelectedIndex = -1;
            dtpDateOfBirth.Value = DateTime.Now.AddYears(-20);
            txtPhone.Clear();
            txtEmail.Clear();
            txtIdentityCard.Clear();
            txtAddress.Clear();
            cboMemberTypeDetail.SelectedIndex = -1;
            dtpExpiryDate.Value = DateTime.Now.AddYears(1);
            txtNotes.Clear();
            lblTotalFine.Text = "0 đ";
        }
    }

    /// <summary>
    /// Form hiển thị lịch sử mượn sách của độc giả
    /// </summary>
    public partial class FormBorrowHistory : Form
    {
        private Member member;

        public FormBorrowHistory(Member member)
        {
            this.member = member;
            InitializeComponent();
            this.Load += FormBorrowHistory_Load;
        }

        private void FormBorrowHistory_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            LoadHistory();
        }

        

        private void LoadHistory()
        {
            var borrowDAO = new BorrowRecordDAO();
            var history = borrowDAO.GetMemberHistory(member.MemberID);

            var dgv = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(740, 400),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgv.Columns.Add("BorrowCode", "Mã phiếu");
            dgv.Columns.Add("BookTitle", "Tên sách");
            dgv.Columns.Add("BorrowDate", "Ngày mượn");
            dgv.Columns.Add("DueDate", "Hạn trả");
            dgv.Columns.Add("ReturnDate", "Ngày trả");
            dgv.Columns.Add("Status", "Trạng thái");
            dgv.Columns.Add("FineAmount", "Tiền phạt");

            dgv.Columns["BorrowCode"]!.Width = 100;
            dgv.Columns["BookTitle"]!.Width = 250;
            dgv.Columns["BorrowDate"]!.Width = 90;
            dgv.Columns["DueDate"]!.Width = 90;
            dgv.Columns["ReturnDate"]!.Width = 90;
            dgv.Columns["Status"]!.Width = 90;
            dgv.Columns["FineAmount"]!.Width = 90;

            foreach (var record in history)
            {
                dgv.Rows.Add(
                    record.BorrowCode,
                    record.BookTitle,
                    record.BorrowDate.ToString("dd/MM/yyyy"),
                    record.DueDate.ToString("dd/MM/yyyy"),
                    record.ReturnDate?.ToString("dd/MM/yyyy"),
                    record.Status,
                    record.FineAmount.ToString("N0") + " đ"
                );
            }

            this.Controls.Add(dgv);

            var btnClose = new Button
            {
                Text = "Đóng",
                Location = new Point(680, 430),
                Size = new Size(80, 30)
            };
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }
    }

    /// <summary>
    /// Form đóng tiền phạt
    /// </summary>
    public partial class FormPayFine : Form
    {
        private Member member;
        private NumericUpDown numAmount = null!;
        private ComboBox cboMethod = null!;
        private TextBox txtNotes = null!;

        public FormPayFine(Member member)
        {
            this.member = member;
            InitializeComponent();
        }

        

        private void BtnPay_Click(object? sender, EventArgs e)
        {
            if (numAmount.Value <= 0)
            {
                MessageBox.Show("Vui lòng nhập số tiền!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var payment = new FinePayment
                {
                    MemberID = member.MemberID,
                    Amount = numAmount.Value,
                    PaymentMethod = cboMethod.SelectedItem?.ToString() ?? FinePayment.METHOD_CASH,
                    Notes = txtNotes.Text.Trim(),
                    StaffID = CurrentUser.User?.UserID
                };

                var paymentDAO = new FinePaymentDAO();
                paymentDAO.Insert(payment);

                MessageBox.Show($"Đã thanh toán {numAmount.Value:N0} VNĐ thành công!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
