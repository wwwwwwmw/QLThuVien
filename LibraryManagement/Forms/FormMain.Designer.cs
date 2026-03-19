using System.Drawing;
using System.Windows.Forms;

namespace LibraryManagement.Forms
{
    partial class FormMain
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelHeader;
        private Panel panelMenu;
        private Panel panelContent;
        private Label lblCurrentUser;
        private Label lblDateTime;
        private Label lblHeaderTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelHeader = new Panel();
            lblHeaderTitle = new Label();
            lblCurrentUser = new Label();
            lblDateTime = new Label();
            panelMenu = new Panel();
            panelContent = new Panel();
            panelHeader.SuspendLayout();
            SuspendLayout();

            // panelHeader
            panelHeader.BackColor = Color.FromArgb(41, 128, 185);
            panelHeader.Controls.Add(lblHeaderTitle);
            panelHeader.Controls.Add(lblCurrentUser);
            panelHeader.Controls.Add(lblDateTime);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(1463, 60);
            panelHeader.TabIndex = 0;

            // lblHeaderTitle
            lblHeaderTitle.AutoSize = true;
            lblHeaderTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.White;
            lblHeaderTitle.Location = new Point(20, 15);
            lblHeaderTitle.Name = "lblHeaderTitle";
            lblHeaderTitle.Size = new Size(401, 37);
            lblHeaderTitle.TabIndex = 0;
            lblHeaderTitle.Text = "📚 HỆ THỐNG QUẢN LÝ THƯ VIỆN";

            // lblCurrentUser
            lblCurrentUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblCurrentUser.Font = new Font("Segoe UI", 10F);
            lblCurrentUser.ForeColor = Color.White;
            lblCurrentUser.Location = new Point(1140, 10);
            lblCurrentUser.Name = "lblCurrentUser";
            lblCurrentUser.Size = new Size(300, 20);
            lblCurrentUser.TabIndex = 1;
            lblCurrentUser.Text = "👤 Người dùng";
            lblCurrentUser.TextAlign = ContentAlignment.MiddleRight;

            // lblDateTime
            lblDateTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblDateTime.Font = new Font("Segoe UI", 10F);
            lblDateTime.ForeColor = Color.White;
            lblDateTime.Location = new Point(1140, 32);
            lblDateTime.Name = "lblDateTime";
            lblDateTime.Size = new Size(300, 20);
            lblDateTime.TabIndex = 2;
            lblDateTime.Text = "🕐 01/01/2026 08:00:00";
            lblDateTime.TextAlign = ContentAlignment.MiddleRight;

            // panelMenu
            panelMenu.BackColor = Color.FromArgb(44, 62, 80);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 60);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(220, 900);
            panelMenu.TabIndex = 1;

            // panelContent
            panelContent.BackColor = Color.FromArgb(236, 240, 241);
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(220, 60);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(20);
            panelContent.Size = new Size(1243, 900);
            panelContent.TabIndex = 2;

            // FormMain
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(236, 240, 241);
            ClientSize = new Size(1463, 960);
            Controls.Add(panelContent);
            Controls.Add(panelMenu);
            Controls.Add(panelHeader);
            Margin = new Padding(3, 4, 3, 4);
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý Thư viện - Hệ thống đa máy LAN";
            WindowState = FormWindowState.Maximized;
            FormClosing += FormMain_FormClosing;
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ResumeLayout(false);
        }
    }
}