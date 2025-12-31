-- =============================================
-- SCRIPT CẬP NHẬT MẬT KHẨU NGƯỜI DÙNG
-- Chạy sau khi đã tạo database LibraryManagement
-- =============================================

USE LibraryManagement
GO

-- Xóa user cũ nếu có
DELETE FROM Users WHERE Username IN ('admin', 'staff1', 'manager')
GO

-- Tạo user mới với mật khẩu đã hash đúng định dạng
-- Password hash được tính bằng SHA256 và convert sang Base64

-- Admin: password = admin123
-- SHA256("admin123") = jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=
INSERT INTO Users (Username, Password, FullName, Email, Role)
VALUES ('admin', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', N'Quản trị viên', 'admin@library.com', 'Admin')

-- Manager: password = 123456
-- SHA256("123456") = jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=
INSERT INTO Users (Username, Password, FullName, Email, Role)
VALUES ('manager', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Quản lý', 'manager@library.com', 'Manager')

-- Staff: password = 123456
INSERT INTO Users (Username, Password, FullName, Email, Role)
VALUES ('staff1', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', N'Nhân viên 1', 'staff1@library.com', 'Staff')

GO

-- Kiểm tra
SELECT UserID, Username, FullName, Role, IsActive FROM Users
GO

PRINT N'Đã cập nhật mật khẩu thành công!'
PRINT N'Tài khoản:'
PRINT N'  - admin / admin123 (Admin)'
PRINT N'  - manager / 123456 (Manager)'
PRINT N'  - staff1 / 123456 (Staff)'
GO
