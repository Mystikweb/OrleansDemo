USE [master]
GO

CREATE LOGIN [RuntimeManager]
    WITH PASSWORD = N'MyPa55w0rd!',
    DEFAULT_LANGUAGE = [us_english];
GO

CREATE DATABASE [DemoRuntime];
GO

USE [DemoRuntime]
GO

CREATE ROLE [Runtime]
    AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Config]
    AUTHORIZATION [Runtime];
GO

CREATE SCHEMA [Runtime]
    AUTHORIZATION [Runtime];
GO

CREATE USER [RuntimeManager]
    FOR LOGIN [RuntimeManager]
    WITH DEFAULT_SCHEMA = [Config];
GO

GRANT CONNECT TO [RuntimeManager];
GO

ALTER ROLE [Runtime]
    ADD MEMBER [RuntimeManager];
GO

GRANT VIEW ANY COLUMN ENCRYPTION KEY DEFINITION TO PUBLIC;
GO

GRANT VIEW ANY COLUMN MASTER KEY DEFINITION TO PUBLIC;
GO

EXEC sp_defaultdb @loginame='RuntimeManager', @defdb='DemoRuntime'