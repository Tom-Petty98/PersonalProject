IF EXISTS ( SELECT * 
		    FROM   sysobjects
			WHERE id = object_id(N'[dbo].[uspAddUserInviteStatus]')
				  and OBJECTPROPERTY(id, N'IsProcedure') = 1)

BEGIN
	DROP PROCEDURE [dbo].[uspAddUserInviteStatus]
END 
GO

CREATE PROCEDURE [dbo].[uspAddUserInviteStatus]
@Id int = NULL,
@Code nvarchar(10) = NULL,
@Description nvarchar(255) = NULL
AS
BEGIN
IF NOT EXISTS(SELECT * FROM [dbo].[UserInviteStatuses] WHERE ID = @Id)
	INSERT INTO [dbo].[UserInviteStatuses] (Id, Description, Code)
	SELECT @Id, @Description, @Code
ELSE 
	UPDATE [dbo].[UserInviteStatuses]
	SET [Code] = @Code,
	[Description] = @Description
	WHERE Id = @Id
END