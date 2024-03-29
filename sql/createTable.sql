USE [Runtime]
GO
/****** Object:  Table [dbo].[BwAnalogTable]    Script Date: 12/9/2023 2:51:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BwAnalogTable](
	[ProjNodeId] [int] NOT NULL,
	[TagName] [varchar](32) NULL,
	[LogDate] [varchar](12) NULL,
	[LogTime] [varchar](12) NULL,
	[MaxValue] [float] NULL,
	[AvgValue] [float] NULL,
	[MinValue] [float] NULL,
	[LastValue] [float] NULL,
	[Alarm] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

create proc [dbo].[sp_init_param_value2]
@fromDt datetime,
@toDt datetime
as
begin
declare @i int
set @i=1
while (@fromDt<=@toDt)
begin
	insert into BwAnalogTable ([ProjNodeId]
           ,[TagName]
           ,[LogDate]
           ,[LogTime]
           ,[LastValue])
	select 4,TagName,CONVERT(VARCHAR, @fromDt, 11),FORMAT(@fromDt , 'HH:mm:ss'),rand()
	from Tag

	set @fromDt=DATEADD(Mi,5,@fromDt)
end
end

exec [dbo].[sp_init_param_value2] '2024-01-26', '2024-01-31'