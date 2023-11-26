IF EXISTS ( SELECT * 
		    FROM   sysobjects
			WHERE id = object_id(N'[dbo].[uspAddUserRole]')
				  and OBJECTPROPERTY(id, N'IsProcedure') = 1)

BEGIN
	DROP PROCEDURE [dbo].[uspAddUserRole]
END 
GO

CREATE PROCEDURE [dbo].[uspAddUserRole]
@Id int = NULL,
@Description nvarchar(255) = NULL
AS
BEGIN
IF NOT EXISTS(SELECT * FROM [dbo].[UserRoles] WHERE ID = @Id)
	INSERT INTO [dbo].[UserRoles] (Id, Description)
	SELECT @Id, @Description
ELSE 
	UPDATE [dbo].[UserRoles]
	SET [Description] = @Description
	WHERE Id = @Id
END