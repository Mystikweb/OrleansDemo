USE [DemoConfiguration]
GO

CREATE TABLE [App].[Monitor] (
    [MonitorId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT [DF_MonitorId] DEFAULT   
        NEWSEQUENTIALID() ROWGUIDCOL,
    [Name] NVARCHAR(100) NOT NULL,
    [HostName] NVARCHAR(100) NOT NULL,
    [UserName] NVARCHAR(100) NULL,
    [Password] NVARCHAR(MAX) NULL,
    [ExchangeName] NVARCHAR(100) NOT NULL,
    [QueueName] NVARCHAR(100) NOT NULL,
    [IsEnabled] BIT NOT NULL
        CONSTRAINT [DF_Monitor_IsEnabled] DEFAULT 0,
    [RunAtStartup] BIT NOT NULL
        CONSTRAINT [DF_Monitor_RunAtStartup] DEFAULT 0,

    CONSTRAINT [PK_MonitorId] PRIMARY KEY CLUSTERED ([MonitorId])
)
GO