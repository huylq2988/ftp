CREATE TYPE [dbo].[DATA_HIS_THONG_SO] AS TABLE
(
    [Id] bigint NOT NULL,
	[MA_TS] [nvarchar](50) NOT NULL,
	[TEN_TS] [nvarchar](250) NULL,
	[TEN_TB] [nvarchar](50) NULL
)
GO
