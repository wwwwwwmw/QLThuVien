using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Dapper;
using Microsoft.Data.SqlClient;
using LibraryManagement.Models;

namespace LibraryManagement.Data
{
    /// <summary>
    /// Data Access Object cho User - Quản lý tài khoản người dùng
    /// </summary>
    public class UserDAO
    {
        /// <summary>
        /// Mã hóa mật khẩu bằng SHA256
        /// </summary>
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
        
        /// <summary>
        /// Đăng nhập hệ thống
        /// </summary>
        public User? Login(string username, string password)
        {
            string hashedPassword = HashPassword(password);
            
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                
                var user = conn.QueryFirstOrDefault<User>(
                    @"SELECT UserID, Username, FullName, Email, Phone, Role, IsActive, CreatedDate, LastLogin
                      FROM Users 
                      WHERE Username = @Username AND Password = @Password AND IsActive = 1",
                    new { Username = username, Password = hashedPassword });
                
                if (user != null)
                {
                    // Cập nhật thời gian đăng nhập
                    conn.Execute("UPDATE Users SET LastLogin = GETDATE() WHERE UserID = @UserID",
                        new { user.UserID });
                }
                
                return user;
            }
        }
        
        /// <summary>
        /// Lấy tất cả người dùng
        /// </summary>
        public List<User> GetAll()
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<User>(
                    "SELECT * FROM Users ORDER BY FullName").AsList();
            }
        }
        
        /// <summary>
        /// Lấy người dùng theo ID
        /// </summary>
        public User? GetById(int userId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.QueryFirstOrDefault<User>(
                    "SELECT * FROM Users WHERE UserID = @UserID",
                    new { UserID = userId });
            }
        }
        
        /// <summary>
        /// Tìm kiếm người dùng
        /// </summary>
        public List<User> Search(string? keyword = null, string? role = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                return conn.Query<User>(
                    @"SELECT * FROM Users 
                      WHERE (@Keyword IS NULL OR Username LIKE '%' + @Keyword + '%' OR FullName LIKE '%' + @Keyword + '%')
                        AND (@Role IS NULL OR Role = @Role)
                      ORDER BY FullName",
                    new { Keyword = keyword, Role = role }).AsList();
            }
        }
        
        /// <summary>
        /// Kiểm tra username đã tồn tại
        /// </summary>
        public bool UsernameExists(string username, int? excludeUserId = null)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                if (excludeUserId.HasValue)
                    sql += " AND UserID != @ExcludeUserId";
                    
                return conn.ExecuteScalar<int>(sql, 
                    new { Username = username, ExcludeUserId = excludeUserId }) > 0;
            }
        }
        
        /// <summary>
        /// Thêm người dùng mới
        /// </summary>
        public (bool Success, string Message) Add(User user, string password)
        {
            if (UsernameExists(user.Username))
            {
                return (false, "Tên đăng nhập đã tồn tại!");
            }
            
            user.Password = HashPassword(password);
            
            using (var conn = DatabaseConnection.GetConnection())
            {
                conn.Execute(
                    @"INSERT INTO Users (Username, Password, FullName, Email, Phone, Role, IsActive)
                      VALUES (@Username, @Password, @FullName, @Email, @Phone, @Role, @IsActive)", user);
                return (true, "Thêm người dùng thành công!");
            }
        }
        
        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        public (bool Success, string Message) Update(User user)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    @"UPDATE Users SET 
                        FullName = @FullName, Email = @Email, Phone = @Phone, 
                        Role = @Role, IsActive = @IsActive
                      WHERE UserID = @UserID", user);
                return affected > 0 
                    ? (true, "Cập nhật thành công!") 
                    : (false, "Không tìm thấy người dùng!");
            }
        }
        
        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        public (bool Success, string Message) ChangePassword(int userId, string newPassword)
        {
            string hashedPassword = HashPassword(newPassword);
            
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Users SET Password = @Password WHERE UserID = @UserID",
                    new { UserID = userId, Password = hashedPassword });
                return affected > 0 
                    ? (true, "Đổi mật khẩu thành công!") 
                    : (false, "Không tìm thấy người dùng!");
            }
        }
        
        /// <summary>
        /// Đổi mật khẩu với xác thực mật khẩu cũ
        /// </summary>
        public bool ChangePasswordWithVerification(int userId, string oldPassword, string newPassword)
        {
            string hashedOldPassword = HashPassword(oldPassword);
            string hashedNewPassword = HashPassword(newPassword);
            
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    @"UPDATE Users SET Password = @NewPassword 
                      WHERE UserID = @UserID AND Password = @OldPassword",
                    new { UserID = userId, OldPassword = hashedOldPassword, NewPassword = hashedNewPassword });
                return affected > 0;
            }
        }
        
        /// <summary>
        /// Xóa người dùng (đánh dấu không hoạt động)
        /// </summary>
        public (bool Success, string Message) Delete(int userId)
        {
            using (var conn = DatabaseConnection.GetConnection())
            {
                int affected = conn.Execute(
                    "UPDATE Users SET IsActive = 0 WHERE UserID = @UserID",
                    new { UserID = userId });
                return affected > 0 
                    ? (true, "Xóa thành công!") 
                    : (false, "Không tìm thấy người dùng!");
            }
        }
    }
}
