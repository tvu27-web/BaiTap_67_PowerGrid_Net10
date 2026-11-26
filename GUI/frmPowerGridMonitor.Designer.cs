namespace GUI;

partial class frmPowerGridMonitor
{
    private System.ComponentModel.IContainer? components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlControl = new Panel();
        lblSubstation = new Label();
        cboSubstation = new ComboBox();
        btnStartScada = new Button();
        btnDisconnect = new Button();
        grpDashboard = new GroupBox();
        pnlVoltageCard = new Panel();
        lblVoltageTitle = new Label();
        lblVoltage = new Label();
        pnlLoadCard = new Panel();
        lblLoadTitle = new Label();
        lblLoadPercent = new Label();
        lblWarningBanner = new Label();
        grpLogs = new GroupBox();
        dgvGridLogs = new DataGridView();
        statusStripGrid = new StatusStrip();
        lblOperator = new ToolStripStatusLabel();
        lblLastPacketTime = new ToolStripStatusLabel();
        lblCycleCount = new ToolStripStatusLabel();
        lblDbStatus = new ToolStripStatusLabel();

        pnlControl.SuspendLayout();
        grpDashboard.SuspendLayout();
        pnlVoltageCard.SuspendLayout();
        pnlLoadCard.SuspendLayout();
        grpLogs.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvGridLogs).BeginInit();
        statusStripGrid.SuspendLayout();
        SuspendLayout();

        // pnlControl
        pnlControl.Dock = DockStyle.Top;
        pnlControl.Height = 72;
        pnlControl.BackColor = Color.FromArgb(240, 244, 250);
        pnlControl.Padding = new Padding(12);

        lblSubstation.Location = new Point(16, 24);
        lblSubstation.AutoSize = true;
        lblSubstation.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblSubstation.Text = "Trạm biến áp:";

        cboSubstation.DropDownStyle = ComboBoxStyle.DropDownList;
        cboSubstation.Location = new Point(125, 21);
        cboSubstation.Size = new Size(240, 27);

        btnStartScada.Location = new Point(385, 16);
        btnStartScada.Size = new Size(210, 38);
        btnStartScada.Text = "Bắt đầu Giám sát SCADA";
        btnStartScada.BackColor = Color.SeaGreen;
        btnStartScada.ForeColor = Color.White;
        btnStartScada.FlatStyle = FlatStyle.Flat;
        btnStartScada.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnStartScada.Click += btnStartScada_Click;

        btnDisconnect.Location = new Point(610, 16);
        btnDisconnect.Size = new Size(150, 38);
        btnDisconnect.Text = "Ngắt Kết nối";
        btnDisconnect.BackColor = Color.Crimson;
        btnDisconnect.ForeColor = Color.White;
        btnDisconnect.FlatStyle = FlatStyle.Flat;
        btnDisconnect.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnDisconnect.Enabled = false;
        btnDisconnect.Click += btnDisconnect_Click;

        pnlControl.Controls.AddRange(new Control[] { lblSubstation, cboSubstation, btnStartScada, btnDisconnect });

        // grpDashboard
        grpDashboard.Dock = DockStyle.Top;
        grpDashboard.Height = 185;
        grpDashboard.Text = "Bảng thông số trực quan (Real-time Dashboard)";
        grpDashboard.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

        pnlVoltageCard.Location = new Point(20, 32);
        pnlVoltageCard.Size = new Size(360, 130);
        pnlVoltageCard.BorderStyle = BorderStyle.FixedSingle;
        pnlVoltageCard.BackColor = Color.White;

        lblVoltageTitle.Location = new Point(16, 12);
        lblVoltageTitle.AutoSize = true;
        lblVoltageTitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        lblVoltageTitle.ForeColor = Color.DimGray;
        lblVoltageTitle.Text = "ĐIỆN ÁP HIỆN TẠI (kV)";

        lblVoltage.Location = new Point(16, 48);
        lblVoltage.Size = new Size(328, 64);
        lblVoltage.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
        lblVoltage.ForeColor = Color.RoyalBlue;
        lblVoltage.Text = "--.- kV";
        lblVoltage.TextAlign = ContentAlignment.MiddleCenter;

        pnlVoltageCard.Controls.AddRange(new Control[] { lblVoltageTitle, lblVoltage });

        pnlLoadCard.Location = new Point(400, 32);
        pnlLoadCard.Size = new Size(470, 130);
        pnlLoadCard.BorderStyle = BorderStyle.FixedSingle;
        pnlLoadCard.BackColor = Color.White;

        lblLoadTitle.Location = new Point(16, 12);
        lblLoadTitle.AutoSize = true;
        lblLoadTitle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        lblLoadTitle.ForeColor = Color.DimGray;
        lblLoadTitle.Text = "MỨC MANG TẢI (%) - NGƯỠNG AN TOÀN <= 85%";

        lblLoadPercent.Location = new Point(16, 42);
        lblLoadPercent.Size = new Size(210, 66);
        lblLoadPercent.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
        lblLoadPercent.ForeColor = Color.ForestGreen;
        lblLoadPercent.Text = "--.- %";
        lblLoadPercent.TextAlign = ContentAlignment.MiddleCenter;

        lblWarningBanner.Location = new Point(235, 50);
        lblWarningBanner.Size = new Size(220, 52);
        lblWarningBanner.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblWarningBanner.ForeColor = Color.ForestGreen;
        lblWarningBanner.Text = "Trạng thái: Chờ tín hiệu";
        lblWarningBanner.TextAlign = ContentAlignment.MiddleLeft;

        pnlLoadCard.Controls.AddRange(new Control[] { lblLoadTitle, lblLoadPercent, lblWarningBanner });
        grpDashboard.Controls.AddRange(new Control[] { pnlVoltageCard, pnlLoadCard });

        // grpLogs
        grpLogs.Dock = DockStyle.Fill;
        grpLogs.Text = "Nhật ký sự cố Quá tải (> 85%)";
        grpLogs.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        grpLogs.Padding = new Padding(8);

        dgvGridLogs.Dock = DockStyle.Fill;
        dgvGridLogs.AllowUserToAddRows = false;
        dgvGridLogs.AllowUserToDeleteRows = false;
        dgvGridLogs.ReadOnly = true;
        dgvGridLogs.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvGridLogs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvGridLogs.RowHeadersVisible = false;
        dgvGridLogs.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

        grpLogs.Controls.Add(dgvGridLogs);

        // statusStripGrid
        statusStripGrid.Items.AddRange(new ToolStripItem[] { lblOperator, lblLastPacketTime, lblCycleCount, lblDbStatus });
        statusStripGrid.Location = new Point(0, 594);
        statusStripGrid.Name = "statusStripGrid";
        statusStripGrid.Size = new Size(920, 26);

        lblOperator.BorderSides = ToolStripStatusLabelBorderSides.Right;
        lblOperator.Text = "Điều độ viên: Vũ Minh Thành - 24801310190";

        lblLastPacketTime.BorderSides = ToolStripStatusLabelBorderSides.Right;
        lblLastPacketTime.Text = "Gói tin cuối: --:--:--";

        lblCycleCount.BorderSides = ToolStripStatusLabelBorderSides.Right;
        lblCycleCount.Text = "Tổng chu kỳ: 0";

        lblDbStatus.Spring = true;
        lblDbStatus.TextAlign = ContentAlignment.MiddleRight;
        lblDbStatus.Text = "Đang kiểm tra kết nối...";

        AutoScaleDimensions = new SizeF(8F, 19F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(920, 620);
        Controls.Add(grpLogs);
        Controls.Add(grpDashboard);
        Controls.Add(pnlControl);
        Controls.Add(statusStripGrid);
        Font = new Font("Segoe UI", 10F);
        Name = "frmPowerGridMonitor";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Trung tâm Điều độ & Giám sát Lưới điện SCADA Thời gian thực (.NET 10)";
        Load += frmPowerGridMonitor_Load;
        FormClosing += frmPowerGridMonitor_FormClosing;

        pnlControl.ResumeLayout(false);
        pnlControl.PerformLayout();
        grpDashboard.ResumeLayout(false);
        pnlVoltageCard.ResumeLayout(false);
        pnlVoltageCard.PerformLayout();
        pnlLoadCard.ResumeLayout(false);
        pnlLoadCard.PerformLayout();
        grpLogs.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvGridLogs).EndInit();
        statusStripGrid.ResumeLayout(false);
        statusStripGrid.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private Panel pnlControl = null!;
    private Label lblSubstation = null!;
    private ComboBox cboSubstation = null!;
    private Button btnStartScada = null!;
    private Button btnDisconnect = null!;
    private GroupBox grpDashboard = null!;
    private Panel pnlVoltageCard = null!;
    private Label lblVoltageTitle = null!;
    private Label lblVoltage = null!;
    private Panel pnlLoadCard = null!;
    private Label lblLoadTitle = null!;
    private Label lblLoadPercent = null!;
    private Label lblWarningBanner = null!;
    private GroupBox grpLogs = null!;
    private DataGridView dgvGridLogs = null!;
    private StatusStrip statusStripGrid = null!;
    private ToolStripStatusLabel lblOperator = null!;
    private ToolStripStatusLabel lblLastPacketTime = null!;
    private ToolStripStatusLabel lblCycleCount = null!;
    private ToolStripStatusLabel lblDbStatus = null!;
}
