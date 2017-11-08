CREATE TABLE [dbo].[OrleansStatisticsTable] (
    [OrleansStatisticsTableId] INT             IDENTITY (1, 1) NOT NULL,
    [DeploymentId]             NVARCHAR (150)  NOT NULL,
    [Timestamp]                DATETIME2 (3)   DEFAULT (getutcdate()) NOT NULL,
    [Id]                       NVARCHAR (250)  NOT NULL,
    [HostName]                 NVARCHAR (150)  NOT NULL,
    [Name]                     NVARCHAR (150)  NOT NULL,
    [IsValueDelta]             BIT             NOT NULL,
    [StatValue]                NVARCHAR (1024) NOT NULL,
    [Statistic]                NVARCHAR (512)  NOT NULL,
    CONSTRAINT [StatisticsTable_StatisticsTableId] PRIMARY KEY CLUSTERED ([OrleansStatisticsTableId] ASC)
);

