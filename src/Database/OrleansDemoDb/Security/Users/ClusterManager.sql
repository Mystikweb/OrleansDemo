CREATE USER [ClusterManager] 
	FOR LOGIN [ClusterManager]
    WITH DEFAULT_SCHEMA = [Cluster];

GO
GRANT CONNECT TO [ClusterManager];