CREATE PROCEDURE [Configuration].[SetupDeviceReading]
	@DeviceName NVARCHAR(50),
	@ReadingTypeName NVARCHAR(50),
	@Enabled BIT = 1
AS
BEGIN
	DECLARE @DeviceId UNIQUEIDENTIFIER
	DECLARE @ReadingTypeId INT

	SELECT @DeviceId = [Id] FROM [Configuration].[Device] WHERE [Name] = @DeviceName
	SELECT @ReadingTypeId = [Id] FROM [Configuration].[ReadingType] WHERE [Name] = @ReadingTypeName

	IF (@DeviceId IS NOT NULL AND @ReadingTypeId IS NOT NULL)
	BEGIN
		DECLARE @ReadingId UNIQUEIDENTIFIER

		SELECT @ReadingId = [Id] FROM [Configuration].[Reading] WHERE [DeviceId] = @DeviceId AND [ReadingTypeId] = @ReadingTypeId
		IF (@ReadingId IS NULL)
			INSERT INTO [Configuration].[Reading] ([DeviceId], [ReadingTypeId], [Enabled], [UpdatedAt], [UpdatedBy]) VALUES (@DeviceId, @ReadingTypeId, @Enabled, GETDATE(), N'Admin')
		ELSE
			UPDATE [Configuration].[Reading]
			SET [Enabled] = @Enabled,
				[UpdatedAt] = GETDATE(),
				[UpdatedBy] = N'Admin'
			WHERE [Id] = @ReadingId
	END
END
