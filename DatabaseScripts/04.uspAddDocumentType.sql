IF EXISTS ( SELECT * 
		    FROM   sysobjects
			WHERE id = object_id(N'[dbo].[uspAddDocumentType]')
				  and OBJECTPROPERTY(id, N'IsProcedure') = 1)

BEGIN
	DROP PROCEDURE [dbo].[uspAddDocumentType]
END 
GO

CREATE PROCEDURE [dbo].[uspAddDocumentType]
@Id int = NULL,
@Description nvarchar(255) = NULL
AS
BEGIN
IF NOT EXISTS(SELECT * FROM [dbo].[DocumentTypes] WHERE ID = @Id)
	INSERT INTO [dbo].[DocumentTypes] (Id, Description)
	SELECT @Id, @Description
ELSE 
	UPDATE [dbo].[DocumentTypes]
	SET [Description] = @Description
	WHERE Id = @Id
END