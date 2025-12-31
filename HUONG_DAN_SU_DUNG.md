# 📖 HƯỚNG DẪN SỬ DỤNG HỆ THỐNG QUẢN LÝ THƯ VIỆN

## 📋 Mục Lục

1. [Tổng Quan Hệ Thống](#1-tổng-quan-hệ-thống)
2. [Hướng Dẫn Cài Đặt & Chạy](#2-hướng-dẫn-cài-đặt--chạy)
3. [Đăng Nhập Hệ Thống](#3-đăng-nhập-hệ-thống)
4. [Các Chức Năng Chính](#4-các-chức-năng-chính)
5. [Quy Trình Nghiệp Vụ](#5-quy-trình-nghiệp-vụ)
6. [Phân Quyền Người Dùng](#6-phân-quyền-người-dùng)

---

## 1. Tổng Quan Hệ Thống

### 🎯 Mục đích

Hệ thống Quản lý Thư viện giúp:

- Quản lý danh mục sách, tác giả, nhà xuất bản
- Quản lý thông tin độc giả/thẻ thư viện
- Xử lý nghiệp vụ mượn/trả sách
- Tự động tính tiền phạt quá hạn
- Thống kê báo cáo hoạt động

### 🏗️ Kiến Trúc Hệ Thống

```
┌─────────────────────────────────────────────────────────────┐
│                    CLIENT (WinForms App)                     │
│  ┌─────────┐  ┌─────────┐  ┌─────────┐  ┌─────────┐        │
│  │ Login   │  │ Quản lý │  │ Mượn/   │  │ Báo cáo │        │
│  │ Form    │  │ Sách    │  │ Trả     │  │ Thống kê│        │
│  └─────────┘  └─────────┘  └─────────┘  └─────────┘        │
│                         ▼                                    │
│  ┌─────────────────────────────────────────────────────┐   │
│  │              Data Access Layer (DAO)                 │   │
│  │   UserDAO | BookDAO | MemberDAO | BorrowRecordDAO   │   │
│  └─────────────────────────────────────────────────────┘   │
└────────────────────────────┬────────────────────────────────┘
                             │ TCP/IP (Port 1433)
                             ▼
┌─────────────────────────────────────────────────────────────┐
│                    SQL SERVER DATABASE                       │
│  ┌─────────┐  ┌─────────┐  ┌─────────┐  ┌─────────┐        │
│  │ Users   │  │ Books   │  │ Members │  │ Borrow  │        │
│  │         │  │         │  │         │  │ Records │        │
│  └─────────┘  └─────────┘  └─────────┘  └─────────┘        │
└─────────────────────────────────────────────────────────────┘
```

### 📁 Cấu Trúc Thư Mục Dự Án

```
QLThuVien/
├── Database/
│   └── LibraryDB.sql          # Script tạo database
├── LibraryManagement/
│   ├── Data/                  # Lớp truy xuất dữ liệu
│   │   ├── DatabaseConnection.cs
│   │   ├── UserDAO.cs
│   │   ├── BookDAO.cs
│   │   ├── MemberDAO.cs
│   │   ├── BorrowRecordDAO.cs
│   │   └── SystemSettingDAO.cs
│   ├── Models/                # Lớp đối tượng
│   │   ├── User.cs
│   │   ├── Book.cs
│   │   ├── Member.cs
│   │   ├── BorrowRecord.cs
│   │   └── SystemSetting.cs
│   ├── Forms/                 # Giao diện người dùng
│   │   ├── FormLogin.cs       # Màn hình đăng nhập
│   │   ├── FormMain.cs        # Màn hình chính (Dashboard)
│   │   ├── FormBookManagement.cs
│   │   ├── FormMemberManagement.cs
│   │   ├── FormBorrow.cs
│   │   ├── FormReturn.cs
│   │   ├── FormReport.cs
│   │   ├── FormSettings.cs
│   │   └── FormUserManagement.cs
│   ├── Images/                # Thư mục lưu ảnh bìa sách
│   ├── App.config             # Cấu hình kết nối
│   └── Program.cs             # Entry point
├── README.md                  # Hướng dẫn cài đặt LAN
└── HUONG_DAN_SU_DUNG.md      # File này
```

---

## 2. Hướng Dẫn Cài Đặt & Chạy

### 📋 Yêu Cầu

| Thành phần   | Yêu cầu                         |
| ------------ | ------------------------------- |
| Hệ điều hành | Windows 10/11                   |
| .NET Runtime | .NET 8.0 trở lên                |
| Database     | SQL Server 2019+ (hoặc Express) |
| RAM          | Tối thiểu 4GB                   |

### 🚀 Các Bước Chạy Ứng Dụng

#### Bước 1: Cài đặt .NET 8.0 Runtime

```
Tải từ: https://dotnet.microsoft.com/download/dotnet/8.0
Chọn: .NET Desktop Runtime 8.0
```

#### Bước 2: Cài đặt SQL Server

```
Tải SQL Server Express từ: https://www.microsoft.com/sql-server/sql-server-downloads
Cài đặt với Mixed Mode Authentication
```

#### Bước 3: Tạo Database

1. Mở **SQL Server Management Studio (SSMS)**
2. Kết nối đến SQL Server
3. Mở file `Database/LibraryDB.sql`
4. Nhấn **F5** hoặc click **Execute** để chạy script
5. Refresh để thấy database **LibraryManagement**

#### Bước 4: Cấu hình Connection String

Mở file `LibraryManagement/App.config` và sửa:

```xml
<connectionStrings>
    <add name="LibraryDB"
         connectionString="Server=localhost;Database=LibraryManagement;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
         providerName="Microsoft.Data.SqlClient" />
</connectionStrings>
```

Thay `YOUR_PASSWORD` bằng mật khẩu SQL Server của bạn.

#### Bước 5: Build và Chạy

**Cách 1: Dùng Visual Studio**

```
1. Mở file LibraryManagement.sln
2. Nhấn F5 hoặc Ctrl+F5 để chạy
```

**Cách 2: Dùng Command Line**

```bash
cd C:\Users\ASUS\Documents\QLThuVien
dotnet build
dotnet run --project LibraryManagement
```

**Cách 3: Chạy file .exe (sau khi publish)**

```bash
dotnet publish -c Release -o ./publish
# Chạy file: publish/LibraryManagement.exe
```

---

## 3. Đăng Nhập Hệ Thống

### 🔐 Tài Khoản Mặc Định

| Tài khoản | Mật khẩu | Vai trò | Quyền hạn         |
| --------- | -------- | ------- | ----------------- |
| admin     | admin123 | Admin   | Toàn quyền        |
| manager   | 123456   | Manager | Quản lý nghiệp vụ |
| staff     | 123456   | Staff   | Mượn/trả sách     |

### 📱 Màn Hình Đăng Nhập

```
┌────────────────────────────────────────┐
│         📚 QUẢN LÝ THƯ VIỆN           │
│                                        │
│  Tên đăng nhập: [__________________]  │
│                                        │
│  Mật khẩu:      [__________________]  │
│                                        │
│  [✔] Nhớ mật khẩu                     │
│                                        │
│  [    🔑 Đăng nhập    ]               │
│                                        │
│  [⚙️ Cấu hình kết nối]                │
└────────────────────────────────────────┘
```

**Lưu ý:** Click **⚙️ Cấu hình kết nối** nếu cần thay đổi Server database.

---

## 4. Các Chức Năng Chính

### 📊 4.1 Dashboard (Trang Chủ)

Hiển thị tổng quan:

- **Tổng số sách** trong thư viện
- **Đang mượn** - số sách đang được mượn
- **Quá hạn** - số phiếu mượn quá hạn (cần nhắc nhở)
- **Độc giả** - tổng số thẻ độc giả
- **Danh sách mượn gần đây**
- **Danh sách quá hạn** cần xử lý

### 📚 4.2 Quản Lý Sách

**Chức năng:**

- ➕ Thêm sách mới (có hỗ trợ upload ảnh bìa)
- ✏️ Sửa thông tin sách
- 🗑️ Xóa sách (đánh dấu ẩn)
- 🔍 Tìm kiếm theo: Tên, ISBN, Tác giả, Thể loại
- 📷 Xem chi tiết sách với hình ảnh

**Thông tin sách bao gồm:**

- ISBN, Tên sách
- Thể loại, Tác giả, Nhà xuất bản
- Năm xuất bản, Giá
- Số lượng tổng / Số lượng còn
- Vị trí kệ sách
- Mô tả, Hình ảnh bìa

### 👥 4.3 Quản Lý Độc Giả

**Chức năng:**

- ➕ Đăng ký thẻ độc giả mới
- ✏️ Cập nhật thông tin
- 📋 Xem lịch sử mượn sách
- 💰 Thu tiền phạt

**Thông tin độc giả:**

- Mã thẻ (tự động tạo: TV001, TV002,...)
- Họ tên, Giới tính, Ngày sinh
- CMND/CCCD
- Địa chỉ, Điện thoại, Email
- Ngày cấp thẻ, Ngày hết hạn
- Tiền phạt còn nợ

### 📖 4.4 Mượn Sách

**Quy trình mượn:**

1. Nhập/quét mã thẻ độc giả
2. Kiểm tra độc giả có được mượn không (hết hạn? nợ phạt? quá số sách?)
3. Chọn sách cần mượn
4. Hệ thống tự động:
   - Tạo mã phiếu mượn
   - Tính ngày hạn trả (mặc định 14 ngày)
   - Cập nhật số lượng sách
5. In phiếu mượn (tùy chọn)

**Điều kiện mượn:**

- Thẻ còn hiệu lực
- Không có tiền phạt chưa đóng
- Chưa đạt số sách tối đa (mặc định 5 cuốn)
- Không có sách quá hạn

### 📥 4.5 Trả Sách

**Quy trình trả:**

1. Tìm phiếu mượn (theo mã phiếu, mã thẻ, tên sách)
2. Chọn phiếu mượn cần trả
3. Hệ thống kiểm tra:
   - Có quá hạn không?
   - Tính tiền phạt (nếu có)
4. Xác nhận trả sách
5. Cập nhật:
   - Trạng thái phiếu mượn → "Đã trả"
   - Số lượng sách còn +1
   - Tiền phạt (nếu quá hạn)

**Tính tiền phạt:**

```
Tiền phạt = Số ngày quá hạn × Mức phạt/ngày
Ví dụ: 5 ngày × 5,000đ = 25,000đ
```

**Chức năng Gia hạn:**

- Gia hạn thêm ngày mượn (nếu chưa quá hạn)
- Mặc định gia hạn thêm 7 ngày

### 📊 4.6 Thống Kê Báo Cáo

**Các loại báo cáo:**

| Báo cáo          | Mô tả                                     |
| ---------------- | ----------------------------------------- |
| Tổng quan        | Số liệu tổng hợp: sách, độc giả, mượn/trả |
| Sách mượn nhiều  | Top sách được mượn nhiều nhất             |
| Độc giả tích cực | Top độc giả mượn nhiều nhất               |
| Sách quá hạn     | Danh sách phiếu mượn quá hạn              |
| Thống kê ngày    | Số lượt mượn/trả theo từng ngày           |
| Phạt chưa thu    | Danh sách độc giả còn nợ tiền phạt        |
| Sách hết         | Danh sách sách đã hết (đang mượn hết)     |

**Xuất báo cáo:**

- 📥 Xuất file CSV (mở được bằng Excel)
- 🖨️ In báo cáo (đang phát triển)

### ⚙️ 4.7 Cài Đặt Hệ Thống

**Thông tin thư viện:**

- Tên thư viện
- Địa chỉ, Điện thoại, Email

**Quy định mượn trả:**
| Cài đặt | Mặc định | Mô tả |
|---------|----------|-------|
| Số ngày mượn tối đa | 14 ngày | Thời hạn mượn sách |
| Số sách mượn tối đa | 5 cuốn | Giới hạn mỗi độc giả |
| Tiền phạt quá hạn | 5,000đ/ngày | Mức phạt cho mỗi ngày quá hạn |

**Nhật ký hoạt động:**

- Ghi lại mọi thao tác của người dùng
- Thời gian, người thực hiện, hành động
- Hỗ trợ kiểm tra và audit

### 👤 4.8 Quản Lý Người Dùng (Admin)

**Chỉ Admin mới có quyền truy cập**

**Chức năng:**

- ➕ Thêm tài khoản mới
- ✏️ Sửa thông tin, đổi vai trò
- 🔑 Reset mật khẩu (mặc định: 123456)
- 🔒 Khóa/Mở khóa tài khoản
- 🗑️ Xóa tài khoản

---

## 5. Quy Trình Nghiệp Vụ

### 🔄 Quy Trình Mượn - Trả Sách

```
┌─────────────┐     ┌─────────────┐     ┌─────────────┐
│ Độc giả     │────▶│ Nhân viên   │────▶│ Hệ thống    │
│ yêu cầu     │     │ xử lý       │     │ cập nhật    │
└─────────────┘     └─────────────┘     └─────────────┘
       │                   │                   │
       │              MƯỢN SÁCH               │
       │                   │                   │
       │    1. Kiểm tra thẻ độc giả           │
       │    2. Chọn sách                       │
       │    3. Tạo phiếu mượn                  │
       │    4. Giảm số sách còn                │
       │                   │                   │
       │              TRẢ SÁCH                │
       │                   │                   │
       │    1. Tìm phiếu mượn                  │
       │    2. Kiểm tra quá hạn                │
       │    3. Tính phạt (nếu có)              │
       │    4. Cập nhật trạng thái             │
       │    5. Tăng số sách còn                │
       ▼                   ▼                   ▼
```

### 📋 Trạng Thái Phiếu Mượn

```
                    ┌─────────────┐
                    │   Đang mượn │
                    └──────┬──────┘
                           │
           ┌───────────────┼───────────────┐
           ▼               ▼               ▼
    ┌─────────────┐ ┌─────────────┐ ┌─────────────┐
    │   Quá hạn   │ │   Đã trả    │ │  Gia hạn    │
    └──────┬──────┘ └─────────────┘ └──────┬──────┘
           │                               │
           ▼                               ▼
    ┌─────────────┐                 ┌─────────────┐
    │ Đã trả     │                 │  Đang mượn  │
    │ (có phạt)  │                 │  (mới)      │
    └─────────────┘                 └─────────────┘
```

---

## 6. Phân Quyền Người Dùng

### 👥 Các Vai Trò

| Vai trò     | Mô tả                                           |
| ----------- | ----------------------------------------------- |
| **Admin**   | Quản trị viên - Toàn quyền hệ thống             |
| **Manager** | Quản lý - Quản lý nghiệp vụ, không quản lý user |
| **Staff**   | Nhân viên - Chỉ mượn/trả sách                   |

### 📋 Bảng Phân Quyền Chi Tiết

| Chức năng          | Admin | Manager | Staff |
| ------------------ | :---: | :-----: | :---: |
| Dashboard          |  ✅   |   ✅    |  ✅   |
| Quản lý sách       |  ✅   |   ✅    |  ❌   |
| Quản lý độc giả    |  ✅   |   ✅    |  ❌   |
| Mượn sách          |  ✅   |   ✅    |  ✅   |
| Trả sách           |  ✅   |   ✅    |  ✅   |
| Thống kê báo cáo   |  ✅   |   ✅    |  ❌   |
| Cài đặt hệ thống   |  ✅   |   ✅    |  ❌   |
| Quản lý người dùng |  ✅   |   ❌    |  ❌   |

---

## 📞 Hỗ Trợ

Nếu gặp vấn đề khi sử dụng, vui lòng:

1. Kiểm tra file `README.md` để xem hướng dẫn cài đặt
2. Kiểm tra kết nối database qua nút **⚙️ Cấu hình kết nối**
3. Xem nhật ký hoạt động trong **Cài đặt hệ thống**

---

**Phiên bản:** 1.0  
**Cập nhật:** 31/12/2025  
**Tác giả:** Library Management System Team
