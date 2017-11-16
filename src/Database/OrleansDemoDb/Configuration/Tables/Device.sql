CREATE TABLE [Configuration].[Device]
(
	[Id] UNIQUEIDENTIFIER NOT NULL  DEFAULT NEWID(), 
    [Name] NVARCHAR(50) NOT NULL, 
    [DeviceTypeId] INT NOT NULL, 
	[Enabled] BIT NOT NULL DEFAULT 1,
	[RunOnStartup] BIT NOT NULL DEFAULT 0,
    [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [CreatedBy] NVARCHAR(75) NOT NULL, 
    [UpdatedAt] DATETIME2 NULL, 
    [UpdatedBy] NVARCHAR(75) NULL, 
    CONSTRAINT [PK_Configuration_Device] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Configuration_Device_DeviceType] FOREIGN KEY ([DeviceTypeId]) REFERENCES [Configuration].[DeviceType]([Id])
)

GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Configuration_Device_Name_DeviceTypeId] ON [Configuration].[Device] ([Name], [DeviceTypeId])
