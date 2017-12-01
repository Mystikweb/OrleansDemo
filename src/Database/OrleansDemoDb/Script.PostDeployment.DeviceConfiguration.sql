/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

/**
 * Device Type Setup
 */

EXEC [Configuration].[SetupDeviceType] @Name = N'Test Client'

/**
 * Reading Type Setup
 */

EXEC [Configuration].[SetupReadingType] @Name = N'Random', @UOM = N'None', @DataType = N'System.Int32'

/**
 * Device Type Reading Type Setup
 */

EXEC [Configuration].[SetupDeviceTypeReadingType] @DeviceTypeName = N'Test Client', @ReadingTypeName = N'Random'

/**
 * Device Setup
 */

EXEC [Configuration].[SetupDevice] @Name = N'Command Line', @DeviceTypeName = N'Test Client'

/**
 * Device Reading Setup
 */

EXEC [Configuration].[SetupDeviceReading] @DeviceName = N'Command Line', @ReadingTypeName = N'Random'
