IF EXISTS ( SELECT * 
		    FROM   sysobjects
			WHERE id = object_id(N'[dbo].[uspAddTechType]')
				  and OBJECTPROPERTY(id, N'IsProcedure') = 1)

BEGIN
	DROP PROCEDURE [dbo].[uspAddTechType]
END 
GO

CREATE PROCEDURE [dbo].[uspAddTechType]
@Id int = NULL,
@Description nvarchar(255) = NULL,
@SortOrder int = NULL
AS
BEGIN
IF NOT EXISTS(SELECT * FROM [dbo].[TechTypes] WHERE ID = @Id)
	INSERT INTO [dbo].[TechTypes] (Id, Description, SortOrder)
	SELECT @Id, @Description, @SortOrder
ELSE 
	UPDATE [dbo].[TechTypes]
	SET [Description] = @Description,
	[SortOrder] = @SortOrder
	WHERE Id = @Id
END