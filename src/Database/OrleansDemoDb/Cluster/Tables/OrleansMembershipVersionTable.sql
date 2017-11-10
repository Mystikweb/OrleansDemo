CREATE TABLE [Cluster].[OrleansMembershipVersionTable] (
    [DeploymentId] NVARCHAR (150) NOT NULL,
    [Timestamp]    DATETIME2 (3)  DEFAULT (getutcdate()) NOT NULL,
    [Version]      INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_OrleansMembershipVersionTable_DeploymentId] PRIMARY KEY CLUSTERED ([DeploymentId] ASC)
);

