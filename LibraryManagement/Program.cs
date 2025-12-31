using System;
using System.Windows.Forms;

namespace LibraryManagement
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            ApplicationConfiguration.Initialize();

            // Mở trang công khai trước - không yêu cầu đăng nhập
            // Người dùng có thể xem sách, tìm kiếm
            // Khi cần mượn sách hoặc các chức năng quản lý -> đăng nhập
            Application.Run(new Forms.FormPublic());
        }
    }
}
