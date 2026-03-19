using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormBookManagement
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private Panel panelSearch;
        private Label lblCategory;
        private Label lblAuthor;

        private DataGridView dgvBooks;
        private TextBox txtSearch;
        private ComboBox cboCategory;
        private ComboBox cboAuthor;
        private CheckBox chkAvailableOnly;

        private Panel panelDetail;
        private Label lblDetailTitle;
        private Panel panelImage;

        private TextBox txtISBN;
        private TextBox txtTitle;
        private ComboBox cboCategoryDetail;
        private ComboBox cboAuthorDetail;
        private ComboBox cboPublisher;
        private NumericUpDown numYear;
        private NumericUpDown numPrice;
        private NumericUpDown numTotalCopies;
        private TextBox txtLocation;
        private TextBox txtDescription;

        private PictureBox picBookImage;
        private Button btnBrowseImage;
        private Button btnRemoveImage;
        private Button btnViewDetail;

        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Button btnSave;
        private Button btnCancel;

        private Label lblISBN;
        private Label lblBookTitle;
        private Label lblCategoryDetail;
        private Label lblAuthorDetail;
        private Label lblPublisher;
        private Label lblYear;
        private Label lblPrice;
        private Label lblCopies;
        private Label lblLocation;
        private Label lblDescription;

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
            panelSearch = new Panel();
            txtSearch = new TextBox();
            lblCategory = new Label();
            cboCategory = new ComboBox();
            lblAuthor = new Label();
            cboAuthor = new ComboBox();
            chkAvailableOnly = new CheckBox();

            dgvBooks = new DataGridView();

            btnAdd = new Button();
            btnEdit = new Button();
            btnDelete = new Button();
            btnRefresh = new Button();

            panelDetail = new Panel();
            lblDetailTitle = new Label();
            panelImage = new Panel();
            picBookImage = new PictureBox();
            btnBrowseImage = new Button();
            btnRemoveImage = new Button();
            btnViewDetail = new Button();

            lblISBN = new Label();
            txtISBN = new TextBox();

            lblBookTitle = new Label();
            txtTitle = new TextBox();

            lblCategoryDetail = new Label();
            cboCategoryDetail = new ComboBox();

            lblAuthorDetail = new Label();
            cboAuthorDetail = new ComboBox();

            lblPublisher = new Label();
            cboPublisher = new ComboBox();

            lblYear = new Label();
            numYear = new NumericUpDown();

            lblPrice = new Label();
            numPrice = new NumericUpDown();

            lblCopies = new Label();
            numTotalCopies = new NumericUpDown();

            lblLocation = new Label();
            txtLocation = new TextBox();

            lblDescription = new Label();
            txtDescription = new TextBox();

            btnSave = new Button();
            btnCancel = new Button();

            panelSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).BeginInit();
            panelDetail.SuspendLayout();
            panelImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBookImage).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numYear).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numTotalCopies).BeginInit();
            SuspendLayout();

            // FormBookManagement
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(236, 240, 241);
            ClientSize = new Size(1240, 600);
            Name = "FormBookManagement";
            Text = "Quản lý sách";

            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(44, 62, 80);
            lblTitle.Location = new Point(20, 10);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(239, 41);
            lblTitle.Text = "📚 QUẢN LÝ SÁCH";

            // panelSearch
            panelSearch.BackColor = Color.White;
            panelSearch.Location = new Point(20, 50);
            panelSearch.Name = "panelSearch";
            panelSearch.Size = new Size(800, 50);

            // txtSearch
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(10, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm kiếm sách...";
            txtSearch.Size = new Size(200, 30);
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // lblCategory
            lblCategory.AutoSize = true;
            lblCategory.Location = new Point(220, 15);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(65, 20);
            lblCategory.Text = "Thể loại:";

            // cboCategory
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategory.Location = new Point(280, 12);
            cboCategory.Name = "cboCategory";
            cboCategory.Size = new Size(150, 28);
            cboCategory.SelectedIndexChanged += CboCategory_SelectedIndexChanged;

            // lblAuthor
            lblAuthor.AutoSize = true;
            lblAuthor.Location = new Point(440, 15);
            lblAuthor.Name = "lblAuthor";
            lblAuthor.Size = new Size(58, 20);
            lblAuthor.Text = "Tác giả:";

            // cboAuthor
            cboAuthor.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAuthor.Location = new Point(500, 12);
            cboAuthor.Name = "cboAuthor";
            cboAuthor.Size = new Size(150, 28);
            cboAuthor.SelectedIndexChanged += CboAuthor_SelectedIndexChanged;

            // chkAvailableOnly
            chkAvailableOnly.AutoSize = true;
            chkAvailableOnly.Location = new Point(670, 14);
            chkAvailableOnly.Name = "chkAvailableOnly";
            chkAvailableOnly.Size = new Size(108, 24);
            chkAvailableOnly.Text = "Chỉ còn sách";
            chkAvailableOnly.UseVisualStyleBackColor = true;
            chkAvailableOnly.CheckedChanged += ChkAvailableOnly_CheckedChanged;

            panelSearch.Controls.Add(txtSearch);
            panelSearch.Controls.Add(lblCategory);
            panelSearch.Controls.Add(cboCategory);
            panelSearch.Controls.Add(lblAuthor);
            panelSearch.Controls.Add(cboAuthor);
            panelSearch.Controls.Add(chkAvailableOnly);

            // dgvBooks
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.AllowUserToDeleteRows = false;
            dgvBooks.BackgroundColor = Color.White;
            dgvBooks.BorderStyle = BorderStyle.None;
            dgvBooks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBooks.Location = new Point(20, 110);
            dgvBooks.MultiSelect = false;
            dgvBooks.Name = "dgvBooks";
            dgvBooks.ReadOnly = true;
            dgvBooks.RowHeadersVisible = false;
            dgvBooks.RowTemplate.Height = 29;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.Size = new Size(800, 350);
            dgvBooks.SelectionChanged += DgvBooks_SelectionChanged;
            dgvBooks.CellDoubleClick += DgvBooks_CellDoubleClick;

            // btnAdd
            btnAdd.BackColor = Color.FromArgb(46, 204, 113);
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Segoe UI", 9F);
            btnAdd.ForeColor = Color.White;
            btnAdd.Location = new Point(20, 470);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(110, 35);
            btnAdd.Text = "Thêm mới";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += BtnAdd_Click;

            // btnEdit
            btnEdit.BackColor = Color.FromArgb(52, 152, 219);
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.Font = new Font("Segoe UI", 9F);
            btnEdit.ForeColor = Color.White;
            btnEdit.Location = new Point(140, 470);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(80, 35);
            btnEdit.Text = "Sửa";
            btnEdit.UseVisualStyleBackColor = false;
            btnEdit.Click += BtnEdit_Click;

            // btnDelete
            btnDelete.BackColor = Color.FromArgb(231, 76, 60);
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.Font = new Font("Segoe UI", 9F);
            btnDelete.ForeColor = Color.White;
            btnDelete.Location = new Point(230, 470);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(80, 35);
            btnDelete.Text = "Xóa";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += BtnDelete_Click;

            // btnRefresh
            btnRefresh.BackColor = Color.FromArgb(149, 165, 166);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(320, 470);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(100, 35);
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;

            // panelDetail
            panelDetail.AutoScroll = true;
            panelDetail.BackColor = Color.White;
            panelDetail.Location = new Point(840, 50);
            panelDetail.Name = "panelDetail";
            panelDetail.Size = new Size(380, 530);

            // lblDetailTitle
            lblDetailTitle.AutoSize = true;
            lblDetailTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblDetailTitle.Location = new Point(15, 10);
            lblDetailTitle.Name = "lblDetailTitle";
            lblDetailTitle.Size = new Size(133, 28);
            lblDetailTitle.Text = "Thông tin sách";

            // panelImage
            panelImage.BackColor = Color.FromArgb(245, 245, 245);
            panelImage.BorderStyle = BorderStyle.FixedSingle;
            panelImage.Location = new Point(15, 40);
            panelImage.Name = "panelImage";
            panelImage.Size = new Size(120, 150);

            // picBookImage
            picBookImage.BackColor = Color.FromArgb(245, 245, 245);
            picBookImage.Cursor = Cursors.Hand;
            picBookImage.Dock = DockStyle.Fill;
            picBookImage.Name = "picBookImage";
            picBookImage.SizeMode = PictureBoxSizeMode.Zoom;
            picBookImage.TabStop = false;
            picBookImage.Click += BtnBrowseImage_Click;
            picBookImage.Paint += PicBookImage_Paint;

            panelImage.Controls.Add(picBookImage);

            // btnBrowseImage
            btnBrowseImage.BackColor = Color.FromArgb(52, 152, 219);
            btnBrowseImage.FlatAppearance.BorderSize = 0;
            btnBrowseImage.FlatStyle = FlatStyle.Flat;
            btnBrowseImage.Font = new Font("Segoe UI", 8F);
            btnBrowseImage.ForeColor = Color.White;
            btnBrowseImage.Location = new Point(145, 40);
            btnBrowseImage.Name = "btnBrowseImage";
            btnBrowseImage.Size = new Size(70, 28);
            btnBrowseImage.Text = "Chọn ảnh";
            btnBrowseImage.UseVisualStyleBackColor = false;
            btnBrowseImage.Click += BtnBrowseImage_Click;

            // btnRemoveImage
            btnRemoveImage.BackColor = Color.FromArgb(231, 76, 60);
            btnRemoveImage.FlatAppearance.BorderSize = 0;
            btnRemoveImage.FlatStyle = FlatStyle.Flat;
            btnRemoveImage.Font = new Font("Segoe UI", 8F);
            btnRemoveImage.ForeColor = Color.White;
            btnRemoveImage.Location = new Point(220, 40);
            btnRemoveImage.Name = "btnRemoveImage";
            btnRemoveImage.Size = new Size(65, 28);
            btnRemoveImage.Text = "Xóa ảnh";
            btnRemoveImage.UseVisualStyleBackColor = false;
            btnRemoveImage.Click += BtnRemoveImage_Click;

            // btnViewDetail
            btnViewDetail.BackColor = Color.FromArgb(155, 89, 182);
            btnViewDetail.FlatAppearance.BorderSize = 0;
            btnViewDetail.FlatStyle = FlatStyle.Flat;
            btnViewDetail.Font = new Font("Segoe UI", 8F);
            btnViewDetail.ForeColor = Color.White;
            btnViewDetail.Location = new Point(145, 75);
            btnViewDetail.Name = "btnViewDetail";
            btnViewDetail.Size = new Size(140, 28);
            btnViewDetail.Text = "Xem chi tiết";
            btnViewDetail.UseVisualStyleBackColor = false;
            btnViewDetail.Click += BtnViewDetail_Click;

            // lblISBN
            lblISBN.AutoSize = true;
            lblISBN.Location = new Point(15, 203);
            lblISBN.Name = "lblISBN";
            lblISBN.Size = new Size(41, 20);
            lblISBN.Text = "ISBN:";

            // txtISBN
            txtISBN.Location = new Point(85, 200);
            txtISBN.Name = "txtISBN";
            txtISBN.Size = new Size(200, 27);

            // lblBookTitle
            lblBookTitle.AutoSize = true;
            lblBookTitle.Location = new Point(15, 233);
            lblBookTitle.Name = "lblBookTitle";
            lblBookTitle.Size = new Size(67, 20);
            lblBookTitle.Text = "Tên sách:";

            // txtTitle
            txtTitle.Location = new Point(85, 230);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(200, 27);

            // lblCategoryDetail
            lblCategoryDetail.AutoSize = true;
            lblCategoryDetail.Location = new Point(15, 263);
            lblCategoryDetail.Name = "lblCategoryDetail";
            lblCategoryDetail.Size = new Size(65, 20);
            lblCategoryDetail.Text = "Thể loại:";

            // cboCategoryDetail
            cboCategoryDetail.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoryDetail.Location = new Point(85, 260);
            cboCategoryDetail.Name = "cboCategoryDetail";
            cboCategoryDetail.Size = new Size(200, 28);

            // lblAuthorDetail
            lblAuthorDetail.AutoSize = true;
            lblAuthorDetail.Location = new Point(15, 293);
            lblAuthorDetail.Name = "lblAuthorDetail";
            lblAuthorDetail.Size = new Size(58, 20);
            lblAuthorDetail.Text = "Tác giả:";

            // cboAuthorDetail
            cboAuthorDetail.DropDownStyle = ComboBoxStyle.DropDownList;
            cboAuthorDetail.Location = new Point(85, 290);
            cboAuthorDetail.Name = "cboAuthorDetail";
            cboAuthorDetail.Size = new Size(200, 28);

            // lblPublisher
            lblPublisher.AutoSize = true;
            lblPublisher.Location = new Point(15, 323);
            lblPublisher.Name = "lblPublisher";
            lblPublisher.Size = new Size(40, 20);
            lblPublisher.Text = "NXB:";

            // cboPublisher
            cboPublisher.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPublisher.Location = new Point(85, 320);
            cboPublisher.Name = "cboPublisher";
            cboPublisher.Size = new Size(200, 28);

            // lblYear
            lblYear.AutoSize = true;
            lblYear.Location = new Point(15, 353);
            lblYear.Name = "lblYear";
            lblYear.Size = new Size(64, 20);
            lblYear.Text = "Năm XB:";

            // numYear
            numYear.Location = new Point(85, 350);
            numYear.Minimum = 1900;
            numYear.Maximum = 2100;
            numYear.Name = "numYear";
            numYear.Size = new Size(80, 27);
            numYear.Value = 2024;

            // lblPrice
            lblPrice.AutoSize = true;
            lblPrice.Location = new Point(15, 383);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(32, 20);
            lblPrice.Text = "Giá:";

            // numPrice
            numPrice.Location = new Point(85, 380);
            numPrice.Maximum = 10000000;
            numPrice.Name = "numPrice";
            numPrice.Size = new Size(100, 27);
            numPrice.ThousandsSeparator = true;

            // lblCopies
            lblCopies.AutoSize = true;
            lblCopies.Location = new Point(15, 413);
            lblCopies.Name = "lblCopies";
            lblCopies.Size = new Size(67, 20);
            lblCopies.Text = "Số lượng:";

            // numTotalCopies
            numTotalCopies.Location = new Point(85, 410);
            numTotalCopies.Minimum = 1;
            numTotalCopies.Maximum = 1000;
            numTotalCopies.Name = "numTotalCopies";
            numTotalCopies.Size = new Size(70, 27);
            numTotalCopies.Value = 1;

            // lblLocation
            lblLocation.AutoSize = true;
            lblLocation.Location = new Point(15, 443);
            lblLocation.Name = "lblLocation";
            lblLocation.Size = new Size(50, 20);
            lblLocation.Text = "Vị trí:";

            // txtLocation
            txtLocation.Location = new Point(85, 440);
            txtLocation.Name = "txtLocation";
            txtLocation.Size = new Size(200, 27);

            // lblDescription
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(15, 473);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(52, 20);
            lblDescription.Text = "Mô tả:";

            // txtDescription
            txtDescription.Location = new Point(85, 470);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(200, 50);

            // btnSave
            btnSave.BackColor = Color.FromArgb(46, 204, 113);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 9F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(15, 530);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(90, 32);
            btnSave.Text = "💾 Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;

            // btnCancel
            btnCancel.BackColor = Color.FromArgb(149, 165, 166);
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 9F);
            btnCancel.ForeColor = Color.White;
            btnCancel.Location = new Point(115, 530);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(90, 32);
            btnCancel.Text = "❌ Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += BtnCancel_Click;

            panelDetail.Controls.Add(lblDetailTitle);
            panelDetail.Controls.Add(panelImage);
            panelDetail.Controls.Add(btnBrowseImage);
            panelDetail.Controls.Add(btnRemoveImage);
            panelDetail.Controls.Add(btnViewDetail);
            panelDetail.Controls.Add(lblISBN);
            panelDetail.Controls.Add(txtISBN);
            panelDetail.Controls.Add(lblBookTitle);
            panelDetail.Controls.Add(txtTitle);
            panelDetail.Controls.Add(lblCategoryDetail);
            panelDetail.Controls.Add(cboCategoryDetail);
            panelDetail.Controls.Add(lblAuthorDetail);
            panelDetail.Controls.Add(cboAuthorDetail);
            panelDetail.Controls.Add(lblPublisher);
            panelDetail.Controls.Add(cboPublisher);
            panelDetail.Controls.Add(lblYear);
            panelDetail.Controls.Add(numYear);
            panelDetail.Controls.Add(lblPrice);
            panelDetail.Controls.Add(numPrice);
            panelDetail.Controls.Add(lblCopies);
            panelDetail.Controls.Add(numTotalCopies);
            panelDetail.Controls.Add(lblLocation);
            panelDetail.Controls.Add(txtLocation);
            panelDetail.Controls.Add(lblDescription);
            panelDetail.Controls.Add(txtDescription);
            panelDetail.Controls.Add(btnSave);
            panelDetail.Controls.Add(btnCancel);

            Controls.Add(lblTitle);
            Controls.Add(panelSearch);
            Controls.Add(dgvBooks);
            Controls.Add(btnAdd);
            Controls.Add(btnEdit);
            Controls.Add(btnDelete);
            Controls.Add(btnRefresh);
            Controls.Add(panelDetail);

            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBooks).EndInit();
            panelDetail.ResumeLayout(false);
            panelDetail.PerformLayout();
            panelImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picBookImage).EndInit();
            ((System.ComponentModel.ISupportInitialize)numYear).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            ((System.ComponentModel.ISupportInitialize)numTotalCopies).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}