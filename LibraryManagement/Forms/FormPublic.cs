using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form công khai - Cho phép xem sách không cần đăng nhập
    /// </summary>
    public partial class FormPublic : Form
    {
        private Panel panelHeader = null!;
        private Panel panelSearch = null!;
        private Panel panelBooks = null!;
        private FlowLayoutPanel flowBooks = null!;
        private Panel panelHighlights = null!;
        private Label lblNewBooks = null!;
        private FlowLayoutPanel flowNewBooks = null!;
        private Label lblCategories = null!;
        private FlowLayoutPanel flowCategories = null!;
        private TextBox txtSearch = null!;
        private ComboBox cboCategory = null!;
        private Label lblTotalBooks = null!;
        private BookDAO bookDAO = new BookDAO();
        private List<Book> allBooks = new List<Book>();

        public FormPublic()
        {
            InitializeComponent();
            this.Load += FormPublic_Load;
        }

        

        private void FormPublic_Load(object? sender, EventArgs e)
        {
            SetupForm();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            LoadCategories();
            LoadBooks();
        }

        private void SetupForm()
        {
            this.BackColor = Color.FromArgb(248, 249, 250);

            // Header Panel
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(41, 128, 185)
            };

            Label lblTitle = new Label
            {
                Text = "📚 THƯ VIỆN SÁCH - Tra cứu & Mượn sách online",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 18),
                AutoSize = true
            };

            // Nút Đăng ký
            Button btnRegister = CreateStyledButton("Đăng ký thẻ", Color.FromArgb(155, 89, 182), new Size(120, 38));
            btnRegister.Click += (s, e) =>
            {
                MessageBox.Show("Để đăng ký thẻ thư viện, vui lòng:\n\n" +
                    "1. Đến trực tiếp thư viện với CMND/CCCD\n" +
                    "2. Điền đơn đăng ký\n" +
                    "3. Nhận thẻ và tài khoản\n\n" +
                    "📞 Liên hệ: 0123-456-789\n" +
                    "📍 Địa chỉ: 123 Đường ABC, Quận XYZ",
                    "Hướng dẫn đăng ký thẻ thư viện", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            // Nút Đăng nhập (cho nhân viên/admin)
            Button btnLogin = CreateStyledButton("Đăng nhập (NV)", Color.FromArgb(46, 204, 113), new Size(140, 38));
            btnLogin.Click += BtnLogin_Click;

            panelHeader.Controls.AddRange(new Control[] { lblTitle, btnRegister, btnLogin });

            // Xử lý resize để đặt vị trí các nút
            panelHeader.Resize += (s, e) =>
            {
                btnLogin.Location = new Point(panelHeader.Width - 130, 18);
                btnRegister.Location = new Point(panelHeader.Width - 245, 18);
            };

            // Set vị trí ban đầu khi form load
            this.Load += (s, e) =>
            {
                btnLogin.Location = new Point(panelHeader.Width - 130, 18);
                btnRegister.Location = new Point(panelHeader.Width - 245, 18);
            };

            // Search Panel - sử dụng TableLayoutPanel để responsive
            panelSearch = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White
            };

            // Tạo TableLayoutPanel cho search area
            TableLayoutPanel searchLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 6,
                RowCount = 1,
                Padding = new Padding(15, 10, 15, 10)
            };
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Label tìm kiếm
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300)); // TextBox
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Label thể loại
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200)); // ComboBox
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); // Label tổng (fill)
            searchLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Button

            Label lblSearch = new Label
            {
                Text = "Tìm kiếm:",
                Font = new Font("Segoe UI", 11),
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(5, 8, 5, 0)
            };

            txtSearch = new TextBox
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill,
                PlaceholderText = "Nhập tên sách, tác giả, ISBN...",
                Margin = new Padding(5, 5, 15, 5)
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            Label lblCategory = new Label
            {
                Text = "Thể loại:",
                Font = new Font("Segoe UI", 11),
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(5, 8, 5, 0)
            };

            cboCategory = new ComboBox
            {
                Font = new Font("Segoe UI", 11),
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(5, 5, 15, 5)
            };
            cboCategory.SelectedIndexChanged += CboCategory_SelectedIndexChanged;

            lblTotalBooks = new Label
            {
                Text = "Tổng: 0 sách",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185),
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(15, 8, 15, 0)
            };

            Button btnRefresh = CreateStyledButton("Làm mới", Color.FromArgb(52, 152, 219), new Size(110, 36));
            btnRefresh.Margin = new Padding(5, 3, 10, 3);
            btnRefresh.Click += (s, e) => LoadBooks();

            searchLayout.Controls.Add(lblSearch, 0, 0);
            searchLayout.Controls.Add(txtSearch, 1, 0);
            searchLayout.Controls.Add(lblCategory, 2, 0);
            searchLayout.Controls.Add(cboCategory, 3, 0);
            searchLayout.Controls.Add(lblTotalBooks, 4, 0);
            searchLayout.Controls.Add(btnRefresh, 5, 0);

            panelSearch.Controls.Add(searchLayout);

            // Highlights Panel on top
            panelHighlights = new Panel
            {
                Dock = DockStyle.Top,
                Height = 330,
                BackColor = Color.FromArgb(248, 249, 250)
            };

            lblNewBooks = new Label
            {
                Text = "TOP SÁCH MỚI",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 10),
                AutoSize = true
            };
            panelHighlights.Controls.Add(lblNewBooks);

            flowNewBooks = new FlowLayoutPanel
            {
                Location = new Point(20, 45),
                Size = new Size(900, 170),
                AutoScroll = false,
                WrapContents = false
            };
            panelHighlights.Controls.Add(flowNewBooks);

            lblCategories = new Label
            {
                Text = "THỂ LOẠI SÁCH",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 225),
                AutoSize = true
            };
            panelHighlights.Controls.Add(lblCategories);

            flowCategories = new FlowLayoutPanel
            {
                Location = new Point(20, 260),
                Size = new Size(900, 60),
                AutoScroll = true,
                WrapContents = true
            };
            panelHighlights.Controls.Add(flowCategories);

            // Books Panel with FlowLayout
            panelBooks = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };

            flowBooks = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                WrapContents = true,
                Padding = new Padding(10)
            };

            panelBooks.Controls.Add(flowBooks);

            // Add panels in correct order
            this.Controls.Add(panelBooks);
            this.Controls.Add(panelHighlights);
            this.Controls.Add(panelSearch);
            this.Controls.Add(panelHeader);
            this.Resize += (s, e) => AdjustHighlightsLayout();
        }

        private void LoadCategories()
        {
            try
            {
                cboCategory.Items.Clear();
                cboCategory.Items.Add(new ComboBoxItem { Value = 0, Text = "-- Tất cả thể loại --" });

                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT CategoryID, CategoryName FROM Categories WHERE IsActive = 1 ORDER BY CategoryName";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cboCategory.Items.Add(new ComboBoxItem
                                {
                                    Value = reader.GetInt32(0),
                                    Text = reader.GetString(1)
                                });
                            }
                        }
                    }
                }

                cboCategory.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải thể loại: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadBooks()
        {
            try
            {
                allBooks = bookDAO.GetAll();
                DisplayBooks(allBooks);
                LoadHighlights();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sách: " + ex.Message + "\n\nVui lòng kiểm tra kết nối database.",
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayBooks(List<Book> books)
        {
            flowBooks.Controls.Clear();
            lblTotalBooks.Text = $"Tổng: {books.Count} sách";

            foreach (var book in books)
            {
                var card = CreateBookCard(book);
                flowBooks.Controls.Add(card);
            }
        }

        private void LoadHighlights()
        {
            flowNewBooks.Controls.Clear();
            var latest3 = allBooks.OrderByDescending(b => b.CreatedDate).Take(3).ToList();
            foreach (var b in latest3)
            {
                flowNewBooks.Controls.Add(CreateSmallBookCard(b));
            }

            flowCategories.Controls.Clear();
            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT CategoryID, CategoryName FROM Categories WHERE IsActive = 1 ORDER BY CategoryName";
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var cat = new Category { CategoryID = reader.GetInt32(0), CategoryName = reader.GetString(1) };
                                flowCategories.Controls.Add(CreateCategoryChip(cat));
                            }
                        }
                    }
                }
            }
            catch { }
            AdjustHighlightsLayout();
        }

        private Control CreateSmallBookCard(Book book)
        {
            Panel card = new Panel { Size = new Size(260, 160), BackColor = Color.White, Margin = new Padding(10), Cursor = Cursors.Hand };
            ApplyRoundedCorners(card, 10);
            card.Paint += (s, e) => ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle, Color.FromArgb(230, 230, 230), ButtonBorderStyle.Solid);

            PictureBox pic = new PictureBox { Size = new Size(110, 140), Location = new Point(15, 10), SizeMode = PictureBoxSizeMode.Zoom, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            if (!string.IsNullOrEmpty(book.ImagePath))
            {
                string path = Path.Combine(Application.StartupPath, "Images", book.ImagePath);
                if (File.Exists(path)) { try { pic.Image = Image.FromFile(path); } catch { } }
            }
            if (pic.Image == null)
            {
                Label ico = new Label { Text = "📖", Font = new Font("Segoe UI", 36), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
                pic.Controls.Add(ico);
            }
            card.Controls.Add(pic);

            Label title = new Label { Text = book.Title, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(130, 12), Size = new Size(120, 40) };
            card.Controls.Add(title);
            Label info = new Label { Text = (book.AuthorName ?? "") + (book.CategoryName != null ? $" • {book.CategoryName}" : ""), Font = new Font("Segoe UI", 9), ForeColor = Color.Gray, Location = new Point(130, 55), Size = new Size(120, 20) };
            card.Controls.Add(info);
            Label status = new Label { Text = book.AvailableCopies > 0 ? $"Còn {book.AvailableCopies} cuốn" : "Hết sách", Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = book.AvailableCopies > 0 ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60), Location = new Point(130, 78), AutoSize = true };
            card.Controls.Add(status);

            card.Click += (s, e) => ShowBookDetail(book);
            foreach (Control c in card.Controls) c.Click += (s, e) => ShowBookDetail(book);
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(240, 248, 255);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;
            return card;
        }

        private Control CreateCategoryChip(Category category)
        {
            var btn = new Button { Text = category.CategoryName, AutoSize = true, Padding = new Padding(12, 6, 12, 6), BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Margin = new Padding(6) };
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;
            btn.Click += (s, e) =>
            {
                for (int i = 0; i < cboCategory.Items.Count; i++)
                {
                    if ((cboCategory.Items[i] is ComboBoxItem ci) && ci.Value == category.CategoryID)
                    {
                        cboCategory.SelectedIndex = i;
                        break;
                    }
                }
            };
            return btn;
        }

        private void AdjustHighlightsLayout()
        {
            if (panelHighlights == null) return;
            int width = panelBooks?.ClientSize.Width > 0 ? panelBooks.ClientSize.Width : this.ClientSize.Width;
            flowNewBooks.Size = new Size(width - 60, flowNewBooks.Height);
            flowCategories.Size = new Size(width - 60, flowCategories.Height);
        }

        private Panel CreateBookCard(Book book)
        {
            Panel card = new Panel
            {
                Size = new Size(220, 320),
                BackColor = Color.White,
                Margin = new Padding(10),
                Cursor = Cursors.Hand,
                Tag = book
            };
            ApplyRoundedCorners(card, 10);

            // Border
            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(e.Graphics, card.ClientRectangle,
                    Color.FromArgb(230, 230, 230), ButtonBorderStyle.Solid);
            };

            // Book Image
            PictureBox picBook = new PictureBox
            {
                Size = new Size(200, 180),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            // Load image
            if (!string.IsNullOrEmpty(book.ImagePath))
            {
                string imagePath = Path.Combine(Application.StartupPath, "Images", book.ImagePath);
                if (File.Exists(imagePath))
                {
                    try
                    {
                        picBook.Image = Image.FromFile(imagePath);
                    }
                    catch
                    {
                        picBook.Image = null;
                    }
                }
            }

            if (picBook.Image == null)
            {
                // Default book icon
                Label lblNoImage = new Label
                {
                    Text = "📖",
                    Font = new Font("Segoe UI", 48),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                picBook.Controls.Add(lblNoImage);
            }

            // Title
            Label lblTitle = new Label
            {
                Text = book.Title,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(10, 195),
                Size = new Size(200, 40),
                MaximumSize = new Size(200, 40)
            };

            // Author
            Label lblAuthor = new Label
            {
                Text = "✍️ " + (book.AuthorName ?? "Chưa rõ"),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(10, 235),
                Size = new Size(200, 20)
            };

            // Category
            Label lblCategory = new Label
            {
                Text = "📁 " + (book.CategoryName ?? "Chưa phân loại"),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(10, 255),
                Size = new Size(200, 20)
            };

            // Status
            Label lblStatus = new Label
            {
                Text = book.AvailableCopies > 0 ? $"✅ Còn {book.AvailableCopies} cuốn" : "❌ Hết sách",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = book.AvailableCopies > 0 ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60),
                Location = new Point(10, 280),
                Size = new Size(200, 25)
            };

            card.Controls.AddRange(new Control[] { picBook, lblTitle, lblAuthor, lblCategory, lblStatus });

            // Click event to show details
            card.Click += (s, e) => ShowBookDetail(book);
            foreach (Control c in card.Controls)
            {
                c.Click += (s, e) => ShowBookDetail(book);
            }

            // Hover effect
            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(240, 248, 255);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            return card;
        }

        private void ShowBookDetail(Book book)
        {
            using (var detailForm = new FormBookDetailPublic(book))
            {
                if (detailForm.ShowDialog() == DialogResult.Yes)
                {
                    // User wants to borrow -> need login
                    BtnLogin_Click(null, null);
                }
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            FilterBooks();
        }

        private void CboCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            FilterBooks();
        }

        private void FilterBooks()
        {
            string searchText = txtSearch.Text.ToLower().Trim();
            int categoryId = (cboCategory.SelectedItem as ComboBoxItem)?.Value ?? 0;

            var filtered = allBooks.FindAll(b =>
            {
                bool matchSearch = string.IsNullOrEmpty(searchText) ||
                    b.Title.ToLower().Contains(searchText) ||
                    (b.AuthorName?.ToLower().Contains(searchText) ?? false) ||
                    (b.ISBN?.ToLower().Contains(searchText) ?? false);

                bool matchCategory = categoryId == 0 || b.CategoryID == categoryId;

                return matchSearch && matchCategory;
            });

            DisplayBooks(filtered);
        }

        private void BtnLogin_Click(object? sender, EventArgs? e)
        {
            using (var loginForm = new FormLogin())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    var mainForm = new FormMain();
                    mainForm.FormClosed += (s2, e2) =>
                    {
                        if (CurrentUser.User == null)
                        {
                            this.Show();
                            LoadBooks();
                        }
                        else
                        {
                            CurrentUser.Logout();
                            this.Show();
                        }
                    };
                    this.Hide();
                    mainForm.Show();
                }
            }
        }

        // Helper class for ComboBox
        private class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; } = "";
            public override string ToString() => Text;
        }
    }

    /// <summary>
    /// Form xem chi tiết sách (công khai)
    /// </summary>
    public partial class FormBookDetailPublic : Form
    {
        private Book book;

        public FormBookDetailPublic(Book book)
        {
            this.book = book;
            InitializeComponent();
            LoadBookInfo();
        }

        

        private void LoadBookInfo()
        {
            // === LEFT PANEL - Image and Status ===
            Panel panelLeft = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(200, this.ClientSize.Height),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            PictureBox picBook = new PictureBox
            {
                Size = new Size(170, 220),
                Location = new Point(15, 20),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            if (!string.IsNullOrEmpty(book.ImagePath))
            {
                string imagePath = Path.Combine(Application.StartupPath, "Images", book.ImagePath);
                if (File.Exists(imagePath))
                {
                    try { picBook.Image = Image.FromFile(imagePath); }
                    catch { }
                }
            }

            if (picBook.Image == null)
            {
                Label lblNoImg = new Label
                {
                    Text = "📖",
                    Font = new Font("Segoe UI", 40),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                picBook.Controls.Add(lblNoImg);
            }
            panelLeft.Controls.Add(picBook);

            // Status label
            string statusText = book.AvailableCopies > 0
                ? $"✅ Còn sách"
                : $"❌ Hết sách";
            Color statusColor = book.AvailableCopies > 0 ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60);

            Label lblStatus = new Label
            {
                Text = statusText,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = statusColor,
                Location = new Point(15, 250),
                AutoSize = true
            };
            panelLeft.Controls.Add(lblStatus);

            Label lblQuantity = new Label
            {
                Text = $"Số lượng: {book.AvailableCopies}/{book.TotalCopies} bản",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(15, 275),
                AutoSize = true
            };
            panelLeft.Controls.Add(lblQuantity);

            this.Controls.Add(panelLeft);

            // === RIGHT PANEL - Book Info ===
            Panel panelRight = new Panel
            {
                Location = new Point(200, 0),
                Size = new Size(485, this.ClientSize.Height - 60),
                BackColor = Color.White,
                Padding = new Padding(15),
                AutoScroll = true
            };

            int y = 15;

            // Title
            Label lblBookTitle = new Label
            {
                Text = book.Title,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(10, y),
                MaximumSize = new Size(450, 50),
                AutoSize = true
            };
            panelRight.Controls.Add(lblBookTitle);
            y += lblBookTitle.PreferredHeight + 15;

            // Info rows
            AddInfoRow("🔢 ISBN:", book.ISBN ?? "N/A", ref y, panelRight);
            AddInfoRow("✍️ Tác giả:", book.AuthorName ?? "Chưa rõ", ref y, panelRight);
            AddInfoRow("📁 Thể loại:", book.CategoryName ?? "Chưa phân loại", ref y, panelRight);
            AddInfoRow("🏢 NXB:", book.PublisherName ?? "N/A", ref y, panelRight);
            AddInfoRow("📅 Năm XB:", book.PublishYear?.ToString() ?? "N/A", ref y, panelRight);
            AddInfoRow("📍 Vị trí:", book.Location ?? "N/A", ref y, panelRight);
            AddInfoRow("💰 Giá trị:", book.Price > 0 ? book.Price.ToString("N0") + " đ" : "N/A", ref y, panelRight);

            y += 10;

            // Description
            Label lblDescTitle = new Label
            {
                Text = "📝 Mô tả:",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(10, y),
                AutoSize = true
            };
            panelRight.Controls.Add(lblDescTitle);
            y += 22;

            TextBox txtDesc = new TextBox
            {
                Text = string.IsNullOrEmpty(book.Description) ? "Chưa có mô tả" : book.Description,
                Font = new Font("Segoe UI", 9),
                Location = new Point(10, y),
                Size = new Size(450, 60),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(250, 250, 250)
            };
            panelRight.Controls.Add(txtDesc);
            y += 70;

            // Fee note
            Label lblFeeNote = new Label
            {
                Text = "💡 Mượn sách MIỄN PHÍ | Chỉ thu phí khi trả trễ",
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.FromArgb(41, 128, 185),
                Location = new Point(10, y),
                AutoSize = true
            };
            panelRight.Controls.Add(lblFeeNote);

            this.Controls.Add(panelRight);

            // === BOTTOM PANEL - Buttons ===
            Panel panelBottom = new Panel
            {
                Location = new Point(200, this.ClientSize.Height - 60),
                Size = new Size(485, 60),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Button btnBorrow = new Button
            {
                Text = "Đăng nhập để mượn sách",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(220, 40),
                Location = new Point(15, 10),
                Enabled = book.AvailableCopies > 0
            };
            StyleButton(btnBorrow, Color.FromArgb(46, 204, 113), 20);
            btnBorrow.Click += (s, e) => { this.DialogResult = DialogResult.Yes; this.Close(); };

            Button btnClose = new Button
            {
                Text = "Đóng",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Size = new Size(100, 40),
                Location = new Point(245, 10)
            };
            StyleButton(btnClose, Color.FromArgb(52, 152, 219), 20);
            btnClose.Click += (s, e) => this.Close();

            panelBottom.Controls.AddRange(new Control[] { btnBorrow, btnClose });
            this.Controls.Add(panelBottom);
        }

        private void AddInfoRow(string label, string value, ref int y, Panel parent)
        {
            Label lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(10, y),
                Size = new Size(90, 22)
            };
            parent.Controls.Add(lblLabel);

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(100, y),
                MaximumSize = new Size(350, 30),
                AutoSize = true
            };
            parent.Controls.Add(lblValue);

            y += 25;
        }

        private void StyleButton(Button btn, Color backColor, int radius)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(12, 0, 12, 0);
            btn.FlatAppearance.MouseOverBackColor = Lighten(backColor, 20);
            btn.FlatAppearance.MouseDownBackColor = Darken(backColor, 15);
            btn.Resize += (s, e) => ApplyRoundedCorners(btn, radius);
        }

        private void ApplyRoundedCorners(Control c, int radius)
        {
            if (c.Width == 0 || c.Height == 0) return;
            using (var path = new GraphicsPath())
            {
                int r = radius;
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(c.Width - r, 0, r, r, 270, 90);
                path.AddArc(c.Width - r, c.Height - r, r, r, 0, 90);
                path.AddArc(0, c.Height - r, r, r, 90, 90);
                path.CloseAllFigures();
                c.Region = new Region(path);
            }
        }

        private Color Lighten(Color color, int amount)
        {
            int r = Math.Min(255, color.R + amount);
            int g = Math.Min(255, color.G + amount);
            int b = Math.Min(255, color.B + amount);
            return Color.FromArgb(r, g, b);
        }

        private Color Darken(Color color, int amount)
        {
            int r = Math.Max(0, color.R - amount);
            int g = Math.Max(0, color.G - amount);
            int b = Math.Max(0, color.B - amount);
            return Color.FromArgb(r, g, b);
        }
    }

    partial class FormPublic
    {
        private Button CreateStyledButton(string text, Color backColor, Size size)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = size,
                Cursor = Cursors.Hand,
                Padding = new Padding(12, 0, 12, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Lighten(backColor, 20);
            btn.FlatAppearance.MouseDownBackColor = Darken(backColor, 15);
            btn.Resize += (s, e) => ApplyRoundedCorners(btn, 18);
            return btn;
        }

        private void ApplyRoundedCorners(Control c, int radius)
        {
            if (c.Width == 0 || c.Height == 0) return;
            using (var path = new GraphicsPath())
            {
                int r = radius;
                path.AddArc(0, 0, r, r, 180, 90);
                path.AddArc(c.Width - r, 0, r, r, 270, 90);
                path.AddArc(c.Width - r, c.Height - r, r, r, 0, 90);
                path.AddArc(0, c.Height - r, r, r, 90, 90);
                path.CloseAllFigures();
                c.Region = new Region(path);
            }
        }

        private Color Lighten(Color color, int amount)
        {
            int r = Math.Min(255, color.R + amount);
            int g = Math.Min(255, color.G + amount);
            int b = Math.Min(255, color.B + amount);
            return Color.FromArgb(r, g, b);
        }

        private Color Darken(Color color, int amount)
        {
            int r = Math.Max(0, color.R - amount);
            int g = Math.Max(0, color.G - amount);
            int b = Math.Max(0, color.B - amount);
            return Color.FromArgb(r, g, b);
        }
    }
}
