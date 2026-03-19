using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormPublic
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelHeader;
        private Panel panelSearch;
        private Panel panelBooks;
        private FlowLayoutPanel flowBooks;
        private Panel panelHighlights;
        private Label lblNewBooks;
        private FlowLayoutPanel flowNewBooks;
        private Label lblCategories;
        private FlowLayoutPanel flowCategories;
        private TextBox txtSearch;
        private ComboBox cboCategory;
        private Label lblTotalBooks;

        private Label lblHeaderTitle;
        private Button btnRegister;
        private Button btnLogin;
        private Button btnRefresh;
        private Label lblSearch;
        private Label lblCategory;

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
            panelHeader = new Panel();
            lblHeaderTitle = new Label();
            btnRegister = new Button();
            btnLogin = new Button();
            panelSearch = new Panel();
            lblSearch = new Label();
            txtSearch = new TextBox();
            lblCategory = new Label();
            cboCategory = new ComboBox();
            lblTotalBooks = new Label();
            btnRefresh = new Button();
            panelHighlights = new Panel();
            lblNewBooks = new Label();
            flowNewBooks = new FlowLayoutPanel();
            lblCategories = new Label();
            flowCategories = new FlowLayoutPanel();
            panelBooks = new Panel();
            flowBooks = new FlowLayoutPanel();
            panelHeader.SuspendLayout();
            panelSearch.SuspendLayout();
            panelHighlights.SuspendLayout();
            panelBooks.SuspendLayout();
            SuspendLayout();
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(41, 128, 185);
            panelHeader.Controls.Add(lblHeaderTitle);
            panelHeader.Controls.Add(btnRegister);
            panelHeader.Controls.Add(btnLogin);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Margin = new Padding(4, 5, 4, 5);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1829, 103);
            panelHeader.TabIndex = 3;
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.Location = new Point(31, 23);
            lblHeaderTitle.Margin = new Padding(4, 0, 4, 0);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(747, 45);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "📚 THƯ VIỆN SÁCH - Tra cứu  Mượn sách online";
            // 
            // btnRegister
            // 
            btnRegister.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRegister.BackColor = Color.FromArgb(155, 89, 182);
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRegister.ForeColor = Color.White;
            btnRegister.Location = new Point(1402, 21);
            btnRegister.Margin = new Padding(4, 5, 4, 5);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(140, 60);
            btnRegister.TabIndex = 1;
            btnRegister.Text = "Đăng ký";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += BtnRegister_Click;
            // 
            // btnLogin
            // 
            btnLogin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnLogin.BackColor = Color.FromArgb(46, 204, 113);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(1590, 23);
            btnLogin.Margin = new Padding(4, 5, 4, 5);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(140, 60);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += BtnLogin_Click;
            // 
            // panelSearch
            // 
            panelSearch.BackColor = Color.White;
            panelSearch.Controls.Add(lblSearch);
            panelSearch.Controls.Add(txtSearch);
            panelSearch.Controls.Add(lblCategory);
            panelSearch.Controls.Add(cboCategory);
            panelSearch.Controls.Add(lblTotalBooks);
            panelSearch.Controls.Add(btnRefresh);
            panelSearch.Dock = DockStyle.Top;
            panelSearch.Location = new Point(0, 103);
            panelSearch.Margin = new Padding(4, 5, 4, 5);
            panelSearch.Name = "panelSearch";
            panelSearch.Size = new Size(1829, 87);
            panelSearch.TabIndex = 2;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Font = new Font("Segoe UI", 10.5F);
            lblSearch.Location = new Point(26, 27);
            lblSearch.Margin = new Padding(4, 0, 4, 0);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(102, 30);
            lblSearch.TabIndex = 0;
            lblSearch.Text = "Tìm kiếm:";
            // 
            // txtSearch
            // 
            txtSearch.Font = new Font("Segoe UI", 10.5F);
            txtSearch.Location = new Point(137, 20);
            txtSearch.Margin = new Padding(4, 5, 4, 5);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Nhập tên sách, tác giả, ISBN...";
            txtSearch.Size = new Size(384, 35);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Font = new Font("Segoe UI", 10.5F);
            lblCategory.Location = new Point(554, 27);
            lblCategory.Margin = new Padding(4, 0, 4, 0);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(91, 30);
            lblCategory.TabIndex = 2;
            lblCategory.Text = "Thể loại:";
            // 
            // cboCategory
            // 
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategory.Font = new Font("Segoe UI", 10.5F);
            cboCategory.Location = new Point(651, 20);
            cboCategory.Margin = new Padding(4, 5, 4, 5);
            cboCategory.Name = "cboCategory";
            cboCategory.Size = new Size(270, 38);
            cboCategory.TabIndex = 3;
            cboCategory.SelectedIndexChanged += CboCategory_SelectedIndexChanged;
            // 
            // lblTotalBooks
            // 
            lblTotalBooks.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTotalBooks.AutoSize = true;
            lblTotalBooks.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            lblTotalBooks.ForeColor = Color.FromArgb(41, 128, 185);
            lblTotalBooks.Location = new Point(1379, 27);
            lblTotalBooks.Margin = new Padding(4, 0, 4, 0);
            lblTotalBooks.Name = "lblTotalBooks";
            lblTotalBooks.Size = new Size(137, 30);
            lblTotalBooks.TabIndex = 4;
            lblTotalBooks.Text = "Tổng: 0 sách";
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.BackColor = Color.FromArgb(52, 152, 219);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(1697, 17);
            btnRefresh.Margin = new Padding(4, 5, 4, 5);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(140, 53);
            btnRefresh.TabIndex = 5;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // panelHighlights
            // 
            panelHighlights.BackColor = Color.FromArgb(248, 249, 250);
            panelHighlights.Controls.Add(lblNewBooks);
            panelHighlights.Controls.Add(flowNewBooks);
            panelHighlights.Controls.Add(lblCategories);
            panelHighlights.Controls.Add(flowCategories);
            panelHighlights.Dock = DockStyle.Top;
            panelHighlights.Location = new Point(0, 190);
            panelHighlights.Margin = new Padding(4, 5, 4, 5);
            panelHighlights.Name = "panelHighlights";
            panelHighlights.Size = new Size(1829, 382);
            panelHighlights.TabIndex = 1;
            // 
            // lblNewBooks
            // 
            lblNewBooks.AutoSize = true;
            lblNewBooks.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblNewBooks.ForeColor = Color.FromArgb(44, 62, 80);
            lblNewBooks.Location = new Point(29, 13);
            lblNewBooks.Margin = new Padding(4, 0, 4, 0);
            lblNewBooks.Name = "lblNewBooks";
            lblNewBooks.Size = new Size(218, 38);
            lblNewBooks.TabIndex = 0;
            lblNewBooks.Text = "TOP SÁCH MỚI";
            // 
            // flowNewBooks
            // 
            flowNewBooks.AutoScroll = true;
            flowNewBooks.BackColor = Color.Transparent;
            flowNewBooks.Location = new Point(29, 63);
            flowNewBooks.Margin = new Padding(4, 5, 4, 5);
            flowNewBooks.Name = "flowNewBooks";
            flowNewBooks.Size = new Size(1743, 233);
            flowNewBooks.TabIndex = 1;
            flowNewBooks.WrapContents = false;
            // 
            // lblCategories
            // 
            lblCategories.AutoSize = true;
            lblCategories.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblCategories.ForeColor = Color.FromArgb(44, 62, 80);
            lblCategories.Location = new Point(31, 330);
            lblCategories.Margin = new Padding(4, 0, 4, 0);
            lblCategories.Name = "lblCategories";
            lblCategories.Size = new Size(222, 38);
            lblCategories.TabIndex = 2;
            lblCategories.Text = "THỂ LOẠI SÁCH";
            // 
            // flowCategories
            // 
            flowCategories.AutoScroll = true;
            flowCategories.BackColor = Color.Transparent;
            flowCategories.Location = new Point(261, 325);
            flowCategories.Margin = new Padding(4, 5, 4, 5);
            flowCategories.Name = "flowCategories";
            flowCategories.Size = new Size(1507, 47);
            flowCategories.TabIndex = 3;
            flowCategories.WrapContents = false;
            // 
            // panelBooks
            // 
            panelBooks.Controls.Add(flowBooks);
            panelBooks.Dock = DockStyle.Fill;
            panelBooks.Location = new Point(0, 572);
            panelBooks.Margin = new Padding(4, 5, 4, 5);
            panelBooks.Name = "panelBooks";
            panelBooks.Padding = new Padding(29, 13, 29, 20);
            panelBooks.Size = new Size(1829, 628);
            panelBooks.TabIndex = 0;
            // 
            // flowBooks
            // 
            flowBooks.AutoScroll = true;
            flowBooks.Dock = DockStyle.Fill;
            flowBooks.Location = new Point(29, 13);
            flowBooks.Margin = new Padding(4, 5, 4, 5);
            flowBooks.Name = "flowBooks";
            flowBooks.Size = new Size(1771, 595);
            flowBooks.TabIndex = 0;
            // 
            // FormPublic
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(248, 249, 250);
            ClientSize = new Size(1829, 1200);
            Controls.Add(panelBooks);
            Controls.Add(panelHighlights);
            Controls.Add(panelSearch);
            Controls.Add(panelHeader);
            DoubleBuffered = true;
            Margin = new Padding(4, 5, 4, 5);
            Name = "FormPublic";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "📚 Thư Viện Sách - Tra cứu công khai";
            WindowState = FormWindowState.Maximized;
            Resize += FormPublic_Resize;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            panelSearch.ResumeLayout(false);
            panelSearch.PerformLayout();
            panelHighlights.ResumeLayout(false);
            panelHighlights.PerformLayout();
            panelBooks.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}