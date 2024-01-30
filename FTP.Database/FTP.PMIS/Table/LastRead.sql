CREATE TABLE [dbo].[LastRead] (
    [TagName]      NVARCHAR (25) NOT NULL,
    [LastReadTime] DATETIME      NULL,
    CONSTRAINT [PK_LastRead] PRIMARY KEY CLUSTERED ([TagName] ASC)
);

