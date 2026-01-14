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
    /// Form mượn sách
    /// </summary>
    public partial class FormBorrow : Form
    {
        private TextBox txtMemberCode = null!;
        private Label lblMemberInfo = null!;
        private Label lblMemberStatus = null!;
        private DataGridView dgvBorrowing = null!;

        private TextBox txtBookSearch = null!;
        private DataGridView dgvBooks = null!;
        private NumericUpDown numDays = null!;

        private MemberDAO memberDAO = new MemberDAO();
        private BookDAO bookDAO = new BookDAO();
        private BorrowRecordDAO borrowDAO = new BorrowRecordDAO();
        private SystemSettingDAO settingDAO = new SystemSettingDAO();

        private Member? currentMember;
        private Book? selectedBook;

        public FormBorrow()
        {
            InitializeComponent();
        }

        private void SetupForm()
        {
            // Title
            var lblTitle = new Label
            {
                Text = "MƯỢN SÁCH",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Left panel - Member info
            var panelMember = new Panel
            {
                Location = new Point(20, 60),
                Size = new Size(500, 200),
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            var lblMemberTitle = new Label
            {
                Text = "👤 Thông tin độc giả",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(15, 10),
                AutoSize = true
            };
            panelMember.Controls.Add(lblMemberTitle);

            var lblCode = new Label { Text = "Mã thẻ:", Location = new Point(15, 45), AutoSize = true };
            txtMemberCode = new TextBox
            {
                Location = new Point(80, 42),
                Size = new Size(150, 28),
                Font = new Font("Segoe UI", 10)
            };
            txtMemberCode.KeyPress += TxtMemberCode_KeyPress;

            var btnFind = new Button
            {
                Text = "Tìm kiếm",
                Location = new Point(240, 40),
                Size = new Size(85, 30),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnFind.FlatAppearance.BorderSize = 0;
            btnFind.Click += BtnFindMember_Click;

            lblMemberInfo = new Label
            {
                Location = new Point(15, 80),
                Size = new Size(460, 60),
                Font = new Font("Segoe UI", 10)
            };

            lblMemberStatus = new Label
            {
                Location = new Point(15, 145),
                Size = new Size(460, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            panelMember.Controls.AddRange(new Control[] { lblCode, txtMemberCode, btnFind, lblMemberInfo, lblMemberStatus });
            this.Controls.Add(panelMember);

            // Member's current borrowing
            var lblCurrentBorrow = new Label
            {
                Text = "📚 Sách đang mượn:",
                Location = new Point(20, 270),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblCurrentBorrow);

            dgvBorrowing = new DataGridView
            {
                Location = new Point(20, 295),
                Size = new Size(500, 180),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvBorrowing.Columns.Add("BookTitle", "Tên sách");
            dgvBorrowing.Columns.Add("BorrowDate", "Ngày mượn");
            dgvBorrowing.Columns.Add("DueDate", "Hạn trả");
            dgvBorrowing.Columns.Add("Status", "Trạng thái");
            dgvBorrowing.Columns["BookTitle"]!.Width = 220;
            dgvBorrowing.Columns["BorrowDate"]!.Width = 90;
            dgvBorrowing.Columns["DueDate"]!.Width = 90;
            dgvBorrowing.Columns["Status"]!.Width = 90;
            this.Controls.Add(dgvBorrowing);

            // Right panel - Book selection
            var panelBook = new Panel
            {
                Location = new Point(540, 60),
                Size = new Size(660, 420),
                BackColor = Color.White,
                Padding = new Padding(15)
            };

            var lblBookTitle = new Label
            {
                Text = "Chọn sách mượn",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Location = new Point(15, 10),
                AutoSize = true
            };
            panelBook.Controls.Add(lblBookTitle);

            var lblSearch = new Label { Text = "Tìm sách:", Location = new Point(15, 45), AutoSize = true };
            txtBookSearch = new TextBox
            {
                Location = new Point(80, 42),
                Size = new Size(300, 28),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Nhập tên sách hoặc ISBN..."
            };
            txtBookSearch.TextChanged += TxtBookSearch_TextChanged;

            dgvBooks = new DataGridView
            {
                Location = new Point(15, 80),
                Size = new Size(620, 250),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvBooks.SelectionChanged += DgvBooks_SelectionChanged;

            dgvBooks.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            dgvBooks.Columns.Add("BookID", "ID");
            dgvBooks.Columns.Add("ISBN", "ISBN");
            dgvBooks.Columns.Add("Title", "Tên sách");
            dgvBooks.Columns.Add("AuthorName", "Tác giả");
            dgvBooks.Columns.Add("AvailableCopies", "Còn lại");
            dgvBooks.Columns.Add("Location", "Vị trí");
            dgvBooks.Columns["BookID"]!.Visible = false;
            dgvBooks.Columns["ISBN"]!.Width = 100;
            dgvBooks.Columns["Title"]!.Width = 220;
            dgvBooks.Columns["AuthorName"]!.Width = 120;
            dgvBooks.Columns["AvailableCopies"]!.Width = 70;
            dgvBooks.Columns["Location"]!.Width = 80;

            var lblDays = new Label { Text = "Số ngày mượn:", Location = new Point(15, 345), AutoSize = true };
            numDays = new NumericUpDown
            {
                Location = new Point(110, 342),
                Size = new Size(70, 28),
                Minimum = 1,
                Maximum = 60,
                Value = settingDAO.GetIntValue(SystemSetting.KEY_MAX_BORROW_DAYS, 14)
            };

            var btnBorrow = new Button
            {
                Text = "Mượn sách",
                Location = new Point(200, 340),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnBorrow.FlatAppearance.BorderSize = 0;
            btnBorrow.Click += BtnBorrow_Click;

            panelBook.Controls.AddRange(new Control[] { lblSearch, txtBookSearch, dgvBooks, lblDays, numDays, btnBorrow });
            this.Controls.Add(panelBook);

            // Load available books
            LoadBooks();
        }

        private void TxtMemberCode_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnFindMember_Click(sender, e);
                e.Handled = true;
            }
        }

        private void BtnFindMember_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMemberCode.Text))
            {
                MessageBox.Show("Vui lòng nhập mã thẻ độc giả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            currentMember = memberDAO.GetByCode(txtMemberCode.Text.Trim());

            if (currentMember == null)
            {
                lblMemberInfo.Text = "Không tìm thấy độc giả!";
                lblMemberInfo.ForeColor = Color.Red;
                lblMemberStatus.Text = "";
                dgvBorrowing.Rows.Clear();
                return;
            }

            // Display member info
            lblMemberInfo.ForeColor = Color.Black;
            lblMemberInfo.Text = $"Họ tên: {currentMember.FullName}\n" +
                                 $"Loại thẻ: {currentMember.MemberType}    Hạn thẻ: {currentMember.ExpiryDate?.ToString("dd/MM/yyyy")}\n" +
                                 $"Nợ phạt: {currentMember.TotalFine:N0} VNĐ";

            // Check if can borrow
            var (canBorrow, message) = memberDAO.CanBorrow(currentMember.MemberID);
            if (canBorrow)
            {
                lblMemberStatus.Text = "✓ Có thể mượn sách";
                lblMemberStatus.ForeColor = Color.Green;
            }
            else
            {
                lblMemberStatus.Text = $"✗ {message}";
                lblMemberStatus.ForeColor = Color.Red;
            }

            // Load current borrowings
            LoadMemberBorrowings();
        }

        private void LoadMemberBorrowings()
        {
            dgvBorrowing.Rows.Clear();
            if (currentMember == null) return;

            var borrowings = borrowDAO.GetMemberBorrowings(currentMember.MemberID);
            foreach (var borrow in borrowings)
            {
                var row = dgvBorrowing.Rows.Add(
                    borrow.BookTitle,
                    borrow.BorrowDate.ToString("dd/MM/yyyy"),
                    borrow.DueDate.ToString("dd/MM/yyyy"),
                    borrow.StatusDisplay
                );

                if (borrow.IsOverdue)
                {
                    dgvBorrowing.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
                }
            }
        }

        private void LoadBooks()
        {
            try
            {
                var books = bookDAO.Search(availableOnly: true);
                dgvBooks.Rows.Clear();

                foreach (var book in books)
                {
                    dgvBooks.Rows.Add(
                        book.BookID,
                        book.ISBN,
                        book.Title,
                        book.AuthorName,
                        book.AvailableCopies,
                        book.Location
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtBookSearch_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                string? keyword = string.IsNullOrWhiteSpace(txtBookSearch.Text) ? null : txtBookSearch.Text.Trim();
                var books = bookDAO.Search(keyword, availableOnly: true);

                dgvBooks.Rows.Clear();
                foreach (var book in books)
                {
                    dgvBooks.Rows.Add(
                        book.BookID,
                        book.ISBN,
                        book.Title,
                        book.AuthorName,
                        book.AvailableCopies,
                        book.Location
                    );
                }
            }
            catch { }
        }

        private void DgvBooks_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvBooks.CurrentRow == null)
            {
                selectedBook = null;
                return;
            }

            int bookId = Convert.ToInt32(dgvBooks.CurrentRow.Cells["BookID"].Value);
            selectedBook = bookDAO.GetById(bookId);
        }

        private void BtnBorrow_Click(object? sender, EventArgs e)
        {
            // Validate
            if (currentMember == null)
            {
                MessageBox.Show("Vui lòng tìm và chọn độc giả trước!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMemberCode.Focus();
                return;
            }

            if (selectedBook == null)
            {
                MessageBox.Show("Vui lòng chọn sách cần mượn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if member can borrow
            var (canBorrow, message) = memberDAO.CanBorrow(currentMember.MemberID);
            if (!canBorrow)
            {
                MessageBox.Show(message, "Không thể mượn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm
            var result = MessageBox.Show(
                $"Xác nhận mượn sách:\n\n" +
                $"Độc giả: {currentMember.FullName} ({currentMember.MemberCode})\n" +
                $"Sách: {selectedBook.Title}\n" +
                $"Số ngày mượn: {numDays.Value} ngày\n" +
                $"Hạn trả: {DateTime.Now.AddDays((int)numDays.Value):dd/MM/yyyy}",
                "Xác nhận mượn sách",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                var (success, msg) = borrowDAO.BorrowBook(
                    currentMember.MemberID,
                    selectedBook.BookID,
                    CurrentUser.User?.UserID ?? 0,
                    (int)numDays.Value
                );

                if (success)
                {
                    MessageBox.Show(msg, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh
                    LoadBooks();
                    LoadMemberBorrowings();
                    BtnFindMember_Click(null, EventArgs.Empty); // Refresh member status

                    // Log
                    var logDAO = new ActivityLogDAO();
                    logDAO.Log($"Mượn sách: {selectedBook.Title}", "BorrowRecords", selectedBook.BookID);
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormBorrow_Load(object sender, EventArgs e)
        {
            SetupForm();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }
        }
    }
}
