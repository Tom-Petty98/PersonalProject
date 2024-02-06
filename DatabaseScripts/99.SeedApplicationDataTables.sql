SET IDENTITY_INSERT [dbo].[ApplicationStatuses] ON;

EXECUTE [dbo].[uspAddApplicationStatus] 1, 'SUB', 'Submitted', 10;
EXECUTE [dbo].[uspAddApplicationStatus] 2, 'INRV', 'In Review',  20;
EXECUTE [dbo].[uspAddApplicationStatus] 3, 'WINST', 'With Installer',  30;
EXECUTE [dbo].[uspAddApplicationStatus] 4, 'QC', 'QC', 40;
EXECUTE [dbo].[uspAddApplicationStatus] 5, 'DA', 'DA', 50;
EXECUTE [dbo].[uspAddApplicationStatus] 6, 'VISSD', 'Voucher Issued',  60;
EXECUTE [dbo].[uspAddApplicationStatus] 7, 'COMPLETE', 'Complete',  70;
EXECUTE [dbo].[uspAddApplicationStatus] 7, 'WDRAWN', 'Withdrawn',  80;
EXECUTE [dbo].[uspAddApplicationStatus] 8, 'RJPEND', 'Rejected Pending',  90;
EXECUTE [dbo].[uspAddApplicationStatus] 9, 'REJCT', 'Rejected',  100;
EXECUTE [dbo].[uspAddApplicationStatus] 10, 'REVOKED', 'Revoked',  110;

SET IDENTITY_INSERT [dbo].[ApplicationStatuses] OFF;

SET IDENTITY_INSERT [dbo].[InstallerStatuses] ON;

EXECUTE [dbo].[uspAddInstallerStatus] 1, 'SUB', 'Submitted', 10;
EXECUTE [dbo].[uspAddInstallerStatus] 2, 'INRV', 'In Review',  20;
EXECUTE [dbo].[uspAddInstallerStatus] 3, 'WINST', 'With Installer',  30;
EXECUTE [dbo].[uspAddInstallerStatus] 4, 'QC', 'QC', 40;
EXECUTE [dbo].[uspAddInstallerStatus] 5, 'DA', 'DA', 50;
EXECUTE [dbo].[uspAddInstallerStatus] 6, 'ACTIVE', 'Active',  60;
EXECUTE [dbo].[uspAddInstallerStatus] 7, 'WDRAWN', 'Withdrawn',  70;
EXECUTE [dbo].[uspAddInstallerStatus] 8, 'REJCT', 'Rejected',  80;
EXECUTE [dbo].[uspAddInstallerStatus] 9, 'REVOKED', 'Revoked',  90;

SET IDENTITY_INSERT [dbo].[InstallerStatuses] OFF;

SET IDENTITY_INSERT [dbo].[UserInviteStatuses] ON;

EXECUTE [dbo].[uspAddUserInviteStatus] 1, 'INVITED', 'Invited';
EXECUTE [dbo].[uspAddUserInviteStatus] 2, 'SIGNEDUP', 'Signed Up';
EXECUTE [dbo].[uspAddUserInviteStatus] 3, 'EXPIRED', 'Expired';
EXECUTE [dbo].[uspAddUserInviteStatus] 4, 'CANCELLED', 'Cancelled';
EXECUTE [dbo].[uspAddUserInviteStatus] 5, 'NOTSENT', 'Not Sent';
EXECUTE [dbo].[uspAddUserInviteStatus] 6, 'NOTDELIVRD', 'Not Delivered';

SET IDENTITY_INSERT [dbo].[UserInviteStatuses] OFF;

SET IDENTITY_INSERT [dbo].[Roles] ON;

EXECUTE [dbo].[uspAddRole] 1, 'AuthorisedRep', 0;
EXECUTE [dbo].[uspAddRole] 2, 'StandardUser', 0;
EXECUTE [dbo].[uspAddRole] 3, 'PendingAuthRep', 0;
EXECUTE [dbo].[uspAddRole] 4, 'Admin', 1;
EXECUTE [dbo].[uspAddRole] 5, 'L1InstallerReviewer', 1;
EXECUTE [dbo].[uspAddRole] 6, 'L2InstallerManager', 1;
EXECUTE [dbo].[uspAddRole] 7, 'L1ApplicationReviewer', 1;
EXECUTE [dbo].[uspAddRole] 8, 'L2ApplicationManager', 1;

SET IDENTITY_INSERT [dbo].[Roles] OFF;

SET IDENTITY_INSERT [dbo].[DocumentTypes] ON;

EXECUTE [dbo].[uspAddDocumentType] 1, 'Proof of purchase';
EXECUTE [dbo].[uspAddDocumentType] 2, 'Proof of ownership';
EXECUTE [dbo].[uspAddDocumentType] 3, 'Schematic';
EXECUTE [dbo].[uspAddDocumentType] 4, 'Other';

SET IDENTITY_INSERT [dbo].[DocumentTypes] OFF;
