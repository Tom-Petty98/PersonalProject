IF EXISTS ( SELECT * 
		    FROM   sysobjects
			WHERE id = object_id(N'[dbo].[uspAddRole]')
				  and OBJECTPROPERTY(id, N'IsProcedure') = 1)

BEGIN
	DROP PROCEDURE [dbo].[uspAddRole]
END 
GO

CREATE PROCEDURE [dbo].[uspAddRole]
@Id int = NULL,
@Description nvarchar(255) = NULL,
@IsInternalRole bit = 0
AS
BEGIN
IF NOT EXISTS(SELECT * FROM [dbo].[Roles] WHERE ID = @Id)
	INSERT INTO [dbo].[Roles] (Id, Description, IsInternalRole)
	SELECT @Id, @Description, @IsInternalRole
ELSE 
	UPDATE [dbo].[Roles]
	SET [Description] = @Description,
	IsInternalRole = @IsInternalRole
	WHERE Id = @Id
END