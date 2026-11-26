using Microsoft.Data.SqlClient;

namespace DAL;

/// <summary>
/// Quản lý chuỗi kết nối SQL Server (mặc định dùng LocalDB có sẵn trong Visual Studio).
/// Tự động chuyển sang chế độ Mô phỏng (In-Memory) nếu chưa tạo Database.
/// </summary>
public static class DbConnectionConfig
{
    public static string ConnectionString { get; set; } =
        "Server=(localdb)\\MSSQLLocalDB;Database=PowerGridScadaDB;Integrated Security=True;TrustServerCertificate=True;Connect Timeout=2;";

    public static bool IsDatabaseAvailable { get; private set; }
    public static string ConnectionStatusMessage { get; private set; } = "Chưa kiểm tra";

    public static bool TestConnection()
    {
        try
        {
            using var conn = new SqlConnection(ConnectionString);
            conn.Open();
            IsDatabaseAvailable = true;
            ConnectionStatusMessage = "SCADA DB: Đã kết nối LocalDB (Online)";
            return true;
        }
        catch
        {
            IsDatabaseAvailable = false;
            ConnectionStatusMessage = "SCADA DB: Chế độ Mô phỏng (In-Memory)";
            return false;
        }
    }
}
