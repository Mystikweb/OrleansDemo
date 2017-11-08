/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'UpdateIAmAlivetimeKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'UpdateIAmAlivetimeKey','
		-- This is expected to never fail by Orleans, so return value
		-- is not needed nor is it checked.
		SET NOCOUNT ON;
		UPDATE OrleansMembershipTable
		SET
			IAmAliveTime = @IAmAliveTime
		WHERE
			DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL
			AND Address = @Address AND @Address IS NOT NULL
			AND Port = @Port AND @Port IS NOT NULL
			AND Generation = @Generation AND @Generation IS NOT NULL;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'UpdateIAmAlivetimeKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'InsertMembershipVersionKey','
		SET NOCOUNT ON;
		INSERT INTO OrleansMembershipVersionTable
		(
			DeploymentId
		)
		SELECT @DeploymentId
		WHERE NOT EXISTS
		(
		SELECT 1
		FROM
			OrleansMembershipVersionTable
		WHERE
			DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL
		);

		SELECT @@ROWCOUNT;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'InsertMembershipKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'InsertMembershipKey','
		SET XACT_ABORT, NOCOUNT ON;
		DECLARE @ROWCOUNT AS INT;
		BEGIN TRANSACTION;
		INSERT INTO OrleansMembershipTable
		(
			DeploymentId,
			Address,
			Port,
			Generation,
			SiloName,
			HostName,
			Status,
			ProxyPort,
			StartTime,
			IAmAliveTime
		)
		SELECT
			@DeploymentId,
			@Address,
			@Port,
			@Generation,
			@SiloName,
			@HostName,
			@Status,
			@ProxyPort,
			@StartTime,
			@IAmAliveTime
		WHERE NOT EXISTS
		(
		SELECT 1
		FROM
			OrleansMembershipTable
		WHERE
			DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL
			AND Address = @Address AND @Address IS NOT NULL
			AND Port = @Port AND @Port IS NOT NULL
			AND Generation = @Generation AND @Generation IS NOT NULL
		);

		UPDATE OrleansMembershipVersionTable
		SET
			Timestamp = GETUTCDATE(),
			Version = Version + 1
		WHERE
			DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL
			AND Version = @Version AND @Version IS NOT NULL
			AND @@ROWCOUNT > 0;

		SET @ROWCOUNT = @@ROWCOUNT;

		IF @ROWCOUNT = 0
			ROLLBACK TRANSACTION
		ELSE
			COMMIT TRANSACTION
		SELECT @ROWCOUNT;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'UpdateMembershipKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'UpdateMembershipKey','
		SET XACT_ABORT, NOCOUNT ON;
		BEGIN TRANSACTION;

		UPDATE OrleansMembershipVersionTable
		SET
			Timestamp = GETUTCDATE(),
			Version = Version + 1
		WHERE
			DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL
			AND Version = @Version AND @Version IS NOT NULL;

		UPDATE OrleansMembershipTable
		SET
			Status = @Status,
			SuspectTimes = @SuspectTimes,
			IAmAliveTime = @IAmAliveTime
		WHERE
			DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL
			AND Address = @Address AND @Address IS NOT NULL
			AND Port = @Port AND @Port IS NOT NULL
			AND Generation = @Generation AND @Generation IS NOT NULL
			AND @@ROWCOUNT > 0;

		SELECT @@ROWCOUNT;
		COMMIT TRANSACTION;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'UpsertReminderRowKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'UpsertReminderRowKey','
		DECLARE @Version AS INT = 0;
		SET XACT_ABORT, NOCOUNT ON;
		BEGIN TRANSACTION;
		UPDATE OrleansRemindersTable WITH(UPDLOCK, ROWLOCK, HOLDLOCK)
		SET
			StartTime = @StartTime,
			Period = @Period,
			GrainHash = @GrainHash,
			@Version = Version = Version + 1
		WHERE
			ServiceId = @ServiceId AND @ServiceId IS NOT NULL
			AND GrainId = @GrainId AND @GrainId IS NOT NULL
			AND ReminderName = @ReminderName AND @ReminderName IS NOT NULL;

		INSERT INTO OrleansRemindersTable
		(
			ServiceId,
			GrainId,
			ReminderName,
			StartTime,
			Period,
			GrainHash,
			Version
		)
		SELECT
			@ServiceId,
			@GrainId,
			@ReminderName,
			@StartTime,
			@Period,
			@GrainHash,
			0
		WHERE
			@@ROWCOUNT=0;
		SELECT @Version AS Version;
		COMMIT TRANSACTION;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'UpsertReportClientMetricsKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'UpsertReportClientMetricsKey','
		SET XACT_ABORT, NOCOUNT ON;
		BEGIN TRANSACTION;
		UPDATE OrleansClientMetricsTable WITH(UPDLOCK, ROWLOCK, HOLDLOCK)
		SET
			Timestamp = GETUTCDATE(),
			Address = @Address,
			HostName = @HostName,
			CpuUsage = @CpuUsage,
			MemoryUsage = @MemoryUsage,
			SendQueueLength = @SendQueueLength,
			ReceiveQueueLength = @ReceiveQueueLength,
			SentMessages = @SentMessages,
			ReceivedMessages = @ReceivedMessages,
			ConnectedGatewayCount = @ConnectedGatewayCount
		WHERE
			DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL
			AND ClientId = @ClientId AND @ClientId IS NOT NULL;

		INSERT INTO OrleansClientMetricsTable
		(
			DeploymentId,
			ClientId,
			Address,
			HostName,
			CpuUsage,
			MemoryUsage,
			SendQueueLength,
			ReceiveQueueLength,
			SentMessages,
			ReceivedMessages,
			ConnectedGatewayCount
		)
		SELECT
			@DeploymentId,
			@ClientId,
			@Address,
			@HostName,
			@CpuUsage,
			@MemoryUsage,
			@SendQueueLength,
			@ReceiveQueueLength,
			@SentMessages,
			@ReceivedMessages,
			@ConnectedGatewayCount
		WHERE
			@@ROWCOUNT=0;
		COMMIT TRANSACTION;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'UpsertSiloMetricsKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'UpsertSiloMetricsKey','
		SET XACT_ABORT, NOCOUNT ON;
		BEGIN TRANSACTION;
		UPDATE OrleansSiloMetricsTable WITH(UPDLOCK, ROWLOCK, HOLDLOCK)
		SET
			Timestamp = GETUTCDATE(),
			Address = @Address,
			Port = @Port,
			Generation = @Generation,
			HostName = @HostName,
			GatewayAddress = @GatewayAddress,
			GatewayPort = @GatewayPort,
			CpuUsage = @CpuUsage,
			MemoryUsage = @MemoryUsage,
			ActivationCount = @ActivationCount,
			RecentlyUsedActivationCount = @RecentlyUsedActivationCount,
			SendQueueLength = @SendQueueLength,
			ReceiveQueueLength = @ReceiveQueueLength,
			RequestQueueLength = @RequestQueueLength,
			SentMessages = @SentMessages,
			ReceivedMessages = @ReceivedMessages,
			IsOverloaded = @IsOverloaded,
			ClientCount = @ClientCount
		WHERE
			DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL
			AND SiloId = @SiloId AND @SiloId IS NOT NULL;

		INSERT INTO OrleansSiloMetricsTable
		(
			DeploymentId,
			SiloId,
			Address,
			Port,
			Generation,
			HostName,
			GatewayAddress,
			GatewayPort,
			CpuUsage,
			MemoryUsage,
			SendQueueLength,
			ReceiveQueueLength,
			SentMessages,
			ReceivedMessages,
			ActivationCount,
			RecentlyUsedActivationCount,
			RequestQueueLength,
			IsOverloaded,
			ClientCount
		)
		SELECT
			@DeploymentId,
			@SiloId,
			@Address,
			@Port,
			@Generation,
			@HostName,
			@GatewayAddress,
			@GatewayPort,
			@CpuUsage,
			@MemoryUsage,
			@SendQueueLength,
			@ReceiveQueueLength,
			@SentMessages,
			@ReceivedMessages,
			@ActivationCount,
			@RecentlyUsedActivationCount,
			@RequestQueueLength,
			@IsOverloaded,
			@ClientCount
		WHERE
			@@ROWCOUNT=0;
		COMMIT TRANSACTION;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'GatewaysQueryKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'GatewaysQueryKey','
		SELECT
			Address,
			ProxyPort,
			Generation
		FROM
			OrleansMembershipTable
		WHERE
			DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL
			AND Status = @Status AND @Status IS NOT NULL
			AND ProxyPort > 0;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'MembershipReadRowKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'MembershipReadRowKey','
		SELECT
			v.DeploymentId,
			m.Address,
			m.Port,
			m.Generation,
			m.SiloName,
			m.HostName,
			m.Status,
			m.ProxyPort,
			m.SuspectTimes,
			m.StartTime,
			m.IAmAliveTime,
			v.Version
		FROM
			OrleansMembershipVersionTable v
			-- This ensures the version table will returned even if there is no matching membership row.
			LEFT OUTER JOIN OrleansMembershipTable m ON v.DeploymentId = m.DeploymentId
			AND Address = @Address AND @Address IS NOT NULL
			AND Port = @Port AND @Port IS NOT NULL
			AND Generation = @Generation AND @Generation IS NOT NULL
		WHERE
			v.DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'MembershipReadAllKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'MembershipReadAllKey','
		SELECT
			v.DeploymentId,
			m.Address,
			m.Port,
			m.Generation,
			m.SiloName,
			m.HostName,
			m.Status,
			m.ProxyPort,
			m.SuspectTimes,
			m.StartTime,
			m.IAmAliveTime,
			v.Version
		FROM
			OrleansMembershipVersionTable v LEFT OUTER JOIN OrleansMembershipTable m
			ON v.DeploymentId = m.DeploymentId
		WHERE
			v.DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'DeleteMembershipTableEntriesKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'DeleteMembershipTableEntriesKey','
		DELETE FROM OrleansMembershipTable
		WHERE DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
		DELETE FROM OrleansMembershipVersionTable
		WHERE DeploymentId = @DeploymentId AND @DeploymentId IS NOT NULL;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'ReadReminderRowsKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'ReadReminderRowsKey','
		SELECT
			GrainId,
			ReminderName,
			StartTime,
			Period,
			Version
		FROM OrleansRemindersTable
		WHERE
			ServiceId = @ServiceId AND @ServiceId IS NOT NULL
			AND GrainId = @GrainId AND @GrainId IS NOT NULL;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'ReadReminderRowKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'ReadReminderRowKey','
		SELECT
			GrainId,
			ReminderName,
			StartTime,
			Period,
			Version
		FROM OrleansRemindersTable
		WHERE
			ServiceId = @ServiceId AND @ServiceId IS NOT NULL
			AND GrainId = @GrainId AND @GrainId IS NOT NULL
			AND ReminderName = @ReminderName AND @ReminderName IS NOT NULL;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'ReadRangeRows1Key')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'ReadRangeRows1Key','
		SELECT
			GrainId,
			ReminderName,
			StartTime,
			Period,
			Version
		FROM OrleansRemindersTable
		WHERE
			ServiceId = @ServiceId AND @ServiceId IS NOT NULL
			AND GrainHash > @BeginHash AND @BeginHash IS NOT NULL
			AND GrainHash <= @EndHash AND @EndHash IS NOT NULL;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'ReadRangeRows2Key')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'ReadRangeRows2Key','
		SELECT
			GrainId,
			ReminderName,
			StartTime,
			Period,
			Version
		FROM OrleansRemindersTable
		WHERE
			ServiceId = @ServiceId AND @ServiceId IS NOT NULL
			AND ((GrainHash > @BeginHash AND @BeginHash IS NOT NULL)
			OR (GrainHash <= @EndHash AND @EndHash IS NOT NULL));
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'InsertOrleansStatisticsKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'InsertOrleansStatisticsKey','
		BEGIN TRANSACTION;
		INSERT INTO OrleansStatisticsTable
		(
			DeploymentId,
			Id,
			HostName,
			Name,
			IsValueDelta,
			StatValue,
			Statistic
		)
		SELECT
			@DeploymentId,
			@Id,
			@HostName,
			@Name,
			@IsValueDelta,
			@StatValue,
			@Statistic;
		COMMIT TRANSACTION;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'DeleteReminderRowKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'DeleteReminderRowKey','
		DELETE FROM OrleansRemindersTable
		WHERE
			ServiceId = @ServiceId AND @ServiceId IS NOT NULL
			AND GrainId = @GrainId AND @GrainId IS NOT NULL
			AND ReminderName = @ReminderName AND @ReminderName IS NOT NULL
			AND Version = @Version AND @Version IS NOT NULL;
		SELECT @@ROWCOUNT;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'DeleteReminderRowsKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'DeleteReminderRowsKey','
		DELETE FROM OrleansRemindersTable
		WHERE
			ServiceId = @ServiceId AND @ServiceId IS NOT NULL;
	');
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'WriteToStorageKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'WriteToStorageKey',
		'-- When Orleans is running in normal, non-split state, there will
		-- be only one grain with the given ID and type combination only. This
		-- grain saves states mostly serially if Orleans guarantees are upheld. Even
		-- if not, the updates should work correctly due to version number.
		--
		-- In split brain situations there can be a situation where there are two or more
		-- grains with the given ID and type combination. When they try to INSERT
		-- concurrently, the table needs to be locked pessimistically before one of
		-- the grains gets @GrainStateVersion = 1 in return and the other grains will fail
		-- to update storage. The following arrangement is made to reduce locking in normal operation.
		--
		-- If the version number explicitly returned is still the same, Orleans interprets it so the update did not succeed
		-- and throws an InconsistentStateException.
		--
		-- See further information at http://dotnet.github.io/orleans/Getting-Started-With-Orleans/Grain-Persistence.
		BEGIN TRANSACTION;
		SET XACT_ABORT, NOCOUNT ON;

		DECLARE @NewGrainStateVersion AS INT = @GrainStateVersion;


		-- If the @GrainStateVersion is not zero, this branch assumes it exists in this database.
		-- The NULL value is supplied by Orleans when the state is new.
		IF @GrainStateVersion IS NOT NULL
		BEGIN
			UPDATE Storage
			SET
				PayloadBinary = @PayloadBinary,
				PayloadJson = @PayloadJson,
				PayloadXml = @PayloadXml,
				ModifiedOn = GETUTCDATE(),
				Version = Version + 1,
				@NewGrainStateVersion = Version + 1,
				@GrainStateVersion = Version + 1
			WHERE
				GrainIdHash = @GrainIdHash AND @GrainIdHash IS NOT NULL
				AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
				AND (GrainIdN0 = @GrainIdN0 OR @GrainIdN0 IS NULL)
				AND (GrainIdN1 = @GrainIdN1 OR @GrainIdN1 IS NULL)
				AND (GrainTypeString = @GrainTypeString OR @GrainTypeString IS NULL)
				AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
				AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
				AND Version IS NOT NULL AND Version = @GrainStateVersion AND @GrainStateVersion IS NOT NULL
				OPTION(FAST 1, OPTIMIZE FOR(@GrainIdHash UNKNOWN, @GrainTypeHash UNKNOWN));
		END

		-- The grain state has not been read. The following locks rather pessimistically
		-- to ensure only one INSERT succeeds.
		IF @GrainStateVersion IS NULL
		BEGIN
			INSERT INTO Storage
			(
				GrainIdHash,
				GrainIdN0,
				GrainIdN1,
				GrainTypeHash,
				GrainTypeString,
				GrainIdExtensionString,
				ServiceId,
				PayloadBinary,
				PayloadJson,
				PayloadXml,
				ModifiedOn,
				Version
			)
			SELECT
				@GrainIdHash,
				@GrainIdN0,
				@GrainIdN1,
				@GrainTypeHash,
				@GrainTypeString,
				@GrainIdExtensionString,
				@ServiceId,
				@PayloadBinary,
				@PayloadJson,
				@PayloadXml,
				GETUTCDATE(),
				1
			 WHERE NOT EXISTS
			 (
				-- There should not be any version of this grain state.
				SELECT 1
				FROM Storage WITH(XLOCK, ROWLOCK, HOLDLOCK, INDEX(IX_Storage))
				WHERE
					GrainIdHash = @GrainIdHash AND @GrainIdHash IS NOT NULL
					AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
					AND (GrainIdN0 = @GrainIdN0 OR @GrainIdN0 IS NULL)
					AND (GrainIdN1 = @GrainIdN1 OR @GrainIdN1 IS NULL)
					AND (GrainTypeString = @GrainTypeString OR @GrainTypeString IS NULL)
					AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
					AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
			 ) OPTION(FAST 1, OPTIMIZE FOR(@GrainIdHash UNKNOWN, @GrainTypeHash UNKNOWN));

			IF @@ROWCOUNT > 0
			BEGIN
				SET @NewGrainStateVersion = 1;
			END
		END

		SELECT @NewGrainStateVersion AS NewGrainStateVersion;
		COMMIT TRANSACTION;'
	);
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'ClearStorageKey')
BEGIN
INSERT INTO OrleansQuery(QueryKey, QueryText)
VALUES
(
	'ClearStorageKey',
	'BEGIN TRANSACTION;
    SET XACT_ABORT, NOCOUNT ON;
    DECLARE @NewGrainStateVersion AS INT = @GrainStateVersion;
    UPDATE Storage
    SET
	    PayloadBinary = NULL,
	    PayloadJson = NULL,
	    PayloadXml = NULL,
	    ModifiedOn = GETUTCDATE(),
	    Version = Version + 1,
        @NewGrainStateVersion = Version + 1
    WHERE
	    GrainIdHash = @GrainIdHash AND @GrainIdHash IS NOT NULL
        AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
        AND (GrainIdN0 = @GrainIdN0 OR @GrainIdN0 IS NULL)
		AND (GrainIdN1 = @GrainIdN1 OR @GrainIdN1 IS NULL)
        AND (GrainTypeString = @GrainTypeString OR @GrainTypeString IS NULL)
		AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
		AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
        AND Version IS NOT NULL AND Version = @GrainStateVersion AND @GrainStateVersion IS NOT NULL
        OPTION(FAST 1, OPTIMIZE FOR(@GrainIdHash UNKNOWN, @GrainTypeHash UNKNOWN));

    SELECT @NewGrainStateVersion;
    COMMIT TRANSACTION;'
);
END

IF NOT EXISTS (SELECT * FROM [OrleansQuery] WHERE [QueryKey] = 'ReadFromStorageKey')
BEGIN
	INSERT INTO OrleansQuery(QueryKey, QueryText)
	VALUES
	(
		'ReadFromStorageKey',
		'-- The application code will deserialize the relevant result. Not that the query optimizer
		-- estimates the result of rows based on its knowledge on the index. It does not know there
		-- will be only one row returned. Forcing the optimizer to process the first found row quickly
		-- creates an estimate for a one-row result and makes a difference on multi-million row tables.
		-- Also the optimizer is instructed to always use the same plan via index using the OPTIMIZE
		-- FOR UNKNOWN flags. These hints are only available in SQL Server 2008 and later. They
		-- should guarantee the execution time is robustly basically the same from query-to-query.
		SELECT
			PayloadBinary,
			PayloadXml,
			PayloadJson,
			Version
		FROM
			Storage
		WHERE
			GrainIdHash = @GrainIdHash AND @GrainIdHash IS NOT NULL
			AND GrainTypeHash = @GrainTypeHash AND @GrainTypeHash IS NOT NULL
			AND (GrainIdN0 = @GrainIdN0 OR @GrainIdN0 IS NULL)
			AND (GrainIdN1 = @GrainIdN1 OR @GrainIdN1 IS NULL)
			AND (GrainTypeString = @GrainTypeString OR @GrainTypeString IS NULL)
			AND ((@GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString IS NOT NULL AND GrainIdExtensionString = @GrainIdExtensionString) OR @GrainIdExtensionString IS NULL AND GrainIdExtensionString IS NULL)
			AND ServiceId = @ServiceId AND @ServiceId IS NOT NULL
			OPTION(FAST 1, OPTIMIZE FOR(@GrainIdHash UNKNOWN, @GrainTypeHash UNKNOWN));'
	);
END