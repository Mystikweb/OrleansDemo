USE [DemoRuntime]
GO

/* Configuration Schema Tables */
CREATE TABLE [Config].[Device] (
    [DeviceId] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWSEQUENTIALID(),
    [Name] NVARCHAR(100) NOT NULL,

    CONSTRAINT [PK_DeviceId] PRIMARY KEY CLUSTERED ([DeviceId])
)
GO

CREATE TABLE [Config].[Sensor] (
    [SensorId] INT NOT NULL IDENTITY(1,1),
    [Name] NVARCHAR(100) NOT NULL,

    CONSTRAINT [PK_SensorId] PRIMARY KEY CLUSTERED ([SensorId])
)
GO

CREATE TABLE [Config].[DeviceSensor] (
    [DeviceSensorId] INT NOT NULL IDENTITY(1,1),
    [DeviceId] UNIQUEIDENTIFIER NOT NULL,
    [SensorId] INT NOT NULL,
    [IsEnabled] BIT NOT NULL,

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

/* Runtime Schema Tables */
CREATE TABLE [Runtime].[DeviceEvent] (
    [Device] NVARCHAR(100) NOT NULL,
    [Event] NVARCHAR(100) NOT NULL,
    [StartTime] DATETIME2(3) NOT NULL,
    [EndTime] DATETIME2(3) NULL,

    CONSTRAINT [PK_DeviceEvent] PRIMARY KEY CLUSTERED ([Device], [StartTime])
)
GO

CREATE TABLE [Runtime].[DeviceSensorValue] (
    [Device] NVARCHAR(100) NOT NULL,
    [Sensor] NVARCHAR(100) NOT NULL,
    [Timestamp] DATETIME2(3) NOT NULL,
    [Value] NVARCHAR(100) NOT NULL,

    CONSTRAINT [PK_DeviceSensorValue] PRIMARY KEY CLUSTERED ([Device], [Sensor], [Timestamp])
)
GO
