-- =============================================
-- QUẢN LÝ THƯ VIỆN - DATABASE SCRIPT
-- SQL Server Database cho ứng dụng WinForms
-- Hỗ trợ chạy trên nhiều máy trong mạng LAN
-- =============================================

-- Tạo Database
USE master
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'LibraryManagement')
BEGIN
    ALTER DATABASE LibraryManagement SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE LibraryManagement
END
GO

CREATE DATABASE LibraryManagement
GO

USE LibraryManagement
GO

-- =============================================
-- BẢNG TÀI KHOẢN NGƯỜI DÙNG HỆ THỐNG
-- =============================================
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(256) NOT NULL, -- Lưu mật khẩu đã hash
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Role NVARCHAR(20) NOT NULL DEFAULT 'Staff', -- Admin, Manager, Staff
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    LastLogin DATETIME,
    CONSTRAINT CHK_Users_Role CHECK (Role IN ('Admin', 'Manager', 'Staff'))
)
GO

-- =============================================
-- BẢNG THỂ LOẠI SÁCH
-- =============================================
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
)
GO

-- =============================================
-- BẢNG NHÀ XUẤT BẢN
-- =============================================
CREATE TABLE Publishers (
    PublisherID INT IDENTITY(1,1) PRIMARY KEY,
    PublisherName NVARCHAR(200) NOT NULL,
    Address NVARCHAR(300),
    Phone NVARCHAR(20),
    Email NVARCHAR(100),
    Website NVARCHAR(200),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
)
GO

-- =============================================
-- BẢNG TÁC GIẢ
-- =============================================
CREATE TABLE Authors (
    AuthorID INT IDENTITY(1,1) PRIMARY KEY,
    AuthorName NVARCHAR(150) NOT NULL,
    Biography NVARCHAR(MAX),
    Country NVARCHAR(100),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
)
GO

-- =============================================
-- BẢNG SÁCH
-- =============================================
CREATE TABLE Books (
    BookID INT IDENTITY(1,1) PRIMARY KEY,
    ISBN NVARCHAR(20) UNIQUE,
    Title NVARCHAR(300) NOT NULL,
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    AuthorID INT FOREIGN KEY REFERENCES Authors(AuthorID),
    PublisherID INT FOREIGN KEY REFERENCES Publishers(PublisherID),
    PublishYear INT,
    Price DECIMAL(18,2) DEFAULT 0,
    TotalCopies INT DEFAULT 1, -- Tổng số bản sách
    AvailableCopies INT DEFAULT 1, -- Số bản sách còn sẵn
    Description NVARCHAR(MAX),
    Location NVARCHAR(100), -- Vị trí kệ sách
    ImagePath NVARCHAR(500),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME,
    CONSTRAINT CHK_Books_Copies CHECK (AvailableCopies >= 0 AND AvailableCopies <= TotalCopies)
)
GO

-- =============================================
-- BẢNG ĐỌC GIẢ (THÀNH VIÊN THƯ VIỆN)
-- =============================================
CREATE TABLE Members (
    MemberID INT IDENTITY(1,1) PRIMARY KEY,
    MemberCode NVARCHAR(20) NOT NULL UNIQUE, -- Mã thẻ thành viên
    FullName NVARCHAR(150) NOT NULL,
    Gender NVARCHAR(10), -- Nam, Nữ, Khác
    DateOfBirth DATE,
    Address NVARCHAR(300),
    Phone NVARCHAR(20),
    Email NVARCHAR(100),
    IdentityCard NVARCHAR(20), -- CMND/CCCD
    MemberType NVARCHAR(50) DEFAULT N'Thường', -- Thường, VIP, Sinh viên, Giáo viên
    JoinDate DATE DEFAULT GETDATE(),
    ExpiryDate DATE, -- Ngày hết hạn thẻ
    TotalFine DECIMAL(18,2) DEFAULT 0, -- Tổng tiền phạt còn nợ
    IsActive BIT DEFAULT 1,
    Notes NVARCHAR(500),
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME,
    CONSTRAINT CHK_Members_Gender CHECK (Gender IN (N'Nam', N'Nữ', N'Khác'))
)
GO

-- =============================================
-- BẢNG MƯỢN SÁCH
-- =============================================
CREATE TABLE BorrowRecords (
    BorrowID INT IDENTITY(1,1) PRIMARY KEY,
    BorrowCode NVARCHAR(20) NOT NULL UNIQUE, -- Mã phiếu mượn
    MemberID INT NOT NULL FOREIGN KEY REFERENCES Members(MemberID),
    BookID INT NOT NULL FOREIGN KEY REFERENCES Books(BookID),
    BorrowDate DATE NOT NULL DEFAULT GETDATE(),
    DueDate DATE NOT NULL, -- Ngày hẹn trả
    ReturnDate DATE, -- Ngày trả thực tế
    Quantity INT DEFAULT 1,
    Status NVARCHAR(20) DEFAULT N'Đang mượn', -- Đang mượn, Đã trả, Quá hạn
    FineAmount DECIMAL(18,2) DEFAULT 0, -- Tiền phạt
    FinePaid BIT DEFAULT 0, -- Đã đóng phạt chưa
    Notes NVARCHAR(500),
    StaffID INT FOREIGN KEY REFERENCES Users(UserID), -- Nhân viên xử lý
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME,
    CONSTRAINT CHK_BorrowRecords_Status CHECK (Status IN (N'Đang mượn', N'Đã trả', N'Quá hạn', N'Mất sách'))
)
GO

-- =============================================
-- BẢNG ĐẶT TRƯỚC SÁCH
-- =============================================
CREATE TABLE Reservations (
    ReservationID INT IDENTITY(1,1) PRIMARY KEY,
    MemberID INT NOT NULL FOREIGN KEY REFERENCES Members(MemberID),
    BookID INT NOT NULL FOREIGN KEY REFERENCES Books(BookID),
    ReservationDate DATETIME DEFAULT GETDATE(),
    ExpiryDate DATETIME, -- Ngày hết hạn đặt trước
    Status NVARCHAR(20) DEFAULT N'Chờ', -- Chờ, Đã nhận, Hủy, Hết hạn
    Notes NVARCHAR(500),
    CreatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT CHK_Reservations_Status CHECK (Status IN (N'Chờ', N'Đã nhận', N'Hủy', N'Hết hạn'))
)
GO

-- =============================================
-- BẢNG THANH TOÁN TIỀN PHẠT
-- =============================================
CREATE TABLE FinePayments (
    PaymentID INT IDENTITY(1,1) PRIMARY KEY,
    MemberID INT NOT NULL FOREIGN KEY REFERENCES Members(MemberID),
    BorrowID INT FOREIGN KEY REFERENCES BorrowRecords(BorrowID),
    Amount DECIMAL(18,2) NOT NULL,
    PaymentDate DATETIME DEFAULT GETDATE(),
    PaymentMethod NVARCHAR(50) DEFAULT N'Tiền mặt', -- Tiền mặt, Chuyển khoản
    Notes NVARCHAR(500),
    StaffID INT FOREIGN KEY REFERENCES Users(UserID),
    CreatedDate DATETIME DEFAULT GETDATE()
)
GO

-- =============================================
-- BẢNG CẤU HÌNH HỆ THỐNG
-- =============================================
CREATE TABLE SystemSettings (
    SettingID INT IDENTITY(1,1) PRIMARY KEY,
    SettingKey NVARCHAR(100) NOT NULL UNIQUE,
    SettingValue NVARCHAR(500),
    Description NVARCHAR(300),
    UpdatedDate DATETIME DEFAULT GETDATE()
)
GO

-- =============================================
-- BẢNG LOG HOẠT ĐỘNG
-- =============================================
CREATE TABLE ActivityLogs (
    LogID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    Action NVARCHAR(100) NOT NULL,
    TableName NVARCHAR(50),
    RecordID INT,
    OldValue NVARCHAR(MAX),
    NewValue NVARCHAR(MAX),
    IPAddress NVARCHAR(50),
    ComputerName NVARCHAR(100),
    LogDate DATETIME DEFAULT GETDATE()
)
GO

-- =============================================
-- INDEX ĐỂ TỐI ƯU TÌM KIẾM
-- =============================================
CREATE INDEX IX_Books_Title ON Books(Title)
CREATE INDEX IX_Books_ISBN ON Books(ISBN)
CREATE INDEX IX_Books_CategoryID ON Books(CategoryID)
CREATE INDEX IX_Members_MemberCode ON Members(MemberCode)
CREATE INDEX IX_Members_FullName ON Members(FullName)
CREATE INDEX IX_BorrowRecords_MemberID ON BorrowRecords(MemberID)
CREATE INDEX IX_BorrowRecords_BookID ON BorrowRecords(BookID)
CREATE INDEX IX_BorrowRecords_Status ON BorrowRecords(Status)
CREATE INDEX IX_BorrowRecords_DueDate ON BorrowRecords(DueDate)
GO

-- =============================================
-- DỮ LIỆU MẪU
-- =============================================

-- Tài khoản Admin mặc định (Password: admin123)
-- SHA256 hash của "admin123" dạng Base64
INSERT INTO Users (Username, Password, FullName, Email, Role)
VALUES ('admin', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', N'Quản trị viên', 'admin@library.com', 'Admin')

-- Tài khoản Manager mẫu (Password: 123456)
-- SHA256 hash của "123456" dạng Base64
INSERT INTO Users (Username, Password, FullName, Email, Role)
VALUES ('manager', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Quản lý', 'manager@library.com', 'Manager')

-- Tài khoản Staff mẫu (Password: 123456)
INSERT INTO Users (Username, Password, FullName, Email, Role)
VALUES ('staff1', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Nhân viên 1', 'staff1@library.com', 'Staff')
GO

-- Thể loại sách mẫu
INSERT INTO Categories (CategoryName, Description) VALUES 
(N'Văn học', N'Sách văn học Việt Nam và thế giới'),
(N'Khoa học', N'Sách khoa học tự nhiên và xã hội'),
(N'Kinh tế', N'Sách kinh tế, kinh doanh'),
(N'Công nghệ thông tin', N'Sách CNTT, lập trình'),
(N'Lịch sử', N'Sách lịch sử'),
(N'Thiếu nhi', N'Sách dành cho thiếu nhi'),
(N'Ngoại ngữ', N'Sách học ngoại ngữ'),
(N'Kỹ năng sống', N'Sách phát triển bản thân'),
(N'Y học', N'Sách y học, sức khỏe'),
(N'Nghệ thuật', N'Sách nghệ thuật, mỹ thuật')
GO

-- Nhà xuất bản mẫu
INSERT INTO Publishers (PublisherName, Address, Phone) VALUES 
(N'NXB Kim Đồng', N'55 Quang Trung, Hà Nội', '024-3822-1633'),
(N'NXB Trẻ', N'161B Lý Chính Thắng, Q.3, TP.HCM', '028-3931-6289'),
(N'NXB Giáo dục Việt Nam', N'81 Trần Hưng Đạo, Hà Nội', '024-3822-0801'),
(N'NXB Tổng hợp TP.HCM', N'62 Nguyễn Thị Minh Khai, Q.1, TP.HCM', '028-3829-4409'),
(N'NXB Văn học', N'18 Nguyễn Trường Tộ, Hà Nội', '024-3829-3450')
GO

-- Tác giả mẫu
INSERT INTO Authors (AuthorName, Country) VALUES 
(N'Nguyễn Nhật Ánh', N'Việt Nam'),
(N'Tô Hoài', N'Việt Nam'),
(N'Nam Cao', N'Việt Nam'),
(N'Dale Carnegie', N'Mỹ'),
(N'Paulo Coelho', N'Brazil'),
(N'Yuval Noah Harari', N'Israel'),
(N'Robert T. Kiyosaki', N'Mỹ'),
(N'Stephen Hawking', N'Anh')
GO

-- Sách mẫu
INSERT INTO Books (ISBN, Title, CategoryID, AuthorID, PublisherID, PublishYear, Price, TotalCopies, AvailableCopies, Location) VALUES 
('978-604-1-12345-1', N'Cho tôi xin một vé đi tuổi thơ', 1, 1, 2, 2019, 85000, 5, 5, 'A1-01'),
('978-604-1-12345-2', N'Dế Mèn phiêu lưu ký', 1, 2, 1, 2020, 55000, 3, 3, 'A1-02'),
('978-604-1-12345-3', N'Chí Phèo', 1, 3, 5, 2018, 45000, 4, 4, 'A1-03'),
('978-604-1-12345-4', N'Đắc nhân tâm', 8, 4, 2, 2021, 95000, 6, 6, 'B2-01'),
('978-604-1-12345-5', N'Nhà giả kim', 1, 5, 2, 2020, 75000, 4, 4, 'A2-01'),
('978-604-1-12345-6', N'Sapiens: Lược sử loài người', 2, 6, 4, 2022, 189000, 3, 3, 'C1-01'),
('978-604-1-12345-7', N'Cha giàu cha nghèo', 3, 7, 2, 2019, 125000, 5, 5, 'B1-01'),
('978-604-1-12345-8', N'Lược sử thời gian', 2, 8, 3, 2021, 145000, 2, 2, 'C2-01')
GO

-- Đọc giả mẫu
INSERT INTO Members (MemberCode, FullName, Gender, DateOfBirth, Address, Phone, Email, MemberType, ExpiryDate) VALUES 
('TV001', N'Nguyễn Văn An', N'Nam', '1995-05-15', N'123 Nguyễn Huệ, Q.1, TP.HCM', '0901234567', 'an.nguyen@email.com', N'Thường', '2026-12-31'),
('TV002', N'Trần Thị Bình', N'Nữ', '1998-08-20', N'456 Lê Lợi, Q.3, TP.HCM', '0912345678', 'binh.tran@email.com', N'VIP', '2026-12-31'),
('TV003', N'Lê Văn Cường', N'Nam', '2000-01-10', N'789 Trần Hưng Đạo, Q.5, TP.HCM', '0923456789', 'cuong.le@email.com', N'Sinh viên', '2026-06-30'),
('TV004', N'Phạm Thị Dung', N'Nữ', '1990-12-25', N'321 Hai Bà Trưng, Q.1, TP.HCM', '0934567890', 'dung.pham@email.com', N'Giáo viên', '2027-01-01'),
('TV005', N'Hoàng Văn Em', N'Nam', '2002-03-08', N'654 Võ Văn Tần, Q.3, TP.HCM', '0945678901', 'em.hoang@email.com', N'Sinh viên', '2026-06-30')
GO

-- Cấu hình hệ thống mặc định
INSERT INTO SystemSettings (SettingKey, SettingValue, Description) VALUES 
('MaxBorrowDays', '14', N'Số ngày mượn tối đa'),
('MaxBooksPerMember', '5', N'Số sách tối đa mỗi lần mượn'),
('FinePerDay', '5000', N'Tiền phạt mỗi ngày quá hạn (VNĐ)'),
('LostBookFinePercent', '200', N'Phần trăm giá sách khi làm mất'),
('LibraryName', N'THƯ VIỆN ABC', N'Tên thư viện'),
('LibraryAddress', N'123 Đường ABC, Quận XYZ, TP.HCM', N'Địa chỉ thư viện'),
('LibraryPhone', '028-1234-5678', N'Số điện thoại thư viện'),
('ReservationDays', '3', N'Số ngày giữ sách đặt trước')
GO

-- =============================================
-- STORED PROCEDURES
-- =============================================

-- SP: Đăng nhập
CREATE PROCEDURE sp_Login
    @Username NVARCHAR(50),
    @Password NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT UserID, Username, FullName, Email, Phone, Role, IsActive
    FROM Users
    WHERE Username = @Username AND Password = @Password AND IsActive = 1
    
    -- Cập nhật thời gian đăng nhập
    IF @@ROWCOUNT > 0
    BEGIN
        UPDATE Users SET LastLogin = GETDATE() WHERE Username = @Username
    END
END
GO

-- SP: Tìm kiếm sách
CREATE PROCEDURE sp_SearchBooks
    @Keyword NVARCHAR(200) = NULL,
    @CategoryID INT = NULL,
    @AuthorID INT = NULL,
    @AvailableOnly BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT b.*, c.CategoryName, a.AuthorName, p.PublisherName
    FROM Books b
    LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
    LEFT JOIN Authors a ON b.AuthorID = a.AuthorID
    LEFT JOIN Publishers p ON b.PublisherID = p.PublisherID
    WHERE b.IsActive = 1
        AND (@Keyword IS NULL OR b.Title LIKE '%' + @Keyword + '%' OR b.ISBN LIKE '%' + @Keyword + '%')
        AND (@CategoryID IS NULL OR b.CategoryID = @CategoryID)
        AND (@AuthorID IS NULL OR b.AuthorID = @AuthorID)
        AND (@AvailableOnly = 0 OR b.AvailableCopies > 0)
    ORDER BY b.Title
END
GO

-- SP: Tìm kiếm độc giả
CREATE PROCEDURE sp_SearchMembers
    @Keyword NVARCHAR(200) = NULL,
    @MemberType NVARCHAR(50) = NULL,
    @ActiveOnly BIT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT *
    FROM Members
    WHERE (@ActiveOnly = 0 OR IsActive = 1)
        AND (@Keyword IS NULL OR MemberCode LIKE '%' + @Keyword + '%' OR FullName LIKE '%' + @Keyword + '%' OR Phone LIKE '%' + @Keyword + '%')
        AND (@MemberType IS NULL OR MemberType = @MemberType)
    ORDER BY FullName
END
GO

-- SP: Mượn sách
CREATE PROCEDURE sp_BorrowBook
    @MemberID INT,
    @BookID INT,
    @StaffID INT,
    @DueDays INT = 14,
    @Result INT OUTPUT,
    @Message NVARCHAR(500) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION
        
        -- Kiểm tra sách còn sẵn không
        DECLARE @AvailableCopies INT
        SELECT @AvailableCopies = AvailableCopies FROM Books WHERE BookID = @BookID
        
        IF @AvailableCopies <= 0
        BEGIN
            SET @Result = 0
            SET @Message = N'Sách đã hết, không thể mượn'
            ROLLBACK
            RETURN
        END
        
        -- Kiểm tra độc giả có sách quá hạn không
        DECLARE @OverdueCount INT
        SELECT @OverdueCount = COUNT(*) FROM BorrowRecords 
        WHERE MemberID = @MemberID AND Status = N'Quá hạn'
        
        IF @OverdueCount > 0
        BEGIN
            SET @Result = 0
            SET @Message = N'Độc giả có sách quá hạn chưa trả'
            ROLLBACK
            RETURN
        END
        
        -- Kiểm tra số sách đang mượn
        DECLARE @BorrowingCount INT, @MaxBooks INT
        SELECT @MaxBooks = CAST(SettingValue AS INT) FROM SystemSettings WHERE SettingKey = 'MaxBooksPerMember'
        SELECT @BorrowingCount = COUNT(*) FROM BorrowRecords 
        WHERE MemberID = @MemberID AND Status = N'Đang mượn'
        
        IF @BorrowingCount >= @MaxBooks
        BEGIN
            SET @Result = 0
            SET @Message = N'Độc giả đã mượn đủ số sách tối đa (' + CAST(@MaxBooks AS NVARCHAR) + N' cuốn)'
            ROLLBACK
            RETURN
        END
        
        -- Tạo mã phiếu mượn
        DECLARE @BorrowCode NVARCHAR(20)
        SET @BorrowCode = 'PM' + FORMAT(GETDATE(), 'yyyyMMdd') + RIGHT('0000' + CAST((SELECT ISNULL(MAX(BorrowID), 0) + 1 FROM BorrowRecords) AS NVARCHAR), 4)
        
        -- Thêm phiếu mượn
        INSERT INTO BorrowRecords (BorrowCode, MemberID, BookID, BorrowDate, DueDate, Status, StaffID)
        VALUES (@BorrowCode, @MemberID, @BookID, GETDATE(), DATEADD(DAY, @DueDays, GETDATE()), N'Đang mượn', @StaffID)
        
        -- Giảm số sách sẵn có
        UPDATE Books SET AvailableCopies = AvailableCopies - 1 WHERE BookID = @BookID
        
        SET @Result = 1
        SET @Message = N'Mượn sách thành công. Mã phiếu: ' + @BorrowCode
        
        COMMIT
    END TRY
    BEGIN CATCH
        ROLLBACK
        SET @Result = 0
        SET @Message = ERROR_MESSAGE()
    END CATCH
END
GO

-- SP: Trả sách
CREATE PROCEDURE sp_ReturnBook
    @BorrowID INT,
    @StaffID INT,
    @Result INT OUTPUT,
    @Message NVARCHAR(500) OUTPUT,
    @FineAmount DECIMAL(18,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION
        
        DECLARE @BookID INT, @MemberID INT, @DueDate DATE, @FinePerDay DECIMAL(18,2)
        
        SELECT @BookID = BookID, @MemberID = MemberID, @DueDate = DueDate
        FROM BorrowRecords WHERE BorrowID = @BorrowID AND Status IN (N'Đang mượn', N'Quá hạn')
        
        IF @BookID IS NULL
        BEGIN
            SET @Result = 0
            SET @Message = N'Không tìm thấy phiếu mượn hoặc sách đã được trả'
            SET @FineAmount = 0
            ROLLBACK
            RETURN
        END
        
        -- Tính tiền phạt nếu quá hạn
        SELECT @FinePerDay = CAST(SettingValue AS DECIMAL(18,2)) FROM SystemSettings WHERE SettingKey = 'FinePerDay'
        SET @FineAmount = 0
        
        IF GETDATE() > @DueDate
        BEGIN
            SET @FineAmount = DATEDIFF(DAY, @DueDate, GETDATE()) * @FinePerDay
        END
        
        -- Cập nhật phiếu mượn
        UPDATE BorrowRecords 
        SET ReturnDate = GETDATE(), 
            Status = N'Đã trả', 
            FineAmount = @FineAmount,
            UpdatedDate = GETDATE()
        WHERE BorrowID = @BorrowID
        
        -- Tăng số sách sẵn có
        UPDATE Books SET AvailableCopies = AvailableCopies + 1 WHERE BookID = @BookID
        
        -- Cập nhật tổng tiền phạt của độc giả
        IF @FineAmount > 0
        BEGIN
            UPDATE Members SET TotalFine = TotalFine + @FineAmount WHERE MemberID = @MemberID
        END
        
        SET @Result = 1
        IF @FineAmount > 0
            SET @Message = N'Trả sách thành công. Tiền phạt: ' + FORMAT(@FineAmount, 'N0') + N' VNĐ'
        ELSE
            SET @Message = N'Trả sách thành công'
        
        COMMIT
    END TRY
    BEGIN CATCH
        ROLLBACK
        SET @Result = 0
        SET @Message = ERROR_MESSAGE()
        SET @FineAmount = 0
    END CATCH
END
GO

-- SP: Cập nhật trạng thái quá hạn
CREATE PROCEDURE sp_UpdateOverdueStatus
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE BorrowRecords 
    SET Status = N'Quá hạn'
    WHERE Status = N'Đang mượn' AND DueDate < GETDATE()
END
GO

-- SP: Thống kê tổng quan
CREATE PROCEDURE sp_GetDashboardStats
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Cập nhật trạng thái quá hạn trước
    EXEC sp_UpdateOverdueStatus
    
    SELECT 
        (SELECT COUNT(*) FROM Books WHERE IsActive = 1) AS TotalBooks,
        (SELECT SUM(TotalCopies) FROM Books WHERE IsActive = 1) AS TotalCopies,
        (SELECT SUM(AvailableCopies) FROM Books WHERE IsActive = 1) AS AvailableCopies,
        (SELECT COUNT(*) FROM Members WHERE IsActive = 1) AS TotalMembers,
        (SELECT COUNT(*) FROM BorrowRecords WHERE Status = N'Đang mượn') AS BorrowingCount,
        (SELECT COUNT(*) FROM BorrowRecords WHERE Status = N'Quá hạn') AS OverdueCount,
        (SELECT COUNT(*) FROM BorrowRecords WHERE CAST(BorrowDate AS DATE) = CAST(GETDATE() AS DATE)) AS TodayBorrows,
        (SELECT COUNT(*) FROM BorrowRecords WHERE CAST(ReturnDate AS DATE) = CAST(GETDATE() AS DATE)) AS TodayReturns
END
GO

-- SP: Lấy danh sách sách đang mượn của độc giả
CREATE PROCEDURE sp_GetMemberBorrowings
    @MemberID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT br.*, b.Title, b.ISBN
    FROM BorrowRecords br
    INNER JOIN Books b ON br.BookID = b.BookID
    WHERE br.MemberID = @MemberID AND br.Status IN (N'Đang mượn', N'Quá hạn')
    ORDER BY br.BorrowDate DESC
END
GO

-- SP: Báo cáo mượn trả theo tháng
CREATE PROCEDURE sp_ReportByMonth
    @Year INT,
    @Month INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        DAY(BorrowDate) AS Day,
        COUNT(CASE WHEN BorrowDate IS NOT NULL THEN 1 END) AS BorrowCount,
        COUNT(CASE WHEN ReturnDate IS NOT NULL THEN 1 END) AS ReturnCount
    FROM BorrowRecords
    WHERE YEAR(BorrowDate) = @Year AND MONTH(BorrowDate) = @Month
    GROUP BY DAY(BorrowDate)
    ORDER BY Day
END
GO

-- SP: Sách được mượn nhiều nhất
CREATE PROCEDURE sp_TopBorrowedBooks
    @TopN INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT TOP(@TopN) 
        b.BookID, b.Title, b.ISBN, c.CategoryName,
        COUNT(br.BorrowID) AS BorrowCount
    FROM Books b
    LEFT JOIN BorrowRecords br ON b.BookID = br.BookID
    LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
    WHERE b.IsActive = 1
    GROUP BY b.BookID, b.Title, b.ISBN, c.CategoryName
    ORDER BY BorrowCount DESC
END
GO

PRINT N'Database LibraryManagement đã được tạo thành công!'
GO
