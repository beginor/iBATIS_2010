#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 638571 $
 * $Date: 2008-03-18 15:11:57 -0600 (Tue, 18 Mar 2008) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006 - Apache Fondation
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

namespace IBatisNet.Common
{
	/// <summary>
	/// IDataSource
	/// </summary>
	public interface IDataSource
	{

		/// <summary>
		/// DataSource Name.
		/// </summary>
		string Name
		{
		    set;
		    get;
		}

		/// <summary>
		/// Connection string used to create connections.
		/// </summary>
		string ConnectionString
		{
		    set;
		    get;
		}

		/// <summary>
		/// The data provider.
		/// </summary>
		IDbProvider DbProvider
		{
			set;
		    get;
		}

/*
		/// <summary>
		/// Create a connection
		/// </summary>
		/// <returns>An IDbConnection</returns>
		IDbConnection CreateConnection();
		
		/// <summary>
		/// Create a command object
		/// </summary>
		/// <returns>An IdbCommand</returns>
		IDbCommand CreateCommand();
		IDbCommand GetCommand(string cmdText);
		IDbCommand GetCommand(string cmdText, IDbConnection connection);
		IDbCommand GetCommand(string cmdText, IDbConnection connection, IDbTransaction transaction);

		/// <summary>
		/// Create a parameter object
		/// </summary>
		/// <returns>An IDataParameter</returns>
		IDataParameter CreateParameter();
		// ou return IDbParameter ???

		IDataParameter GetParameter(string name, DbType dataType);
		IDataParameter GetParameter(string name, object value);
		IDataParameter GetParameter(string name, DbType dataType, int size);
		IDataParameter GetParameter(string name, DbType dataType, int size, string srcColumn);

		/// <summary>
		/// Create a DataAdapter object
		/// </summary>
		/// <returns>An IDataAdapter</returns>
		IDataAdapter CreateDataAdapter();
		IDataAdapter GetDataAdapter(IDbCommand selectCommand);
		IDataAdapter GetDataAdapter(string selectCommandText, string selectConnectionString);
		IDataAdapter GetDataAdapter(string selectCommandText, IDbConnection selectConnection);
		*/
	}
}
