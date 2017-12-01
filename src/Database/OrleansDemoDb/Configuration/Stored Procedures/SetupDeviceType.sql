CREATE PROCEDURE [Configuration].[SetupDeviceType]
	@Name NVARCHAR(50),
	@Active BIT = 1
AS
BEGIN
 DECLARE @DeviceTypeId INT

 SELECT @DeviceTypeId = [Id] FROM [Configuration].[DeviceType] WHERE [Name] = @Name
 IF (@DeviceTypeId IS NULL)
	INSERT INTO [Configuration].[DeviceType] ([Name], [Active]) VALUES (@Name, @Active)
 ELSE
	UPDATE [Configuration].[DeviceType]
	SET [Name] = @Name,
		[Active] = @Active
	WHERE [Id] = @DeviceTypeId
END
