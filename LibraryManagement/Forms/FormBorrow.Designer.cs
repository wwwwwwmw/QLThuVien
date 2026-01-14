using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    public partial class FormBorrow
    {
        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // FormBorrow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(236, 240, 241);
            ClientSize = new Size(958, 433);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormBorrow";
            Load += FormBorrow_Load;
            ResumeLayout(false);
        }
    }
}
