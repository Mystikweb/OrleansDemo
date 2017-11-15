CREATE TABLE [Runtime].[Configuration]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [DeviceId] UNIQUEIDENTIFIER NOT NULL, 
	[Data] NVARCHAR(MAX) NOT NULL,
    [EffectiveDate] DATETIME2 NOT NULL, 
    [UploadDate] DATETIME2 NOT NULL, 
    [UploadUser] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_Runtime_Configuration] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Runtime_Configuration_Device] FOREIGN KEY ([DeviceId]) REFERENCES [Runtime].[Device]([Id]) 
)
