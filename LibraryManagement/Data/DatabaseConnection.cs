using System;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace LibraryManagement.Data
{
    /// <summary>
    /// Lớp quản lý kết nối cơ sở dữ liệu SQL Server
    /// Hỗ trợ kết nối LAN với nhiều máy tính
    /// </summary>
    public static class DatabaseConnection
    {
        private static string? _connectionString;

        /// <summary>
        /// Lấy chuỗi kết nối từ file cấu hình
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["LibraryDB"]?.ConnectionString
                        ?? throw new InvalidOperationException("Không tìm thấy chuỗi kết nối 'LibraryDB' trong file cấu hình.");
                }
                return _connectionString;
            }
            set => _connectionString = value;
        }

        /// <summary>
        /// Tạo kết nối mới đến SQL Server
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Kiểm tra kết nối đến SQL Server
        /// </summary>
        public static bool TestConnection(out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra kết nối với chuỗi kết nối tùy chỉnh
        /// </summary>
        public static bool TestConnection(string connectionString, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Cập nhật chuỗi kết nối động (khi thay đổi server)
        /// </summary>
        public static void UpdateConnectionString(string server, string database,
            bool useWindowsAuth = true, string? userId = null, string? password = null)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                IntegratedSecurity = useWindowsAuth,
                TrustServerCertificate = true,
                MultipleActiveResultSets = true
            };

            if (!useWindowsAuth && !string.IsNullOrEmpty(userId))
            {
                builder.UserID = userId;
                builder.Password = password;
            }

            _connectionString = builder.ConnectionString;
        }
    }
}
