
To pass tests for MS Sql Server
------------------------------
1/ Create the database with the script 'scripts\(database name)\DBCreation.sql'

2/ In 'bin/IBatisNet.Test.dll.config' :
		set the database value to the name of database server : MSSQL,Oracle, Acces, MySql
		set the providerType key to a provider :
			- 'SqlClient' to run test via native .Net provider for Sql Server.
			- 'ByteFx' to run test via native .Net provider for MySql.
			- 'OracleClient' to run test via native .Net provider for Oracle.
			- 'Oledb' to run test via Oledb provider. (Access)
			- 'Odbc' to run test via Odbc provider.
3/ With the help of the DataBase-Template.config
   create a file named DataBase.config with your own value for datasource.
   (WARNInG : don't included it in the solution and don't commit it in SVN)
   Put it in the bin/debug directory
   
4/ To test for .NET V2, enabled provider 'sqlServer2.0' and 
   set <provider name="sqlServer2.0"/> in SqlMap_MSSQL_SqlClient.config
   
Remarks :
- TestSelectByPK fails for MSSQL with Oledb [it's normal]
- TestJIRA11 fails for MSSQL with Oledb/odbc [it's normal]	