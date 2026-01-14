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
    /// Form quản lý sách
    /// </summary>
    public partial class FormBookManagement : Form
    {
        private DataGridView dgvBooks = null!;
        private TextBox txtSearch = null!;
        private ComboBox cboCategory = null!;
        private ComboBox cboAuthor = null!;
        private CheckBox chkAvailableOnly = null!;

        // Detail fields
        private TextBox txtISBN = null!;
        private TextBox txtTitle = null!;
        private ComboBox cboCategoryDetail = null!;
        private ComboBox cboAuthorDetail = null!;
        private ComboBox cboPublisher = null!;
        private NumericUpDown numYear = null!;
        private NumericUpDown numPrice = null!;
        private NumericUpDown numTotalCopies = null!;
        private TextBox txtLocation = null!;
        private TextBox txtDescription = null!;

        // Image controls
        private PictureBox picBookImage = null!;
        private Button btnBrowseImage = null!;
        private Button btnRemoveImage = null!;
        private string? currentImagePath = null;
        private string imagesFolder = Path.Combine(Application.StartupPath, "Images");

        private BookDAO bookDAO = new BookDAO();
        private CategoryDAO categoryDAO = new CategoryDAO();
        private AuthorDAO authorDAO = new AuthorDAO();
        private PublisherDAO publisherDAO = new PublisherDAO();

        private Book? currentBook;

        public FormBookManagement()
        {
            InitializeComponent();
            this.Load += FormBookManagement_Load;
        }

        private void FormBookManagement_Load(object? sender, EventArgs e)
        {
            SetupForm();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                EnsureImagesFolderExists();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void EnsureImagesFolderExists()
        {
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }
        }

        

        private void SetupForm()
        {
            // Title
            var lblTitle = new Label
            {
                Text = "📚 QUẢN LÝ SÁCH",
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
                Size = new Size(200, 28),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Tìm kiếm sách..."
            };
            txtSearch.TextChanged += (s, e) => SearchBooks();

            var lblCategory = new Label { Text = "Thể loại:", Location = new Point(220, 15), AutoSize = true };
            cboCategory = new ComboBox
            {
                Location = new Point(280, 12),
                Size = new Size(150, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboCategory.SelectedIndexChanged += (s, e) => SearchBooks();

            var lblAuthor = new Label { Text = "Tác giả:", Location = new Point(440, 15), AutoSize = true };
            cboAuthor = new ComboBox
            {
                Location = new Point(500, 12),
                Size = new Size(150, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboAuthor.SelectedIndexChanged += (s, e) => SearchBooks();

            chkAvailableOnly = new CheckBox
            {
                Text = "Chỉ còn sách",
                Location = new Point(670, 14),
                AutoSize = true
            };
            chkAvailableOnly.CheckedChanged += (s, e) => SearchBooks();

            panelSearch.Controls.AddRange(new Control[] { txtSearch, lblCategory, cboCategory, lblAuthor, cboAuthor, chkAvailableOnly });
            this.Controls.Add(panelSearch);

            // DataGridView
            dgvBooks = new DataGridView
            {
                Location = new Point(20, 110),
                Size = new Size(800, 350),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            };
            dgvBooks.SelectionChanged += DgvBooks_SelectionChanged;
            dgvBooks.CellDoubleClick += (s, e) => EditBook();

            dgvBooks.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            dgvBooks.Columns.Add("BookID", "ID");
            dgvBooks.Columns.Add("ISBN", "ISBN");
            dgvBooks.Columns.Add("Title", "Tên sách");
            dgvBooks.Columns.Add("CategoryName", "Thể loại");
            dgvBooks.Columns.Add("AuthorName", "Tác giả");
            dgvBooks.Columns.Add("TotalCopies", "Tổng");
            dgvBooks.Columns.Add("AvailableCopies", "Còn");
            dgvBooks.Columns.Add("Location", "Vị trí");

            dgvBooks.Columns["BookID"]!.Visible = false;
            dgvBooks.Columns["ISBN"]!.Width = 100;
            dgvBooks.Columns["Title"]!.Width = 250;
            dgvBooks.Columns["CategoryName"]!.Width = 100;
            dgvBooks.Columns["AuthorName"]!.Width = 120;
            dgvBooks.Columns["TotalCopies"]!.Width = 60;
            dgvBooks.Columns["AvailableCopies"]!.Width = 60;
            dgvBooks.Columns["Location"]!.Width = 80;

            this.Controls.Add(dgvBooks);

            // Buttons
            int btnY = 470;

            var btnAdd = CreateButton("Thêm mới", 20, btnY, Color.FromArgb(46, 204, 113));
            btnAdd.Click += (s, e) => AddBook();
            this.Controls.Add(btnAdd);

            var btnEdit = CreateButton("Sửa", 130, btnY, Color.FromArgb(52, 152, 219));
            btnEdit.Click += (s, e) => EditBook();
            this.Controls.Add(btnEdit);

            var btnDelete = CreateButton("Xóa", 220, btnY, Color.FromArgb(231, 76, 60));
            btnDelete.Click += (s, e) => DeleteBook();
            this.Controls.Add(btnDelete);

            var btnRefresh = CreateButton("Làm mới", 310, btnY, Color.FromArgb(149, 165, 166));
            btnRefresh.Click += (s, e) => LoadData();
            this.Controls.Add(btnRefresh);

            // Detail panel - with scroll support
            var panelDetail = new Panel
            {
                Location = new Point(840, 50),
                Size = new Size(380, 530),
                BackColor = Color.White,
                AutoScroll = true
            };

            var lblDetailTitle = new Label
            {
                Text = "Thông tin sách",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(15, 10),
                AutoSize = true
            };
            panelDetail.Controls.Add(lblDetailTitle);

            // Book Image Panel - smaller
            var panelImage = new Panel
            {
                Location = new Point(15, 40),
                Size = new Size(120, 150),
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.FixedSingle
            };

            picBookImage = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(245, 245, 245),
                Cursor = Cursors.Hand
            };
            picBookImage.Click += (s, e) => BrowseImage();
            picBookImage.Paint += PicBookImage_Paint;
            panelImage.Controls.Add(picBookImage);
            panelDetail.Controls.Add(panelImage);

            // Image buttons - repositioned
            btnBrowseImage = new Button
            {
                Text = "Chọn ảnh",
                Location = new Point(145, 40),
                Size = new Size(70, 28),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8),
                Cursor = Cursors.Hand
            };
            btnBrowseImage.FlatAppearance.BorderSize = 0;
            btnBrowseImage.Click += (s, e) => BrowseImage();
            panelDetail.Controls.Add(btnBrowseImage);

            btnRemoveImage = new Button
            {
                Text = "Xóa ảnh",
                Location = new Point(220, 40),
                Size = new Size(65, 28),
                BackColor = Color.FromArgb(231, 76, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8),
                Cursor = Cursors.Hand
            };
            btnRemoveImage.FlatAppearance.BorderSize = 0;
            btnRemoveImage.Click += (s, e) => RemoveImage();
            panelDetail.Controls.Add(btnRemoveImage);

            // View detail button
            var btnViewDetail = new Button
            {
                Text = "Xem chi tiết",
                Location = new Point(145, 75),
                Size = new Size(140, 28),
                BackColor = Color.FromArgb(155, 89, 182),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8),
                Cursor = Cursors.Hand
            };
            btnViewDetail.FlatAppearance.BorderSize = 0;
            btnViewDetail.Click += (s, e) => ShowBookDetail();
            panelDetail.Controls.Add(btnViewDetail);

            int detailY = 200;
            int labelWidth = 70;
            int inputWidth = 200;

            AddDetailLabel("ISBN:", 15, detailY, panelDetail);
            txtISBN = AddDetailTextBox(labelWidth + 15, detailY, inputWidth, panelDetail);
            detailY += 30;

            AddDetailLabel("Tên sách:", 15, detailY, panelDetail);
            txtTitle = AddDetailTextBox(labelWidth + 15, detailY, inputWidth, panelDetail);
            detailY += 30;

            AddDetailLabel("Thể loại:", 15, detailY, panelDetail);
            cboCategoryDetail = new ComboBox
            {
                Location = new Point(labelWidth + 15, detailY),
                Size = new Size(inputWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelDetail.Controls.Add(cboCategoryDetail);
            detailY += 30;

            AddDetailLabel("Tác giả:", 15, detailY, panelDetail);
            cboAuthorDetail = new ComboBox
            {
                Location = new Point(labelWidth + 15, detailY),
                Size = new Size(inputWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelDetail.Controls.Add(cboAuthorDetail);
            detailY += 30;

            AddDetailLabel("NXB:", 15, detailY, panelDetail);
            cboPublisher = new ComboBox
            {
                Location = new Point(labelWidth + 15, detailY),
                Size = new Size(inputWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panelDetail.Controls.Add(cboPublisher);
            detailY += 30;

            AddDetailLabel("Năm XB:", 15, detailY, panelDetail);
            numYear = new NumericUpDown
            {
                Location = new Point(labelWidth + 15, detailY),
                Size = new Size(80, 25),
                Minimum = 1900,
                Maximum = DateTime.Now.Year,
                Value = DateTime.Now.Year
            };
            panelDetail.Controls.Add(numYear);
            detailY += 30;

            AddDetailLabel("Giá:", 15, detailY, panelDetail);
            numPrice = new NumericUpDown
            {
                Location = new Point(labelWidth + 15, detailY),
                Size = new Size(100, 25),
                Minimum = 0,
                Maximum = 10000000,
                ThousandsSeparator = true
            };
            panelDetail.Controls.Add(numPrice);
            detailY += 30;

            AddDetailLabel("Số lượng:", 15, detailY, panelDetail);
            numTotalCopies = new NumericUpDown
            {
                Location = new Point(labelWidth + 15, detailY),
                Size = new Size(70, 25),
                Minimum = 1,
                Maximum = 1000,
                Value = 1
            };
            panelDetail.Controls.Add(numTotalCopies);
            detailY += 30;

            AddDetailLabel("Vị trí:", 15, detailY, panelDetail);
            txtLocation = AddDetailTextBox(labelWidth + 15, detailY, inputWidth, panelDetail);
            detailY += 30;

            AddDetailLabel("Mô tả:", 15, detailY, panelDetail);
            txtDescription = new TextBox
            {
                Location = new Point(labelWidth + 15, detailY),
                Size = new Size(inputWidth, 50),
                Multiline = true
            };
            panelDetail.Controls.Add(txtDescription);
            detailY += 60;

            // Save/Cancel buttons
            var btnSave = CreateButton("💾 Lưu", 15, detailY, Color.FromArgb(46, 204, 113));
            btnSave.Size = new Size(90, 32);
            btnSave.Click += BtnSave_Click;
            panelDetail.Controls.Add(btnSave);

            var btnCancel = CreateButton("❌ Hủy", 115, detailY, Color.FromArgb(149, 165, 166));
            btnCancel.Size = new Size(90, 32);
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
                Size = new Size(width, 28),
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
            try
            {
                // Load categories
                var categories = categoryDAO.GetAll();
                cboCategory.Items.Clear();
                cboCategory.Items.Add(new Category { CategoryID = 0, CategoryName = "-- Tất cả --" });
                cboCategory.Items.AddRange(categories.ToArray());
                cboCategory.DisplayMember = "CategoryName";
                cboCategory.ValueMember = "CategoryID";
                cboCategory.SelectedIndex = 0;

                cboCategoryDetail.Items.Clear();
                cboCategoryDetail.Items.AddRange(categories.ToArray());
                cboCategoryDetail.DisplayMember = "CategoryName";
                cboCategoryDetail.ValueMember = "CategoryID";

                // Load authors
                var authors = authorDAO.GetAll();
                cboAuthor.Items.Clear();
                cboAuthor.Items.Add(new Author { AuthorID = 0, AuthorName = "-- Tất cả --" });
                cboAuthor.Items.AddRange(authors.ToArray());
                cboAuthor.DisplayMember = "AuthorName";
                cboAuthor.ValueMember = "AuthorID";
                cboAuthor.SelectedIndex = 0;

                cboAuthorDetail.Items.Clear();
                cboAuthorDetail.Items.AddRange(authors.ToArray());
                cboAuthorDetail.DisplayMember = "AuthorName";
                cboAuthorDetail.ValueMember = "AuthorID";

                // Load publishers
                var publishers = publisherDAO.GetAll();
                cboPublisher.Items.Clear();
                cboPublisher.Items.AddRange(publishers.ToArray());
                cboPublisher.DisplayMember = "PublisherName";
                cboPublisher.ValueMember = "PublisherID";

                // Load books
                SearchBooks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchBooks()
        {
            try
            {
                string? keyword = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim();
                int? categoryId = (cboCategory.SelectedItem as Category)?.CategoryID;
                if (categoryId == 0) categoryId = null;
                int? authorId = (cboAuthor.SelectedItem as Author)?.AuthorID;
                if (authorId == 0) authorId = null;

                var books = bookDAO.Search(keyword, categoryId, authorId, chkAvailableOnly.Checked);

                dgvBooks.Rows.Clear();
                foreach (var book in books)
                {
                    dgvBooks.Rows.Add(
                        book.BookID, book.ISBN, book.Title, book.CategoryName,
                        book.AuthorName, book.TotalCopies, book.AvailableCopies, book.Location
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvBooks_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvBooks.CurrentRow == null) return;

            int bookId = Convert.ToInt32(dgvBooks.CurrentRow.Cells["BookID"].Value);
            currentBook = bookDAO.GetById(bookId);

            if (currentBook != null)
            {
                txtISBN.Text = currentBook.ISBN;
                txtTitle.Text = currentBook.Title;

                for (int i = 0; i < cboCategoryDetail.Items.Count; i++)
                {
                    if (((Category)cboCategoryDetail.Items[i]!).CategoryID == currentBook.CategoryID)
                    {
                        cboCategoryDetail.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < cboAuthorDetail.Items.Count; i++)
                {
                    if (((Author)cboAuthorDetail.Items[i]!).AuthorID == currentBook.AuthorID)
                    {
                        cboAuthorDetail.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < cboPublisher.Items.Count; i++)
                {
                    if (((Publisher)cboPublisher.Items[i]!).PublisherID == currentBook.PublisherID)
                    {
                        cboPublisher.SelectedIndex = i;
                        break;
                    }
                }

                numYear.Value = currentBook.PublishYear ?? DateTime.Now.Year;
                numPrice.Value = currentBook.Price;
                numTotalCopies.Value = currentBook.TotalCopies;
                txtLocation.Text = currentBook.Location;
                txtDescription.Text = currentBook.Description;

                // Load book image
                LoadBookImage(currentBook.ImagePath);
                currentImagePath = currentBook.ImagePath;
            }
        }

        private void AddBook()
        {
            currentBook = null;
            ClearDetailForm();
            txtISBN.Focus();
        }

        private void EditBook()
        {
            if (dgvBooks.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sách cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txtTitle.Focus();
        }

        private void DeleteBook()
        {
            if (dgvBooks.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn sách cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc muốn xóa sách này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int bookId = Convert.ToInt32(dgvBooks.CurrentRow.Cells["BookID"].Value);
                    if (bookDAO.Delete(bookId))
                    {
                        MessageBox.Show("Xóa sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SearchBooks();
                        ClearDetailForm();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi xóa sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            try
            {
                var book = currentBook ?? new Book();
                book.ISBN = txtISBN.Text.Trim();
                book.Title = txtTitle.Text.Trim();
                book.CategoryID = (cboCategoryDetail.SelectedItem as Category)?.CategoryID;
                book.AuthorID = (cboAuthorDetail.SelectedItem as Author)?.AuthorID;
                book.PublisherID = (cboPublisher.SelectedItem as Publisher)?.PublisherID;
                book.PublishYear = (int)numYear.Value;
                book.Price = numPrice.Value;
                book.TotalCopies = (int)numTotalCopies.Value;
                book.Location = txtLocation.Text.Trim();
                book.Description = txtDescription.Text.Trim();
                book.ImagePath = currentImagePath;

                if (currentBook == null)
                {
                    // Add new
                    book.AvailableCopies = book.TotalCopies;
                    bookDAO.Insert(book);
                    MessageBox.Show("Thêm sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Update
                    // Adjust available copies if total changed
                    int diff = book.TotalCopies - currentBook.TotalCopies;
                    book.AvailableCopies = Math.Max(0, currentBook.AvailableCopies + diff);
                    bookDAO.Update(book);
                    MessageBox.Show("Cập nhật sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                SearchBooks();
                ClearDetailForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu sách: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearDetailForm()
        {
            currentBook = null;
            txtISBN.Clear();
            txtTitle.Clear();
            cboCategoryDetail.SelectedIndex = -1;
            cboAuthorDetail.SelectedIndex = -1;
            cboPublisher.SelectedIndex = -1;
            numYear.Value = DateTime.Now.Year;
            numPrice.Value = 0;
            numTotalCopies.Value = 1;
            txtLocation.Clear();
            txtDescription.Clear();

            // Clear image
            currentImagePath = null;
            if (picBookImage.Image != null)
            {
                picBookImage.Image.Dispose();
                picBookImage.Image = null;
            }
            picBookImage.Invalidate();
        }

        #region Image Handling Methods

        private void PicBookImage_Paint(object? sender, PaintEventArgs e)
        {
            if (picBookImage.Image == null)
            {
                // Draw placeholder
                var rect = picBookImage.ClientRectangle;
                using (var brush = new SolidBrush(Color.FromArgb(200, 200, 200)))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    // Draw book icon
                    var font = new Font("Segoe UI", 24);
                    var text = "📖";
                    var textSize = e.Graphics.MeasureString(text, font);
                    var x = (rect.Width - textSize.Width) / 2;
                    var y = (rect.Height - textSize.Height) / 2 - 15;
                    e.Graphics.DrawString(text, font, brush, x, y);

                    // Draw hint text
                    var hintFont = new Font("Segoe UI", 8);
                    var hint = "Nhấn để chọn ảnh";
                    var hintSize = e.Graphics.MeasureString(hint, hintFont);
                    var hx = (rect.Width - hintSize.Width) / 2;
                    var hy = y + textSize.Height + 5;
                    e.Graphics.DrawString(hint, hintFont, brush, hx, hy);
                }
            }
        }

        private void LoadBookImage(string? imagePath)
        {
            // Dispose old image
            if (picBookImage.Image != null)
            {
                picBookImage.Image.Dispose();
                picBookImage.Image = null;
            }

            if (string.IsNullOrEmpty(imagePath))
            {
                picBookImage.Invalidate();
                return;
            }

            try
            {
                string fullPath = imagePath;
                if (!Path.IsPathRooted(imagePath))
                {
                    fullPath = Path.Combine(imagesFolder, imagePath);
                }

                if (File.Exists(fullPath))
                {
                    using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                    {
                        picBookImage.Image = Image.FromStream(stream);
                    }
                }
                else
                {
                    picBookImage.Invalidate();
                }
            }
            catch
            {
                picBookImage.Invalidate();
            }
        }

        private void BrowseImage()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Chọn hình ảnh sách";
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp|All Files|*.*";
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Generate new filename
                        string ext = Path.GetExtension(dialog.FileName);
                        string newFileName = $"book_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid().ToString().Substring(0, 8)}{ext}";
                        string destPath = Path.Combine(imagesFolder, newFileName);

                        // Copy file to images folder
                        File.Copy(dialog.FileName, destPath, true);

                        // Update current image path
                        currentImagePath = newFileName;

                        // Load and display image
                        LoadBookImage(newFileName);

                        MessageBox.Show("Đã tải hình ảnh thành công!\n\n⚠️ Nhớ nhấn nút [💾 Lưu] để lưu thay đổi vào database.",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi tải hình ảnh: {ex.Message}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RemoveImage()
        {
            if (string.IsNullOrEmpty(currentImagePath))
            {
                MessageBox.Show("Không có hình ảnh để xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Bạn có chắc muốn xóa hình ảnh này?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Dispose and clear image
                if (picBookImage.Image != null)
                {
                    picBookImage.Image.Dispose();
                    picBookImage.Image = null;
                }
                currentImagePath = null;
                picBookImage.Invalidate();
            }
        }

        private void ShowBookDetail()
        {
            if (currentBook == null)
            {
                MessageBox.Show("Vui lòng chọn sách để xem chi tiết!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var form = new FormBookDetail(currentBook, imagesFolder))
            {
                form.ShowDialog(this);
            }
        }

        #endregion
    }

    /// <summary>
    /// Form xem chi tiết sách với hình ảnh lớn và danh sách người đang mượn
    /// </summary>
    public class FormBookDetail : Form
    {
        private Book book;
        private string imagesFolder;

        public FormBookDetail(Book book, string imagesFolder)
        {
            this.book = book;
            this.imagesFolder = imagesFolder;
            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = $"Chi tiết sách: {book.Title}";
            this.Size = new Size(850, 580);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // === LEFT PANEL - Image ===
            Panel panelLeft = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(220, this.ClientSize.Height),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            PictureBox picBook = new PictureBox
            {
                Location = new Point(15, 20),
                Size = new Size(190, 250),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            LoadBookImage(picBook);
            panelLeft.Controls.Add(picBook);

            // Status
            Label lblStatus = new Label
            {
                Text = book.IsAvailable ? "✅ Còn sách" : "❌ Hết sách",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = book.IsAvailable ? Color.FromArgb(46, 204, 113) : Color.FromArgb(231, 76, 60),
                Location = new Point(15, 280),
                AutoSize = true
            };
            panelLeft.Controls.Add(lblStatus);

            Label lblQuantity = new Label
            {
                Text = $"Số lượng: {book.AvailableCopies}/{book.TotalCopies} bản",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 100, 100),
                Location = new Point(15, 305),
                AutoSize = true
            };
            panelLeft.Controls.Add(lblQuantity);

            this.Controls.Add(panelLeft);

            // === RIGHT PANEL - Info ===
            Panel panelRight = new Panel
            {
                Location = new Point(220, 0),
                Size = new Size(this.ClientSize.Width - 220, this.ClientSize.Height - 50),
                BackColor = Color.White,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            int y = 15;
            int labelX = 15;
            int valueX = 110;

            // Title
            Label lblTitle = new Label
            {
                Text = book.Title,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(labelX, y),
                MaximumSize = new Size(580, 50),
                AutoSize = true
            };
            panelRight.Controls.Add(lblTitle);
            y += lblTitle.PreferredHeight + 15;

            // Info rows
            AddRow("📖 ISBN:", book.ISBN ?? "N/A", labelX, valueX, ref y, panelRight);
            AddRow("✍️ Tác giả:", book.AuthorName ?? "N/A", labelX, valueX, ref y, panelRight);
            AddRow("📁 Thể loại:", book.CategoryName ?? "N/A", labelX, valueX, ref y, panelRight);
            AddRow("🏢 NXB:", book.PublisherName ?? "N/A", labelX, valueX, ref y, panelRight);
            AddRow("📅 Năm XB:", book.PublishYear?.ToString() ?? "N/A", labelX, valueX, ref y, panelRight);
            AddRow("💰 Giá trị:", book.Price.ToString("N0") + " đ", labelX, valueX, ref y, panelRight);
            AddRow("📍 Vị trí:", book.Location ?? "N/A", labelX, valueX, ref y, panelRight);

            y += 5;

            // Description
            Label lblDescTitle = new Label
            {
                Text = "📝 Mô tả:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(labelX, y),
                AutoSize = true
            };
            panelRight.Controls.Add(lblDescTitle);
            y += 20;

            TextBox txtDesc = new TextBox
            {
                Text = string.IsNullOrEmpty(book.Description) ? "Chưa có mô tả" : book.Description,
                Font = new Font("Segoe UI", 9),
                Location = new Point(labelX, y),
                Size = new Size(580, 50),
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(250, 250, 250),
                BorderStyle = BorderStyle.FixedSingle
            };
            panelRight.Controls.Add(txtDesc);
            y += 55;

            // Borrowers section
            Label lblBorrowers = new Label
            {
                Text = "👥 Người đang mượn sách này:",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(155, 89, 182),
                Location = new Point(labelX, y),
                AutoSize = true
            };
            panelRight.Controls.Add(lblBorrowers);
            y += 22;

            DataGridView dgvBorrowers = new DataGridView
            {
                Location = new Point(labelX, y),
                Size = new Size(580, 100),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvBorrowers.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(155, 89, 182),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            dgvBorrowers.Columns.Add("MemberName", "Độc giả");
            dgvBorrowers.Columns.Add("BorrowDate", "Ngày mượn");
            dgvBorrowers.Columns.Add("DueDate", "Hạn trả");
            dgvBorrowers.Columns.Add("Status", "Trạng thái");
            dgvBorrowers.Columns["MemberName"]!.Width = 180;
            dgvBorrowers.Columns["BorrowDate"]!.Width = 130;
            dgvBorrowers.Columns["DueDate"]!.Width = 130;
            dgvBorrowers.Columns["Status"]!.Width = 120;

            LoadBorrowers(dgvBorrowers);
            panelRight.Controls.Add(dgvBorrowers);

            this.Controls.Add(panelRight);

            // === BOTTOM PANEL - Close Button ===
            Panel panelBottom = new Panel
            {
                Location = new Point(220, this.ClientSize.Height - 50),
                Size = new Size(this.ClientSize.Width - 220, 50),
                BackColor = Color.FromArgb(248, 249, 250)
            };

            Button btnClose = new Button
            {
                Text = "Đóng",
                Font = new Font("Segoe UI", 10),
                Size = new Size(100, 35),
                Location = new Point(panelBottom.Width - 120, 8),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            panelBottom.Controls.Add(btnClose);

            this.Controls.Add(panelBottom);
        }

        private void AddRow(string label, string value, int labelX, int valueX, ref int y, Panel parent)
        {
            Label lblLabel = new Label
            {
                Text = label,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(labelX, y),
                Size = new Size(90, 20)
            };
            parent.Controls.Add(lblLabel);

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(valueX, y),
                MaximumSize = new Size(480, 25),
                AutoSize = true
            };
            parent.Controls.Add(lblValue);
            y += 23;
        }

        private void LoadBorrowers(DataGridView dgv)
        {
            try
            {
                using (var conn = Data.DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            SELECT m.FullName, br.BorrowDate, br.DueDate, br.Status
                            FROM BorrowRecords br
                            INNER JOIN Members m ON br.MemberID = m.MemberID
                            WHERE br.BookID = @BookID AND br.Status IN (N'Đang mượn', N'Quá hạn')
                            ORDER BY br.BorrowDate DESC";
                        cmd.Parameters.AddWithValue("@BookID", book.BookID);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string status = reader.GetString(3);
                                dgv.Rows.Add(
                                    reader.GetString(0),
                                    reader.GetDateTime(1).ToString("dd/MM/yyyy"),
                                    reader.GetDateTime(2).ToString("dd/MM/yyyy"),
                                    status
                                );
                                if (status == "Quá hạn")
                                    dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.Red;
                            }
                        }
                    }
                }

                if (dgv.Rows.Count == 0)
                {
                    dgv.Rows.Add("Không có ai đang mượn", "-", "-", "-");
                    dgv.Rows[0].DefaultCellStyle.ForeColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                dgv.Rows.Add("Lỗi: " + ex.Message, "-", "-", "-");
            }
        }

        private void LoadBookImage(PictureBox pic)
        {
            if (string.IsNullOrEmpty(book.ImagePath))
            {
                var bmp = new Bitmap(190, 250);
                using (var g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.FromArgb(245, 245, 245));
                    using (var font = new Font("Segoe UI", 36))
                    using (var brush = new SolidBrush(Color.FromArgb(180, 180, 180)))
                    {
                        g.DrawString("📚", font, brush, 60, 80);
                    }
                    using (var font = new Font("Segoe UI", 9))
                    using (var brush = new SolidBrush(Color.FromArgb(150, 150, 150)))
                    {
                        g.DrawString("Không có hình ảnh", font, brush, 35, 150);
                    }
                }
                pic.Image = bmp;
                return;
            }

            try
            {
                string fullPath = Path.IsPathRooted(book.ImagePath)
                    ? book.ImagePath
                    : Path.Combine(imagesFolder, book.ImagePath);

                if (File.Exists(fullPath))
                {
                    using (var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                    {
                        pic.Image = Image.FromStream(stream);
                    }
                }
            }
            catch { }
        }
    }
}
