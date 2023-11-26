IF EXISTS ( SELECT * 
		    FROM   sysobjects
			WHERE id = object_id(N'[dbo].[uspAddInstallerStatus]')
				  and OBJECTPROPERTY(id, N'IsProcedure') = 1)

BEGIN
	DROP PROCEDURE [dbo].[uspAddInstallerStatus]
END 
GO

CREATE PROCEDURE [dbo].[uspAddInstallerStatus]
@Id int = NULL,
@Code nvarchar(10) = NULL,
@Description nvarchar(255) = NULL,
@SortOrder int = NULL
AS
BEGIN
IF NOT EXISTS(SELECT * FROM [dbo].[InstallerStatuses] WHERE ID = @Id)
	INSERT INTO [dbo].[InstallerStatuses] (Id, Description, Code, SortOrder)
	SELECT @Id, @Description, @Code, @SortOrder
ELSE 
	UPDATE [dbo].[InstallerStatuses]
	SET [Code] = @Code,
	[Description] = @Description,
	[SortOrder] = @SortOrder
	WHERE Id = @Id
END