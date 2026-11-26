using DAL;
using DTO;

namespace BUS;

public class PowerGridBUS
{
    private readonly GridLogDAL _dal = new();

    public string InitializeConnection()
    {
        DbConnectionConfig.TestConnection();
        return DbConnectionConfig.ConnectionStatusMessage;
    }

    public List<string> GetSubstations() =>
    [
        "Trạm 110kV Hà Đông",
        "Trạm 220kV Chèm",
        "Trạm 110kV Thanh Xuân",
        "Trạm 220kV Thường Tín"
    ];

    /// <summary>
    /// Sinh ngẫu nhiên thông số SCADA: Điện áp (108kV - 115kV), Mức tải (50% - 98%).
    /// </summary>
    public GridTelemetryDTO SampleTelemetry(string substationName, int cycleNumber)
    {
        double voltage = Math.Round(108.0 + Random.Shared.NextDouble() * 7.0, 2);
        double loadPercent = Math.Round(50.0 + Random.Shared.NextDouble() * 48.0, 1);

        return new GridTelemetryDTO
        {
            SubstationName = substationName,
            Timestamp = DateTime.Now,
            VoltageKv = voltage,
            LoadPercent = loadPercent,
            CycleNumber = cycleNumber
        };
    }

    public GridOverloadLogDTO RecordOverloadIfNeeded(GridTelemetryDTO telemetry)
    {
        string severity = telemetry.LoadPercent >= 93.0
            ? "NGUY HIỂM (Quá tải cao)"
            : "Cảnh báo Quá tải";

        var log = new GridOverloadLogDTO
        {
            SubstationName = telemetry.SubstationName,
            RecordedTime = telemetry.Timestamp,
            VoltageKv = telemetry.VoltageKv,
            LoadPercent = telemetry.LoadPercent,
            Severity = severity,
            Note = $"Mức tải đạt {telemetry.LoadPercent:F1}% (> 85%), điện áp {telemetry.VoltageKv:F2} kV"
        };

        _dal.InsertOverloadLog(log);
        return log;
    }

    public List<GridOverloadLogDTO> GetRecentOverloadLogs() => _dal.GetRecentLogs();
}
