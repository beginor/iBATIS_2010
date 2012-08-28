
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 476843 $
 * $Date: 2006-11-19 09:07:45 -0700 (Sun, 19 Nov 2006) $
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

#region Imports
using System.Collections;
#if dotnet2
using System.Collections.Generic;
#endif
using IBatisNet.DataMapper.Commands;
using IBatisNet.DataMapper.Configuration.Statements;
#endregion

namespace IBatisNet.DataMapper.MappedStatements
{
	/// <summary>
	/// 
	/// </summary>
	public delegate void ExecuteEventHandler(object sender, ExecuteEventArgs e);

	/// <summary>
	/// Summary description for IMappedStatement.
	/// </summary>
	public interface IMappedStatement
	{

		#region Event

		/// <summary>
		/// Event launch on exceute query
		/// </summary>
		event ExecuteEventHandler Execute;

		#endregion 

		#region Properties


		/// <summary>
		/// The IPreparedCommand to use
		/// </summary>
		IPreparedCommand PreparedCommand
		{
			get;
		}

		/// <summary>
		/// Name used to identify the MappedStatement amongst the others.
		/// This the name of the SQL statment by default.
		/// </summary>
		string Id
		{
			get;
		}

		/// <summary>
		/// The SQL statment used by this MappedStatement
		/// </summary>
		IStatement Statement
		{
			get;
		}

		
		/// <summary>
		/// The SqlMap used by this MappedStatement
		/// </summary>
        ISqlMapper SqlMap
		{
			get;
		}
		#endregion

		#region ExecuteQueryForMap

		/// <summary>
		/// Executes the SQL and retuns all rows selected in a map that is keyed on the property named
		/// in the keyProperty parameter.  The value at each key will be the value of the property specified
		/// in the valueProperty parameter.  If valueProperty is null, the entire result object will be entered.
		/// </summary>
		/// <param name="session">The session used to execute the statement</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL. </param>
		/// <param name="keyProperty">The property of the result object to be used as the key. </param>
		/// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
		/// <returns>A hashtable of object containing the rows keyed by keyProperty.</returns>
		///<exception cref="IBatisNet.DataMapper.Exceptions.DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
		IDictionary ExecuteQueryForMap( ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty );

		#endregion

        #region ExecuteQueryForMap .NET 2.0
#if dotnet2
        /// <summary>
        /// Executes the SQL and retuns all rows selected in a map that is keyed on the property named
        /// in the keyProperty parameter.  The value at each key will be the value of the property specified
        /// in the valueProperty parameter.  If valueProperty is null, the entire result object will be entered.
        /// </summary>
        /// <param name="session">The session used to execute the statement</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL. </param>
        /// <param name="keyProperty">The property of the result object to be used as the key. </param>
        /// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
        /// <returns>A IDictionary of object containing the rows keyed by keyProperty.</returns>
        ///<exception cref="IBatisNet.DataMapper.Exceptions.DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
        IDictionary<K, V> ExecuteQueryForDictionary<K, V>(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty);

        /// <summary>
        /// Runs a query with a custom object that gets a chance 
        /// to deal with each row as it is processed.
        /// </summary>
        /// <param name="session">The session used to execute the statement</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL. </param>
        /// <param name="keyProperty">The property of the result object to be used as the key. </param>
        /// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
        /// <param name="rowDelegate">A delegate called once per row in the QueryForDictionary method</param>
        /// <returns>A hashtable of object containing the rows keyed by keyProperty.</returns>
        /// <exception cref="IBatisNet.DataMapper.Exceptions.DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
        IDictionary<K, V> ExecuteQueryForDictionary<K, V>(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty, DictionaryRowDelegate<K, V> rowDelegate);


#endif
        #endregion

		#region ExecuteUpdate

		/// <summary>
		/// Execute an update statement. Also used for delete statement.
		/// Return the number of row effected.
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <returns>The number of row effected.</returns>
		int ExecuteUpdate(ISqlMapSession session, object parameterObject );

		#endregion

		#region ExecuteInsert

		/// <summary>
		/// Execute an insert statement. Fill the parameter object with 
		/// the ouput parameters if any, also could return the insert generated key
		/// </summary>
		/// <param name="session">The session</param>
		/// <param name="parameterObject">The parameter object used to fill the statement.</param>
		/// <returns>Can return the insert generated key.</returns>
		object ExecuteInsert(ISqlMapSession session, object parameterObject );

		#endregion

        #region ExecuteQueryForList

		/// <summary>
		/// Executes the SQL and and fill a strongly typed collection.
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="resultObject">A strongly typed collection of result objects.</param>
		void ExecuteQueryForList(ISqlMapSession session, object parameterObject, IList resultObject );

		/// <summary>
		/// Executes the SQL and retuns a subset of the rows selected.
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="skipResults">The number of rows to skip over.</param>
		/// <param name="maxResults">The maximum number of rows to return.</param>
		/// <returns>A List of result objects.</returns>
		IList ExecuteQueryForList( ISqlMapSession session, object parameterObject, int skipResults, int maxResults );

		/// <summary>
		/// Executes the SQL and retuns all rows selected. This is exactly the same as
		/// calling ExecuteQueryForList(session, parameterObject, NO_SKIPPED_RESULTS, NO_MAXIMUM_RESULTS).
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <returns>A List of result objects.</returns>
		IList ExecuteQueryForList( ISqlMapSession session, object parameterObject );

		#endregion

        #region ExecuteQueryForList .NET 2.0
#if dotnet2
        /// <summary>
        /// Executes the SQL and and fill a strongly typed collection.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">A strongly typed collection of result objects.</param>
        void ExecuteQueryForList<T>(ISqlMapSession session, object parameterObject, IList<T> resultObject);

        /// <summary>
        /// Executes the SQL and retuns a subset of the rows selected.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="skipResults">The number of rows to skip over.</param>
        /// <param name="maxResults">The maximum number of rows to return.</param>
        /// <returns>A List of result objects.</returns>
        IList<T> ExecuteQueryForList<T>(ISqlMapSession session, object parameterObject, int skipResults, int maxResults);

        /// <summary>
        /// Executes the SQL and retuns all rows selected. This is exactly the same as
        /// calling ExecuteQueryForList(session, parameterObject, NO_SKIPPED_RESULTS, NO_MAXIMUM_RESULTS).
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <returns>A List of result objects.</returns>
        IList<T> ExecuteQueryForList<T>(ISqlMapSession session, object parameterObject);
#endif
        #endregion

		#region ExecuteForObject

		/// <summary>
		/// Executes an SQL statement that returns a single row as an Object.
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <returns>The object</returns>
		object ExecuteQueryForObject( ISqlMapSession session, object parameterObject );

		/// <summary>
		/// Executes an SQL statement that returns a single row as an Object of the type of
		/// the resultObject passed in as a parameter.
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="resultObject">The result object.</param>
		/// <returns>The object</returns>
		object ExecuteQueryForObject( ISqlMapSession session, object parameterObject, object resultObject );

		#endregion

        #region ExecuteForObject .NET 2.0
#if dotnet2

        /// <summary>
        /// Executes an SQL statement that returns a single row as an Object.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <returns>The object</returns>
        T ExecuteQueryForObject<T>(ISqlMapSession session, object parameterObject);

        /// <summary>
        /// Executes an SQL statement that returns a single row as an Object of the type of
        /// the resultObject passed in as a parameter.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">The result object.</param>
        /// <returns>The object</returns>
        T ExecuteQueryForObject<T>(ISqlMapSession session, object parameterObject, T resultObject);
#endif
        #endregion

		#region Delegate

		/// <summary>
		/// Runs a query with a custom object that gets a chance 
		/// to deal with each row as it is processed.
		/// </summary>
		/// <param name="session">The session used to execute the statement.</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
		/// <param name="rowDelegate"></param>param>
		/// <returns></returns>
		IList ExecuteQueryForRowDelegate( ISqlMapSession session, object parameterObject, RowDelegate rowDelegate );

 
		/// <summary>
		/// Runs a query with a custom object that gets a chance 
		/// to deal with each row as it is processed.
		/// </summary>
		/// <param name="session">The session used to execute the statement</param>
		/// <param name="parameterObject">The object used to set the parameters in the SQL. </param>
		/// <param name="keyProperty">The property of the result object to be used as the key. </param>
		/// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
		/// <param name="rowDelegate"></param>
		/// <returns>A hashtable of object containing the rows keyed by keyProperty.</returns>
		/// <exception cref="IBatisNet.DataMapper.Exceptions.DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
		IDictionary ExecuteQueryForMapWithRowDelegate( ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty, DictionaryRowDelegate rowDelegate );

		#endregion 
		
        #region ExecuteQueryForRowDelegate .NET 2.0
#if dotnet2
        /// <summary>
        /// Runs a query with a custom object that gets a chance 
        /// to deal with each row as it is processed.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="rowDelegate"></param>param>
        /// <returns></returns>
        IList<T> ExecuteQueryForRowDelegate<T>(ISqlMapSession session, object parameterObject, RowDelegate<T> rowDelegate);
#endif
        #endregion
	    
	    
    }
}
