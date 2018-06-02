USE [DemoConfiguration]
GO

INSERT INTO Config.State ([Name]) VALUES ('UNKNOWN')
INSERT INTO Config.State ([Name]) VALUES ('STARTING')
INSERT INTO Config.State ([Name]) VALUES ('RUNNING')
INSERT INTO Config.State ([Name]) VALUES ('STOPPING')
INSERT INTO Config.State ([Name]) VALUES ('STOPPED')
