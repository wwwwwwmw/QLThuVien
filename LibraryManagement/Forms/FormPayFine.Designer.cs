using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    public partial class FormPayFine
    {
        private void InitializeComponent()
        {
            this.Text = "Đóng tiền phạt";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblMember = new Label
            {
                Text = $"Độc giả: {member.FullName} ({member.MemberCode})",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lblCurrentFine = new Label
            {
                Text = $"Số tiền nợ: {member.TotalFine:N0} VNĐ",
                Location = new Point(20, 50),
                AutoSize = true,
                ForeColor = Color.FromArgb(192, 57, 43)
            };

            var lblAmount = new Label { Text = "Số tiền đóng:", Location = new Point(20, 90), AutoSize = true };
            numAmount = new NumericUpDown
            {
                Location = new Point(120, 87),
                Size = new Size(150, 28),
                Maximum = member.TotalFine,
                Value = member.TotalFine,
                ThousandsSeparator = true
            };

            var lblMethod = new Label { Text = "Hình thức:", Location = new Point(20, 125), AutoSize = true };
            cboMethod = new ComboBox
            {
                Location = new Point(120, 122),
                Size = new Size(150, 28),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cboMethod.Items.AddRange(new object[] { FinePayment.METHOD_CASH, FinePayment.METHOD_TRANSFER });
            cboMethod.SelectedIndex = 0;

            var lblNotes = new Label { Text = "Ghi chú:", Location = new Point(20, 160), AutoSize = true };
            txtNotes = new TextBox
            {
                Location = new Point(120, 157),
                Size = new Size(230, 50),
                Multiline = true
            };

            var btnPay = new Button
            {
                Text = "💰 Thanh toán",
                Location = new Point(120, 220),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnPay.FlatAppearance.BorderSize = 0;
            btnPay.Click += BtnPay_Click;

            var btnCancel = new Button
            {
                Text = "Hủy",
                Location = new Point(230, 220),
                Size = new Size(80, 35),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.AddRange(new Control[] {
                lblMember, lblCurrentFine, lblAmount, numAmount,
                lblMethod, cboMethod, lblNotes, txtNotes, btnPay, btnCancel
            });
        }
    }
}
