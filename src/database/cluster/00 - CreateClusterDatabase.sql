USE [master]
GO

CREATE LOGIN [ClusterManager]
    WITH PASSWORD = N'MyPa55w0rd!', 
    DEFAULT_LANGUAGE = [us_english];
GO

CREATE DATABASE [DemoCluster];
GO

USE [DemoCluster]
GO

CREATE ROLE [ClusterManagement]
    AUTHORIZATION [dbo];
GO

CREATE SCHEMA [Cluster]
    AUTHORIZATION [ClusterManagement];
GO

CREATE USER [ClusterManager] 
	FOR LOGIN [ClusterManager]
    WITH DEFAULT_SCHEMA = [Cluster];
GO

GRANT CONNECT TO [ClusterManager];

ALTER ROLE [ClusterManagement] 
    ADD MEMBER [ClusterManager];
GO

GRANT VIEW ANY COLUMN ENCRYPTION KEY DEFINITION TO PUBLIC;
GO

GRANT VIEW ANY COLUMN MASTER KEY DEFINITION TO PUBLIC;
GO

EXEC sp_defaultdb @loginame='ClusterManager', @defdb='DemoCluster'