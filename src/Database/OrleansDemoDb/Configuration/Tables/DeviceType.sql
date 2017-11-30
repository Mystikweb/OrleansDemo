CREATE TABLE [Configuration].[DeviceType]
(
	[Id] INT NOT NULL IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Active] BIT NOT NULL DEFAULT 1, 
    [FileId] INT NULL, 
    CONSTRAINT [PK_Configuration_DeviceType] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_DeviceType_File] FOREIGN KEY ([FileId]) REFERENCES [Configuration].[File]([Id]) 
)
