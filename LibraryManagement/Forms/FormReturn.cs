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
        private TextBox txtSearch = null!;
        private ComboBox cboStatus = null!;
        private DataGridView dgvBorrowRecords = null!;
        private Label lblSelectedInfo = null!;

        private BorrowRecordDAO borrowDAO = new BorrowRecordDAO();
        private BorrowRecord? selectedRecord;

        public FormReturn()
        {
            InitializeComponent();
            SetupForm();
            this.Load += FormReturn_Load;
        }

        private void FormReturn_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }

            LoadData();
        }


        private void SetupForm()
        {
            // Title
            var lblTitle = new Label
            {
                Text = "TRẢ SÁCH",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 10),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Search panel
            var panelSearch = new Panel
            {
                Location = new Point(20, 55),
                Size = new Size(1180, 55),
                BackColor = Color.White
            };

            var lblSearch = new Label { Text = "Tìm kiếm:", Location = new Point(15, 17), AutoSize = true };
            txtSearch = new TextBox
            {
                Location = new Point(80, 14),
                Size = new Size(250, 28),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Mã phiếu, mã thẻ, tên độc giả, tên sách..."
            };
            txtSearch.TextChanged += (s, e) => SearchRecords();

            var lblStatus = new Label { Text = "Trạng thái:", Location = new Point(350, 17), AutoSize = true };
            cboStatus = new ComboBox
            {
                Location = new Point(420, 14),
                Size = new Size(150, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboStatus.Items.AddRange(new object[] { "-- Tất cả --", BorrowRecord.STATUS_BORROWING, BorrowRecord.STATUS_OVERDUE });
            cboStatus.SelectedIndex = 0;
            cboStatus.SelectedIndexChanged += (s, e) => SearchRecords();

            var btnRefresh = new Button
            {
                Text = "Làm mới",
                Location = new Point(590, 12),
                Size = new Size(90, 32),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadData();

            panelSearch.Controls.AddRange(new Control[] { lblSearch, txtSearch, lblStatus, cboStatus, btnRefresh });
            this.Controls.Add(panelSearch);

            // DataGridView
            dgvBorrowRecords = new DataGridView
            {
                Location = new Point(20, 120),
                Size = new Size(1180, 330),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvBorrowRecords.SelectionChanged += DgvBorrowRecords_SelectionChanged;
            dgvBorrowRecords.CellDoubleClick += (s, e) => ReturnBook();

            dgvBorrowRecords.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            dgvBorrowRecords.Columns.Add("BorrowID", "ID");
            dgvBorrowRecords.Columns.Add("BorrowCode", "Mã phiếu");
            dgvBorrowRecords.Columns.Add("MemberCode", "Mã thẻ");
            dgvBorrowRecords.Columns.Add("MemberName", "Tên độc giả");
            dgvBorrowRecords.Columns.Add("BookTitle", "Tên sách");
            dgvBorrowRecords.Columns.Add("BorrowDate", "Ngày mượn");
            dgvBorrowRecords.Columns.Add("DueDate", "Hạn trả");
            dgvBorrowRecords.Columns.Add("DaysLeft", "Còn/Quá hạn");
            dgvBorrowRecords.Columns.Add("Status", "Trạng thái");

            dgvBorrowRecords.Columns["BorrowID"]!.Visible = false;
            dgvBorrowRecords.Columns["BorrowCode"]!.Width = 110;
            dgvBorrowRecords.Columns["MemberCode"]!.Width = 80;
            dgvBorrowRecords.Columns["MemberName"]!.Width = 150;
            dgvBorrowRecords.Columns["BookTitle"]!.Width = 280;
            dgvBorrowRecords.Columns["BorrowDate"]!.Width = 100;
            dgvBorrowRecords.Columns["DueDate"]!.Width = 100;
            dgvBorrowRecords.Columns["DaysLeft"]!.Width = 100;
            dgvBorrowRecords.Columns["Status"]!.Width = 100;

            this.Controls.Add(dgvBorrowRecords);

            // Selected info panel
            var panelInfo = new Panel
            {
                Location = new Point(20, 460),
                Size = new Size(800, 60),
                BackColor = Color.FromArgb(236, 240, 241)
            };

            lblSelectedInfo = new Label
            {
                Location = new Point(0, 5),
                Size = new Size(800, 50),
                Font = new Font("Segoe UI", 11)
            };
            panelInfo.Controls.Add(lblSelectedInfo);
            this.Controls.Add(panelInfo);

            // Buttons
            var btnReturn = new Button
            {
                Text = "Trả sách",
                Location = new Point(840, 465),
                Size = new Size(130, 50),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnReturn.FlatAppearance.BorderSize = 0;
            btnReturn.Click += (s, e) => ReturnBook();
            this.Controls.Add(btnReturn);

            var btnRenew = new Button
            {
                Text = "Gia hạn",
                Location = new Point(990, 465),
                Size = new Size(100, 50),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };
            btnRenew.FlatAppearance.BorderSize = 0;
            btnRenew.Click += (s, e) => RenewBook();
            this.Controls.Add(btnRenew);
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

    /// <summary>
    /// Form gia hạn sách
    /// </summary>
    public class FormRenewBook : Form
    {
        private BorrowRecord record;
        private NumericUpDown numDays = null!;

        public FormRenewBook(BorrowRecord record)
        {
            this.record = record;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Gia hạn sách";
            this.Size = new Size(400, 280);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblBook = new Label
            {
                Text = $"Sách: {record.BookTitle}",
                Location = new Point(20, 20),
                Size = new Size(350, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lblMember = new Label
            {
                Text = $"Độc giả: {record.MemberName}",
                Location = new Point(20, 50),
                AutoSize = true
            };

            var lblCurrentDue = new Label
            {
                Text = $"Hạn trả hiện tại: {record.DueDate:dd/MM/yyyy}",
                Location = new Point(20, 80),
                AutoSize = true
            };

            var lblDays = new Label { Text = "Gia hạn thêm (ngày):", Location = new Point(20, 120), AutoSize = true };
            numDays = new NumericUpDown
            {
                Location = new Point(160, 117),
                Size = new Size(80, 28),
                Minimum = 1,
                Maximum = 30,
                Value = 7
            };

            var lblNewDue = new Label
            {
                Location = new Point(20, 155),
                AutoSize = true,
                Font = new Font("Segoe UI", 10)
            };

            numDays.ValueChanged += (s, e) =>
            {
                DateTime newDue = record.DueDate.AddDays((int)numDays.Value);
                lblNewDue.Text = $"Hạn trả mới: {newDue:dd/MM/yyyy}";
            };
            numDays.Value = 7; // Trigger the event

            var btnRenew = new Button
            {
                Text = "🔄 Gia hạn",
                Location = new Point(80, 195),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRenew.FlatAppearance.BorderSize = 0;
            btnRenew.Click += BtnRenew_Click;

            var btnCancel = new Button
            {
                Text = "Hủy",
                Location = new Point(200, 195),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblBook, lblMember, lblCurrentDue, lblDays, numDays, lblNewDue, btnRenew, btnCancel
            });
        }

        private void BtnRenew_Click(object? sender, EventArgs e)
        {
            try
            {
                var borrowDAO = new BorrowRecordDAO();
                var (success, message) = borrowDAO.RenewBook(record.BorrowID, (int)numDays.Value);

                if (success)
                {
                    var logDAO = new ActivityLogDAO();
                    logDAO.Log($"Gia hạn sách: {record.BookTitle} thêm {numDays.Value} ngày", "BorrowRecords", record.BorrowID);

                    MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
