CREATE ROLE [ClusterManagement]
    AUTHORIZATION [dbo];


GO
ALTER ROLE [ClusterManagement] ADD MEMBER [ClusterManager];

