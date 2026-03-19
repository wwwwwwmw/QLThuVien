using LibraryManagement.Data;
using LibraryManagement.Models;
using System;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form chính của ứng dụng - Dashboard
    /// </summary>
    public partial class FormMain : Form
    {
        private System.Windows.Forms.Timer timerDateTime = null!;

        // Dashboard controls
        private Label lblTotalBooks = null!;
        private Label lblTotalMembers = null!;
        private Label lblBorrowing = null!;
        private Label lblOverdue = null!;
        private Label lblTodayBorrow = null!;
        private Label lblTodayReturn = null!;
        private DataGridView dgvRecentBorrows = null!;
        private DataGridView dgvOverdueList = null!;
        private Label lblRecentLabel = null!;
        private Label lblOverdueLabel = null!;
        private bool isLoggingOut = false;

        public FormMain()
        {
            InitializeComponent();
            Load += FormMain_Load;
        }

        private void FormMain_Load(object? sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            BackColor = Color.FromArgb(236, 240, 241);

            lblCurrentUser.Text = $"👤 {CurrentUser.User?.FullName ?? "Người dùng"} ({CurrentUser.User?.Role ?? "N/A"})";
            lblDateTime.Text = $"🕐 {DateTime.Now:dd/MM/yyyy HH:mm:ss}";

            panelMenu.Controls.Clear();
            panelContent.Controls.Clear();

            SetupMenu();

            panelContent.Resize -= PanelContent_Resize;
            panelContent.Resize += PanelContent_Resize;

            panelHeader.Resize -= PanelHeader_Resize;
            panelHeader.Resize += PanelHeader_Resize;
            PanelHeader_Resize(null, EventArgs.Empty);

            timerDateTime = new System.Windows.Forms.Timer { Interval = 1000 };
            timerDateTime.Tick += TimerDateTime_Tick;
            timerDateTime.Start();

            SetupDashboard();
            LoadDashboard();
        }

        private void TimerDateTime_Tick(object? sender, EventArgs e)
        {
            lblDateTime.Text = $"🕐 {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
        }

        private void PanelHeader_Resize(object? sender, EventArgs e)
        {
            if (lblCurrentUser != null)
                lblCurrentUser.Location = new Point(panelHeader.Width - 320, 10);

            if (lblDateTime != null)
                lblDateTime.Location = new Point(panelHeader.Width - 320, 32);
        }

        private void SetupMenu()
        {
            int y = 20;
            int btnHeight = 45;
            int spacing = 5;

            AddMenuButton("Trang chủ", y, BtnHome_Click);
            y += btnHeight + spacing;

            AddMenuButton("Quản lý Sách", y, BtnBooks_Click);
            y += btnHeight + spacing;

            AddMenuButton("Quản lý Độc giả", y, BtnMembers_Click);
            y += btnHeight + spacing;

            AddMenuButton("Mượn sách", y, BtnBorrow_Click);
            y += btnHeight + spacing;

            AddMenuButton("Trả sách", y, BtnReturn_Click);
            y += btnHeight + spacing;

            AddMenuButton("Báo cáo & Thống kê", y, BtnReport_Click);
            y += btnHeight + spacing;

            if (CurrentUser.User?.IsAdmin == true)
            {
                y += 20;
                AddMenuButton("Quản lý Tài khoản", y, BtnUsers_Click);
                y += btnHeight + spacing;

                AddMenuButton("Cấu hình hệ thống", y, BtnSettings_Click);
                y += btnHeight + spacing;
            }

            var btnLogout = new Button
            {
                Text = "Đăng xuất",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(192, 57, 43),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 40),
                Location = new Point(10, panelMenu.Height - 60),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += BtnLogout_Click;
            panelMenu.Controls.Add(btnLogout);
        }

        private void AddMenuButton(string text, int y, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(52, 73, 94),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 45),
                Location = new Point(10, y),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(41, 128, 185);
            btn.Click += onClick;
            panelMenu.Controls.Add(btn);
        }

        private void BtnHome_Click(object? sender, EventArgs e)
        {
            ClearContent();
            SetupDashboard();
            LoadDashboard();
        }

        private void BtnBooks_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormBookManagement());
        }

        private void BtnMembers_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormMemberManagement());
        }

        private void BtnBorrow_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormBorrow());
        }

        private void BtnReturn_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormReturn());
        }

        private void BtnReport_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormReport());
        }

        private void BtnUsers_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormUserManagement());
        }

        private void BtnSettings_Click(object? sender, EventArgs e)
        {
            OpenForm(new FormSettings());
        }

        private void SetupDashboard()
        {
            panelContent.Controls.Clear();

            int cardWidth = 180;
            int cardHeight = 100;
            int spacing = 20;
            int x = 20;
            int y = 20;

            var card1 = CreateStatCard("Tổng số sách", "0", Color.FromArgb(52, 152, 219), x, y, cardWidth, cardHeight);
            lblTotalBooks = (Label)card1.Controls["lblValue"]!;
            panelContent.Controls.Add(card1);
            x += cardWidth + spacing;

            var card2 = CreateStatCard("Độc giả", "0", Color.FromArgb(46, 204, 113), x, y, cardWidth, cardHeight);
            lblTotalMembers = (Label)card2.Controls["lblValue"]!;
            panelContent.Controls.Add(card2);
            x += cardWidth + spacing;

            var card3 = CreateStatCard("Đang mượn", "0", Color.FromArgb(155, 89, 182), x, y, cardWidth, cardHeight);
            lblBorrowing = (Label)card3.Controls["lblValue"]!;
            panelContent.Controls.Add(card3);
            x += cardWidth + spacing;

            var card4 = CreateStatCard("Quá hạn", "0", Color.FromArgb(231, 76, 60), x, y, cardWidth, cardHeight);
            lblOverdue = (Label)card4.Controls["lblValue"]!;
            panelContent.Controls.Add(card4);
            x += cardWidth + spacing;

            var card5 = CreateStatCard("Mượn hôm nay", "0", Color.FromArgb(241, 196, 15), x, y, cardWidth, cardHeight);
            lblTodayBorrow = (Label)card5.Controls["lblValue"]!;
            panelContent.Controls.Add(card5);
            x += cardWidth + spacing;

            var card6 = CreateStatCard("Trả hôm nay", "0", Color.FromArgb(26, 188, 156), x, y, cardWidth, cardHeight);
            lblTodayReturn = (Label)card6.Controls["lblValue"]!;
            panelContent.Controls.Add(card6);

            int margin = 20;
            int splitSpacing = 20;
            int yTop = 140;
            int halfWidth = (panelContent.ClientSize.Width - margin * 2 - splitSpacing) / 2;
            int leftX = margin;
            int rightX = margin + halfWidth + splitSpacing;
            int gridTop = yTop + 30;
            int gridHeight = Math.Max(200, panelContent.ClientSize.Height - gridTop - 70);

            lblRecentLabel = new Label
            {
                Text = "Mượn sách gần đây",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(leftX, yTop),
                AutoSize = true
            };
            panelContent.Controls.Add(lblRecentLabel);

            dgvRecentBorrows = CreateDataGridView(leftX, gridTop, halfWidth, gridHeight);
            dgvRecentBorrows.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;
            dgvRecentBorrows.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecentBorrows.Columns.Add("BorrowCode", "Mã phiếu");
            dgvRecentBorrows.Columns.Add("MemberName", "Độc giả");
            dgvRecentBorrows.Columns.Add("BookTitle", "Tên sách");
            dgvRecentBorrows.Columns.Add("BorrowDate", "Ngày mượn");
            dgvRecentBorrows.Columns.Add("DueDate", "Hạn trả");
            panelContent.Controls.Add(dgvRecentBorrows);

            lblOverdueLabel = new Label
            {
                Text = "Sách quá hạn",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(192, 57, 43),
                Location = new Point(rightX, yTop),
                AutoSize = true
            };
            panelContent.Controls.Add(lblOverdueLabel);

            dgvOverdueList = CreateDataGridView(rightX, gridTop, halfWidth, gridHeight);
            dgvOverdueList.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            dgvOverdueList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOverdueList.Columns.Add("MemberName", "Độc giả");
            dgvOverdueList.Columns.Add("BookTitle", "Tên sách");
            dgvOverdueList.Columns.Add("DueDate", "Hạn trả");
            dgvOverdueList.Columns.Add("DaysOverdue", "Quá hạn");
            panelContent.Controls.Add(dgvOverdueList);

            var btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 35),
                Location = new Point(margin, panelContent.ClientSize.Height - 45),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefreshDashboard_Click;
            panelContent.Controls.Add(btnRefresh);

            AdjustGridLayout();
        }

        private void BtnRefreshDashboard_Click(object? sender, EventArgs e)
        {
            LoadDashboard();
        }

        private Panel CreateStatCard(string title, string value, Color color, int x, int y, int width, int height)
        {
            var panel = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = color
            };

            var lblTitle = new Label
            {
                Text = title,
                Name = "lblTitle",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.White,
                Location = new Point(10, 10),
                Size = new Size(width - 20, 25)
            };

            var lblValueCard = new Label
            {
                Text = value,
                Name = "lblValue",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 40),
                Size = new Size(width - 20, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblValueCard);
            return panel;
        }

        private DataGridView CreateDataGridView(int x, int y, int width, int height)
        {
            var dgv = new DataGridView
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
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
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Padding = new Padding(5)
            };

            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9),
                Padding = new Padding(5)
            };

            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(245, 245, 245)
            };

            dgv.EnableHeadersVisualStyles = false;
            return dgv;
        }

        private void LoadDashboard()
        {
            try
            {
                var borrowDAO = new BorrowRecordDAO();
                var stats = borrowDAO.GetDashboardStats();

                lblTotalBooks.Text = stats.TotalBooks.ToString("N0");
                lblTotalMembers.Text = stats.TotalMembers.ToString("N0");
                lblBorrowing.Text = stats.BorrowingCount.ToString("N0");
                lblOverdue.Text = stats.OverdueCount.ToString("N0");
                lblTodayBorrow.Text = stats.TodayBorrows.ToString("N0");
                lblTodayReturn.Text = stats.TodayReturns.ToString("N0");

                var recentBorrows = borrowDAO.Search(status: BorrowRecord.STATUS_BORROWING);
                dgvRecentBorrows.Rows.Clear();
                foreach (var borrow in recentBorrows.Take(10))
                {
                    dgvRecentBorrows.Rows.Add(
                        borrow.BorrowCode,
                        borrow.MemberName,
                        borrow.BookTitle,
                        borrow.BorrowDate.ToString("dd/MM/yyyy"),
                        borrow.DueDate.ToString("dd/MM/yyyy")
                    );
                }

                var overdueList = borrowDAO.GetOverdueRecords();
                dgvOverdueList.Rows.Clear();
                foreach (var record in overdueList.Take(10))
                {
                    int daysOverdue = (int)(DateTime.Now - record.DueDate).TotalDays;
                    dgvOverdueList.Rows.Add(
                        record.MemberName,
                        record.BookTitle,
                        record.DueDate.ToString("dd/MM/yyyy"),
                        $"{daysOverdue} ngày"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearContent()
        {
            panelContent.Controls.Clear();
        }

        private void OpenForm(Form form)
        {
            ClearContent();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelContent.Controls.Add(form);
            form.Show();
        }

        private void PanelContent_Resize(object? sender, EventArgs e)
        {
            AdjustGridLayout();
        }

        private void AdjustGridLayout()
        {
            if (panelContent == null || dgvRecentBorrows == null || dgvOverdueList == null)
                return;

            int margin = 20;
            int splitSpacing = 20;
            int yTop = 140;
            int gridTop = yTop + 30;
            int halfWidth = Math.Max(200, (panelContent.ClientSize.Width - margin * 2 - splitSpacing) / 2);
            int leftX = margin;
            int rightX = margin + halfWidth + splitSpacing;
            int gridHeight = Math.Max(200, panelContent.ClientSize.Height - gridTop - 70);

            dgvRecentBorrows.Location = new Point(leftX, gridTop);
            dgvRecentBorrows.Size = new Size(halfWidth, gridHeight);

            dgvOverdueList.Location = new Point(rightX, gridTop);
            dgvOverdueList.Size = new Size(halfWidth, gridHeight);

            if (lblRecentLabel != null) lblRecentLabel.Location = new Point(leftX, yTop);
            if (lblOverdueLabel != null) lblOverdueLabel.Location = new Point(rightX, yTop);
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var logDAO = new ActivityLogDAO();
                logDAO.Log("Đăng xuất hệ thống");
                isLoggingOut = true;
                CurrentUser.Logout();
                Close();
            }
        }

        private void FormMain_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (isLoggingOut)
            {
                timerDateTime?.Stop();
                return;
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
                var result = MessageBox.Show("Bạn có chắc muốn thoát ứng dụng?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    timerDateTime?.Stop();
                    var logDAO = new ActivityLogDAO();
                    logDAO.Log("Thoát ứng dụng");
                }
            }
        }
    }
}