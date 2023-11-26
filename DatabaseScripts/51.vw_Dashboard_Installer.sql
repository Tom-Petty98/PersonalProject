SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO

CREATE OR ALTER VIEW [dbo].[vw_Dashboard_Installer] AS
SELECT 
Ins.RefNumber AS 'AppRefNumber',
InsD.InstallerName,
IStat.Description AS 'StatusDescription',
IStat.Code AS 'StatusCode',
Ins.ReviewRecommendation,
Ins.FlaggedForAudit,
(SELECT StartDate FROM InstallerStatusHistories WHERE InstallerId = Ins.Id AND EndDate IS NULL) AS 'LastStatusChangeDate'
FROM [dbo].[Installers] AS Ins
INNER JOIN [dbo].[InstallerStatuses] AS IStat ON Ins.StatusId = IStat.Id
INNER JOIN [dbo].[InstallerDetails] AS InsD ON Ins.InstallerDetailId = InsD.Id

GO