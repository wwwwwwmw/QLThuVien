using System;
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    /// <summary>
    /// Form chính của ứng dụng - Dashboard
    /// </summary>
    public partial class FormMain : Form
    {
        private Panel panelMenu = null!;
        private Panel panelContent = null!;
        private Panel panelHeader = null!;
        private Label lblCurrentUser = null!;
        private Label lblDateTime = null!;
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

        public FormMain()
        {
            InitializeComponent();
            SetupForm();
            LoadDashboard();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1280, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Quản lý Thư viện - Hệ thống đa máy LAN";
            this.WindowState = FormWindowState.Maximized;
            this.FormClosing += FormMain_FormClosing;
            this.ResumeLayout(false);
        }

        private void SetupForm()
        {
            this.BackColor = Color.FromArgb(236, 240, 241);

            // Header Panel
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(41, 128, 185)
            };

            Label lblTitle = new Label
            {
                Text = "📚 HỆ THỐNG QUẢN LÝ THƯ VIỆN",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };

            lblCurrentUser = new Label
            {
                Text = $"👤 {CurrentUser.User?.FullName} ({CurrentUser.User?.Role})",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Size = new Size(300, 25)
            };

            lblDateTime = new Label
            {
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleRight,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Size = new Size(300, 25)
            };

            panelHeader.Controls.AddRange(new Control[] { lblTitle, lblCurrentUser, lblDateTime });

            // Xử lý resize để đặt vị trí các label bên phải
            panelHeader.Resize += (s, e) =>
            {
                lblCurrentUser.Location = new Point(panelHeader.Width - 320, 10);
                lblDateTime.Location = new Point(panelHeader.Width - 320, 32);
            };

            // Set vị trí ban đầu khi form load
            this.Load += (s, e) =>
            {
                lblCurrentUser.Location = new Point(panelHeader.Width - 320, 10);
                lblDateTime.Location = new Point(panelHeader.Width - 320, 32);
            };

            // Menu Panel
            panelMenu = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.FromArgb(44, 62, 80)
            };

            SetupMenu();

            // Content Panel
            panelContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(236, 240, 241),
                Padding = new Padding(20)
            };

            // Add controls in correct order
            this.Controls.Add(panelContent);
            this.Controls.Add(panelMenu);
            this.Controls.Add(panelHeader);

            // Timer for datetime
            timerDateTime = new System.Windows.Forms.Timer { Interval = 1000 };
            timerDateTime.Tick += (s, e) => lblDateTime.Text = $"🕐 {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            timerDateTime.Start();

            SetupDashboard();
        }

        private void SetupMenu()
        {
            int y = 20;
            int btnHeight = 45;
            int spacing = 5;

            // Menu buttons
            AddMenuButton("Trang chủ", y, () => { ClearContent(); SetupDashboard(); LoadDashboard(); });
            y += btnHeight + spacing;

            AddMenuButton("Quản lý Sách", y, () => OpenForm(new FormBookManagement()));
            y += btnHeight + spacing;

            AddMenuButton("Quản lý Độc giả", y, () => OpenForm(new FormMemberManagement()));
            y += btnHeight + spacing;

            AddMenuButton("Mượn sách", y, () => OpenForm(new FormBorrow()));
            y += btnHeight + spacing;

            AddMenuButton("Trả sách", y, () => OpenForm(new FormReturn()));
            y += btnHeight + spacing;

            AddMenuButton("Báo cáo & Thống kê", y, () => OpenForm(new FormReport()));
            y += btnHeight + spacing;

            // Admin only
            if (CurrentUser.User?.IsAdmin == true)
            {
                y += 20; // separator
                AddMenuButton("Quản lý Tài khoản", y, () => OpenForm(new FormUserManagement()));
                y += btnHeight + spacing;

                AddMenuButton("Cấu hình hệ thống", y, () => OpenForm(new FormSettings()));
                y += btnHeight + spacing;
            }

            // Logout button at bottom
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

        private void AddMenuButton(string text, int y, Action onClick)
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
            btn.Click += (s, e) => onClick();
            panelMenu.Controls.Add(btn);
        }

        private void SetupDashboard()
        {
            panelContent.Controls.Clear();

            // Stats cards
            int cardWidth = 180;
            int cardHeight = 100;
            int spacing = 20;
            int x = 20;
            int y = 20;

            // Card 1: Total Books
            var card1 = CreateStatCard("Tổng số sách", "0", Color.FromArgb(52, 152, 219), x, y, cardWidth, cardHeight);
            lblTotalBooks = (Label)card1.Controls[1];
            panelContent.Controls.Add(card1);
            x += cardWidth + spacing;

            // Card 2: Total Members
            var card2 = CreateStatCard("Độc giả", "0", Color.FromArgb(46, 204, 113), x, y, cardWidth, cardHeight);
            lblTotalMembers = (Label)card2.Controls[1];
            panelContent.Controls.Add(card2);
            x += cardWidth + spacing;

            // Card 3: Borrowing
            var card3 = CreateStatCard("Đang mượn", "0", Color.FromArgb(155, 89, 182), x, y, cardWidth, cardHeight);
            lblBorrowing = (Label)card3.Controls[1];
            panelContent.Controls.Add(card3);
            x += cardWidth + spacing;

            // Card 4: Overdue
            var card4 = CreateStatCard("Quá hạn", "0", Color.FromArgb(231, 76, 60), x, y, cardWidth, cardHeight);
            lblOverdue = (Label)card4.Controls[1];
            panelContent.Controls.Add(card4);
            x += cardWidth + spacing;

            // Card 5: Today Borrow
            var card5 = CreateStatCard("Mượn hôm nay", "0", Color.FromArgb(241, 196, 15), x, y, cardWidth, cardHeight);
            lblTodayBorrow = (Label)card5.Controls[1];
            panelContent.Controls.Add(card5);
            x += cardWidth + spacing;

            // Card 6: Today Return
            var card6 = CreateStatCard("Trả hôm nay", "0", Color.FromArgb(26, 188, 156), x, y, cardWidth, cardHeight);
            lblTodayReturn = (Label)card6.Controls[1];
            panelContent.Controls.Add(card6);

            // DataGridViews
            y = 140;

            // Recent borrows
            var lblRecent = new Label
            {
                Text = "Mượn sách gần đây",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, y),
                AutoSize = true
            };
            panelContent.Controls.Add(lblRecent);
            y += 30;

            dgvRecentBorrows = CreateDataGridView(20, y, 580, 250);
            dgvRecentBorrows.Columns.Add("BorrowCode", "Mã phiếu");
            dgvRecentBorrows.Columns.Add("MemberName", "Độc giả");
            dgvRecentBorrows.Columns.Add("BookTitle", "Tên sách");
            dgvRecentBorrows.Columns.Add("BorrowDate", "Ngày mượn");
            dgvRecentBorrows.Columns.Add("DueDate", "Hạn trả");
            dgvRecentBorrows.Columns["BorrowCode"]!.Width = 90;
            dgvRecentBorrows.Columns["MemberName"]!.Width = 110;
            dgvRecentBorrows.Columns["BookTitle"]!.Width = 170;
            dgvRecentBorrows.Columns["BorrowDate"]!.Width = 95;
            dgvRecentBorrows.Columns["DueDate"]!.Width = 85;
            panelContent.Controls.Add(dgvRecentBorrows);

            // Overdue list
            var lblOverdueList = new Label
            {
                Text = "Sách quá hạn",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(192, 57, 43),
                Location = new Point(620, 140),
                AutoSize = true
            };
            panelContent.Controls.Add(lblOverdueList);

            dgvOverdueList = CreateDataGridView(620, 170, 580, 250);
            dgvOverdueList.Columns.Add("MemberName", "Độc giả");
            dgvOverdueList.Columns.Add("BookTitle", "Tên sách");
            dgvOverdueList.Columns.Add("DueDate", "Hạn trả");
            dgvOverdueList.Columns.Add("DaysOverdue", "Quá hạn");
            dgvOverdueList.Columns["MemberName"]!.Width = 150;
            dgvOverdueList.Columns["BookTitle"]!.Width = 220;
            dgvOverdueList.Columns["DueDate"]!.Width = 100;
            dgvOverdueList.Columns["DaysOverdue"]!.Width = 100;
            panelContent.Controls.Add(dgvOverdueList);

            // Refresh button
            var btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(120, 35),
                Location = new Point(20, 430),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadDashboard();
            panelContent.Controls.Add(btnRefresh);
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
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.White,
                Location = new Point(10, 10),
                Size = new Size(width - 20, 25)
            };

            var lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 40),
                Size = new Size(width - 20, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.AddRange(new Control[] { lblTitle, lblValue });
            return panel;
        }

        private DataGridView CreateDataGridView(int x, int y, int width, int height)
        {
            return new DataGridView
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
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(52, 73, 94),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Padding = new Padding(5)
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Segoe UI", 9),
                    Padding = new Padding(5)
                },
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(245, 245, 245)
                }
            };
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

                // Load recent borrows
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

                // Load overdue list
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

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                var logDAO = new ActivityLogDAO();
                logDAO.Log("Đăng xuất hệ thống");

                CurrentUser.Logout();
                this.Hide();

                using (var loginForm = new FormLogin())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        // Re-setup with new user
                        SetupMenu();
                        ClearContent();
                        SetupDashboard();
                        LoadDashboard();
                        lblCurrentUser.Text = $"👤 {CurrentUser.User?.FullName} ({CurrentUser.User?.Role})";
                        this.Show();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
            }
        }

        private void FormMain_FormClosing(object? sender, FormClosingEventArgs e)
        {
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
