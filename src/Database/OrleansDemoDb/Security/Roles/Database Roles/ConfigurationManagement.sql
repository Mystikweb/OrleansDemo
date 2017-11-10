CREATE ROLE [ConfigurationManagement]
	AUTHORIZATION [dbo];


GO
ALTER ROLE [ConfigurationManagement] ADD MEMBER [DeviceManager];