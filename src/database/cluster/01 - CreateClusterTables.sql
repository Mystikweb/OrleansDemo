USE [DemoCluster]
GO

CREATE TABLE [Cluster].[OrleansQuery] (
    [QueryKey]  VARCHAR (64)   NOT NULL,
    [QueryText] VARCHAR (8000) NOT NULL,
    CONSTRAINT [OrleansQuery_Key] PRIMARY KEY CLUSTERED ([QueryKey] ASC)
)
GO

CREATE TABLE [Cluster].[OrleansMembershipVersionTable]
(
	[DeploymentId] NVARCHAR(150) NOT NULL,
	[Timestamp] DATETIME2(3) NOT NULL DEFAULT GETUTCDATE(),
	[Version] INT NOT NULL DEFAULT 0,

	CONSTRAINT [PK_OrleansMembershipVersionTable_DeploymentId] PRIMARY KEY([DeploymentId])
)
GO

CREATE TABLE [Cluster].[OrleansMembershipTable]
(
	[DeploymentId] NVARCHAR(150) NOT NULL,
	[Address] VARCHAR(45) NOT NULL,
	[Port] INT NOT NULL,
	[Generation] INT NOT NULL,
	[SiloName] NVARCHAR(150) NOT NULL,
	[HostName] NVARCHAR(150) NOT NULL,
	[Status] INT NOT NULL,
	[ProxyPort] INT NULL,
	[SuspectTimes] VARCHAR(8000) NULL,
	[StartTime] DATETIME2(3) NOT NULL,
	[IAmAliveTime] DATETIME2(3) NOT NULL,

	CONSTRAINT [PK_MembershipTable_DeploymentId] PRIMARY KEY([DeploymentId], [Address], [Port], [Generation]),
	CONSTRAINT [FK_MembershipTable_MembershipVersionTable_DeploymentId] FOREIGN KEY ([DeploymentId]) REFERENCES [Cluster].[OrleansMembershipVersionTable] ([DeploymentId])
)
GO

CREATE TABLE [Cluster].[OrleansRemindersTable]
(
	[ServiceId] NVARCHAR(150) NOT NULL,
	[GrainId] VARCHAR(150) NOT NULL,
	[ReminderName] NVARCHAR(150) NOT NULL,
	[StartTime] DATETIME2(3) NOT NULL,
	[Period] INT NOT NULL,
	[GrainHash] INT NOT NULL,
	[Version] INT NOT NULL,

	CONSTRAINT [PK_RemindersTable_ServiceId_GrainId_ReminderName] PRIMARY KEY([ServiceId], [GrainId], [ReminderName])
)
GO

CREATE TABLE [Cluster].[OrleansStatisticsTable]
(
	[OrleansStatisticsTableId] INT IDENTITY(1,1) NOT NULL,
	[DeploymentId] NVARCHAR(150) NOT NULL,
	[Timestamp] DATETIME2(3) NOT NULL DEFAULT GETUTCDATE(),
	[Id] NVARCHAR(250) NOT NULL,
	[HostName] NVARCHAR(150) NOT NULL,
	[Name] NVARCHAR(150) NOT NULL,
	[IsValueDelta] BIT NOT NULL,
	[StatValue] NVARCHAR(1024) NOT NULL,
	[Statistic] NVARCHAR(512) NOT NULL,

	CONSTRAINT [StatisticsTable_StatisticsTableId] PRIMARY KEY([OrleansStatisticsTableId])
)
GO

CREATE TABLE [Cluster].[OrleansClientMetricsTable]
(
	[DeploymentId] NVARCHAR(150) NOT NULL,
	[ClientId] NVARCHAR(150) NOT NULL,
	[Timestamp] DATETIME2(3) NOT NULL DEFAULT GETUTCDATE(),
	[Address] VARCHAR(45) NOT NULL,
	[HostName] NVARCHAR(150) NOT NULL,
	[CpuUsage] FLOAT NOT NULL,
	[MemoryUsage] BIGINT NOT NULL,
	[SendQueueLength] INT NOT NULL,
	[ReceiveQueueLength] INT NOT NULL,
	[SentMessages] BIGINT NOT NULL,
	[ReceivedMessages] BIGINT NOT NULL,
	[ConnectedGatewayCount] BIGINT NOT NULL,

	CONSTRAINT [PK_ClientMetricsTable_DeploymentId_ClientId] PRIMARY KEY ([DeploymentId], [ClientId])
)
GO

CREATE TABLE [Cluster].[OrleansSiloMetricsTable]
(
	[DeploymentId] NVARCHAR(150) NOT NULL,
	[SiloId] NVARCHAR(150) NOT NULL,
	[Timestamp] DATETIME2(3) NOT NULL DEFAULT GETUTCDATE(),
	[Address] VARCHAR(45) NOT NULL,
	[Port] INT NOT NULL,
	[Generation] INT NOT NULL,
	[HostName] NVARCHAR(150) NOT NULL,
	[GatewayAddress] VARCHAR(45) NOT NULL,
	[GatewayPort] INT NOT NULL,
	[CpuUsage] FLOAT NOT NULL,
	[MemoryUsage] BIGINT NOT NULL,
	[SendQueueLength] INT NOT NULL,
	[ReceiveQueueLength] INT NOT NULL,
	[SentMessages] BIGINT NOT NULL,
	[ReceivedMessages] BIGINT NOT NULL,
	[ActivationCount] INT NOT NULL,
	[RecentlyUsedActivationCount] INT NOT NULL,
	[RequestQueueLength] BIGINT NOT NULL,
	[IsOverloaded] BIT NOT NULL,
	[ClientCount] BIGINT NOT NULL,

	CONSTRAINT [PK_SiloMetricsTable_DeploymentId_SiloId] PRIMARY KEY ([DeploymentId], [SiloId])
)
GO

CREATE TABLE [Cluster].[Storage]
(
    [GrainIdHash]               INT NOT NULL,
    [GrainIdN0]	                BIGINT NOT NULL,
    [GrainIdN1]	                BIGINT NOT NULL,
    [GrainTypeHash]	            INT NOT NULL,
    [GrainTypeString]           NVARCHAR(512) NOT NULL,
    [GrainIdExtensionString]    NVARCHAR(512) NULL,
    [ServiceId]					NVARCHAR(150) NOT NULL,
    [PayloadBinary]			    VARBINARY(MAX) NULL,
    [PayloadXml]		        XML NULL,
    [PayloadJson]		        NVARCHAR(MAX) NULL,
    [ModifiedOn]				DATETIME2(3) NOT NULL,
    [Version]					INT NULL
)
GO

CREATE NONCLUSTERED INDEX [IX_Storage] ON [Cluster].[Storage]([GrainIdHash], [GrainTypeHash])
GO

ALTER TABLE [Cluster].[Storage] SET(LOCK_ESCALATION = DISABLE)
GO

IF EXISTS (SELECT 1 FROM sys.dm_db_persisted_sku_features WHERE feature_id = 100)
BEGIN
    ALTER TABLE [Cluster].[Storage] REBUILD PARTITION = ALL WITH(DATA_COMPRESSION = PAGE);
END