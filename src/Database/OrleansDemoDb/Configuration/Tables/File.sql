CREATE TABLE [Configuration].[File]
(
	[Id] INT NOT NULL, 
    [Name] NVARCHAR(150) NOT NULL, 
    [Extension] NVARCHAR(10) NOT NULL, 
    [MimeType] NVARCHAR(50) NOT NULL, 
    [FileType] INT NOT NULL, 
	[Data] VARBINARY(MAX) NOT NULL,
    [UpdatedAt] DATETIME2 NOT NULL, 
    [UpdatedBy] NVARCHAR(100) NOT NULL, 
    CONSTRAINT [PK_Files] PRIMARY KEY ([Id]) 
)
