
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 591621 $
 * $Date: 2007-11-03 07:44:57 -0600 (Sat, 03 Nov 2007) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
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

using System;
using System.Collections;
using System.Data;
using System.Reflection;
using IBatisNet.Common.Exceptions;

namespace IBatisNet.Common.Utilities
{
	/// <summary>
	/// DBHelperParameterCache provides functions to leverage a 
	/// static cache of procedure parameters, and the
	/// ability to discover parameters for stored procedures at run-time.
	/// </summary>
	public sealed class DBHelperParameterCache
	{
		private Hashtable _paramCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Initializes a new instance of the <see cref="DBHelperParameterCache"/> class.
        /// </summary>
        public DBHelperParameterCache() { }

		#region private methods

		/// <summary>
		/// Resolve at run time the appropriate set of Parameters for a stored procedure
		/// </summary>
		/// <param name="session">An IDalSession object</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">whether or not to include their return value parameter</param>
		/// <returns></returns>
		private IDataParameter[] DiscoverSpParameterSet(IDalSession session, string spName, bool includeReturnValueParameter)
		{
			return InternalDiscoverSpParameterSet(
                session,
                spName, 
                includeReturnValueParameter);	
		}


        /// <summary>
        /// Discover at run time the appropriate set of Parameters for a stored procedure
        /// </summary>
		/// <param name="session">An IDalSession object</param>
		/// <param name="spName">Name of the stored procedure.</param>
        /// <param name="includeReturnValueParameter">if set to <c>true</c> [include return value parameter].</param>
        /// <returns>The stored procedure parameters.</returns>
		private IDataParameter[] InternalDiscoverSpParameterSet(IDalSession session, string spName, 
            bool includeReturnValueParameter)
		{
#if !dotnet2
        	// SqlCommandBuilder.DeriveParameters(<command>) does not support transactions. 
        	// If the command is within a transaction, you will get the following error: 
			// “SqlCommandBuilder Execute requires the command to have a transaction object 
        	// when the connection assigned to the command is in a pending local transaction” 
        	// even when the command object does in fact have a transaction object. 
			using (IDbConnection connection = session.DataSource.DbProvider.CreateConnection())
			{
				connection.ConnectionString = session.DataSource.ConnectionString;
				connection.Open();
				using (IDbCommand cmd = connection.CreateCommand())
				{
				cmd.CommandType = CommandType.StoredProcedure;
#else
				using (IDbCommand cmd = session.CreateCommand(CommandType.StoredProcedure))
				{
#endif
					cmd.CommandText = spName;

				    // The session connection object is always created but the connection is not alwys open
				    // so we try to open it in case.
					session.OpenConnection();

					DeriveParameters(session.DataSource.DbProvider, cmd);

					if (cmd.Parameters.Count > 0) {
						IDataParameter firstParameter = (IDataParameter)cmd.Parameters[0];
						if (firstParameter.Direction == ParameterDirection.ReturnValue) {
							if (!includeReturnValueParameter) {
								cmd.Parameters.RemoveAt(0);
							}
						}	
					}


					IDataParameter[] discoveredParameters = new IDataParameter[cmd.Parameters.Count];
					cmd.Parameters.CopyTo(discoveredParameters, 0);
					return discoveredParameters;
				}
#if !dotnet2
			}
#endif
		}
		
		private void DeriveParameters(IDbProvider provider, IDbCommand command)
		{
			Type commandBuilderType;

			// Find the CommandBuilder
			if (provider == null)
				throw new ArgumentNullException("provider");
			if ((provider.CommandBuilderClass == null) || (provider.CommandBuilderClass.Length < 1))
				throw new Exception(String.Format(
					"CommandBuilderClass not defined for provider \"{0}\".",
					provider.Name));
			commandBuilderType = provider.CommandBuilderType;

			// Invoke the static DeriveParameter method on the CommandBuilder class
			// NOTE: OracleCommandBuilder has no DeriveParameter method
			try
			{
				commandBuilderType.InvokeMember("DeriveParameters",
				                                BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static, null, null,
					new object[] {command});
			}
			catch(Exception ex)
			{
				throw new IBatisNetException("Could not retrieve parameters for the store procedure named "+command.CommandText, ex);
			}
		}

		/// <summary>
		/// Deep copy of cached IDataParameter array.
		/// </summary>
		/// <param name="originalParameters"></param>
		/// <returns></returns>
		private IDataParameter[] CloneParameters(IDataParameter[] originalParameters)
		{
			IDataParameter[] clonedParameters = new IDataParameter[originalParameters.Length];

			int length = originalParameters.Length;
			for (int i = 0, j = length; i < j; i++)
			{
				clonedParameters[i] = (IDataParameter) ((ICloneable) originalParameters[i]).Clone();
			}

			return clonedParameters;
		}

		#endregion private methods, variables, and constructors

		#region caching functions

		/// <summary>
		/// Add parameter array to the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for an IDbConnection</param>
		/// <param name="commandText">the stored procedure name or SQL command</param>
		/// <param name="commandParameters">an array of IDataParameters to be cached</param>
		public void CacheParameterSet(string connectionString, string commandText, params IDataParameter[] commandParameters)
		{
			string hashKey = connectionString + ":" + commandText;

			_paramCache[hashKey] = commandParameters;
		}


		/// <summary>
		/// Clear the parameter cache.
		/// </summary>
		public void Clear()
		{
			_paramCache.Clear();
		}

		/// <summary>
		/// retrieve a parameter array from the cache
		/// </summary>
		/// <param name="connectionString">a valid connection string for an IDbConnection</param>
		/// <param name="commandText">the stored procedure name or SQL command</param>
		/// <returns>an array of IDataParameters</returns>
		public IDataParameter[] GetCachedParameterSet(string connectionString, string commandText)
		{
			string hashKey = connectionString + ":" + commandText;

			IDataParameter[] cachedParameters = (IDataParameter[]) _paramCache[hashKey];
			
			if (cachedParameters == null)
			{			
				return null;
			}
			else
			{
				return CloneParameters(cachedParameters);
			}
		}

		#endregion caching functions

		#region Parameter Discovery Functions

		/// <summary>
		/// Retrieves the set of IDataParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="session">a valid session</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <returns>an array of IDataParameters</returns>
		public IDataParameter[] GetSpParameterSet(IDalSession session, string spName)
		{
			return GetSpParameterSet(session, spName, false);
		}

		/// <summary>
		/// Retrieves the set of IDataParameters appropriate for the stored procedure
		/// </summary>
		/// <remarks>
		/// This method will query the database for this information, and then store it in a cache for future requests.
		/// </remarks>
		/// <param name="session">a valid session</param>
		/// <param name="spName">the name of the stored procedure</param>
		/// <param name="includeReturnValueParameter">a bool value indicating whether the return value parameter should be included in the results</param>
		/// <returns>an array of IDataParameters</returns>
		public IDataParameter[] GetSpParameterSet(IDalSession session, 
			string spName, bool includeReturnValueParameter)
		{
			string hashKey = session.DataSource.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter":"");

			IDataParameter[] cachedParameters;
			
			cachedParameters = (IDataParameter[]) _paramCache[hashKey];

			if (cachedParameters == null)
			{
				_paramCache[hashKey] = DiscoverSpParameterSet(session, spName, includeReturnValueParameter);
				cachedParameters = (IDataParameter[]) _paramCache[hashKey];
			}
			
			return CloneParameters(cachedParameters);
		}

		#endregion Parameter Discovery Functions
	}
}
