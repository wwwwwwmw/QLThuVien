using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
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
        private readonly BookDAO bookDAO = new BookDAO();
        private List<Book> allBooks = new List<Book>();

        public FormPublic()
        {
            InitializeComponent();
            Load += FormPublic_Load;
        }

        private void FormPublic_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            StyleTopButtons();
            AdjustHighlightsLayout();
            LoadCategories();
            LoadBooks();
        }

        private void FormPublic_Resize(object? sender, EventArgs e)
        {
            AdjustHighlightsLayout();
        }

        private void StyleTopButtons()
        {
            StyleButton(btnRegister, Color.FromArgb(155, 89, 182), 16);
            StyleButton(btnLogin, Color.FromArgb(46, 204, 113), 16);
            StyleButton(btnRefresh, Color.FromArgb(52, 152, 219), 14);
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(
                "Để đăng ký thẻ thư viện, vui lòng:\n\n" +
                "1. Đến trực tiếp thư viện với CMND/CCCD\n" +
                "2. Điền đơn đăng ký\n" +
                "3. Nhận thẻ và tài khoản\n\n" +
                "📞 Liên hệ: 0123-456-789\n" +
                "📍 Địa chỉ: 123 Đường ABC, Quận XYZ",
                "Hướng dẫn đăng ký thẻ thư viện",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadBooks();
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
                allBooks = bookDAO.GetAll() ?? new List<Book>();
                DisplayBooks(allBooks);
                LoadHighlights();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tải danh sách sách: " + ex.Message + "\n\nVui lòng kiểm tra kết nối database.",
                    "Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void DisplayBooks(List<Book> books)
        {
            flowBooks.Controls.Clear();
            lblTotalBooks.Text = $"Tổng: {books.Count} sách";

            foreach (var book in books)
            {
                flowBooks.Controls.Add(CreateBookCard(book));
            }
        }

        private void LoadHighlights()
        {
            flowNewBooks.Controls.Clear();

            var latestBooks = allBooks
                .OrderByDescending(b => b.CreatedDate)
                .Take(3)
                .ToList();

            foreach (var book in latestBooks)
            {
                flowNewBooks.Controls.Add(CreateSmallBookCard(book));
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
                                var category = new Category
                                {
                                    CategoryID = reader.GetInt32(0),
                                    CategoryName = reader.GetString(1)
                                };
                                flowCategories.Controls.Add(CreateCategoryChip(category));
                            }
                        }
                    }
                }
            }
            catch
            {
            }

            AdjustHighlightsLayout();
        }

        private Control CreateSmallBookCard(Book book)
        {
            Panel card = new Panel
            {
                Size = new Size(230, 140),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 14, 0),
                Cursor = Cursors.Hand
            };

            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(
                    e.Graphics,
                    card.ClientRectangle,
                    Color.FromArgb(228, 228, 228),
                    ButtonBorderStyle.Solid
                );
            };

            PictureBox pic = new PictureBox
            {
                Size = new Size(72, 102),
                Location = new Point(12, 12),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            LoadBookImage(pic, book.ImagePath);

            if (pic.Image == null)
            {
                Label ico = new Label
                {
                    Text = "📖",
                    Font = new Font("Segoe UI", 28),
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                pic.Controls.Add(ico);
            }

            Label title = new Label
            {
                Text = book.Title ?? "",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 35, 35),
                Location = new Point(92, 14),
                Size = new Size(125, 42)
            };

            Label info = new Label
            {
                Text = (book.AuthorName ?? "") + (string.IsNullOrWhiteSpace(book.CategoryName) ? "" : $" • {book.CategoryName}"),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                Location = new Point(92, 58),
                Size = new Size(126, 34)
            };

            Label status = new Label
            {
                Text = book.AvailableCopies > 0 ? $"Còn {book.AvailableCopies} cuốn" : "Hết sách",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = book.AvailableCopies > 0 ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60),
                Location = new Point(92, 98),
                AutoSize = true
            };

            card.Controls.Add(pic);
            card.Controls.Add(title);
            card.Controls.Add(info);
            card.Controls.Add(status);

            card.Click += (s, e) => ShowBookDetail(book);
            foreach (Control c in card.Controls)
                c.Click += (s, e) => ShowBookDetail(book);

            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(248, 252, 255);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            return card;
        }

        private Control CreateCategoryChip(Category category)
        {
            var btn = new Button
            {
                Text = category.CategoryName,
                AutoSize = true,
                Padding = new Padding(14, 7, 14, 7),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(0, 0, 10, 8),
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9)
            };

            btn.FlatAppearance.BorderSize = 0;
            StyleButton(btn, Color.FromArgb(52, 152, 219), 12);

            btn.Click += (s, e) =>
            {
                for (int i = 0; i < cboCategory.Items.Count; i++)
                {
                    if (cboCategory.Items[i] is ComboBoxItem item && item.Value == category.CategoryID)
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
            if (flowNewBooks == null || flowCategories == null)
                return;

            int width = ClientSize.Width - 40;
            if (width < 600) width = 600;

            flowNewBooks.Size = new Size(width, 140);
            flowCategories.Size = new Size(width, 48);
        }

        private Panel CreateBookCard(Book book)
        {
            Panel card = new Panel
            {
                Size = new Size(195, 275),
                BackColor = Color.White,
                Margin = new Padding(0, 0, 14, 14),
                Cursor = Cursors.Hand,
                Tag = book
            };

            card.Paint += (s, e) =>
            {
                ControlPaint.DrawBorder(
                    e.Graphics,
                    card.ClientRectangle,
                    Color.FromArgb(230, 230, 230),
                    ButtonBorderStyle.Solid
                );
            };

            PictureBox picBook = new PictureBox
            {
                Size = new Size(165, 160),
                Location = new Point(15, 12),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(245, 245, 245)
            };

            LoadBookImage(picBook, book.ImagePath);

            if (picBook.Image == null)
            {
                Label lblNoImage = new Label
                {
                    Text = "📖",
                    Font = new Font("Segoe UI", 42),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill
                };
                picBook.Controls.Add(lblNoImage);
            }

            Label lblTitle = new Label
            {
                Text = book.Title ?? "",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(12, 178),
                Size = new Size(170, 26)
            };

            Label lblAuthor = new Label
            {
                Text = "✍ " + (book.AuthorName ?? "Chưa rõ"),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                Location = new Point(12, 208),
                Size = new Size(170, 18)
            };

            Label lblCategory = new Label
            {
                Text = "📁 " + (book.CategoryName ?? "Chưa phân loại"),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Color.Gray,
                Location = new Point(12, 226),
                Size = new Size(170, 18)
            };

            Label lblStatus = new Label
            {
                Text = book.AvailableCopies > 0 ? $"✅ Còn {book.AvailableCopies} cuốn" : "❌ Hết sách",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = book.AvailableCopies > 0 ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60),
                Location = new Point(12, 246),
                Size = new Size(170, 20)
            };

            card.Controls.Add(picBook);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblAuthor);
            card.Controls.Add(lblCategory);
            card.Controls.Add(lblStatus);

            card.Click += (s, e) => ShowBookDetail(book);
            foreach (Control c in card.Controls)
                c.Click += (s, e) => ShowBookDetail(book);

            card.MouseEnter += (s, e) => card.BackColor = Color.FromArgb(248, 252, 255);
            card.MouseLeave += (s, e) => card.BackColor = Color.White;

            return card;
        }

        private void LoadBookImage(PictureBox pictureBox, string? relativeImagePath)
        {
            if (string.IsNullOrWhiteSpace(relativeImagePath))
                return;

            try
            {
                string imagePath = Path.Combine(Application.StartupPath, "Images", relativeImagePath);
                if (!File.Exists(imagePath))
                    return;

                using (var img = Image.FromFile(imagePath))
                {
                    pictureBox.Image = new Bitmap(img);
                }
            }
            catch
            {
            }
        }

        private void ShowBookDetail(Book book)
        {
            using (var detailForm = new FormBookDetailPublic(book))
            {
                if (detailForm.ShowDialog() == DialogResult.Yes)
                {
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
            string searchText = (txtSearch.Text ?? string.Empty).ToLower().Trim();
            int categoryId = (cboCategory.SelectedItem as ComboBoxItem)?.Value ?? 0;

            var filtered = allBooks.FindAll(b =>
            {
                string title = (b.Title ?? string.Empty).ToLower();
                string author = (b.AuthorName ?? string.Empty).ToLower();
                string isbn = (b.ISBN ?? string.Empty).ToLower();

                bool matchSearch =
                    string.IsNullOrEmpty(searchText) ||
                    title.Contains(searchText) ||
                    author.Contains(searchText) ||
                    isbn.Contains(searchText);

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
                            Show();
                            LoadBooks();
                        }
                        else
                        {
                            CurrentUser.Logout();
                            Show();
                        }
                    };

                    Hide();
                    mainForm.Show();
                }
            }
        }

        private void StyleButton(Button btn, Color backColor, int radius)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.MouseOverBackColor = Lighten(backColor, 18);
            btn.FlatAppearance.MouseDownBackColor = Darken(backColor, 12);
            btn.Resize -= Button_ResizeApplyRegion;
            btn.Resize += Button_ResizeApplyRegion;
            ApplyRoundedCorners(btn, radius);
            btn.Tag = radius;
        }

        private void Button_ResizeApplyRegion(object? sender, EventArgs e)
        {
            if (sender is Button btn && btn.Tag is int radius)
            {
                ApplyRoundedCorners(btn, radius);
            }
        }

        private void ApplyRoundedCorners(Control c, int radius)
        {
            if (c.Width == 0 || c.Height == 0)
                return;

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

        private class ComboBoxItem
        {
            public int Value { get; set; }
            public string Text { get; set; } = "";
            public override string ToString() => Text;
        }
    }
}