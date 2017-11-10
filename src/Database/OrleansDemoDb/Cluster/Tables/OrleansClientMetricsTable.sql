CREATE TABLE [Cluster].[OrleansClientMetricsTable] (
    [DeploymentId]          NVARCHAR (150) NOT NULL,
    [ClientId]              NVARCHAR (150) NOT NULL,
    [Timestamp]             DATETIME2 (3)  DEFAULT (getutcdate()) NOT NULL,
    [Address]               VARCHAR (45)   NOT NULL,
    [HostName]              NVARCHAR (150) NOT NULL,
    [CpuUsage]              FLOAT (53)     NOT NULL,
    [MemoryUsage]           BIGINT         NOT NULL,
    [SendQueueLength]       INT            NOT NULL,
    [ReceiveQueueLength]    INT            NOT NULL,
    [SentMessages]          BIGINT         NOT NULL,
    [ReceivedMessages]      BIGINT         NOT NULL,
    [ConnectedGatewayCount] BIGINT         NOT NULL,
    CONSTRAINT [PK_ClientMetricsTable_DeploymentId_ClientId] PRIMARY KEY CLUSTERED ([DeploymentId] ASC, [ClientId] ASC)
);

