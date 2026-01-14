# ✅ README - WinForms Designer trắng / không thấy Controls & cách refactor chuẩn (LibraryManagement)

Tài liệu này tổng hợp **nguyên nhân** khiến WinForms Designer (`[Design]`) bị trống/không hiện control, và hướng dẫn **cách khắc phục chuẩn** đã áp dụng trong project **LibraryManagement**.

---

## 1) Hiện tượng thường gặp

Khi mở các form như:

- `FormMain.cs`
- `FormPublic.cs`
- `FormLogin.cs`
- `FormUserManagement.cs`
- `FormMemberManagement.cs`
- `FormBorrowHistory.cs`
- `FormSettings.cs`
- `FormReturn.cs`
- `FormBookManagement.cs`
- `FormBorrow.cs`
- `FormReport.cs`
- `FormBorrowReturnDetails.cs`

👉 Có thể gặp:

- Form Designer **trắng hoàn toàn**
- Designer mở chậm / crash
- Không kéo thả được vì Designer không show control
- Build chạy được nhưng Designer không hiển thị UI

---

## 2) Nguyên nhân & cách khắc phục

### ✅ Nguyên nhân 1: UI được tạo bằng code runtime (SetupForm) nên Designer không biết
Một số form dựng giao diện bằng code như:

- `new Panel()`
- `new Button()`
- `new DataGridView()`
- `Controls.Add(...)`

📌 Ví dụ điển hình: `FormBookManagement` / `FormMain` / `FormBorrow`…

✅ Khắc phục:
- Nếu muốn kéo thả Designer thật sự → thiết kế UI bằng Designer (`InitializeComponent()`).
- Nếu muốn dựng UI runtime → chấp nhận Designer không kéo thả, nhưng có thể **cho Designer “preview layout”** bằng cách gọi `SetupForm()` cả khi mở Designer (xem Template bên dưới).

---

### ✅ Nguyên nhân 2: Constructor/Load gọi DB/IO/logic nặng → Designer bị lỗi hoặc trắng
Các lỗi phổ biến:

- gọi DAO truy vấn DB (`LoadData()`, `SearchBooks()`, `LoadDashboard()`)
- đọc file ảnh (`FileStream`, `Image.FromStream`)
- tạo folder / scan folder
- show `MessageBox.Show(...)`

❗ Visual Studio Designer cũng tạo instance của form → code chạy → dễ crash.

✅ Khắc phục chuẩn:
- **Di chuyển DB/IO ra khỏi constructor**
- Chặn DB/IO khi đang ở **DesignTime** bằng:

```csharp
if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
    return;
```

---

### ✅ Nguyên nhân 3: Mở nhầm file `.Designer.cs`
❌ Mở `FormX.Designer.cs` sẽ không thao tác đúng.
✅ Luôn mở:

- `FormX.cs` → Right click → **View Designer**
- Hoặc **Shift + F7**

---

### ✅ Nguyên nhân 4: Mở project bằng Folder View
Nếu mở project bằng `Open Folder` → nhiều lúc không hiện Designer chuẩn.

✅ Khắc phục:
- Mở bằng `*.sln` hoặc `*.csproj`
  - File → Open → Project or Solution

---

### ✅ Nguyên nhân 5: Lỗi build / thiếu workload
Designer thường cần project build OK.

✅ Khắc phục:
- Build → Rebuild Solution
- Visual Studio Installer → tick **.NET desktop development**

---

## 3) Refactor đã thực hiện (Tóm tắt)

✅ Đã xác nhận build thành công (`dotnet build`) sau khi refactor:

- **FormMain**
  - Đưa `SetupForm()` + `LoadDashboard()` sang Load event
  - Thêm guard `LicenseManager.UsageMode`

- **FormPublic**
  - Đưa `SetupForm`, `LoadCategories`, `LoadBooks` sang Load event
  - Thêm guard

- **FormLogin**
  - Guard trong `FormLogin_Load` để tránh test DB khi mở Designer

- **Các Form đã refactor tương tự**
  - `FormUserManagement`: LoadData chạy trong Load + guard
  - `FormMemberManagement`: LoadData chạy trong Load + guard
  - `FormBorrowHistory`: guard trong Load
  - `FormSettings`: LoadSettings/LoadLogs chạy trong Load + guard
  - `FormReturn`: LoadData chạy trong Load + guard
  - `FormBookManagement`: EnsureImagesFolderExists + LoadData chạy trong Load + guard
  - `FormBorrow`: SetupForm chuyển vào Load + guard
  - `FormReport`: LoadDashboardStats chạy trong Load + guard
  - `FormBorrowReturnDetails`: LoadData chạy trong Load + guard

✅ Cảnh báo còn lại (không ảnh hưởng chạy):
- `FormBookDetailPublic.initialized` không dùng
- `FormUserManagement.isEditing` gán nhưng chưa dùng

---

## 4) Template chuẩn áp dụng đồng nhất cho tất cả Form

### ✅ Template A (Form có dựng UI runtime bằng SetupForm)
👉 Dùng cho form tạo control bằng code như `SetupForm()`.

**Mục tiêu:**  
- Designer vẫn preview layout  
- Runtime mới load DB/IO

```csharp
using System.ComponentModel;
using System.Windows.Forms;

public partial class FormX : Form
{
    public FormX()
    {
        InitializeComponent();
        this.Load += FormX_Load;
    }

    private void FormX_Load(object? sender, EventArgs e)
    {
        // ✅ Cho Designer preview UI layout
        SetupForm();

        // ✅ Chặn DB/IO khi mở Designer
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;

        // ✅ Runtime only
        LoadData();
    }

    private void SetupForm()
    {
        // tạo control, Controls.Add, layout...
    }

    private void LoadData()
    {
        // DAO/DB/IO...
    }
}
```

---

### ✅ Template B (Form thiết kế bằng Designer sẵn)
👉 Dùng cho form kéo thả control trong Designer.

```csharp
using System.ComponentModel;
using System.Windows.Forms;

public partial class FormX : Form
{
    public FormX()
    {
        InitializeComponent();
        this.Load += FormX_Load;
    }

    private void FormX_Load(object? sender, EventArgs e)
    {
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;

        LoadData();
    }

    private void LoadData()
    {
        // DAO/DB...
    }
}
```

---

## 5) Checklist nhanh khi Designer trắng

1) ✅ Mở đúng `*.sln` / `*.csproj`  
2) ✅ Mở đúng file `FormX.cs` → View Designer  
3) ✅ Rebuild Solution  
4) ✅ Không gọi DB/IO trong constructor  
5) ✅ Thêm guard `LicenseManager.UsageMode`  
6) ✅ Nếu form dựng runtime UI: gọi `SetupForm()` ngay trong Load để preview

---

## 6) Dọn warning (tùy chọn, không bắt buộc)

### 6.1 Xóa biến không dùng
- `FormBookDetailPublic.initialized`
- `FormUserManagement.isEditing`

Nếu chưa dùng → xóa để sạch build.

Nếu muốn dùng đúng mục đích → dùng kiểu:

```csharp
private bool initialized;

private void FormX_Load(object sender, EventArgs e)
{
    if (initialized) return;
    initialized = true;
    // init...
}
```

---

✅ Done.
