CREATE proc [dbo].[sp_SaveConfig]
	@username nvarchar(50),
	@password nvarchar(50),
	@source nvarchar(50),
	@destination nvarchar(50),
	@timer int
as
begin
	if not exists(select 1 from Config)
	begin
		insert into Config(Username, [Password], Source, Destination, Timer)
		values (@username, @password, @source, @destination, @timer)
	end
	else
	begin
		update Config
		set Username = @username,
		[Password] = @password,
		Source = @source,
		Destination = @destination,
		Timer = @timer
	end
end