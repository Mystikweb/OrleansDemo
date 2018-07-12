USE [master]
GO

CREATE LOGIN [ConfigManager]
    WITH PASSWORD = N'MyPa55w0rd123',
    DEFAULT_LANGUAGE = [us_english]
GO

CREATE DATABASE [DemoConfiguration]
GO

USE [DemoConfiguration]
GO

CREATE ROLE [Configuration]
    AUTHORIZATION [dbo]
GO

CREATE SCHEMA [App]
    AUTHORIZATION [Configuration]
GO

CREATE SCHEMA [Config]
    AUTHORIZATION [Configuration]
GO

CREATE USER [ConfigManager]
    FOR LOGIN [ConfigManager]
    WITH DEFAULT_SCHEMA = [Config]
GO

GRANT CONNECT TO [ConfigManager]
GO

ALTER ROLE [Configuration]
    ADD MEMBER [ConfigManager]
GO

GRANT VIEW ANY COLUMN ENCRYPTION KEY DEFINITION TO PUBLIC;
GO

GRANT VIEW ANY COLUMN MASTER KEY DEFINITION TO PUBLIC;
GO

EXEC sp_defaultdb @loginame='ConfigManager', @defdb='DemoConfiguration'