CREATE procedure [dbo].[sp_GetHisConfigurationSelect]
as
begin
	select ID, TagName, Description, Device FROM Params
end