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
    /// Form trả sách
    /// </summary>
    public partial class FormReturn : Form
    {
        private BorrowRecordDAO borrowDAO = new BorrowRecordDAO();
        private BorrowRecord? selectedRecord;

        public FormReturn()
        {
            InitializeComponent();
            this.Load += FormReturn_Load;
        }

        private void FormReturn_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                cboStatus.SelectedIndex = 0;
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            SearchRecords();
        }

        private void CboStatus_SelectedIndexChanged(object? sender, EventArgs e)
        {
            SearchRecords();
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadData();
        }

        private void DgvBorrowRecords_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            ReturnBook();
        }

        private void BtnReturnBook_Click(object? sender, EventArgs e)
        {
            ReturnBook();
        }

        private void BtnRenewBook_Click(object? sender, EventArgs e)
        {
            RenewBook();
        }

        private void LoadData()
        {
            // Update overdue status first
            borrowDAO.UpdateOverdueStatus();
            SearchRecords();
        }

        private void SearchRecords()
        {
            try
            {
                string? keyword = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text.Trim();
                string? status = cboStatus.SelectedIndex > 0 ? cboStatus.SelectedItem?.ToString() : null;

                // Only show borrowing/overdue records (not returned)
                var records = borrowDAO.Search(keyword, status);
                records = records.Where(r => r.Status == BorrowRecord.STATUS_BORROWING || r.Status == BorrowRecord.STATUS_OVERDUE).ToList();

                dgvBorrowRecords.Rows.Clear();
                foreach (var record in records)
                {
                    string daysText;
                    if (record.IsOverdue)
                    {
                        daysText = $"Quá {record.DaysOverdue} ngày";
                    }
                    else
                    {
                        daysText = $"Còn {record.DaysRemaining} ngày";
                    }

                    var row = dgvBorrowRecords.Rows.Add(
                        record.BorrowID,
                        record.BorrowCode,
                        record.MemberCode,
                        record.MemberName,
                        record.BookTitle,
                        record.BorrowDate.ToString("dd/MM/yyyy"),
                        record.DueDate.ToString("dd/MM/yyyy"),
                        daysText,
                        record.Status
                    );

                    // Highlight overdue
                    if (record.IsOverdue)
                    {
                        dgvBorrowRecords.Rows[row].DefaultCellStyle.BackColor = Color.FromArgb(255, 200, 200);
                        dgvBorrowRecords.Rows[row].DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvBorrowRecords_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvBorrowRecords.CurrentRow == null)
            {
                selectedRecord = null;
                lblSelectedInfo.Text = "";
                return;
            }

            int borrowId = Convert.ToInt32(dgvBorrowRecords.CurrentRow.Cells["BorrowID"].Value);
            selectedRecord = borrowDAO.GetById(borrowId);

            if (selectedRecord != null)
            {
                var settingDAO = new SystemSettingDAO();
                decimal finePerDay = settingDAO.GetDecimalValue(SystemSetting.KEY_FINE_PER_DAY, 5000);
                decimal estimatedFine = 0;

                if (selectedRecord.IsOverdue)
                {
                    estimatedFine = selectedRecord.DaysOverdue * finePerDay;
                }

                lblSelectedInfo.Text = $"📌 Đã chọn: {selectedRecord.BookTitle}\n" +
                    $"   Độc giả: {selectedRecord.MemberName} | " +
                    $"Mượn: {selectedRecord.BorrowDate:dd/MM/yyyy} | " +
                    $"Hạn: {selectedRecord.DueDate:dd/MM/yyyy} | " +
                    (estimatedFine > 0 ? $"Tiền phạt dự kiến: {estimatedFine:N0} VNĐ" : "Không phạt");

                lblSelectedInfo.ForeColor = selectedRecord.IsOverdue ? Color.DarkRed : Color.Black;
            }
        }

        private void ReturnBook()
        {
            if (selectedRecord == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu mượn cần trả!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calculate fine preview
            var settingDAO = new SystemSettingDAO();
            decimal finePerDay = settingDAO.GetDecimalValue(SystemSetting.KEY_FINE_PER_DAY, 5000);
            decimal estimatedFine = selectedRecord.IsOverdue ? selectedRecord.DaysOverdue * finePerDay : 0;

            string message = $"Xác nhận trả sách:\n\n" +
                $"Mã phiếu: {selectedRecord.BorrowCode}\n" +
                $"Độc giả: {selectedRecord.MemberName}\n" +
                $"Sách: {selectedRecord.BookTitle}\n" +
                $"Ngày mượn: {selectedRecord.BorrowDate:dd/MM/yyyy}\n" +
                $"Hạn trả: {selectedRecord.DueDate:dd/MM/yyyy}\n";

            if (estimatedFine > 0)
            {
                message += $"\n⚠️ Quá hạn {selectedRecord.DaysOverdue} ngày\n" +
                    $"Tiền phạt: {estimatedFine:N0} VNĐ";
            }

            var result = MessageBox.Show(message, "Xác nhận trả sách",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            try
            {
                var (success, msg, fineAmount) = borrowDAO.ReturnBook(selectedRecord.BorrowID, CurrentUser.User?.UserID ?? 0);

                if (success)
                {
                    // Log
                    var logDAO = new ActivityLogDAO();
                    logDAO.Log($"Trả sách: {selectedRecord.BookTitle}", "BorrowRecords", selectedRecord.BorrowID);

                    if (fineAmount > 0)
                    {
                        MessageBox.Show($"Trả sách thành công!\n\nTiền phạt: {fineAmount:N0} VNĐ\n" +
                            "Tiền phạt đã được cộng vào tài khoản độc giả.",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Trả sách thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    LoadData();
                    lblSelectedInfo.Text = "";
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

        private void RenewBook()
        {
            if (selectedRecord == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu mượn cần gia hạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (selectedRecord.IsOverdue)
            {
                MessageBox.Show("Không thể gia hạn sách đã quá hạn!\nVui lòng trả sách và mượn lại.",
                    "Không thể gia hạn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var renewForm = new FormRenewBook(selectedRecord))
            {
                if (renewForm.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }
    }
}
