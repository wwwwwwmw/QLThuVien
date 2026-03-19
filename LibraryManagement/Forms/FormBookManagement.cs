using LibraryManagement.Data;
using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form quản lý sách
    /// </summary>
    public partial class FormBookManagement : Form
    {
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
            Load += FormBookManagement_Load;
        }

        private void FormBookManagement_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                EnsureImagesFolderExists();
                ConfigureBookGrid();
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

        private void ConfigureBookGrid()
        {
            dgvBooks.Columns.Clear();

            dgvBooks.EnableHeadersVisualStyles = false;
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

            dgvBooks.Columns["BookID"].Visible = false;
            dgvBooks.Columns["ISBN"].Width = 100;
            dgvBooks.Columns["Title"].Width = 250;
            dgvBooks.Columns["CategoryName"].Width = 100;
            dgvBooks.Columns["AuthorName"].Width = 120;
            dgvBooks.Columns["TotalCopies"].Width = 60;
            dgvBooks.Columns["AvailableCopies"].Width = 60;
            dgvBooks.Columns["Location"].Width = 80;
        }

        private void LoadData()
        {
            try
            {
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

                var publishers = publisherDAO.GetAll();
                cboPublisher.Items.Clear();
                cboPublisher.Items.AddRange(publishers.ToArray());
                cboPublisher.DisplayMember = "PublisherName";
                cboPublisher.ValueMember = "PublisherID";

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
                        book.BookID,
                        book.ISBN,
                        book.Title,
                        book.CategoryName,
                        book.AuthorName,
                        book.TotalCopies,
                        book.AvailableCopies,
                        book.Location
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
            if (dgvBooks.CurrentRow == null || dgvBooks.CurrentRow.Cells["BookID"].Value == null)
                return;

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

                LoadBookImage(currentBook.ImagePath);
                currentImagePath = currentBook.ImagePath;
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            SearchBooks();
        }

        private void CboCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            SearchBooks();
        }

        private void CboAuthor_SelectedIndexChanged(object? sender, EventArgs e)
        {
            SearchBooks();
        }

        private void ChkAvailableOnly_CheckedChanged(object? sender, EventArgs e)
        {
            SearchBooks();
        }

        private void DgvBooks_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            EditBook();
        }

        private void BtnAdd_Click(object? sender, EventArgs e)
        {
            AddBook();
        }

        private void BtnEdit_Click(object? sender, EventArgs e)
        {
            EditBook();
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            DeleteBook();
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadData();
        }

        private void BtnBrowseImage_Click(object? sender, EventArgs e)
        {
            BrowseImage();
        }

        private void BtnRemoveImage_Click(object? sender, EventArgs e)
        {
            RemoveImage();
        }

        private void BtnViewDetail_Click(object? sender, EventArgs e)
        {
            ShowBookDetail();
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            ClearDetailForm();
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
                    book.AvailableCopies = book.TotalCopies;
                    bookDAO.Insert(book);
                    MessageBox.Show("Thêm sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
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
                var rect = picBookImage.ClientRectangle;
                using (var brush = new SolidBrush(Color.FromArgb(200, 200, 200)))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    var font = new Font("Segoe UI", 24);
                    var text = "📖";
                    var textSize = e.Graphics.MeasureString(text, font);
                    var x = (rect.Width - textSize.Width) / 2;
                    var y = (rect.Height - textSize.Height) / 2 - 15;
                    e.Graphics.DrawString(text, font, brush, x, y);

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
                        string ext = Path.GetExtension(dialog.FileName);
                        string newFileName = $"book_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid().ToString().Substring(0, 8)}{ext}";
                        string destPath = Path.Combine(imagesFolder, newFileName);

                        File.Copy(dialog.FileName, destPath, true);

                        currentImagePath = newFileName;
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
            Text = $"Chi tiết sách: {book.Title}";
            Size = new Size(850, 580);
            StartPosition = FormStartPosition.CenterParent;
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            Panel panelLeft = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(220, ClientSize.Height),
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

            Controls.Add(panelLeft);

            Panel panelRight = new Panel
            {
                Location = new Point(220, 0),
                Size = new Size(ClientSize.Width - 220, ClientSize.Height - 50),
                BackColor = Color.White,
                AutoScroll = true,
                Padding = new Padding(10)
            };

            int y = 15;
            int labelX = 15;
            int valueX = 110;

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

            AddRow("📖 ISBN:", book.ISBN ?? "N/A", labelX, valueX, ref y, panelRight);
            AddRow("✍️ Tác giả:", book.AuthorName ?? "N/A", labelX, valueX, ref y, panelRight);
            AddRow("📁 Thể loại:", book.CategoryName ?? "N/A", labelX, valueX, ref y, panelRight);
            AddRow("🏢 NXB:", book.PublisherName ?? "N/A", labelX, valueX, ref y, panelRight);
            AddRow("📅 Năm XB:", book.PublishYear?.ToString() ?? "N/A", labelX, valueX, ref y, panelRight);
            AddRow("💰 Giá trị:", book.Price.ToString("N0") + " đ", labelX, valueX, ref y, panelRight);
            AddRow("📍 Vị trí:", book.Location ?? "N/A", labelX, valueX, ref y, panelRight);

            y += 5;

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

            Controls.Add(panelRight);

            Panel panelBottom = new Panel
            {
                Location = new Point(220, ClientSize.Height - 50),
                Size = new Size(ClientSize.Width - 220, 50),
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
            btnClose.Click += (s, e) => Close();
            panelBottom.Controls.Add(btnClose);

            Controls.Add(panelBottom);
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
            catch
            {
            }
        }
    }
}