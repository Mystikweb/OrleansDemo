CREATE USER [ClusterClient]
	FOR LOGIN [ClusterClient]
	WITH DEFAULT_SCHEMA = [Cluster]

GO

GRANT CONNECT TO [ClusterClient]
