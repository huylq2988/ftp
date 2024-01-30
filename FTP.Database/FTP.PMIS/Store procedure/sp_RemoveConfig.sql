CREATE proc [dbo].[sp_RemoveConfig]
@tagName nvarchar(50)
as
begin
	delete Params
	where TagName = @tagName
end