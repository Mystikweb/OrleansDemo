CREATE TABLE [Configuration].[Reading]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [DeviceId] UNIQUEIDENTIFIER NOT NULL, 
    [ReadingTypeId] INT NOT NULL, 
	[CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [CreatedBy] NVARCHAR(75) NOT NULL, 
    [UpdatedAt] DATETIME2 NULL, 
    [UpdatedBy] NVARCHAR(75) NULL, 
    CONSTRAINT [PK_Configuration_Reading] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Configuration_Reading_Device] FOREIGN KEY ([DeviceId]) REFERENCES [Configuration].[Device]([Id]), 
    CONSTRAINT [FK_Configuration_Reading_ReadingType] FOREIGN KEY ([ReadingTypeId]) REFERENCES [Configuration].[ReadingType]([Id]) 
)
