CREATE TABLE [dbo].[Storage] (
    [GrainIdHash]            INT             NOT NULL,
    [GrainIdN0]              BIGINT          NOT NULL,
    [GrainIdN1]              BIGINT          NOT NULL,
    [GrainTypeHash]          INT             NOT NULL,
    [GrainTypeString]        NVARCHAR (512)  NOT NULL,
    [GrainIdExtensionString] NVARCHAR (512)  NULL,
    [ServiceId]              NVARCHAR (150)  NOT NULL,
    [PayloadBinary]          VARBINARY (MAX) NULL,
    [PayloadXml]             XML             NULL,
    [PayloadJson]            NVARCHAR (MAX)  NULL,
    [ModifiedOn]             DATETIME2 (3)   NOT NULL,
    [Version]                INT             NULL
);


GO
ALTER TABLE [dbo].[Storage] SET (LOCK_ESCALATION = DISABLE);


GO
CREATE NONCLUSTERED INDEX [IX_Storage]
    ON [dbo].[Storage]([GrainIdHash] ASC, [GrainTypeHash] ASC);

