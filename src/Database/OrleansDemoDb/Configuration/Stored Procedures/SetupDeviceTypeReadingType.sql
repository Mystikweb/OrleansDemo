CREATE PROCEDURE [Configuration].[SetupDeviceTypeReadingType]
	@DeviceTypeName NVARCHAR(50),
	@ReadingTypeName NVARCHAR(50),
	@Active BIT = 1
AS
BEGIN
	DECLARE @DeviceTypeId INT
	DECLARE @ReadingTypeId INT

	SELECT @DeviceTypeId = [Id] FROM [Configuration].[DeviceType] WHERE [Name] = @DeviceTypeName
	SELECT @ReadingTypeId = [Id] FROM [Configuration].[ReadingType] WHERE [Name] = @ReadingTypeName

	IF (@DeviceTypeId IS NOT NULL AND @ReadingTypeId IS NOT NULL)
	BEGIN
		DECLARE @DeviceTypeReadingTypeId INT

		SELECT @DeviceTypeReadingTypeId = [Id] FROM [Configuration].[DeviceTypeReadingType] WHERE [DeviceTypeId] = @DeviceTypeId AND [ReadingTypeId] = @ReadingTypeId
		IF (@DeviceTypeReadingTypeId IS NULL)
			INSERT INTO [Configuration].[DeviceTypeReadingType] ([DeviceTypeId], [ReadingTypeId], [Active]) VALUES (@DeviceTypeId, @ReadingTypeId, @Active)
		ELSE
			UPDATE [Configuration].[DeviceTypeReadingType]
			SET [Active] = @Active
			WHERE [Id] = @DeviceTypeReadingTypeId
	END
END
