namespace DTO;

public class GridTelemetryDTO
{
    public string SubstationName { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.Now;
    public double VoltageKv { get; set; }
    public double LoadPercent { get; set; }
    public int CycleNumber { get; set; }
    public bool IsOverloaded => LoadPercent > 85.0;
}

public class GridOverloadLogDTO
{
    public int EventId { get; set; }
    public string SubstationName { get; set; } = string.Empty;
    public DateTime RecordedTime { get; set; } = DateTime.Now;
    public double VoltageKv { get; set; }
    public double LoadPercent { get; set; }
    public string Severity { get; set; } = "Cảnh báo Quá tải";
    public string Note { get; set; } = string.Empty;
}
