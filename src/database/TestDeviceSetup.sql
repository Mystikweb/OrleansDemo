DECLARE @testDeviceId UNIQUEIDENTIFIER = (SELECT DeviceId FROM Config.Device WHERE [Name] = N'Test')
IF @testDeviceId IS NULL
BEGIN
    INSERT INTO Config.Device (Name, IsEnabled) VALUES (N'Test', 1)
    SET @testDeviceId = (SELECT DeviceId FROM Config.Device WHERE [Name] = N'Test')
END

DECLARE @testTempId INT = (SELECT SensorId FROM Config.Sensor WHERE [Name] = N'TestTemp')
IF @testTempId IS NULL
BEGIN
    INSERT INTO Config.Sensor (Name, UOM) VALUES (N'TestTemp', N'°')
    SET @testTempId = @@IDENTITY
END

DECLARE @testDeviceSensorId INT = (SELECT DeviceSensorId FROM Config.DeviceSensor WHERE DeviceId = @testDeviceId AND SensorId = @testTempId )
IF @testDeviceSensorId IS NULL
    INSERT INTO Config.DeviceSensor (DeviceId, SensorId, IsEnabled) VALUES (@testDeviceId, @testTempId, 1)
GO
