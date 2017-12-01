CREATE PROCEDURE [Configuration].[SetupReadingType]
	@Name NVARCHAR(50),
	@UOM NVARCHAR(10),
	@DataType NVARCHAR(50)
AS
BEGIN
	DECLARE @ReadingTypeId INT

	SELECT @ReadingTypeId = [Id] FROM [Configuration].[ReadingType] WHERE [Name] = @Name
	IF (@ReadingTypeId IS NULL)
		INSERT INTO [Configuration].[ReadingType] ([Name], [UOM], [DataType]) VALUES (@Name, @UOM, @DataType)
	ELSE
		UPDATE [Configuration].[ReadingType]
		SET [Name] = @Name,
			[UOM] = @UOM,
			[DataType] = @DataType
		WHERE [Id] = @ReadingTypeId
END
