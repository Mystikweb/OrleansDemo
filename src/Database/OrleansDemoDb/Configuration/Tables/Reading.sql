CREATE TABLE [Configuration].[Reading]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [DeviceId] UNIQUEIDENTIFIER NOT NULL, 
    [ReadingTypeId] INT NOT NULL, 
	[Enabled] BIT NOT NULL DEFAULT 1,
    [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [UpdatedBy] NVARCHAR(75) NOT NULL, 
    CONSTRAINT [PK_Configuration_Reading] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Configuration_Reading_Device] FOREIGN KEY ([DeviceId]) REFERENCES [Configuration].[Device]([Id]), 
    CONSTRAINT [FK_Configuration_Reading_ReadingType] FOREIGN KEY ([ReadingTypeId]) REFERENCES [Configuration].[ReadingType]([Id]) 
)
