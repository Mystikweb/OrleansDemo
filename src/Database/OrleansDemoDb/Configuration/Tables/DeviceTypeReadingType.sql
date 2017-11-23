CREATE TABLE [Configuration].[DeviceTypeReadingType]
(
	[Id] INT NOT NULL IDENTITY, 
    [DeviceTypeId] INT NOT NULL, 
    [ReadingTypeId] INT NOT NULL, 
    [Active] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_DeviceTypeReadingType] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_DeviceTypeReadingType_DeviceType] FOREIGN KEY ([DeviceTypeId]) REFERENCES [Configuration].[DeviceType]([Id]), 
    CONSTRAINT [FK_DeviceTypeReadingType_ReadingType] FOREIGN KEY ([ReadingTypeId]) REFERENCES [Configuration].[ReadingType]([Id]) 
)
