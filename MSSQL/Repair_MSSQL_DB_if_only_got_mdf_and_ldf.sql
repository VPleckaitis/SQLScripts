-- Adapted from https://www.mssqltips.com/sqlservertip/3579/how-to-attach-a-sql-server-database-without-a-transaction-log-and-with-open-transactions/

-- Essentially what needs to be done :
-- Grab database’s mdf & ldf files from remote machine
-- Create dummy database on your machine just so you can attach database

CREATE DATABASE [TestDB_Repair]
ON  PRIMARY 
( NAME = N'TestDB_Repair_file1', 
                          FILENAME = N'C:\[FILEPATH]\TestDB_Repair_1.mdf',
                          SIZE = 8MB , 
                          MAXSIZE = UNLIMITED, 
                          FILEGROWTH = 64MB) 
 LOG ON 
( NAME = N'TestDB_Repair_log_file1',
                          FILENAME = N'C:\[FILEPATH]\TestDB_Repair_1.ldf',
                          SIZE = 8MB,
                          MAXSIZE = 2048GB,
                          FILEGROWTH = 32MB)
GO

-- Set database offline
ALTER DATABASE [TestDB_Repair] SET OFFLINE WITH ROLLBACK IMMEDIATE
GO

-- Change file locations to where recovered files are
ALTER DATABASE [TestDB_Repair] MODIFY FILE(NAME='TestDB_Repair_file1', FILENAME= 'C:\ [FILEPATH]\correct_mdf_filename.mdf')
ALTER DATABASE [TestDB_Repair] MODIFY FILE(NAME='TestDB_Repair_log_file1', FILENAME= 'C:\ [FILEPATH]\correct_ldf_filename.ldf')
GO

--	Run recovery script (a.k.a. “MAGIC”).
DBCC TRACEON(3604)
GO

ALTER DATABASE TestDB_Repair SET EMERGENCY
GO

ALTER DATABASE TestDB_Repair SET SINGLE_USER
GO

DBCC CHECKDB('TestDB_Repair', REPAIR_ALLOW_DATA_LOSS) WITH ALL_ERRORMSGS -- SUPER DUPER IMPORTANT
GO

ALTER DATABASE TestDB_Repair SET MULTI_USER
GO

-- https://tenor.com/view/imagination-spongebob-squarepants-dreams-magic-gif-12725683

--	Done. Make sure you make backup before db dies again.
