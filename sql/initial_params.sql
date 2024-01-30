USE [DEMO_MIDDLE]
GO
alter TABLE params
add Folder nvarchar(50)
GO
create proc [dbo].[sp_init_param]
as
begin
declare @i int
declare @folder nvarchar(50)
set @i=1
while (@i<=10000)
begin
	if(@i<1600)
		set @folder='S1'
	else if(@i<3200)
		set @folder='S2'
	else if(@i<4800)
		set @folder='S3'
	else if(@i<6400)
		set @folder='NL'
	else if(@i<8000)
		set @folder='XLN'
	else
		set @folder='DC'
	insert into params values ('PARAM'+cast(@i as nvarchar(25)),'PARAM'+cast(@i as nvarchar(25)),'m','TM',1,1,60,NULL,1,@folder)
	set @i=@i+1
end
end
GO
exec sp_init_param
GO
USE [Runtime]
GO
create proc [dbo].[sp_init_param]
as
begin
declare @i int
set @i=1
while (@i<=10000)
begin
	insert into Tag values ('PARAM'+cast(@i as nvarchar(25)))
	set @i=@i+1
end
end
GO
exec sp_init_param