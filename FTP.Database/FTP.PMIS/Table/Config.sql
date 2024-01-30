CREATE TABLE [dbo].[Config] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [Username]    VARCHAR (50)  NOT NULL,
    [Password]    VARCHAR (50)  NOT NULL,
    [Source]      VARCHAR (500) NOT NULL,
    [Destination] VARCHAR (500) NOT NULL,
    [Timer]       INT           NOT NULL,
    [TYPE]        INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);



