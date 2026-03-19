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
        private MemberDAO memberDAO = new MemberDAO();
        private BookDAO bookDAO = new BookDAO();
        private BorrowRecordDAO borrowDAO = new BorrowRecordDAO();
        private SystemSettingDAO settingDAO = new SystemSettingDAO();

        private Member? currentMember;
        private Book? selectedBook;

        public FormBorrow()
        {
            InitializeComponent();
            this.Load += FormBorrow_Load;
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

        private void FormBorrow_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            try
            {
                int maxDays = settingDAO.GetIntValue(SystemSetting.KEY_MAX_BORROW_DAYS, 14);
                maxDays = Math.Max((int)numDays.Minimum, Math.Min((int)numDays.Maximum, maxDays));
                numDays.Value = maxDays;
            }
            catch
            {
                numDays.Value = 14;
            }

            // Load available books
            LoadBooks();
        }
    }
}
