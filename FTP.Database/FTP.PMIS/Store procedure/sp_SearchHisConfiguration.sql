CREATE proc [dbo].[sp_SearchHisConfiguration]
@txt nvarchar(255)
as
begin
	declare @like nvarchar(255)
	set @like = '%' + @txt + '%'

	select * from Params
	where TagName like @like or Description like @like or Device like @like or @txt is null or @txt = ''
end