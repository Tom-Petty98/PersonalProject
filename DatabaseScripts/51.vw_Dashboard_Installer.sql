SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO

CREATE OR ALTER VIEW [dbo].[vw_Dashboard_Installer] AS
SELECT 
Ins.RefNumber AS 'RefNumber',
InsD.InstallerName,
IStat.Description AS 'StatusDescription',
IStat.Code AS 'StatusCode',
CASE 
	WHEN Ins.ReviewRecommendation = 1 THEN 'Pass'
	WHEN Ins.ReviewRecommendation = 0 THEN 'Fail'
	ELSE 'None'
END AS 'ReviewRecommendation',
CASE 
	WHEN Ins.FlaggedForAudit = 1 THEN 'True'
	ELSE 'False'
END AS 'FlaggedForAudit',
(SELECT StartDate FROM InstallerStatusHistories WHERE InstallerId = Ins.Id AND EndDate IS NULL) AS 'LastStatusChangeDate'
FROM [dbo].[Installers] AS Ins
INNER JOIN [dbo].[InstallerStatuses] AS IStat ON Ins.StatusId = IStat.Id
INNER JOIN [dbo].[InstallerDetails] AS InsD ON Ins.InstallerDetailId = InsD.Id

GO