Quy tắc đã áp dụng:
Constructor: Chỉ giữ InitializeComponent() và gán sự kiện Load.

Form_Load:

Gọi SetupForm() đầu tiên (để Designer hiển thị được khung giao diện).

Chặn code chạy trong Designer bằng LicenseManager.UsageMode.

Bọc code Runtime (kết nối DB, load dữ liệu) trong try-catch.

1. LibraryManagement/Forms/FormLogin.cs
C#

using System;
using System.ComponentModel; // Quan trọng
using System.Drawing;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    public partial class FormLogin : Form
    {
        // Các biến field giữ nguyên
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private CheckBox chkRemember = null!;
        private Button btnLogin = null!;
        private Button btnConfig = null!;

        public FormLogin()
        {
            InitializeComponent();
            this.Load += FormLogin_Load; // Chuyển logic sang Load
        }

        private void FormLogin_Load(object? sender, EventArgs e)
        {
            // 1. Setup UI để Designer hiển thị layout
            SetupForm();

            // 2. Chặn Designer chạy code kết nối DB
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            // 3. Code Runtime
            try
            {
                // Kiểm tra kết nối database
                if (!DatabaseConnection.TestConnection(out string error))
                {
                    MessageBox.Show($"Không thể kết nối đến Database!\nLỗi: {error}\nVui lòng cấu hình lại kết nối.",
                        "Lỗi Kết Nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                    // Mở form cấu hình nếu lỗi
                    var formConfig = new FormConnectionConfig();
                    formConfig.ShowDialog();
                }
                else
                {
                    // Tự động điền nếu đã nhớ mật khẩu (Logic cũ)
                    if (Properties.Settings.Default.RememberMe)
                    {
                        txtUsername.Text = Properties.Settings.Default.Username;
                        txtPassword.Text = Properties.Settings.Default.Password;
                        chkRemember.Checked = true;
                    }
                }
                txtUsername.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo: " + ex.Message);
            }
        }

        // ... (Giữ nguyên hàm SetupForm, btnLogin_Click, v.v...)
    }
}
2. LibraryManagement/Forms/FormBookManagement.cs
C#

using System;
using System.ComponentModel;
using System.IO; // Cho Path.Combine
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    public partial class FormBookManagement : Form
    {
        public FormBookManagement()
        {
            InitializeComponent();
            this.Load += FormBookManagement_Load;
        }

        private void FormBookManagement_Load(object? sender, EventArgs e)
        {
            SetupForm(); // Cho phép Designer preview UI

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                EnsureImagesFolderExists(); // IO Operation
                LoadData(); // DB Operation
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu sách: " + ex.Message, "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ... (Giữ nguyên SetupForm, LoadData, EnsureImagesFolderExists và các event khác)
    }
}
3. LibraryManagement/Forms/FormMemberManagement.cs
C#

using System;
using System.ComponentModel;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    public partial class FormMemberManagement : Form
    {
        public FormMemberManagement()
        {
            InitializeComponent();
            this.Load += FormMemberManagement_Load;
        }

        private void FormMemberManagement_Load(object? sender, EventArgs e)
        {
            SetupForm();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu độc giả: " + ex.Message);
            }
        }

        // ... (Giữ nguyên phần còn lại)
    }
}
4. LibraryManagement/Forms/FormBorrow.cs
C#

using System;
using System.ComponentModel;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    public partial class FormBorrow : Form
    {
        public FormBorrow()
        {
            InitializeComponent();
            this.Load += FormBorrow_Load;
        }

        private void FormBorrow_Load(object? sender, EventArgs e)
        {
            SetupForm();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;
            
            // Form này không có LoadData() lúc khởi tạo, 
            // dữ liệu được load khi người dùng nhập mã thẻ/sách.
        }

        // ... (Giữ nguyên phần còn lại)
    }
}
5. LibraryManagement/Forms/FormReturn.cs
C#

using System;
using System.ComponentModel;
using System.Windows.Forms;
using LibraryManagement.Data;

namespace LibraryManagement.Forms
{
    public partial class FormReturn : Form
    {
        public FormReturn()
        {
            InitializeComponent();
            this.Load += FormReturn_Load;
        }

        private void FormReturn_Load(object? sender, EventArgs e)
        {
            SetupForm();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu trả sách: " + ex.Message);
            }
        }

        // ... (Giữ nguyên phần còn lại)
    }
}
6. LibraryManagement/Forms/FormReport.cs
C#

using System;
using System.ComponentModel;
using System.Windows.Forms;
using LibraryManagement.Data;

namespace LibraryManagement.Forms
{
    public partial class FormReport : Form
    {
        public FormReport()
        {
            InitializeComponent();
            this.Load += FormReport_Load;
        }

        private void FormReport_Load(object? sender, EventArgs e)
        {
            SetupForm();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                LoadDashboardStats();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải báo cáo: " + ex.Message);
            }
        }

        // ... (Giữ nguyên phần còn lại)
    }
}
7. LibraryManagement/Forms/FormUserManagement.cs
C#

using System;
using System.ComponentModel;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    public partial class FormUserManagement : Form
    {
        public FormUserManagement()
        {
            InitializeComponent();
            this.Load += FormUserManagement_Load;
        }

        private void FormUserManagement_Load(object? sender, EventArgs e)
        {
            SetupForm();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách người dùng: " + ex.Message);
            }
        }

        // ... (Giữ nguyên phần còn lại)
    }
}
8. LibraryManagement/Forms/FormSettings.cs
C#

using System;
using System.ComponentModel;
using System.Windows.Forms;
using LibraryManagement.Data;
using LibraryManagement.Models;

namespace LibraryManagement.Forms
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            this.Load += FormSettings_Load;
        }

        private void FormSettings_Load(object? sender, EventArgs e)
        {
            SetupForm();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                LoadSettings();
                LoadLogs();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải cấu hình: " + ex.Message);
            }
        }

        // ... (Giữ nguyên phần còn lại)
    }
}
9. LibraryManagement/Forms/FormBorrowReturnDetails.cs
C#

using System;
using System.ComponentModel;
using System.Windows.Forms;
using LibraryManagement.Data;

namespace LibraryManagement.Forms
{
    public partial class FormBorrowReturnDetails : Form
    {
        // ... fields giữ nguyên

        public FormBorrowReturnDetails(int borrowId = 0) // Constructor có tham số
        {
            InitializeComponent();
            // Lưu borrowId vào biến global nếu cần (đã có trong code cũ)
            this._borrowId = borrowId; 
            this.Load += FormBorrowReturnDetails_Load;
        }

        private void FormBorrowReturnDetails_Load(object? sender, EventArgs e)
        {
            SetupForm();

            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết phiếu mượn: " + ex.Message);
            }
        }

        // ... (Giữ nguyên phần còn lại)
    }
}
✅ Xác nhận
Bạn có thể copy-paste các đoạn code trên vào file tương ứng. Sau khi thay thế, hãy Rebuild Solution và mở lại các Form trong Designer (Shift + F7), giao diện sẽ hiển thị bình thường mà không bị lỗi trắng màn hình nữa.