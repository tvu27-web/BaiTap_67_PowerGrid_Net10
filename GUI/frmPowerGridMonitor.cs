using BUS;
using DTO;

namespace GUI;

public partial class frmPowerGridMonitor : Form
{
    private readonly PowerGridBUS _bus = new();
    private CancellationTokenSource? _scadaCts;
    private int _cycleCounter;

    public frmPowerGridMonitor()
    {
        InitializeComponent();
    }

    private void frmPowerGridMonitor_Load(object sender, EventArgs e)
    {
        cboSubstation.DataSource = _bus.GetSubstations();
        string connStatus = _bus.InitializeConnection();
        lblDbStatus.Text = connStatus;
        lblDbStatus.ForeColor = connStatus.Contains("Online") ? Color.DarkGreen : Color.DarkOrange;

        ConfigureGridLogs();
        LoadSavedLogs();
    }

    /// <summary>
    /// Cấu hình thủ công các cột cho DataGridView nhật ký sự cố bằng code.
    /// </summary>
    private void ConfigureGridLogs()
    {
        dgvGridLogs.AutoGenerateColumns = false;
        dgvGridLogs.Columns.Clear();

        dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colTime",
            HeaderText = "Thời gian sự cố",
            FillWeight = 18
        });

        dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colSubstation",
            HeaderText = "Trạm biến áp",
            FillWeight = 22
        });

        dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colVoltage",
            HeaderText = "Điện áp (kV)",
            FillWeight = 14,
            DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight }
        });

        dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colLoad",
            HeaderText = "Mức tải (%)",
            FillWeight = 14,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment = DataGridViewContentAlignment.MiddleRight,
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
            }
        });

        dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colSeverity",
            HeaderText = "Mức cảnh báo",
            FillWeight = 18
        });

        dgvGridLogs.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colNote",
            HeaderText = "Chi tiết sự cố",
            FillWeight = 30
        });
    }

    private void LoadSavedLogs()
    {
        var logs = _bus.GetRecentOverloadLogs();
        foreach (var log in logs)
        {
            dgvGridLogs.Rows.Add(
                log.RecordedTime.ToString("dd/MM/yyyy HH:mm:ss"),
                log.SubstationName,
                $"{log.VoltageKv:F2} kV",
                $"{log.LoadPercent:F1} %",
                log.Severity,
                log.Note);
        }
    }

    private void btnStartScada_Click(object sender, EventArgs e)
    {
        if (_scadaCts is not null) return;

        string selectedSubstation = cboSubstation.SelectedItem?.ToString() ?? "Trạm 110kV Hà Đông";
        _scadaCts = new CancellationTokenSource();
        CancellationToken token = _scadaCts.Token;

        btnStartScada.Enabled = false;
        btnDisconnect.Enabled = true;
        cboSubstation.Enabled = false;

        // Khởi chạy vòng lặp ngầm trên Background Thread (Task.Run), định kỳ 2 giây/lần
        _ = Task.Run(async () =>
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    int currentCycle = Interlocked.Increment(ref _cycleCounter);

                    GridTelemetryDTO telemetry = _bus.SampleTelemetry(selectedSubstation, currentCycle);

                    GridOverloadLogDTO? overloadLog = null;
                    if (telemetry.IsOverloaded)
                    {
                        overloadLog = _bus.RecordOverloadIfNeeded(telemetry);
                    }

                    // BẮT BUỘC: Chuyển ngữ cảnh về Main UI Thread qua InvokeRequired / Invoke
                    UpdateDashboardThreadSafe(telemetry, overloadLog);

                    await Task.Delay(2000, token);
                }
            }
            catch (OperationCanceledException)
            {
                // Dừng vòng lặp SCADA an toàn khi người dùng nhấn Ngắt Kết nối
            }
        }, token);
    }

    /// <summary>
    /// Tuân thủ nghiêm ngặt quy tắc Cross-Thread Safety trong WinForms:
    /// Kiểm tra this.InvokeRequired và gọi this.Invoke(new Action(...)).
    /// </summary>
    private void UpdateDashboardThreadSafe(GridTelemetryDTO telemetry, GridOverloadLogDTO? overloadLog)
    {
        if (this.IsDisposed || !this.IsHandleCreated) return;

        if (this.InvokeRequired)
        {
            try
            {
                this.Invoke(new Action(() => UpdateDashboardThreadSafe(telemetry, overloadLog)));
            }
            catch (ObjectDisposedException)
            {
            }
            return;
        }

        // Cập nhật an toàn trên Main UI Thread
        lblVoltage.Text = $"{telemetry.VoltageKv:F2} kV";
        lblLoadPercent.Text = $"{telemetry.LoadPercent:F1} %";

        if (telemetry.IsOverloaded)
        {
            lblLoadPercent.ForeColor = Color.Red;
            lblWarningBanner.ForeColor = Color.Red;
            lblWarningBanner.Text = "CẢNH BÁO: QUÁ TẢI (> 85%)!";
            pnlLoadCard.BackColor = Color.FromArgb(255, 240, 240);
        }
        else
        {
            lblLoadPercent.ForeColor = Color.ForestGreen;
            lblWarningBanner.ForeColor = Color.ForestGreen;
            lblWarningBanner.Text = "Trạng thái: Vận hành ổn định";
            pnlLoadCard.BackColor = Color.White;
        }

        lblLastPacketTime.Text = $"Gói tin cuối: {telemetry.Timestamp:HH:mm:ss}";
        lblCycleCount.Text = $"Tổng chu kỳ: {telemetry.CycleNumber}";

        if (overloadLog is not null)
        {
            dgvGridLogs.Rows.Insert(
                0,
                overloadLog.RecordedTime.ToString("dd/MM/yyyy HH:mm:ss"),
                overloadLog.SubstationName,
                $"{overloadLog.VoltageKv:F2} kV",
                $"{overloadLog.LoadPercent:F1} %",
                overloadLog.Severity,
                overloadLog.Note);
        }
    }

    private void btnDisconnect_Click(object sender, EventArgs e)
    {
        StopScadaMonitoring();
    }

    private void StopScadaMonitoring()
    {
        if (_scadaCts is not null)
        {
            _scadaCts.Cancel();
            _scadaCts.Dispose();
            _scadaCts = null;
        }

        btnStartScada.Enabled = true;
        btnDisconnect.Enabled = false;
        cboSubstation.Enabled = true;
        lblWarningBanner.ForeColor = Color.DimGray;
        lblWarningBanner.Text = "Trạng thái: Đã ngắt kết nối";
        pnlLoadCard.BackColor = Color.White;
    }

    private void frmPowerGridMonitor_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (_scadaCts is not null)
        {
            _scadaCts.Cancel();
            _scadaCts.Dispose();
            _scadaCts = null;
        }
    }
}
