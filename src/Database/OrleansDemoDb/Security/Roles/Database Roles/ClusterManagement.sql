CREATE ROLE [ClusterManagement]
    AUTHORIZATION [dbo];


GO
ALTER ROLE [ClusterManagement] ADD MEMBER [ClusterManager];

GO
ALTER ROLE [ClusterManagement] ADD MEMBER [ClusterClient];

