using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormBorrow
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Panel panelMember;
        private Label lblMemberTitle;
        private Label lblCode;
        private Button btnFindMember;
        private Label lblCurrentBorrow;
        private Panel panelBook;
        private Label lblBookTitle;
        private Label lblSearch;
        private Label lblDays;
        private Button btnBorrow;

        private TextBox txtMemberCode;
        private Label lblMemberInfo;
        private Label lblMemberStatus;
        private DataGridView dgvBorrowing;
        private TextBox txtBookSearch;
        private DataGridView dgvBooks;
        private NumericUpDown numDays;

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
            lblTitle = new Label();
            panelMember = new Panel();
            lblMemberTitle = new Label();
            lblCode = new Label();
            txtMemberCode = new TextBox();
            btnFindMember = new Button();
            lblMemberInfo = new Label();
            lblMemberStatus = new Label();
            lblCurrentBorrow = new Label();
            dgvBorrowing = new DataGridView();
            panelBook = new Panel();
            lblBookTitle = new Label();
            lblSearch = new Label();
            txtBookSearch = new TextBox();
            dgvBooks = new DataGridView();
            lblDays = new Label();
            numDays = new NumericUpDown();
            btnBorrow = new Button();
            panelMember.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBorrowing).BeginInit();
            panelBook.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDays).BeginInit();
            SuspendLayout();

            // FormBorrow
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(236, 240, 241);
            ClientSize = new Size(1220, 540);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormBorrow";
            Text = "Mượn sách";

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(44, 62, 80);
            lblTitle.Location = new Point(20, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(194, 41);
            lblTitle.Text = "MƯỢN SÁCH";

            // panelMember
            panelMember.BackColor = Color.White;
            panelMember.Controls.Add(lblMemberTitle);
            panelMember.Controls.Add(lblCode);
            panelMember.Controls.Add(txtMemberCode);
            panelMember.Controls.Add(btnFindMember);
            panelMember.Controls.Add(lblMemberInfo);
            panelMember.Controls.Add(lblMemberStatus);
            panelMember.Location = new Point(20, 60);
            panelMember.Name = "panelMember";
            panelMember.Padding = new Padding(15);
            panelMember.Size = new Size(500, 200);

            // lblMemberTitle
            lblMemberTitle.AutoSize = true;
            lblMemberTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblMemberTitle.Location = new Point(15, 10);
            lblMemberTitle.Name = "lblMemberTitle";
            lblMemberTitle.Size = new Size(178, 25);
            lblMemberTitle.Text = "👤 Thông tin độc giả";

            // lblCode
            lblCode.AutoSize = true;
            lblCode.Location = new Point(15, 45);
            lblCode.Name = "lblCode";
            lblCode.Size = new Size(56, 20);
            lblCode.Text = "Mã thẻ:";

            // txtMemberCode
            txtMemberCode.Font = new Font("Segoe UI", 10F);
            txtMemberCode.Location = new Point(80, 42);
            txtMemberCode.Name = "txtMemberCode";
            txtMemberCode.Size = new Size(150, 30);
            txtMemberCode.KeyPress += TxtMemberCode_KeyPress;

            // btnFindMember
            btnFindMember.BackColor = Color.FromArgb(52, 152, 219);
            btnFindMember.FlatAppearance.BorderSize = 0;
            btnFindMember.FlatStyle = FlatStyle.Flat;
            btnFindMember.ForeColor = Color.White;
            btnFindMember.Location = new Point(240, 40);
            btnFindMember.Name = "btnFindMember";
            btnFindMember.Size = new Size(85, 30);
            btnFindMember.Text = "Tìm kiếm";
            btnFindMember.UseVisualStyleBackColor = false;
            btnFindMember.Click += BtnFindMember_Click;

            // lblMemberInfo
            lblMemberInfo.Font = new Font("Segoe UI", 10F);
            lblMemberInfo.Location = new Point(15, 80);
            lblMemberInfo.Name = "lblMemberInfo";
            lblMemberInfo.Size = new Size(460, 60);

            // lblMemberStatus
            lblMemberStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblMemberStatus.Location = new Point(15, 145);
            lblMemberStatus.Name = "lblMemberStatus";
            lblMemberStatus.Size = new Size(460, 25);

            // lblCurrentBorrow
            lblCurrentBorrow.AutoSize = true;
            lblCurrentBorrow.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCurrentBorrow.Location = new Point(20, 270);
            lblCurrentBorrow.Name = "lblCurrentBorrow";
            lblCurrentBorrow.Size = new Size(167, 23);
            lblCurrentBorrow.Text = "📚 Sách đang mượn:";

            // dgvBorrowing
            dgvBorrowing.AllowUserToAddRows = false;
            dgvBorrowing.AllowUserToDeleteRows = false;
            dgvBorrowing.BackgroundColor = Color.White;
            dgvBorrowing.BorderStyle = BorderStyle.None;
            dgvBorrowing.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBorrowing.Location = new Point(20, 295);
            dgvBorrowing.Name = "dgvBorrowing";
            dgvBorrowing.ReadOnly = true;
            dgvBorrowing.RowHeadersVisible = false;
            dgvBorrowing.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBorrowing.Size = new Size(500, 180);
            dgvBorrowing.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "BookTitle", HeaderText = "Tên sách", Width = 220, ReadOnly = true },
                new DataGridViewTextBoxColumn { Name = "BorrowDate", HeaderText = "Ngày mượn", Width = 90, ReadOnly = true },
                new DataGridViewTextBoxColumn { Name = "DueDate", HeaderText = "Hạn trả", Width = 90, ReadOnly = true },
                new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Trạng thái", Width = 90, ReadOnly = true }
            );

            // panelBook
            panelBook.BackColor = Color.White;
            panelBook.Controls.Add(lblBookTitle);
            panelBook.Controls.Add(lblSearch);
            panelBook.Controls.Add(txtBookSearch);
            panelBook.Controls.Add(dgvBooks);
            panelBook.Controls.Add(lblDays);
            panelBook.Controls.Add(numDays);
            panelBook.Controls.Add(btnBorrow);
            panelBook.Location = new Point(540, 60);
            panelBook.Name = "panelBook";
            panelBook.Padding = new Padding(15);
            panelBook.Size = new Size(660, 420);

            // lblBookTitle
            lblBookTitle.AutoSize = true;
            lblBookTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblBookTitle.Location = new Point(15, 10);
            lblBookTitle.Name = "lblBookTitle";
            lblBookTitle.Size = new Size(141, 25);
            lblBookTitle.Text = "Chọn sách mượn";

            // lblSearch
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(15, 45);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(63, 20);
            lblSearch.Text = "Tìm sách:";

            // txtBookSearch
            txtBookSearch.Font = new Font("Segoe UI", 10F);
            txtBookSearch.Location = new Point(80, 42);
            txtBookSearch.Name = "txtBookSearch";
            txtBookSearch.PlaceholderText = "Nhập tên sách hoặc ISBN...";
            txtBookSearch.Size = new Size(300, 30);
            txtBookSearch.TextChanged += TxtBookSearch_TextChanged;

            // dgvBooks
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.AllowUserToDeleteRows = false;
            dgvBooks.BackgroundColor = Color.White;
            dgvBooks.BorderStyle = BorderStyle.FixedSingle;
            dgvBooks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBooks.Location = new Point(15, 80);
            dgvBooks.Name = "dgvBooks";
            dgvBooks.ReadOnly = true;
            dgvBooks.RowHeadersVisible = false;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.Size = new Size(620, 250);
            dgvBooks.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            dgvBooks.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "BookID", HeaderText = "ID", Visible = false, ReadOnly = true },
                new DataGridViewTextBoxColumn { Name = "ISBN", HeaderText = "ISBN", Width = 100, ReadOnly = true },
                new DataGridViewTextBoxColumn { Name = "Title", HeaderText = "Tên sách", Width = 220, ReadOnly = true },
                new DataGridViewTextBoxColumn { Name = "AuthorName", HeaderText = "Tác giả", Width = 120, ReadOnly = true },
                new DataGridViewTextBoxColumn { Name = "AvailableCopies", HeaderText = "Còn lại", Width = 70, ReadOnly = true },
                new DataGridViewTextBoxColumn { Name = "Location", HeaderText = "Vị trí", Width = 80, ReadOnly = true }
            );
            dgvBooks.SelectionChanged += DgvBooks_SelectionChanged;

            // lblDays
            lblDays.AutoSize = true;
            lblDays.Location = new Point(15, 345);
            lblDays.Name = "lblDays";
            lblDays.Size = new Size(94, 20);
            lblDays.Text = "Số ngày mượn:";

            // numDays
            numDays.Location = new Point(110, 342);
            numDays.Maximum = 60;
            numDays.Minimum = 1;
            numDays.Name = "numDays";
            numDays.Size = new Size(70, 27);
            numDays.Value = 14;

            // btnBorrow
            btnBorrow.BackColor = Color.FromArgb(46, 204, 113);
            btnBorrow.FlatAppearance.BorderSize = 0;
            btnBorrow.FlatStyle = FlatStyle.Flat;
            btnBorrow.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnBorrow.ForeColor = Color.White;
            btnBorrow.Location = new Point(200, 340);
            btnBorrow.Name = "btnBorrow";
            btnBorrow.Size = new Size(120, 40);
            btnBorrow.Text = "Mượn sách";
            btnBorrow.UseVisualStyleBackColor = false;
            btnBorrow.Click += BtnBorrow_Click;

            Controls.Add(lblTitle);
            Controls.Add(panelMember);
            Controls.Add(lblCurrentBorrow);
            Controls.Add(dgvBorrowing);
            Controls.Add(panelBook);

            panelMember.ResumeLayout(false);
            panelMember.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBorrowing).EndInit();
            panelBook.ResumeLayout(false);
            panelBook.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDays).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
