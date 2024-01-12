SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO

CREATE OR ALTER VIEW [dbo].[vw_Dashboard_Application] AS
SELECT 
App.RefNumber AS 'RefNumber',
Addr.Postcode,
AStat.Description AS 'StatusDescription',
AStat.Code AS 'StatusCode',
--CONVERT(VARCHAR, App.ReviewRecommendation) AS 'ReviewRecommendation',
--CONVERT(VARCHAR, App.FlaggedForAudit) AS 'FlaggedForAudit',
ReviewRecommendation,
App.FlaggedForAudit,
(SELECT StartDate FROM ApplicationStatusHistories WHERE ApplicationId = App.Id AND EndDate IS NULL) AS 'LastStatusChangeDate'
FROM [dbo].[Applications] AS App
INNER JOIN [dbo].[ApplicationStatuses] AS AStat ON App.StatusId = AStat.Id
INNER JOIN [dbo].[ApplicationDetails] AS AppD ON App.ApplicationDetailId = AppD.Id
INNER JOIN [dbo].[Address] AS Addr ON AppD.InstallationAddressId = Addr.Id 

GO