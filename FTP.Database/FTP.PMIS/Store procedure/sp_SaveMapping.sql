
CREATE procedure [dbo].[sp_SaveMapping]
@Id nvarchar(50),
@ameterId nvarchar(50)
as
begin
	update Params
	set Ameterid = @ameterId
	where ID = @Id
end