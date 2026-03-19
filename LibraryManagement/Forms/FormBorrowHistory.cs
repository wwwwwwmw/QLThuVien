using System;
using System.ComponentModel;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form hiển thị lịch sử mượn sách của độc giả
    /// </summary>
    public partial class FormBorrowHistory : Form
    {
        private Member member;

        public FormBorrowHistory()
            : this(new Member())
        {
        }

        public FormBorrowHistory(Member member)
        {
            this.member = member ?? new Member();
            InitializeComponent();
            this.Load += FormBorrowHistory_Load;
        }

        private void FormBorrowHistory_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            Text = $"Lịch sử mượn sách - {member.FullName}";
            LoadHistory();
        }

        private void LoadHistory()
        {
            var borrowDAO = new BorrowRecordDAO();
            var history = borrowDAO.GetMemberHistory(member.MemberID);

            dgvHistory.Rows.Clear();

            foreach (var record in history)
            {
                dgvHistory.Rows.Add(
                    record.BorrowCode,
                    record.BookTitle,
                    record.BorrowDate.ToString("dd/MM/yyyy"),
                    record.DueDate.ToString("dd/MM/yyyy"),
                    record.ReturnDate?.ToString("dd/MM/yyyy"),
                    record.Status,
                    record.FineAmount.ToString("N0") + " đ"
                );
            }
        }

        private void BtnCloseHistory_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}
