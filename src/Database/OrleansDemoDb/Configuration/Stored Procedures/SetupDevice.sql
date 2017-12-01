CREATE PROCEDURE [Configuration].[SetupDevice]
	@Name NVARCHAR(50),
	@DeviceTypeName NVARCHAR(50),
	@Enabled BIT = 1,
	@RunOnStartup BIT = 1
AS
BEGIN
	DECLARE @DeviceTypeId INT

	SELECT @DeviceTypeId = [Id] FROM [Configuration].[DeviceType] WHERE [Name] = @DeviceTypeName
	IF (@DeviceTypeId IS NOT NULL)
	BEGIN
		DECLARE @DeviceId UNIQUEIDENTIFIER

		SELECT @DeviceId = [Id] FROM [Configuration].[Device] WHERE [Name] = @Name
		IF (@DeviceId IS NULL)
			INSERT INTO [Configuration].[Device] ([Name], [DeviceTypeId], [Enabled], [RunOnStartup], [CreatedAt], [CreatedBy]) VALUES (@Name, @DeviceTypeId, @Enabled, @RunOnStartup, GETDATE(), N'Admin')
		ELSE
			UPDATE [Configuration].[Device]
			SET [Name] = @Name,
				[Enabled] = @Enabled,
				[RunOnStartup] = @RunOnStartup,
				[UpdatedAt] = GETDATE(),
				[UpdatedBy] = N'Admin'
			WHERE [Id] = @DeviceId
	END
END
