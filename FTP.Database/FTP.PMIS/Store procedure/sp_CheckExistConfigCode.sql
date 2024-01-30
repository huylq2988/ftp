
CREATE proc [dbo].[sp_CheckExistConfigCode]
@tagName nvarchar(50)
as
begin
	select * from Params
	where TagName = @tagName
end