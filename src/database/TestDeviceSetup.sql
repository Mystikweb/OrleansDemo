USE [DemoConfiguration]
GO

DECLARE @testDeviceId UNIQUEIDENTIFIER = (SELECT DeviceId FROM Config.Device WHERE [Name] = N'Test')
IF @testDeviceId IS NULL
BEGIN
    INSERT INTO Config.Device (Name, IsEnabled) VALUES (N'Test', 1)
    SET @testDeviceId = (SELECT DeviceId FROM Config.Device WHERE [Name] = N'Test')
END

DECLARE @testTempId INT = (SELECT SensorId FROM Config.Sensor WHERE [Name] = N'Temperature')
IF @testTempId IS NULL
BEGIN
    INSERT INTO Config.Sensor (Name, UOM) VALUES (N'Temperature', N'°')
    SET @testTempId = @@IDENTITY
END

DECLARE @testHumidity INT = (SELECT SensorId FROM Config.Sensor WHERE [Name] = N'Humidity')
IF @testHumidity IS NULL
BEGIN
    INSERT INTO Config.Sensor (Name, UOM) VALUES (N'Humidity', N'%')
    SET @testHumidity = @@IDENTITY
END

DECLARE @testDeviceTempSensorId INT = (SELECT DeviceSensorId FROM Config.DeviceSensor WHERE DeviceId = @testDeviceId AND SensorId = @testTempId )
IF @testDeviceTempSensorId IS NULL
    INSERT INTO Config.DeviceSensor (DeviceId, SensorId, IsEnabled) VALUES (@testDeviceId, @testTempId, 1)

DECLARE @testDeviceHumidSensorId INT = (SELECT DeviceSensorId FROM Config.DeviceSensor WHERE DeviceId = @testDeviceId AND SensorId = @testHumidity )
IF @testDeviceHumidSensorId IS NULL
    INSERT INTO Config.DeviceSensor (DeviceId, SensorId, IsEnabled) VALUES (@testDeviceId, @testHumidity, 1)

DECLARE @eventTypeId INT
DECLARE runner CURSOR LOCAL FOR (SELECT [StateId] FROM Config.State)
OPEN runner
FETCH NEXT FROM runner INTO @eventTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
	IF NOT EXISTS (SELECT * FROM Config.DeviceState WHERE [DeviceId] = @testDeviceId AND [StateId] = @eventTypeId)
        INSERT INTO Config.DeviceState ([DeviceId], [StateId], [IsEnabled]) VALUES (@testDeviceId, @eventTypeId, 1)

	FETCH NEXT FROM runner INTO @eventTypeId
END
CLOSE runner
DEALLOCATE runner