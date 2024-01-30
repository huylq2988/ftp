CREATE proc [dbo].[sp_UpdateHisConfig]
@id bigint,
@tagName nvarchar(50),
@Description nvarchar(250),
@unit nvarchar(50),
@device nvarchar(50),
@interval int,
@enable bit
as
begin
	if not exists(select 1 from Params where TagName = @tagName and ID != @id)
	begin
		update [dbo].Params
		set TagName = @tagName
		, Description = @Description
		, Unit = @unit
		, Device = @device
		, Interval = @interval
		, Enable = @enable
		where ID = @id
	end
end