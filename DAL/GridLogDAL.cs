using DTO;
using Microsoft.Data.SqlClient;

namespace DAL;

public class GridLogDAL
{
    private static readonly List<GridOverloadLogDTO> _memoryLogs = [];
    private static readonly object _syncLock = new();
    private static int _nextId = 1;

    public void InsertOverloadLog(GridOverloadLogDTO log)
    {
        if (!DbConnectionConfig.IsDatabaseAvailable)
        {
            lock (_syncLock)
            {
                log.EventId = _nextId++;
                _memoryLogs.Add(log);
            }
            return;
        }

        const string sql = @"
            INSERT INTO dbo.GridOverloadLogs (SubstationName, RecordedTime, VoltageKv, LoadPercent, Severity, Note)
            VALUES (@SubstationName, @RecordedTime, @VoltageKv, @LoadPercent, @Severity, @Note)";

        using var conn = new SqlConnection(DbConnectionConfig.ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@SubstationName", log.SubstationName);
        cmd.Parameters.AddWithValue("@RecordedTime", log.RecordedTime);
        cmd.Parameters.AddWithValue("@VoltageKv", log.VoltageKv);
        cmd.Parameters.AddWithValue("@LoadPercent", log.LoadPercent);
        cmd.Parameters.AddWithValue("@Severity", log.Severity);
        cmd.Parameters.AddWithValue("@Note", log.Note);

        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public List<GridOverloadLogDTO> GetRecentLogs()
    {
        if (!DbConnectionConfig.IsDatabaseAvailable)
        {
            lock (_syncLock)
            {
                return _memoryLogs.OrderByDescending(x => x.RecordedTime).ToList();
            }
        }

        var list = new List<GridOverloadLogDTO>();
        const string sql = @"
            SELECT TOP 100 EventId, SubstationName, RecordedTime, VoltageKv, LoadPercent, Severity, Note
            FROM dbo.GridOverloadLogs
            ORDER BY RecordedTime DESC";

        using var conn = new SqlConnection(DbConnectionConfig.ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            list.Add(new GridOverloadLogDTO
            {
                EventId = reader.GetInt32(0),
                SubstationName = reader.GetString(1),
                RecordedTime = reader.GetDateTime(2),
                VoltageKv = reader.GetDouble(3),
                LoadPercent = reader.GetDouble(4),
                Severity = reader.GetString(5),
                Note = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
            });
        }

        return list;
    }
}
