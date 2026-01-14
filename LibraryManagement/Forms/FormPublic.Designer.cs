using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormPublic
    {
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1280, 720);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "📚 Thư Viện Sách - Tra cứu công khai";
            this.WindowState = FormWindowState.Maximized;
            this.DoubleBuffered = true;
            this.ResumeLayout(false);
        }
    }
}
