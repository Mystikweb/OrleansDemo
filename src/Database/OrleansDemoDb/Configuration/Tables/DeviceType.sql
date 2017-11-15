CREATE TABLE [Configuration].[DeviceType]
(
	[Id] INT NOT NULL IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Active] BIT NOT NULL DEFAULT 1, 
    CONSTRAINT [PK_Configuration_DeviceType] PRIMARY KEY ([Id]) 
)
