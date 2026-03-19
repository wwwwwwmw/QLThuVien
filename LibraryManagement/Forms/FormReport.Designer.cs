using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormReport
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitle = null!;
        private Panel panelFilter = null!;
        private Label lblType = null!;
        private Label lblFrom = null!;
        private Label lblTo = null!;
        private Button btnGenerate = null!;
        private Button btnExport = null!;
        private Button btnPrint = null!;
        private Label lblQuickActions = null!;
        private Button btnBorrowReturnDetails = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new Label();
            this.panelFilter = new Panel();
            this.btnBorrowReturnDetails = new Button();
            this.lblQuickActions = new Label();
            this.btnPrint = new Button();
            this.btnExport = new Button();
            this.btnGenerate = new Button();
            this.lblTo = new Label();
            this.lblFrom = new Label();
            this.lblType = new Label();
            this.cboReportType = new ComboBox();
            this.dtpFrom = new DateTimePicker();
            this.dtpTo = new DateTimePicker();
            this.panelContent = new Panel();
            this.lblSummary = new Label();
            this.panelFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTitle.ForeColor = Color.FromArgb(44, 62, 80);
            this.lblTitle.Location = new Point(20, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new Size(291, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "THỐNG KÊ - BÁO CÁO";
            // 
            // panelFilter
            // 
            this.panelFilter.BackColor = Color.White;
            this.panelFilter.Controls.Add(this.btnBorrowReturnDetails);
            this.panelFilter.Controls.Add(this.lblQuickActions);
            this.panelFilter.Controls.Add(this.btnPrint);
            this.panelFilter.Controls.Add(this.btnExport);
            this.panelFilter.Controls.Add(this.btnGenerate);
            this.panelFilter.Controls.Add(this.lblTo);
            this.panelFilter.Controls.Add(this.lblFrom);
            this.panelFilter.Controls.Add(this.lblType);
            this.panelFilter.Controls.Add(this.cboReportType);
            this.panelFilter.Controls.Add(this.dtpFrom);
            this.panelFilter.Controls.Add(this.dtpTo);
            this.panelFilter.Location = new Point(20, 55);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new Size(1180, 100);
            this.panelFilter.TabIndex = 1;
            // 
            // btnBorrowReturnDetails
            // 
            this.btnBorrowReturnDetails.BackColor = Color.FromArgb(41, 128, 185);
            this.btnBorrowReturnDetails.Cursor = Cursors.Hand;
            this.btnBorrowReturnDetails.FlatAppearance.BorderSize = 0;
            this.btnBorrowReturnDetails.FlatStyle = FlatStyle.Flat;
            this.btnBorrowReturnDetails.Font = new Font("Segoe UI", 9F);
            this.btnBorrowReturnDetails.ForeColor = Color.White;
            this.btnBorrowReturnDetails.Location = new Point(95, 57);
            this.btnBorrowReturnDetails.Name = "btnBorrowReturnDetails";
            this.btnBorrowReturnDetails.Size = new Size(80, 32);
            this.btnBorrowReturnDetails.TabIndex = 10;
            this.btnBorrowReturnDetails.Text = "Mượn/Trả";
            this.btnBorrowReturnDetails.UseVisualStyleBackColor = false;
            this.btnBorrowReturnDetails.Click += this.BtnBorrowReturnDetails_Click;
            // 
            // lblQuickActions
            // 
            this.lblQuickActions.AutoSize = true;
            this.lblQuickActions.Font = new Font("Segoe UI", 9F);
            this.lblQuickActions.Location = new Point(15, 62);
            this.lblQuickActions.Name = "lblQuickActions";
            this.lblQuickActions.Size = new Size(74, 15);
            this.lblQuickActions.TabIndex = 9;
            this.lblQuickActions.Text = "Xem chi tiết:";
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = Color.FromArgb(155, 89, 182);
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatStyle = FlatStyle.Flat;
            this.btnPrint.Font = new Font("Segoe UI", 9F);
            this.btnPrint.ForeColor = Color.White;
            this.btnPrint.Location = new Point(825, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new Size(50, 32);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "In";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += this.BtnPrint_Click;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = Color.FromArgb(46, 204, 113);
            this.btnExport.FlatAppearance.BorderSize = 0;
            this.btnExport.FlatStyle = FlatStyle.Flat;
            this.btnExport.Font = new Font("Segoe UI", 9F);
            this.btnExport.ForeColor = Color.White;
            this.btnExport.Location = new Point(730, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new Size(85, 32);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Xuất Excel";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += this.BtnExport_Click;
            // 
            // btnGenerate
            // 
            this.btnGenerate.BackColor = Color.FromArgb(52, 152, 219);
            this.btnGenerate.FlatAppearance.BorderSize = 0;
            this.btnGenerate.FlatStyle = FlatStyle.Flat;
            this.btnGenerate.Font = new Font("Segoe UI", 9F);
            this.btnGenerate.ForeColor = Color.White;
            this.btnGenerate.Location = new Point(630, 12);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new Size(90, 32);
            this.btnGenerate.TabIndex = 6;
            this.btnGenerate.Text = "Tạo báo cáo";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += this.BtnGenerate_Click;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new Font("Segoe UI", 9F);
            this.lblTo.Location = new Point(460, 17);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new Size(30, 15);
            this.lblTo.TabIndex = 4;
            this.lblTo.Text = "Đến:";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new Font("Segoe UI", 9F);
            this.lblFrom.Location = new Point(305, 17);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new Size(22, 15);
            this.lblFrom.TabIndex = 2;
            this.lblFrom.Text = "Từ:";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new Font("Segoe UI", 9F);
            this.lblType.Location = new Point(15, 17);
            this.lblType.Name = "lblType";
            this.lblType.Size = new Size(52, 15);
            this.lblType.TabIndex = 0;
            this.lblType.Text = "Báo cáo:";
            // 
            // cboReportType
            // 
            this.cboReportType.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboReportType.Font = new Font("Segoe UI", 9F);
            this.cboReportType.FormattingEnabled = true;
            this.cboReportType.Items.AddRange(new object[] {
            "Tổng quan",
            "Sách mượn nhiều nhất",
            "Độc giả mượn nhiều nhất",
            "Sách quá hạn",
            "Thống kê theo ngày",
            "Danh sách phạt chưa thu",
            "Danh sách sách hết"});
            this.cboReportType.Location = new Point(70, 14);
            this.cboReportType.Name = "cboReportType";
            this.cboReportType.Size = new Size(220, 23);
            this.cboReportType.TabIndex = 1;
            this.cboReportType.SelectedIndexChanged += this.CboReportType_SelectedIndexChanged;
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = DateTimePickerFormat.Short;
            this.dtpFrom.Location = new Point(330, 14);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new Size(120, 23);
            this.dtpFrom.TabIndex = 3;
            // 
            // dtpTo
            // 
            this.dtpTo.Format = DateTimePickerFormat.Short;
            this.dtpTo.Location = new Point(495, 14);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new Size(120, 23);
            this.dtpTo.TabIndex = 5;
            // 
            // panelContent
            // 
            this.panelContent.AutoScroll = true;
            this.panelContent.BackColor = Color.White;
            this.panelContent.Location = new Point(20, 165);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new Size(1180, 355);
            this.panelContent.TabIndex = 2;
            // 
            // lblSummary
            // 
            this.lblSummary.Font = new Font("Segoe UI", 11F);
            this.lblSummary.Location = new Point(20, 530);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new Size(1180, 30);
            this.lblSummary.TabIndex = 3;
            // 
            // FormReport
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(236, 240, 241);
            this.ClientSize = new Size(1220, 570);
            this.Controls.Add(this.lblSummary);
            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelFilter);
            this.Controls.Add(this.lblTitle);
            this.DoubleBuffered = true;
            this.Name = "FormReport";
            this.Text = "Thống kê báo cáo";
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private Panel panelContent;
        private ComboBox cboReportType;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private DataGridView dgvReport = null!;
        private Label lblSummary;
    }
}
