CREATE TABLE [Configuration].[ReadingType]
(
	[Id] INT NOT NULL IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [UOM] NVARCHAR(10) NOT NULL, 
    [DataType] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_Configuration_ReadingType] PRIMARY KEY ([Id]) 
)
