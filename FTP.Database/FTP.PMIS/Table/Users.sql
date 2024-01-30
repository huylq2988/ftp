CREATE TABLE [dbo].[Users] (
    [Userid]   NVARCHAR (50) NOT NULL,
    [Password] NVARCHAR (50) NULL,
    [Enable]   BIT           NULL,
    CONSTRAINT [PK_PQ_USER] PRIMARY KEY CLUSTERED ([Userid] ASC)
);

