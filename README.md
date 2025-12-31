# 📚 Hệ Thống Quản Lý Thư Viện (Library Management System)

## Giới Thiệu

Hệ thống quản lý thư viện hoàn chỉnh được xây dựng bằng **C# WinForms** và **SQL Server**, hỗ trợ chạy trên nhiều máy tính trong mạng **LAN**.

### Tính Năng Chính

- 📖 **Quản lý sách**: Thêm, sửa, xóa, tìm kiếm sách theo danh mục, tác giả, NXB
- 👥 **Quản lý độc giả**: Đăng ký thẻ, quản lý thông tin, theo dõi lịch sử mượn
- 🔄 **Mượn/Trả sách**: Quản lý phiếu mượn, gia hạn, tính phạt tự động
- 📊 **Thống kê báo cáo**: Báo cáo sách mượn nhiều, độc giả tích cực, quá hạn
- ⚙️ **Cài đặt hệ thống**: Cấu hình quy định mượn trả, tiền phạt
- 👤 **Phân quyền người dùng**: Admin, Manager, Staff

---

## 🖥️ Yêu Cầu Hệ Thống

### Server (Máy chủ SQL Server)

- Windows 10/11 hoặc Windows Server 2016+
- SQL Server 2019 hoặc mới hơn (Express cũng được)
- RAM tối thiểu 4GB
- Ổ cứng trống tối thiểu 10GB

### Client (Máy trạm)

- Windows 10/11
- .NET 8.0 Runtime
- Kết nối mạng LAN với Server

---

## 🗄️ Hướng Dẫn Cài Đặt SQL Server Cho Mạng LAN

### Bước 1: Cài Đặt SQL Server

1. Tải SQL Server Express từ: https://www.microsoft.com/sql-server/sql-server-downloads
2. Chọn **Custom** hoặc **Download Media** để cài đặt đầy đủ
3. Trong quá trình cài đặt:
   - Chọn **Database Engine Services**
   - Đặt Instance Name: `MSSQLSERVER` (default) hoặc `SQLEXPRESS`
   - Chọn **Mixed Mode Authentication**
   - Đặt mật khẩu cho tài khoản **sa**

### Bước 2: Cấu Hình SQL Server Cho Mạng LAN

#### 2.1 Bật TCP/IP Protocol

1. Mở **SQL Server Configuration Manager**

   - Tìm kiếm "SQL Server Configuration" trong Start Menu
   - Hoặc đường dẫn: `C:\Windows\SysWOW64\SQLServerManager16.msc` (cho SQL 2022)

2. Điều hướng đến: **SQL Server Network Configuration** → **Protocols for [InstanceName]**

3. **Bật TCP/IP**:

   - Click đúp vào **TCP/IP**
   - Tab **Protocol**: Đặt **Enabled** = Yes
   - Tab **IP Addresses**:
     - Cuộn xuống phần **IPAll**
     - **TCP Port**: Nhập `1433`
     - **TCP Dynamic Ports**: Để trống

4. Click **OK** để lưu

#### 2.2 Khởi Động Lại SQL Server

```
Trong SQL Server Configuration Manager:
- SQL Server Services → SQL Server (MSSQLSERVER)
- Click chuột phải → Restart
```

#### 2.3 Bật SQL Server Browser Service

1. Trong **SQL Server Configuration Manager** → **SQL Server Services**
2. **SQL Server Browser**:
   - Click chuột phải → Properties
   - Tab Service: Start Mode = **Automatic**
   - Click Apply
   - Click chuột phải → **Start**

### Bước 3: Cấu Hình Windows Firewall

#### 3.1 Mở Port SQL Server

**Cách 1: Qua Windows Firewall (Khuyên dùng)**

1. Mở **Windows Defender Firewall with Advanced Security**
2. Click **Inbound Rules** → **New Rule**
3. Chọn **Port** → Next
4. Chọn **TCP**, Specific local ports: `1433` → Next
5. Chọn **Allow the connection** → Next
6. Chọn cả 3: Domain, Private, Public → Next
7. Đặt tên: `SQL Server Port 1433` → Finish

8. Lặp lại cho **UDP port 1434** (SQL Server Browser):
   - New Rule → Port → UDP → `1434` → Allow → Đặt tên: `SQL Browser UDP 1434`

**Cách 2: Qua Command Prompt (Nhanh)**

Mở Command Prompt với quyền Administrator và chạy:

```cmd
netsh advfirewall firewall add rule name="SQL Server" dir=in action=allow protocol=tcp localport=1433
netsh advfirewall firewall add rule name="SQL Browser" dir=in action=allow protocol=udp localport=1434
```

### Bước 4: Tạo Database

1. Mở **SQL Server Management Studio (SSMS)**
2. Kết nối đến SQL Server
3. Mở file `Database/LibraryDB.sql` trong dự án
4. Nhấn **Execute** (F5) để tạo database và dữ liệu mẫu

---

## 🌐 Cấu Hình Kết Nối Mạng LAN

### Trên Máy Client

#### Cách 1: Sửa file App.config

Mở file `App.config` trong thư mục ứng dụng và sửa connection string:

```xml
<connectionStrings>
    <add name="LibraryDB"
         connectionString="Server=192.168.1.100,1433;Database=LibraryDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
         providerName="Microsoft.Data.SqlClient" />
</connectionStrings>
```

**Thay đổi:**

- `192.168.1.100` → IP của máy Server (chạy `ipconfig` trên server để xem)
- `YourPassword` → Mật khẩu tài khoản sa

#### Cách 2: Qua giao diện Login

1. Chạy ứng dụng
2. Tại màn hình Login, click nút **⚙️ Cấu hình kết nối**
3. Điền thông tin:
   - Server: `IP_Server` hoặc `IP_Server,1433`
   - Database: `LibraryDB`
   - User: `sa`
   - Password: `YourPassword`
4. Click **Test kết nối** để kiểm tra
5. Click **Lưu** nếu kết nối thành công

### Xác Định IP Server

Trên máy Server, mở Command Prompt và chạy:

```cmd
ipconfig
```

Tìm dòng **IPv4 Address** trong phần **Ethernet adapter** hoặc **Wi-Fi adapter**.

Ví dụ: `192.168.1.100`

---

## 🔧 Khắc Phục Sự Cố

### Lỗi: "Cannot connect to server"

1. **Kiểm tra Server có chạy không**:

   - Mở Services.msc → Kiểm tra "SQL Server" đang Running

2. **Kiểm tra Firewall**:

   - Tạm thời tắt Windows Firewall để test
   - Nếu kết nối được, nghĩa là do Firewall chặn

3. **Kiểm tra TCP/IP**:

   - SQL Server Configuration Manager → TCP/IP đã Enabled chưa

4. **Kiểm tra Port**:

   - Mở Command Prompt trên Server: `netstat -an | find "1433"`
   - Phải thấy `LISTENING`

5. **Kiểm tra kết nối mạng**:
   - Từ client, ping Server: `ping 192.168.1.100`

### Lỗi: "Login failed for user 'sa'"

1. Kiểm tra mật khẩu sa đúng chưa
2. Kiểm tra SQL Server đang dùng Mixed Mode Authentication:
   - SSMS → Click chuột phải Server → Properties
   - Security → Server authentication: Chọn **SQL Server and Windows Authentication mode**
   - Restart SQL Server

### Lỗi: "A network-related or instance-specific error"

1. Kiểm tra SQL Server Browser đang chạy
2. Thử thêm port vào connection string: `Server=192.168.1.100,1433`
3. Kiểm tra không bị chặn bởi Antivirus

---

## 📂 Cấu Trúc Dự Án

```
QLThuVien/
├── Database/
│   └── LibraryDB.sql          # Script tạo database
├── LibraryManagement/
│   ├── App.config             # Cấu hình connection string
│   ├── Program.cs             # Entry point
│   ├── Data/                  # Data Access Layer
│   │   ├── DatabaseConnection.cs
│   │   ├── UserDAO.cs
│   │   ├── BookDAO.cs
│   │   ├── MemberDAO.cs
│   │   ├── BorrowRecordDAO.cs
│   │   └── SystemSettingDAO.cs
│   ├── Models/                # Domain Models
│   │   ├── User.cs
│   │   ├── Book.cs
│   │   ├── Member.cs
│   │   ├── BorrowRecord.cs
│   │   └── SystemSetting.cs
│   └── Forms/                 # Windows Forms
│       ├── FormLogin.cs
│       ├── FormMain.cs
│       ├── FormBookManagement.cs
│       ├── FormMemberManagement.cs
│       ├── FormBorrow.cs
│       ├── FormReturn.cs
│       ├── FormReport.cs
│       ├── FormSettings.cs
│       └── FormUserManagement.cs
├── LibraryManagement.sln      # Solution file
└── README.md                  # Hướng dẫn này
```

---

## 🔐 Tài Khoản Mặc Định

| Tài khoản | Mật khẩu | Vai trò |
| --------- | -------- | ------- |
| admin     | admin123 | Admin   |
| manager   | 123456   | Manager |
| staff     | 123456   | Staff   |

**Lưu ý**: Hãy đổi mật khẩu sau khi đăng nhập lần đầu!

---

## 🚀 Build & Run

### Yêu cầu

- Visual Studio 2022 hoặc mới hơn
- .NET 8.0 SDK

### Cách Build

1. Mở `LibraryManagement.sln` bằng Visual Studio
2. Nhấn **Ctrl + Shift + B** để Build
3. Nhấn **F5** để Run

### Cách Publish

1. Right-click dự án → Publish
2. Chọn **Folder** → Đường dẫn output
3. Configuration: Release
4. Deployment Mode: Framework-dependent hoặc Self-contained
5. Click Publish

### Cài đặt trên máy Client

1. Copy folder publish đến máy Client
2. Cài đặt .NET 8.0 Runtime (nếu chọn Framework-dependent)
3. Sửa file `App.config` với IP Server
4. Chạy file `LibraryManagement.exe`

---

## 📧 Liên Hệ & Hỗ Trợ

Nếu gặp vấn đề trong quá trình cài đặt, vui lòng kiểm tra lại các bước theo hướng dẫn hoặc tham khảo tài liệu Microsoft:

- https://docs.microsoft.com/sql/sql-server/
- https://docs.microsoft.com/dotnet/

---

## 📜 License

MIT License - Free to use for educational purposes.
# QLThuVien
