CREATE proc [dbo].[sp_InsertHisConfig]
@tagName nvarchar(50),
@description nvarchar(250),
@unit nvarchar(50),
@device nvarchar(50),
@interval int
as
begin
	INSERT INTO [dbo].Params
			   (TagName
			   ,Description
			   ,Unit
			   ,Device			   
			   ,Ratio			   
			   ,Interval
			   ,Enable
			   )
		 VALUES
			   (@tagName
			   ,@description
			   ,@unit
			   ,@device
			   , 1
			   ,@interval
			   , 1
			   )
end