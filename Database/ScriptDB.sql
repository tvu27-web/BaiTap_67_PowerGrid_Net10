-- =====================================================================
-- SCRIPT KHỞI TẠO CƠ SỞ DỮ LIỆU CHO BÀI TẬP 6.7 (.NET 10 - 3-TIER)
-- Trung tâm Điều độ & Giám sát Lưới điện Thời gian thực (SCADA)
-- =====================================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'PowerGridScadaDB')
BEGIN
    CREATE DATABASE PowerGridScadaDB;
END
GO

USE PowerGridScadaDB;
GO

-- Bảng Nhật ký Sự cố Quá tải Lưới điện (> 85%)
IF OBJECT_ID('dbo.GridOverloadLogs', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.GridOverloadLogs (
        EventId        INT IDENTITY(1,1) PRIMARY KEY,
        SubstationName NVARCHAR(150) NOT NULL,
        RecordedTime   DATETIME      NOT NULL DEFAULT GETDATE(),
        VoltageKv      FLOAT         NOT NULL,
        LoadPercent    FLOAT         NOT NULL,
        Severity       NVARCHAR(50)  NOT NULL,
        Note           NVARCHAR(255) NULL
    );
END
GO
