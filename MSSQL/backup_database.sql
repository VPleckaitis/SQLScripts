DECLARE @name VARCHAR(50) = 'DatabaseName'
DECLARE @path VARCHAR(256) = 'C:\Backups\' -- path to store backup files
DECLARE @fileName VARCHAR(256) --  backup filename 
DECLARE @fileDate VARCHAR(20) -- used for file name

-- specify date format that we will append to db name to generate backup file name
SELECT @fileDate = CONVERT(VARCHAR(20),GETDATE(),112) --yyyyMMdd
                                 
SET @fileName = @path + @name + '_' + @fileDate + '.BAK'
BACKUP DATABASE @name TO DISK = @fileName
