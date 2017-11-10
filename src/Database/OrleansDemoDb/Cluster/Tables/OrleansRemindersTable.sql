CREATE TABLE [Cluster].[OrleansRemindersTable] (
    [ServiceId]    NVARCHAR (150) NOT NULL,
    [GrainId]      VARCHAR (150)  NOT NULL,
    [ReminderName] NVARCHAR (150) NOT NULL,
    [StartTime]    DATETIME2 (3)  NOT NULL,
    [Period]       INT            NOT NULL,
    [GrainHash]    INT            NOT NULL,
    [Version]      INT            NOT NULL,
    CONSTRAINT [PK_RemindersTable_ServiceId_GrainId_ReminderName] PRIMARY KEY CLUSTERED ([ServiceId] ASC, [GrainId] ASC, [ReminderName] ASC)
);

