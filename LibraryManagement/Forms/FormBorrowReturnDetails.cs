using System;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Data;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form xem chi tiết phiếu mượn/trả
    /// </summary>
    public class FormBorrowReturnDetails : Form
    {
        private TabControl tabControl = null!;
        private DataGridView dgvBorrow = null!;
        private DataGridView dgvReturn = null!;
        private DateTimePicker dtpFrom = null!;
        private DateTimePicker dtpTo = null!;
        private Label lblBorrowSummary = null!;
        private Label lblReturnSummary = null!;

        public FormBorrowReturnDetails()
        {
            SetupForm();
            LoadData();
        }

        private void SetupForm()
        {
            this.Text = "Chi tiết phiếu mượn / trả sách";
            this.Size = new Size(1000, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Header
            var lblTitle = new Label
            {
                Text = "CHI TIẾT PHIẾU MƯỢN / TRẢ SÁCH",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(20, 15),
                AutoSize = true
            };
            this.Controls.Add(lblTitle);

            // Filter panel
            var panelFilter = new Panel
            {
                Location = new Point(20, 55),
                Size = new Size(945, 50),
                BackColor = Color.White
            };

            var lblFrom = new Label { Text = "Từ ngày:", Location = new Point(15, 15), AutoSize = true };
            dtpFrom = new DateTimePicker
            {
                Location = new Point(80, 12),
                Size = new Size(130, 28),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today.AddMonths(-1)
            };

            var lblTo = new Label { Text = "Đến:", Location = new Point(225, 15), AutoSize = true };
            dtpTo = new DateTimePicker
            {
                Location = new Point(265, 12),
                Size = new Size(130, 28),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };

            var btnFilter = new Button
            {
                Text = "Lọc dữ liệu",
                Location = new Point(410, 10),
                Size = new Size(100, 32),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnFilter.FlatAppearance.BorderSize = 0;
            btnFilter.Click += (s, e) => LoadData();

            var btnExport = new Button
            {
                Text = "Xuất Excel",
                Location = new Point(520, 10),
                Size = new Size(90, 32),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Click += (s, e) => ExportToExcel();

            panelFilter.Controls.AddRange(new Control[] { lblFrom, dtpFrom, lblTo, dtpTo, btnFilter, btnExport });
            this.Controls.Add(panelFilter);

            // Tab Control
            tabControl = new TabControl
            {
                Location = new Point(20, 115),
                Size = new Size(945, 440),
                Font = new Font("Segoe UI", 10)
            };

            // Tab Phiếu mượn
            var tabBorrow = new TabPage
            {
                Text = "Phiếu mượn",
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            dgvBorrow = CreateDataGridView();
            dgvBorrow.Columns.Add("BorrowCode", "Mã phiếu");
            dgvBorrow.Columns.Add("MemberName", "Độc giả");
            dgvBorrow.Columns.Add("BookTitle", "Tên sách");
            dgvBorrow.Columns.Add("BorrowDate", "Ngày mượn");
            dgvBorrow.Columns.Add("DueDate", "Hạn trả");
            dgvBorrow.Columns.Add("Status", "Trạng thái");
            dgvBorrow.Columns.Add("StaffName", "Nhân viên");

            dgvBorrow.Columns["BorrowCode"]!.Width = 120;
            dgvBorrow.Columns["MemberName"]!.Width = 150;
            dgvBorrow.Columns["BookTitle"]!.Width = 220;
            dgvBorrow.Columns["BorrowDate"]!.Width = 90;
            dgvBorrow.Columns["DueDate"]!.Width = 90;
            dgvBorrow.Columns["Status"]!.Width = 90;
            dgvBorrow.Columns["StaffName"]!.Width = 120;

            tabBorrow.Controls.Add(dgvBorrow);

            lblBorrowSummary = new Label
            {
                Location = new Point(10, 370),
                Size = new Size(900, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185)
            };
            tabBorrow.Controls.Add(lblBorrowSummary);

            // Tab Phiếu trả
            var tabReturn = new TabPage
            {
                Text = "Phiếu trả",
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            dgvReturn = CreateDataGridView();
            dgvReturn.Columns.Add("BorrowCode", "Mã phiếu");
            dgvReturn.Columns.Add("MemberName", "Độc giả");
            dgvReturn.Columns.Add("BookTitle", "Tên sách");
            dgvReturn.Columns.Add("BorrowDate", "Ngày mượn");
            dgvReturn.Columns.Add("ReturnDate", "Ngày trả");
            dgvReturn.Columns.Add("FineAmount", "Tiền phạt");
            dgvReturn.Columns.Add("StaffName", "Nhân viên");

            dgvReturn.Columns["BorrowCode"]!.Width = 120;
            dgvReturn.Columns["MemberName"]!.Width = 150;
            dgvReturn.Columns["BookTitle"]!.Width = 220;
            dgvReturn.Columns["BorrowDate"]!.Width = 90;
            dgvReturn.Columns["ReturnDate"]!.Width = 90;
            dgvReturn.Columns["FineAmount"]!.Width = 90;
            dgvReturn.Columns["StaffName"]!.Width = 120;

            tabReturn.Controls.Add(dgvReturn);

            lblReturnSummary = new Label
            {
                Location = new Point(10, 370),
                Size = new Size(900, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(39, 174, 96)
            };
            tabReturn.Controls.Add(lblReturnSummary);

            tabControl.TabPages.Add(tabBorrow);
            tabControl.TabPages.Add(tabReturn);
            this.Controls.Add(tabControl);

            // Close button
            var btnClose = new Button
            {
                Text = "Đóng",
                Location = new Point(865, 565),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);
        }

        private DataGridView CreateDataGridView()
        {
            var dgv = new DataGridView
            {
                Location = new Point(10, 10),
                Size = new Size(905, 355),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
            };
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(52, 73, 94),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            return dgv;
        }

        private void LoadData()
        {
            LoadBorrowRecords();
            LoadReturnRecords();
        }

        private void LoadBorrowRecords()
        {
            dgvBorrow.Rows.Clear();

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            SELECT br.BorrowCode, m.FullName AS MemberName, b.Title AS BookTitle,
                                   br.BorrowDate, br.DueDate, br.Status, u.FullName AS StaffName
                            FROM BorrowRecords br
                            INNER JOIN Members m ON br.MemberID = m.MemberID
                            INNER JOIN Books b ON br.BookID = b.BookID
                            LEFT JOIN Users u ON br.StaffID = u.UserID
                            WHERE br.BorrowDate BETWEEN @FromDate AND @ToDate
                            ORDER BY br.BorrowDate DESC";
                        cmd.Parameters.AddWithValue("@FromDate", dtpFrom.Value.Date);
                        cmd.Parameters.AddWithValue("@ToDate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1));

                        int count = 0;
                        int borrowing = 0;
                        int overdue = 0;
                        int returned = 0;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string status = reader["Status"]?.ToString() ?? "";
                                int rowIndex = dgvBorrow.Rows.Add(
                                    reader["BorrowCode"],
                                    reader["MemberName"],
                                    reader["BookTitle"],
                                    ((DateTime)reader["BorrowDate"]).ToString("dd/MM/yyyy"),
                                    ((DateTime)reader["DueDate"]).ToString("dd/MM/yyyy"),
                                    status,
                                    reader["StaffName"]
                                );

                                // Color by status
                                if (status == "Quá hạn")
                                {
                                    dgvBorrow.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Red;
                                    overdue++;
                                }
                                else if (status == "Đã trả")
                                {
                                    dgvBorrow.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Green;
                                    returned++;
                                }
                                else if (status == "Đang mượn")
                                {
                                    dgvBorrow.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.FromArgb(41, 128, 185);
                                    borrowing++;
                                }

                                count++;
                            }
                        }

                        lblBorrowSummary.Text = $"📊 Tổng: {count} phiếu | 📖 Đang mượn: {borrowing} | ⚠️ Quá hạn: {overdue} | ✅ Đã trả: {returned}";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu phiếu mượn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadReturnRecords()
        {
            dgvReturn.Rows.Clear();

            try
            {
                using (var conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"
                            SELECT br.BorrowCode, m.FullName AS MemberName, b.Title AS BookTitle,
                                   br.BorrowDate, br.ReturnDate, br.FineAmount, u.FullName AS StaffName
                            FROM BorrowRecords br
                            INNER JOIN Members m ON br.MemberID = m.MemberID
                            INNER JOIN Books b ON br.BookID = b.BookID
                            LEFT JOIN Users u ON br.StaffID = u.UserID
                            WHERE br.Status = N'Đã trả' 
                              AND br.ReturnDate BETWEEN @FromDate AND @ToDate
                            ORDER BY br.ReturnDate DESC";
                        cmd.Parameters.AddWithValue("@FromDate", dtpFrom.Value.Date);
                        cmd.Parameters.AddWithValue("@ToDate", dtpTo.Value.Date.AddDays(1).AddSeconds(-1));

                        decimal totalFine = 0;
                        int count = 0;

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                decimal fine = reader["FineAmount"] != DBNull.Value ? (decimal)reader["FineAmount"] : 0;
                                totalFine += fine;

                                int rowIndex = dgvReturn.Rows.Add(
                                    reader["BorrowCode"],
                                    reader["MemberName"],
                                    reader["BookTitle"],
                                    ((DateTime)reader["BorrowDate"]).ToString("dd/MM/yyyy"),
                                    reader["ReturnDate"] != DBNull.Value ? ((DateTime)reader["ReturnDate"]).ToString("dd/MM/yyyy") : "-",
                                    fine > 0 ? fine.ToString("N0") + " đ" : "-",
                                    reader["StaffName"]
                                );

                                if (fine > 0)
                                    dgvReturn.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.OrangeRed;

                                count++;
                            }
                        }

                        lblReturnSummary.Text = $"📊 Tổng: {count} phiếu trả | 💰 Tổng tiền phạt đã thu: {totalFine:N0} đ";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu phiếu trả: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToExcel()
        {
            DataGridView currentDgv = tabControl.SelectedIndex == 0 ? dgvBorrow : dgvReturn;
            string type = tabControl.SelectedIndex == 0 ? "PhieuMuon" : "PhieuTra";

            if (currentDgv.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
                saveDialog.FileName = $"{type}_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var writer = new System.IO.StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8))
                        {
                            // Headers
                            var headers = new System.Collections.Generic.List<string>();
                            foreach (DataGridViewColumn col in currentDgv.Columns)
                            {
                                headers.Add(col.HeaderText);
                            }
                            writer.WriteLine(string.Join(",", headers));

                            // Data
                            foreach (DataGridViewRow row in currentDgv.Rows)
                            {
                                var values = new System.Collections.Generic.List<string>();
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    values.Add($"\"{cell.Value?.ToString() ?? ""}\"");
                                }
                                writer.WriteLine(string.Join(",", values));
                            }
                        }

                        MessageBox.Show("Xuất file thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{saveDialog.FileName}\"");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi xuất file: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
