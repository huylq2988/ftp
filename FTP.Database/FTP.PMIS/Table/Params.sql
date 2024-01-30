CREATE TABLE [dbo].[Params] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [TagName]     NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (250) NULL,
    [Unit]        NVARCHAR (50)  NULL,
    [Device]      NVARCHAR (50)  NULL,
    [Ameterid]    NVARCHAR (50)  NULL,
    [Ratio]       DECIMAL (5, 2) NULL,
    [Interval]    INT            NULL,
    [Ord]         INT            NULL,
    [Enable]      BIT            NULL,
    CONSTRAINT [PK_HIS_THONG_SO] PRIMARY KEY CLUSTERED ([ID] ASC)
);

