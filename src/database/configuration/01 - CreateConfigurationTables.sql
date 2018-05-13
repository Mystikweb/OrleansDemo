USE [DemoConfiguration]
GO

CREATE TABLE [Config].[Device] (
    [DeviceId] UNIQUEIDENTIFIER NOT NULL
        CONSTRAINT [DF_DeviceId] DEFAULT   
        NEWSEQUENTIALID() ROWGUIDCOL,
    [Name] NVARCHAR(100) NOT NULL,
    [IsEnabled] BIT NOT NULL
        CONSTRAINT [DF_Device_IsEnabled] DEFAULT 0,

    CONSTRAINT [PK_DeviceId] PRIMARY KEY CLUSTERED ([DeviceId])
)
GO

CREATE TABLE [Config].[Sensor] (
    [SensorId] INT NOT NULL IDENTITY(1,1),
    [Name] NVARCHAR(100) NOT NULL,
    [UOM] NVARCHAR(50) NOT NULL

    CONSTRAINT [PK_SensorId] PRIMARY KEY CLUSTERED ([SensorId])
)
GO

CREATE TABLE [Config].[DeviceSensor] (
    [DeviceSensorId] INT NOT NULL IDENTITY(1,1),
    [DeviceId] UNIQUEIDENTIFIER NOT NULL,
    [SensorId] INT NOT NULL,
    [IsEnabled] BIT NOT NULL
        CONSTRAINT [DF_DeviceSensor_IsEnabled] DEFAULT 0,

    CONSTRAINT [PK_DeviceSensorId] PRIMARY KEY CLUSTERED ([DeviceSensorId]),
    CONSTRAINT [FK_DeviceSensor_Device] FOREIGN KEY ([DeviceId]) REFERENCES [Config].[Device]([DeviceId]),
    CONSTRAINT [FK_DeviceSensor_Sensor] FOREIGN KEY ([SensorId]) REFERENCES [Config].[Sensor]([SensorId])
)
GO

CREATE TABLE [Config].[EventType] (
    [EventTypeId] INT NOT NULL IDENTITY(1,1),
    [Name] NVARCHAR(100) NOT NULL,

    CONSTRAINT [PK_EventTypeId] PRIMARY KEY CLUSTERED ([EventTypeId])
)
GO

CREATE TABLE [Config].[DeviceEventType] (
    [DeviceEventTypeId] INT NOT NULL IDENTITY(1,1),
    [DeviceId] UNIQUEIDENTIFIER NOT NULL,
    [EventTypeId] INT NOT NULL,
    [IsEnabled] BIT NOT NULL
        CONSTRAINT [DF_DeviceEventType_IsEnabled] DEFAULT 0,

    CONSTRAINT [PK_DeviceEventTypeId] PRIMARY KEY CLUSTERED ([DeviceEventTypeId]),
    CONSTRAINT [FK_DeviceEventType_Device] FOREIGN KEY ([DeviceId]) REFERENCES [Config].[Device]([DeviceId]),
    CONSTRAINT [FK_DeviceEventType_EventType] FOREIGN KEY ([EventTypeId]) REFERENCES [Config].[EventType]([EventTypeId])
)
GO