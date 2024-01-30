CREATE proc [dbo].[sp_SaveConfigConnection]
	@username nvarchar(50),
	@password nvarchar(50),
	@server nvarchar(500),
	@database nvarchar(500)
as
begin
	if not exists(select 1 from Config where [Type] = 2)
	begin
		insert into Config(Username, [Password], [Server], [Database], [Type])
		values (@username, @password, @server, @database, 2)
	end
	else
	begin
		update Config
		set Username = @username,
		[Password] = @password,
		[Server] = @server,
		[Database] = @database
		where [Type] = 2
	end
end