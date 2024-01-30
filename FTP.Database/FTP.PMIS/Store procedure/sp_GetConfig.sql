CREATE proc [dbo].[sp_GetConfig]
@type int
as
begin
	select top 1 * from Config where [Type] = @type
end