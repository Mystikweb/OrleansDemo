CREATE TABLE [dbo].[OrleansQuery] (
    [QueryKey]  VARCHAR (64)   NOT NULL,
    [QueryText] VARCHAR (8000) NOT NULL,
    CONSTRAINT [OrleansQuery_Key] PRIMARY KEY CLUSTERED ([QueryKey] ASC)
);

