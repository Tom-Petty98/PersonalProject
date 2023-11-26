IF EXISTS ( SELECT * 
		    FROM   sysobjects
			WHERE id = object_id(N'[dbo].[uspAddApplicationStatus]')
				  and OBJECTPROPERTY(id, N'IsProcedure') = 1)

BEGIN
	DROP PROCEDURE [dbo].[uspAddApplicationStatus]
END 
GO

CREATE PROCEDURE [dbo].[uspAddApplicationStatus]
@Id int = NULL,
@Code nvarchar(10) = NULL,
@Description nvarchar(255) = NULL,
@SortOrder int = NULL
AS
BEGIN
IF NOT EXISTS(SELECT * FROM [dbo].[ApplicationStatuses] WHERE ID = @Id)
	INSERT INTO [dbo].[ApplicationStatuses] (Id, Description, Code, SortOrder)
	SELECT @Id, @Description, @Code, @SortOrder
ELSE 
	UPDATE [dbo].[ApplicationStatuses]
	SET [Code] = @Code,
	[Description] = @Description,
	[SortOrder] = @SortOrder
	WHERE Id = @Id
END