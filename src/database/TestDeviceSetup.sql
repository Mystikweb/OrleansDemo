USE [DemoConfiguration]
GO

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

DECLARE @eventTypeId INT
DECLARE runner CURSOR LOCAL FOR (SELECT [EventTypeId] FROM Config.EventType)
OPEN runner
FETCH NEXT FROM runner INTO @eventTypeId
WHILE @@FETCH_STATUS = 0
BEGIN
	IF NOT EXISTS (SELECT * FROM Config.DeviceEventType WHERE [DeviceId] = @testDeviceId AND [EventTypeId] = @eventTypeId)
        INSERT INTO Config.DeviceEventType ([DeviceId], [EventTypeId], [IsEnabled]) VALUES (@testDeviceId, @eventTypeId, 1)

	FETCH NEXT FROM runner INTO @eventTypeId
END
CLOSE runner
DEALLOCATE runner