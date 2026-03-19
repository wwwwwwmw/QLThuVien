using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormMemberManagement
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle = null!;
        private Panel panelSearch = null!;
        private Label lblType = null!;
        private Button btnAdd = null!;
        private Button btnEdit = null!;
        private Button btnDelete = null!;
        private Button btnHistory = null!;
        private Button btnPayFine = null!;
        private Button btnRefresh = null!;
        private Panel panelDetail = null!;
        private Label lblDetailTitle = null!;
        private Label lblMemberCode = null!;
        private Label lblFullName = null!;
        private Label lblGender = null!;
        private Label lblDateOfBirth = null!;
        private Label lblPhone = null!;
        private Label lblEmail = null!;
        private Label lblIdentityCard = null!;
        private Label lblAddress = null!;
        private Label lblMemberTypeDetail = null!;
        private Label lblExpiryDate = null!;
        private Label lblFineLabel = null!;
        private Label lblNotes = null!;
        private Button btnSave = null!;
        private Button btnCancel = null!;
        private DataGridViewTextBoxColumn colMemberId = null!;
        private DataGridViewTextBoxColumn colMemberCode = null!;
        private DataGridViewTextBoxColumn colFullName = null!;
        private DataGridViewTextBoxColumn colGender = null!;
        private DataGridViewTextBoxColumn colPhone = null!;
        private DataGridViewTextBoxColumn colMemberType = null!;
        private DataGridViewTextBoxColumn colExpiryDate = null!;
        private DataGridViewTextBoxColumn colTotalFine = null!;
        private DataGridViewTextBoxColumn colStatus = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            this.lblTitle = new Label();
            this.panelSearch = new Panel();
            this.chkActiveOnly = new CheckBox();
            this.cboMemberType = new ComboBox();
            this.lblType = new Label();
            this.txtSearch = new TextBox();
            this.dgvMembers = new DataGridView();
            this.colMemberId = new DataGridViewTextBoxColumn();
            this.colMemberCode = new DataGridViewTextBoxColumn();
            this.colFullName = new DataGridViewTextBoxColumn();
            this.colGender = new DataGridViewTextBoxColumn();
            this.colPhone = new DataGridViewTextBoxColumn();
            this.colMemberType = new DataGridViewTextBoxColumn();
            this.colExpiryDate = new DataGridViewTextBoxColumn();
            this.colTotalFine = new DataGridViewTextBoxColumn();
            this.colStatus = new DataGridViewTextBoxColumn();
            this.btnAdd = new Button();
            this.btnEdit = new Button();
            this.btnDelete = new Button();
            this.btnHistory = new Button();
            this.btnPayFine = new Button();
            this.btnRefresh = new Button();
            this.panelDetail = new Panel();
            this.btnCancel = new Button();
            this.btnSave = new Button();
            this.txtNotes = new TextBox();
            this.lblNotes = new Label();
            this.lblTotalFine = new Label();
            this.lblFineLabel = new Label();
            this.dtpExpiryDate = new DateTimePicker();
            this.lblExpiryDate = new Label();
            this.cboMemberTypeDetail = new ComboBox();
            this.lblMemberTypeDetail = new Label();
            this.txtAddress = new TextBox();
            this.lblAddress = new Label();
            this.txtIdentityCard = new TextBox();
            this.lblIdentityCard = new Label();
            this.txtEmail = new TextBox();
            this.lblEmail = new Label();
            this.txtPhone = new TextBox();
            this.lblPhone = new Label();
            this.dtpDateOfBirth = new DateTimePicker();
            this.lblDateOfBirth = new Label();
            this.cboGender = new ComboBox();
            this.lblGender = new Label();
            this.txtFullName = new TextBox();
            this.lblFullName = new Label();
            this.txtMemberCode = new TextBox();
            this.lblMemberCode = new Label();
            this.lblDetailTitle = new Label();
            this.panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvMembers).BeginInit();
            this.panelDetail.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblTitle.Location = new Point(20, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(274, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ ĐỘC GIẢ";
            // 
            // panelSearch
            // 
            this.panelSearch.BackColor = Color.White;
            this.panelSearch.Controls.Add(this.chkActiveOnly);
            this.panelSearch.Controls.Add(this.cboMemberType);
            this.panelSearch.Controls.Add(this.lblType);
            this.panelSearch.Controls.Add(this.txtSearch);
            this.panelSearch.Location = new Point(20, 50);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new Size(800, 50);
            this.panelSearch.TabIndex = 1;
            // 
            // chkActiveOnly
            // 
            this.chkActiveOnly.AutoSize = true;
            this.chkActiveOnly.Checked = true;
            this.chkActiveOnly.CheckState = CheckState.Checked;
            this.chkActiveOnly.Location = new Point(500, 14);
            this.chkActiveOnly.Name = "chkActiveOnly";
            this.chkActiveOnly.Size = new Size(124, 19);
            this.chkActiveOnly.TabIndex = 3;
            this.chkActiveOnly.Text = "Chỉ đang hoạt động";
            this.chkActiveOnly.UseVisualStyleBackColor = true;
            this.chkActiveOnly.CheckedChanged += this.ChkActiveOnly_CheckedChanged;
            // 
            // cboMemberType
            // 
            this.cboMemberType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboMemberType.FormattingEnabled = true;
            this.cboMemberType.Items.AddRange(new object[] {
            "-- Tất cả --",
            "Thường",
            "VIP",
            "Sinh viên",
            "Giáo viên"});
            this.cboMemberType.Location = new Point(330, 12);
            this.cboMemberType.Name = "cboMemberType";
            this.cboMemberType.Size = new Size(150, 23);
            this.cboMemberType.TabIndex = 2;
            this.cboMemberType.SelectedIndexChanged += this.CboMemberType_SelectedIndexChanged;
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new Point(270, 15);
            this.lblType.Name = "lblType";
            this.lblType.Size = new Size(51, 15);
            this.lblType.TabIndex = 1;
            this.lblType.Text = "Loại thẻ:";
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new Font("Segoe UI", 10F);
            this.txtSearch.Location = new Point(10, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Tìm theo mã, tên, SĐT...";
            this.txtSearch.Size = new Size(250, 25);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += this.TxtSearch_TextChanged;
            // 
            // dgvMembers
            // 
            this.dgvMembers.AllowUserToAddRows = false;
            this.dgvMembers.AllowUserToDeleteRows = false;
            this.dgvMembers.BackgroundColor = Color.White;
            this.dgvMembers.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(52, 73, 94);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            this.dgvMembers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMembers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMembers.Columns.AddRange(new DataGridViewColumn[] {
            this.colMemberId,
            this.colMemberCode,
            this.colFullName,
            this.colGender,
            this.colPhone,
            this.colMemberType,
            this.colExpiryDate,
            this.colTotalFine,
            this.colStatus});
            this.dgvMembers.Location = new Point(20, 110);
            this.dgvMembers.Name = "dgvMembers";
            this.dgvMembers.ReadOnly = true;
            this.dgvMembers.RowHeadersVisible = false;
            this.dgvMembers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvMembers.Size = new Size(800, 350);
            this.dgvMembers.TabIndex = 2;
            this.dgvMembers.CellDoubleClick += this.DgvMembers_CellDoubleClick;
            this.dgvMembers.SelectionChanged += this.DgvMembers_SelectionChanged;
            // 
            // colMemberId
            // 
            this.colMemberId.HeaderText = "ID";
            this.colMemberId.Name = "MemberID";
            this.colMemberId.ReadOnly = true;
            this.colMemberId.Visible = false;
            // 
            // colMemberCode
            // 
            this.colMemberCode.HeaderText = "Mã thẻ";
            this.colMemberCode.Name = "MemberCode";
            this.colMemberCode.ReadOnly = true;
            this.colMemberCode.Width = 70;
            // 
            // colFullName
            // 
            this.colFullName.HeaderText = "Họ tên";
            this.colFullName.Name = "FullName";
            this.colFullName.ReadOnly = true;
            this.colFullName.Width = 140;
            // 
            // colGender
            // 
            this.colGender.HeaderText = "Giới tính";
            this.colGender.Name = "Gender";
            this.colGender.ReadOnly = true;
            this.colGender.Width = 75;
            // 
            // colPhone
            // 
            this.colPhone.HeaderText = "Điện thoại";
            this.colPhone.Name = "Phone";
            this.colPhone.ReadOnly = true;
            this.colPhone.Width = 100;
            // 
            // colMemberType
            // 
            this.colMemberType.HeaderText = "Loại thẻ";
            this.colMemberType.Name = "MemberType";
            this.colMemberType.ReadOnly = true;
            this.colMemberType.Width = 85;
            // 
            // colExpiryDate
            // 
            this.colExpiryDate.HeaderText = "Hạn thẻ";
            this.colExpiryDate.Name = "ExpiryDate";
            this.colExpiryDate.ReadOnly = true;
            this.colExpiryDate.Width = 95;
            // 
            // colTotalFine
            // 
            this.colTotalFine.HeaderText = "Nợ phạt";
            this.colTotalFine.Name = "TotalFine";
            this.colTotalFine.ReadOnly = true;
            this.colTotalFine.Width = 80;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Trạng thái";
            this.colStatus.Name = "Status";
            this.colStatus.ReadOnly = true;
            this.colStatus.Width = 85;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = Color.FromArgb(46, 204, 113);
            this.btnAdd.Cursor = Cursors.Hand;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.Font = new Font("Segoe UI", 9F);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.Location = new Point(20, 470);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new Size(110, 35);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Thêm mới";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += this.BtnAdd_Click;
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = Color.FromArgb(52, 152, 219);
            this.btnEdit.Cursor = Cursors.Hand;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            this.btnEdit.FlatStyle = FlatStyle.Flat;
            this.btnEdit.Font = new Font("Segoe UI", 9F);
            this.btnEdit.ForeColor = Color.White;
            this.btnEdit.Location = new Point(130, 470);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new Size(90, 35);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Sửa";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += this.BtnEdit_Click;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = Color.FromArgb(231, 76, 60);
            this.btnDelete.Cursor = Cursors.Hand;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.FlatStyle = FlatStyle.Flat;
            this.btnDelete.Font = new Font("Segoe UI", 9F);
            this.btnDelete.ForeColor = Color.White;
            this.btnDelete.Location = new Point(220, 470);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new Size(90, 35);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += this.BtnDelete_Click;
            // 
            // btnHistory
            // 
            this.btnHistory.BackColor = Color.FromArgb(155, 89, 182);
            this.btnHistory.Cursor = Cursors.Hand;
            this.btnHistory.FlatAppearance.BorderSize = 0;
            this.btnHistory.FlatStyle = FlatStyle.Flat;
            this.btnHistory.Font = new Font("Segoe UI", 9F);
            this.btnHistory.ForeColor = Color.White;
            this.btnHistory.Location = new Point(310, 470);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new Size(110, 35);
            this.btnHistory.TabIndex = 6;
            this.btnHistory.Text = "Lịch sử mượn";
            this.btnHistory.UseVisualStyleBackColor = false;
            this.btnHistory.Click += this.BtnHistory_Click;
            // 
            // btnPayFine
            // 
            this.btnPayFine.BackColor = Color.FromArgb(241, 196, 15);
            this.btnPayFine.Cursor = Cursors.Hand;
            this.btnPayFine.FlatAppearance.BorderSize = 0;
            this.btnPayFine.FlatStyle = FlatStyle.Flat;
            this.btnPayFine.Font = new Font("Segoe UI", 9F);
            this.btnPayFine.ForeColor = Color.White;
            this.btnPayFine.Location = new Point(430, 470);
            this.btnPayFine.Name = "btnPayFine";
            this.btnPayFine.Size = new Size(100, 35);
            this.btnPayFine.TabIndex = 7;
            this.btnPayFine.Text = "Đóng phạt";
            this.btnPayFine.UseVisualStyleBackColor = false;
            this.btnPayFine.Click += this.BtnPayFine_Click;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = Color.FromArgb(149, 165, 166);
            this.btnRefresh.Cursor = Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.Font = new Font("Segoe UI", 9F);
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.Location = new Point(540, 470);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new Size(110, 35);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "Làm mới";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += this.BtnRefresh_Click;
            // 
            // panelDetail
            // 
            this.panelDetail.BackColor = Color.White;
            this.panelDetail.Controls.Add(this.btnCancel);
            this.panelDetail.Controls.Add(this.btnSave);
            this.panelDetail.Controls.Add(this.txtNotes);
            this.panelDetail.Controls.Add(this.lblNotes);
            this.panelDetail.Controls.Add(this.lblTotalFine);
            this.panelDetail.Controls.Add(this.lblFineLabel);
            this.panelDetail.Controls.Add(this.dtpExpiryDate);
            this.panelDetail.Controls.Add(this.lblExpiryDate);
            this.panelDetail.Controls.Add(this.cboMemberTypeDetail);
            this.panelDetail.Controls.Add(this.lblMemberTypeDetail);
            this.panelDetail.Controls.Add(this.txtAddress);
            this.panelDetail.Controls.Add(this.lblAddress);
            this.panelDetail.Controls.Add(this.txtIdentityCard);
            this.panelDetail.Controls.Add(this.lblIdentityCard);
            this.panelDetail.Controls.Add(this.txtEmail);
            this.panelDetail.Controls.Add(this.lblEmail);
            this.panelDetail.Controls.Add(this.txtPhone);
            this.panelDetail.Controls.Add(this.lblPhone);
            this.panelDetail.Controls.Add(this.dtpDateOfBirth);
            this.panelDetail.Controls.Add(this.lblDateOfBirth);
            this.panelDetail.Controls.Add(this.cboGender);
            this.panelDetail.Controls.Add(this.lblGender);
            this.panelDetail.Controls.Add(this.txtFullName);
            this.panelDetail.Controls.Add(this.lblFullName);
            this.panelDetail.Controls.Add(this.txtMemberCode);
            this.panelDetail.Controls.Add(this.lblMemberCode);
            this.panelDetail.Controls.Add(this.lblDetailTitle);
            this.panelDetail.Location = new Point(840, 50);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Padding = new Padding(15);
            this.panelDetail.Size = new Size(380, 480);
            this.panelDetail.TabIndex = 9;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = Color.FromArgb(149, 165, 166);
            this.btnCancel.Cursor = Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = FlatStyle.Flat;
            this.btnCancel.Font = new Font("Segoe UI", 9F);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.Location = new Point(125, 452);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(100, 35);
            this.btnCancel.TabIndex = 26;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += this.BtnCancel_Click;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = Color.FromArgb(46, 204, 113);
            this.btnSave.Cursor = Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.Font = new Font("Segoe UI", 9F);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.Location = new Point(15, 452);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new Size(100, 35);
            this.btnSave.TabIndex = 25;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += this.BtnSave_Click;
            // 
            // txtNotes
            // 
            this.txtNotes.Font = new Font("Segoe UI", 9F);
            this.txtNotes.Location = new Point(105, 397);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new Size(250, 45);
            this.txtNotes.TabIndex = 24;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Font = new Font("Segoe UI", 9F);
            this.lblNotes.Location = new Point(15, 400);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new Size(48, 15);
            this.lblNotes.TabIndex = 23;
            this.lblNotes.Text = "Ghi chú:";
            // 
            // lblTotalFine
            // 
            this.lblTotalFine.AutoSize = true;
            this.lblTotalFine.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.lblTotalFine.ForeColor = Color.FromArgb(192, 57, 43);
            this.lblTotalFine.Location = new Point(105, 368);
            this.lblTotalFine.Name = "lblTotalFine";
            this.lblTotalFine.Size = new Size(28, 19);
            this.lblTotalFine.TabIndex = 22;
            this.lblTotalFine.Text = "0 đ";
            // 
            // lblFineLabel
            // 
            this.lblFineLabel.AutoSize = true;
            this.lblFineLabel.Font = new Font("Segoe UI", 9F);
            this.lblFineLabel.Location = new Point(15, 368);
            this.lblFineLabel.Name = "lblFineLabel";
            this.lblFineLabel.Size = new Size(50, 15);
            this.lblFineLabel.TabIndex = 21;
            this.lblFineLabel.Text = "Nợ phạt:";
            // 
            // dtpExpiryDate
            // 
            this.dtpExpiryDate.Format = DateTimePickerFormat.Short;
            this.dtpExpiryDate.Location = new Point(105, 333);
            this.dtpExpiryDate.Name = "dtpExpiryDate";
            this.dtpExpiryDate.Size = new Size(150, 23);
            this.dtpExpiryDate.TabIndex = 20;
            // 
            // lblExpiryDate
            // 
            this.lblExpiryDate.AutoSize = true;
            this.lblExpiryDate.Font = new Font("Segoe UI", 9F);
            this.lblExpiryDate.Location = new Point(15, 336);
            this.lblExpiryDate.Name = "lblExpiryDate";
            this.lblExpiryDate.Size = new Size(49, 15);
            this.lblExpiryDate.TabIndex = 19;
            this.lblExpiryDate.Text = "Hạn thẻ:";
            // 
            // cboMemberTypeDetail
            // 
            this.cboMemberTypeDetail.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboMemberTypeDetail.FormattingEnabled = true;
            this.cboMemberTypeDetail.Items.AddRange(new object[] {
            "Thường",
            "VIP",
            "Sinh viên",
            "Giáo viên"});
            this.cboMemberTypeDetail.Location = new Point(105, 301);
            this.cboMemberTypeDetail.Name = "cboMemberTypeDetail";
            this.cboMemberTypeDetail.Size = new Size(150, 23);
            this.cboMemberTypeDetail.TabIndex = 18;
            // 
            // lblMemberTypeDetail
            // 
            this.lblMemberTypeDetail.AutoSize = true;
            this.lblMemberTypeDetail.Font = new Font("Segoe UI", 9F);
            this.lblMemberTypeDetail.Location = new Point(15, 304);
            this.lblMemberTypeDetail.Name = "lblMemberTypeDetail";
            this.lblMemberTypeDetail.Size = new Size(51, 15);
            this.lblMemberTypeDetail.TabIndex = 17;
            this.lblMemberTypeDetail.Text = "Loại thẻ:";
            // 
            // txtAddress
            // 
            this.txtAddress.Font = new Font("Segoe UI", 9F);
            this.txtAddress.Location = new Point(105, 269);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new Size(250, 23);
            this.txtAddress.TabIndex = 16;
            // 
            // lblAddress
            // 
            this.lblAddress.AutoSize = true;
            this.lblAddress.Font = new Font("Segoe UI", 9F);
            this.lblAddress.Location = new Point(15, 272);
            this.lblAddress.Name = "lblAddress";
            this.lblAddress.Size = new Size(46, 15);
            this.lblAddress.TabIndex = 15;
            this.lblAddress.Text = "Địa chỉ:";
            // 
            // txtIdentityCard
            // 
            this.txtIdentityCard.Font = new Font("Segoe UI", 9F);
            this.txtIdentityCard.Location = new Point(105, 237);
            this.txtIdentityCard.Name = "txtIdentityCard";
            this.txtIdentityCard.Size = new Size(150, 23);
            this.txtIdentityCard.TabIndex = 14;
            // 
            // lblIdentityCard
            // 
            this.lblIdentityCard.AutoSize = true;
            this.lblIdentityCard.Font = new Font("Segoe UI", 9F);
            this.lblIdentityCard.Location = new Point(15, 240);
            this.lblIdentityCard.Name = "lblIdentityCard";
            this.lblIdentityCard.Size = new Size(38, 15);
            this.lblIdentityCard.TabIndex = 13;
            this.lblIdentityCard.Text = "CCCD:";
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new Font("Segoe UI", 9F);
            this.txtEmail.Location = new Point(105, 205);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new Size(250, 23);
            this.txtEmail.TabIndex = 12;
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new Font("Segoe UI", 9F);
            this.lblEmail.Location = new Point(15, 208);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new Size(39, 15);
            this.lblEmail.TabIndex = 11;
            this.lblEmail.Text = "Email:";
            // 
            // txtPhone
            // 
            this.txtPhone.Font = new Font("Segoe UI", 9F);
            this.txtPhone.Location = new Point(105, 173);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new Size(150, 23);
            this.txtPhone.TabIndex = 10;
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new Font("Segoe UI", 9F);
            this.lblPhone.Location = new Point(15, 176);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new Size(60, 15);
            this.lblPhone.TabIndex = 9;
            this.lblPhone.Text = "Điện thoại:";
            // 
            // dtpDateOfBirth
            // 
            this.dtpDateOfBirth.Format = DateTimePickerFormat.Short;
            this.dtpDateOfBirth.Location = new Point(105, 141);
            this.dtpDateOfBirth.Name = "dtpDateOfBirth";
            this.dtpDateOfBirth.Size = new Size(150, 23);
            this.dtpDateOfBirth.TabIndex = 8;
            // 
            // lblDateOfBirth
            // 
            this.lblDateOfBirth.AutoSize = true;
            this.lblDateOfBirth.Font = new Font("Segoe UI", 9F);
            this.lblDateOfBirth.Location = new Point(15, 144);
            this.lblDateOfBirth.Name = "lblDateOfBirth";
            this.lblDateOfBirth.Size = new Size(58, 15);
            this.lblDateOfBirth.TabIndex = 7;
            this.lblDateOfBirth.Text = "Ngày sinh:";
            // 
            // cboGender
            // 
            this.cboGender.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboGender.FormattingEnabled = true;
            this.cboGender.Items.AddRange(new object[] {
            "Nam",
            "Nữ",
            "Khác"});
            this.cboGender.Location = new Point(105, 109);
            this.cboGender.Name = "cboGender";
            this.cboGender.Size = new Size(100, 23);
            this.cboGender.TabIndex = 6;
            // 
            // lblGender
            // 
            this.lblGender.AutoSize = true;
            this.lblGender.Font = new Font("Segoe UI", 9F);
            this.lblGender.Location = new Point(15, 112);
            this.lblGender.Name = "lblGender";
            this.lblGender.Size = new Size(55, 15);
            this.lblGender.TabIndex = 5;
            this.lblGender.Text = "Giới tính:";
            // 
            // txtFullName
            // 
            this.txtFullName.Font = new Font("Segoe UI", 9F);
            this.txtFullName.Location = new Point(105, 77);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new Size(250, 23);
            this.txtFullName.TabIndex = 4;
            // 
            // lblFullName
            // 
            this.lblFullName.AutoSize = true;
            this.lblFullName.Font = new Font("Segoe UI", 9F);
            this.lblFullName.Location = new Point(15, 80);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new Size(47, 15);
            this.lblFullName.TabIndex = 3;
            this.lblFullName.Text = "Họ tên:";
            // 
            // txtMemberCode
            // 
            this.txtMemberCode.BackColor = Color.FromArgb(245, 245, 245);
            this.txtMemberCode.Font = new Font("Segoe UI", 9F);
            this.txtMemberCode.Location = new Point(105, 45);
            this.txtMemberCode.Name = "txtMemberCode";
            this.txtMemberCode.ReadOnly = true;
            this.txtMemberCode.Size = new Size(250, 23);
            this.txtMemberCode.TabIndex = 2;
            // 
            // lblMemberCode
            // 
            this.lblMemberCode.AutoSize = true;
            this.lblMemberCode.Font = new Font("Segoe UI", 9F);
            this.lblMemberCode.Location = new Point(15, 48);
            this.lblMemberCode.Name = "lblMemberCode";
            this.lblMemberCode.Size = new Size(45, 15);
            this.lblMemberCode.TabIndex = 1;
            this.lblMemberCode.Text = "Mã thẻ:";
            // 
            // lblDetailTitle
            // 
            this.lblDetailTitle.AutoSize = true;
            this.lblDetailTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblDetailTitle.Location = new Point(15, 10);
            this.lblDetailTitle.Name = "lblDetailTitle";
            this.lblDetailTitle.Size = new Size(145, 21);
            this.lblDetailTitle.TabIndex = 0;
            this.lblDetailTitle.Text = "Thông tin độc giả";
            // 
            // FormMemberManagement
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.ClientSize = new Size(1240, 550);
            this.Controls.Add(this.panelDetail);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnPayFine);
            this.Controls.Add(this.btnHistory);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dgvMembers);
            this.Controls.Add(this.panelSearch);
            this.Controls.Add(this.lblTitle);
            this.DoubleBuffered = true;
            this.Name = "FormMemberManagement";
            this.Text = "Quản lý độc giả";
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.dgvMembers).EndInit();
            this.panelDetail.ResumeLayout(false);
            this.panelDetail.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private DataGridView dgvMembers;
        private TextBox txtSearch;
        private ComboBox cboMemberType;
        private CheckBox chkActiveOnly;
        private TextBox txtMemberCode;
        private TextBox txtFullName;
        private ComboBox cboGender;
        private DateTimePicker dtpDateOfBirth;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtIdentityCard;
        private TextBox txtAddress;
        private ComboBox cboMemberTypeDetail;
        private DateTimePicker dtpExpiryDate;
        private TextBox txtNotes;
        private Label lblTotalFine;
    }
}
