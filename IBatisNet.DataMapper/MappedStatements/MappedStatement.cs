
#region Apache Notice
/*****************************************************************************
 * $Revision: 575902 $
 * $LastChangedDate: 2007-09-15 04:40:19 -0600 (Sat, 15 Sep 2007) $
 * $LastChangedBy: gbayon $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006/2005 - The Apache Software Foundation
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

#region Using

using System;
using System.Collections;
#if dotnet2
using System.Collections.Generic;
#endif
using System.Data;
using System.Text;

using IBatisNet.Common.Utilities.Objects;


using IBatisNet.DataMapper.Commands;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.MappedStatements.ResultStrategy;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.MappedStatements.PostSelectStrategy;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.TypeHandlers;

#endregion

namespace IBatisNet.DataMapper.MappedStatements
{

    /// <summary>
    /// Summary description for MappedStatement.
    /// </summary>
    public class MappedStatement : IMappedStatement
    {
        /// <summary>
        /// Event launch on exceute query
        /// </summary>
        public event ExecuteEventHandler Execute;

        #region Fields

        // Magic number used to set the the maximum number of rows returned to 'all'. 
        internal const int NO_MAXIMUM_RESULTS = -1;
        // Magic number used to set the the number of rows skipped to 'none'. 
        internal const int NO_SKIPPED_RESULTS = -1;

        private IStatement _statement = null;
        private ISqlMapper _sqlMap = null;
        private IPreparedCommand _preparedCommand = null;
        private IResultStrategy _resultStrategy = null;
        #endregion

        #region Properties


        /// <summary>
        /// The IPreparedCommand to use
        /// </summary>
        public IPreparedCommand PreparedCommand
        {
            get { return _preparedCommand; }
        }

        /// <summary>
        /// Name used to identify the MappedStatement amongst the others.
        /// This the name of the SQL statement by default.
        /// </summary>
        public string Id
        {
            get { return _statement.Id; }
        }

        /// <summary>
        /// The SQL statment used by this MappedStatement
        /// </summary>
        public IStatement Statement
        {
            get { return _statement; }
        }

        /// <summary>
        /// The SqlMap used by this MappedStatement
        /// </summary>
        public ISqlMapper SqlMap
        {
            get { return _sqlMap; }
        }
        #endregion

        #region Constructor (s) / Destructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlMap">An SqlMap</param>
        /// <param name="statement">An SQL statement</param>
        internal MappedStatement(ISqlMapper sqlMap, IStatement statement)
        {
            _sqlMap = sqlMap;
            _statement = statement;
            _preparedCommand = PreparedCommandFactory.GetPreparedCommand(false);
            _resultStrategy = ResultStrategyFactory.Get(_statement);
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieve the output parameter and map them on the result object.
        /// This routine is only use is you specified a ParameterMap and some output attribute
        /// or if you use a store procedure with output parameter...
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session">The current session.</param>
        /// <param name="result">The result object.</param>
        /// <param name="command">The command sql.</param>
        private void RetrieveOutputParameters(RequestScope request, ISqlMapSession session, IDbCommand command, object result)
        {
            if (request.ParameterMap != null)
            {
                int count = request.ParameterMap.PropertiesList.Count;
                for (int i = 0; i < count; i++)
                {
                    ParameterProperty mapping = request.ParameterMap.GetProperty(i);
                    if (mapping.Direction == ParameterDirection.Output ||
                        mapping.Direction == ParameterDirection.InputOutput)
                    {
                        string parameterName = string.Empty;
                        if (session.DataSource.DbProvider.UseParameterPrefixInParameter == false)
                        {
                            parameterName = mapping.ColumnName;
                        }
                        else
                        {
                            parameterName = session.DataSource.DbProvider.ParameterPrefix +
                                mapping.ColumnName;
                        }

                        if (mapping.TypeHandler == null) // Find the TypeHandler
                        {
                            lock (mapping)
                            {
                                if (mapping.TypeHandler == null)
                                {
                                    Type propertyType = ObjectProbe.GetMemberTypeForGetter(result, mapping.PropertyName);

                                    mapping.TypeHandler = request.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(propertyType);
                                }
                            }
                        }

                        // Fix IBATISNET-239
                        //"Normalize" System.DBNull parameters
                        IDataParameter dataParameter = (IDataParameter)command.Parameters[parameterName];
                        object dbValue = dataParameter.Value;

                        object value = null;

                        bool wasNull = (dbValue == DBNull.Value);
                        if (wasNull)
                        {
                            if (mapping.HasNullValue)
                            {
                               value = mapping.TypeHandler.ValueOf(mapping.GetAccessor.MemberType, mapping.NullValue);
                            }
                            else
                            {
                                value = mapping.TypeHandler.NullValue;
                            }
                        }
                        else
                        {
                            value = mapping.TypeHandler.GetDataBaseValue(dataParameter.Value, result.GetType());
                        }

                        request.IsRowDataFound = request.IsRowDataFound || (value != null);

                        request.ParameterMap.SetOutputParameter(ref result, mapping, value);
                    }
                }
            }
        }


        #region ExecuteForObject

        /// <summary>
        /// Executes an SQL statement that returns a single row as an Object.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <returns>The object</returns>
        public virtual object ExecuteQueryForObject(ISqlMapSession session, object parameterObject)
        {
            return ExecuteQueryForObject(session, parameterObject, null);
        }


        /// <summary>
        /// Executes an SQL statement that returns a single row as an Object of the type of
        /// the resultObject passed in as a parameter.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">The result object.</param>
        /// <returns>The object</returns>
        public virtual object ExecuteQueryForObject(ISqlMapSession session, object parameterObject, object resultObject)
        {
            object obj = null;
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            obj = RunQueryForObject(request, session, parameterObject, resultObject);

            return obj;
        }


        /// <summary>
        /// Executes an SQL statement that returns a single row as an Object of the type of
        /// the resultObject passed in as a parameter.
        /// </summary>
        /// <param name="request">The request scope.</param>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">The result object.</param>
        /// <returns>The object</returns>
        internal object RunQueryForObject(RequestScope request, ISqlMapSession session, object parameterObject, object resultObject)
        {
            object result = resultObject;

            using (IDbCommand command = request.IDbCommand)
            {
                IDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        object obj = _resultStrategy.Process(request, ref reader, resultObject);
                        if (obj != BaseStrategy.SKIP)
                        {
                            result = obj;
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }

                ExecutePostSelect(request);

                #region remark
                // If you are using the OleDb data provider (as you are), you need to close the
                // DataReader before output parameters are visible.
                #endregion

                RetrieveOutputParameters(request, session, command, parameterObject);
            }

            RaiseExecuteEvent();

            return result;
        }

        #endregion

        #region ExecuteForObject .NET 2.0
#if dotnet2

        /// <summary>
        /// Executes an SQL statement that returns a single row as an Object.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <returns>The object</returns>
        public virtual T ExecuteQueryForObject<T>(ISqlMapSession session, object parameterObject)
        {
            return ExecuteQueryForObject<T>(session, parameterObject, default(T));
        }


        /// <summary>
        /// Executes an SQL statement that returns a single row as an Object of the type of
        /// the resultObject passed in as a parameter.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">The result object.</param>
        /// <returns>The object</returns>
        public virtual T ExecuteQueryForObject<T>(ISqlMapSession session, object parameterObject, T resultObject)
        {
            T obj = default(T);
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            obj = RunQueryForObject<T>(request, session, parameterObject, resultObject);

            return obj;
        }


        /// <summary>
        /// Executes an SQL statement that returns a single row as an Object of the type of
        /// the resultObject passed in as a parameter.
        /// </summary>
        /// <param name="request">The request scope.</param>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">The result object.</param>
        /// <returns>The object</returns>
        internal T RunQueryForObject<T>(RequestScope request, ISqlMapSession session, object parameterObject, T resultObject)
        {
            T result = resultObject;

            using (IDbCommand command = request.IDbCommand)
            {
                IDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        object obj = _resultStrategy.Process(request, ref reader, resultObject);
                        if (obj != BaseStrategy.SKIP)
                        {
                            result = (T)obj;
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }

                ExecutePostSelect(request);

                #region remark
                // If you are using the OleDb data provider, you need to close the
                // DataReader before output parameters are visible.
                #endregion

                RetrieveOutputParameters(request, session, command, parameterObject);
            }

            RaiseExecuteEvent();

            return result;
        }
#endif
        #endregion

        #region ExecuteQueryForList

        /// <summary>
        /// Runs a query with a custom object that gets a chance 
        /// to deal with each row as it is processed.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="rowDelegate"></param>
        public virtual IList ExecuteQueryForRowDelegate(ISqlMapSession session, object parameterObject, RowDelegate rowDelegate)
        {
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            if (rowDelegate == null)
            {
                throw new DataMapperException("A null RowDelegate was passed to QueryForRowDelegate.");
            }

            return RunQueryForList(request, session, parameterObject, null, rowDelegate);
        }

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
        ///<exception cref="DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
        public virtual IDictionary ExecuteQueryForMapWithRowDelegate(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty, DictionaryRowDelegate rowDelegate)
        {
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            if (rowDelegate == null)
            {
                throw new DataMapperException("A null DictionaryRowDelegate was passed to QueryForMapWithRowDelegate.");
            }

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            return RunQueryForMap(request, session, parameterObject, keyProperty, valueProperty, rowDelegate);
        }


        /// <summary>
        /// Executes the SQL and retuns all rows selected. This is exactly the same as
        /// calling ExecuteQueryForList(session, parameterObject, NO_SKIPPED_RESULTS, NO_MAXIMUM_RESULTS).
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <returns>A List of result objects.</returns>
        public virtual IList ExecuteQueryForList(ISqlMapSession session, object parameterObject)
        {
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            return RunQueryForList(request, session, parameterObject, null, null);
        }


        /// <summary>
        /// Executes the SQL and retuns a subset of the rows selected.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="skipResults">The number of rows to skip over.</param>
        /// <param name="maxResults">The maximum number of rows to return.</param>
        /// <returns>A List of result objects.</returns>
        public virtual IList ExecuteQueryForList(ISqlMapSession session, object parameterObject, int skipResults, int maxResults)
        {
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            return RunQueryForList(request, session, parameterObject, skipResults, maxResults);
        }

        /// <summary>
        /// Runs the query for list.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="skipResults">The number of rows to skip over.</param>
        /// <param name="maxResults">The maximum number of rows to return.</param>
        /// <returns>A List of result objects.</returns>
        internal IList RunQueryForList(RequestScope request, ISqlMapSession session, object parameterObject, int skipResults, int maxResults)
        {
            IList list = null;
            
            using (IDbCommand command = request.IDbCommand)
            {
                if (_statement.ListClass == null)
                {
                    list = new ArrayList();
                }
                else
                {
                    list = _statement.CreateInstanceOfListClass();
                }

                IDataReader reader = command.ExecuteReader();

                try
                {
                    // skip results
                    for (int i = 0; i < skipResults; i++)
                    {
                        if (!reader.Read())
                        {
                            break;
                        }
                    }

                    // Get Results
                    int resultsFetched = 0;
                    while ((maxResults == NO_MAXIMUM_RESULTS || resultsFetched < maxResults)
                        && reader.Read())
                    {
                        object obj = _resultStrategy.Process(request, ref reader, null);
                        if (obj != BaseStrategy.SKIP)
                        {
                            list.Add(obj);
                        }
                        resultsFetched++;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }

                ExecutePostSelect(request);

                RetrieveOutputParameters(request, session, command, parameterObject);
            }

            return list;
        }

        /// <summary>
        /// Executes the SQL and retuns a List of result objects.
        /// </summary>
        /// <param name="request">The request scope.</param>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">A strongly typed collection of result objects.</param>
        /// <param name="rowDelegate"></param>
        /// <returns>A List of result objects.</returns>
        internal IList RunQueryForList(RequestScope request, ISqlMapSession session, object parameterObject, IList resultObject, RowDelegate rowDelegate)
        {
            IList list = resultObject;

            using (IDbCommand command = request.IDbCommand)
            {
                if (resultObject==null)
                {
                    if (_statement.ListClass == null)
                    {
                        list = new ArrayList();
                    }
                    else
                    {
                        list = _statement.CreateInstanceOfListClass();
                    }
                }

                IDataReader reader = command.ExecuteReader();

                try
                { 
                    do
                    {
                        if (rowDelegate == null)
                        {
                            while (reader.Read())
                            {
                                object obj = _resultStrategy.Process(request, ref reader, null);
                                if (obj != BaseStrategy.SKIP)
                                {
                                    list.Add(obj);
                                }
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                object obj = _resultStrategy.Process(request, ref reader, null);
                                rowDelegate(obj, parameterObject, list);
                            }
                        }
                    }
                    while (reader.NextResult());
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }

                ExecutePostSelect(request);
                RetrieveOutputParameters(request, session, command, parameterObject);
            }

            return list;
        }


        /// <summary>
        /// Executes the SQL and and fill a strongly typed collection.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">A strongly typed collection of result objects.</param>
        public virtual void ExecuteQueryForList(ISqlMapSession session, object parameterObject, IList resultObject)
        {
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            RunQueryForList(request, session, parameterObject, resultObject, null);
        }


        #endregion

        #region ExecuteQueryForList .NET 2.0
#if dotnet2

        /// <summary>
        /// Runs a query with a custom object that gets a chance 
        /// to deal with each row as it is processed.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="rowDelegate"></param>
        public virtual IList<T> ExecuteQueryForRowDelegate<T>(ISqlMapSession session, object parameterObject, RowDelegate<T> rowDelegate)
        {
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            if (rowDelegate == null)
            {
                throw new DataMapperException("A null RowDelegate was passed to QueryForRowDelegate.");
            }
            return RunQueryForList<T>(request, session, parameterObject, null, rowDelegate);
        }


        /// <summary>
        /// Executes the SQL and retuns all rows selected. This is exactly the same as
        /// calling ExecuteQueryForList(session, parameterObject, NO_SKIPPED_RESULTS, NO_MAXIMUM_RESULTS).
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <returns>A List of result objects.</returns>
        public virtual IList<T> ExecuteQueryForList<T>(ISqlMapSession session, object parameterObject)
        {
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            return RunQueryForList<T>(request, session, parameterObject, null, null);
        }


        /// <summary>
        /// Executes the SQL and retuns a subset of the rows selected.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="skipResults">The number of rows to skip over.</param>
        /// <param name="maxResults">The maximum number of rows to return.</param>
        /// <returns>A List of result objects.</returns>
        public virtual IList<T> ExecuteQueryForList<T>(ISqlMapSession session, object parameterObject, int skipResults, int maxResults)
        {
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            return RunQueryForList<T>(request, session, parameterObject, skipResults, maxResults);
        }


        /// <summary>
        /// Executes the SQL and retuns a List of result objects.
        /// </summary>
        /// <param name="request">The request scope.</param>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="skipResults">The number of rows to skip over.</param>
        /// <param name="maxResults">The maximum number of rows to return.</param>
        /// <returns>A List of result objects.</returns>
        internal IList<T> RunQueryForList<T>(RequestScope request, ISqlMapSession session, object parameterObject, int skipResults, int maxResults)
        {
            IList<T> list = null;

            using (IDbCommand command = request.IDbCommand)
            {
                if (_statement.ListClass == null)
                {
                    list = new List<T>();
                }
                else
                {
                    list = _statement.CreateInstanceOfGenericListClass<T>();
                }

                IDataReader reader = command.ExecuteReader();
                try
                {
                    // skip results
                    for (int i = 0; i < skipResults; i++)
                    {
                        if (!reader.Read())
                        {
                            break;
                        }
                    }

                    int resultsFetched = 0;
                    while ((maxResults == NO_MAXIMUM_RESULTS || resultsFetched < maxResults)
                        && reader.Read())
                    {
                        object obj = _resultStrategy.Process(request, ref reader, null);
                        if (obj != BaseStrategy.SKIP)
                        {
                            list.Add((T)obj);
                        }
                        resultsFetched++;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }

                ExecutePostSelect(request);

                RetrieveOutputParameters(request, session, command, parameterObject);
            }

            return list;
        }

        /// <summary>
        /// Executes the SQL and retuns a List of result objects.
        /// </summary>
        /// <param name="request">The request scope.</param>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">The result object</param>
        /// <param name="rowDelegate"></param>
        /// <returns>A List of result objects.</returns>
        internal IList<T> RunQueryForList<T>(RequestScope request, ISqlMapSession session, 
                                             object parameterObject, IList<T> resultObject, RowDelegate<T> rowDelegate)
        {
            IList<T> list = resultObject;

            using (IDbCommand command = request.IDbCommand)
            {
                if (resultObject == null)
                {
                    if (_statement.ListClass == null)
                    {
                        list = new List<T>();
                    }
                    else
                    {
                        list = _statement.CreateInstanceOfGenericListClass<T>();
                    }
                }

                IDataReader reader = command.ExecuteReader();
                try
                {
                    do
                    {
                        if (rowDelegate == null)
                        {
                            while (reader.Read())
                            {
                                object obj = _resultStrategy.Process(request, ref reader, null);
                                if (obj != BaseStrategy.SKIP)
                                {
                                    list.Add((T)obj);
                                }
                            }
                        }
                        else
                        {
                            while (reader.Read())
                            {
                                T obj = (T)_resultStrategy.Process(request, ref reader, null);
                                rowDelegate(obj, parameterObject, list);
                            }
                        }
                    }
                    while (reader.NextResult());
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }

                ExecutePostSelect(request);
                RetrieveOutputParameters(request, session, command, parameterObject);
            }

            return list;
        }

        /// <summary>
        /// Executes the SQL and and fill a strongly typed collection.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="resultObject">A strongly typed collection of result objects.</param>
        public virtual void ExecuteQueryForList<T>(ISqlMapSession session, object parameterObject, IList<T> resultObject)
        {
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            RunQueryForList<T>(request, session, parameterObject, resultObject, null);
        }

#endif
        #endregion

        #region ExecuteUpdate, ExecuteInsert

        /// <summary>
        /// Execute an update statement. Also used for delete statement.
        /// Return the number of row effected.
        /// </summary>
        /// <param name="session">The session used to execute the statement.</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <returns>The number of row effected.</returns>
        public virtual int ExecuteUpdate(ISqlMapSession session, object parameterObject)
        {
            int rows = 0; // the number of rows affected
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            using (IDbCommand command = request.IDbCommand)
            {
                rows = command.ExecuteNonQuery();

                //ExecutePostSelect(request);

                RetrieveOutputParameters(request, session, command, parameterObject);
            }

            RaiseExecuteEvent();

            return rows;
        }


        /// <summary>
        /// Execute an insert statement. Fill the parameter object with 
        /// the ouput parameters if any, also could return the insert generated key
        /// </summary>
        /// <param name="session">The session</param>
        /// <param name="parameterObject">The parameter object used to fill the statement.</param>
        /// <returns>Can return the insert generated key.</returns>
        public virtual object ExecuteInsert(ISqlMapSession session, object parameterObject)
        {
            object generatedKey = null;
            SelectKey selectKeyStatement = null;
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            if (_statement is Insert)
            {
                selectKeyStatement = ((Insert)_statement).SelectKey;
            }

            if (selectKeyStatement != null && !selectKeyStatement.isAfter)
            {
                IMappedStatement mappedStatement = _sqlMap.GetMappedStatement(selectKeyStatement.Id);
                generatedKey = mappedStatement.ExecuteQueryForObject(session, parameterObject);

                ObjectProbe.SetMemberValue(parameterObject, selectKeyStatement.PropertyName, generatedKey,
                    request.DataExchangeFactory.ObjectFactory,
                    request.DataExchangeFactory.AccessorFactory);
            }

            _preparedCommand.Create(request, session, this.Statement, parameterObject);
            using (IDbCommand command = request.IDbCommand)
            {
                if (_statement is Insert)
                {
                    command.ExecuteNonQuery();
                }
                // Retrieve output parameter if the result class is specified
                else if (_statement is Procedure && (_statement.ResultClass != null) &&
                        _sqlMap.TypeHandlerFactory.IsSimpleType(_statement.ResultClass))
                {
                    IDataParameter returnValueParameter = command.CreateParameter();
                    returnValueParameter.Direction = ParameterDirection.ReturnValue;
                    command.Parameters.Add(returnValueParameter);

                    command.ExecuteNonQuery();
                    generatedKey = returnValueParameter.Value;

                    ITypeHandler typeHandler = _sqlMap.TypeHandlerFactory.GetTypeHandler(_statement.ResultClass);
                    generatedKey = typeHandler.GetDataBaseValue(generatedKey, _statement.ResultClass);
                }
                else
                {
                    generatedKey = command.ExecuteScalar();
                    if ((_statement.ResultClass != null) &&
                        _sqlMap.TypeHandlerFactory.IsSimpleType(_statement.ResultClass))
                    {
                        ITypeHandler typeHandler = _sqlMap.TypeHandlerFactory.GetTypeHandler(_statement.ResultClass);
                        generatedKey = typeHandler.GetDataBaseValue(generatedKey, _statement.ResultClass);
                    }
                }

                if (selectKeyStatement != null && selectKeyStatement.isAfter)
                {
                    IMappedStatement mappedStatement = _sqlMap.GetMappedStatement(selectKeyStatement.Id);
                    generatedKey = mappedStatement.ExecuteQueryForObject(session, parameterObject);

                    ObjectProbe.SetMemberValue(parameterObject, selectKeyStatement.PropertyName, generatedKey,
                        request.DataExchangeFactory.ObjectFactory,
                        request.DataExchangeFactory.AccessorFactory);
                }

                //ExecutePostSelect(request);

                RetrieveOutputParameters(request, session, command, parameterObject);
            }

            RaiseExecuteEvent();

            return generatedKey;
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
        ///<exception cref="DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
        public virtual IDictionary ExecuteQueryForMap(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty)
        {
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            return RunQueryForMap(request, session, parameterObject, keyProperty, valueProperty, null);
        }

       
        /// <summary>
        /// Executes the SQL and retuns all rows selected in a map that is keyed on the property named
        /// in the keyProperty parameter.  The value at each key will be the value of the property specified
        /// in the valueProperty parameter.  If valueProperty is null, the entire result object will be entered.
        /// </summary>
        /// <param name="request">The request scope.</param>
        /// <param name="session">The session used to execute the statement</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="keyProperty">The property of the result object to be used as the key.</param>
        /// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
        /// <param name="rowDelegate">A delegate called once per row in the QueryForMapWithRowDelegate method</param>
        /// <returns>A hashtable of object containing the rows keyed by keyProperty.</returns>
        ///<exception cref="DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
        internal IDictionary RunQueryForMap(RequestScope request,
            ISqlMapSession session,
            object parameterObject,
            string keyProperty,
            string valueProperty,
            DictionaryRowDelegate rowDelegate)
        {
            IDictionary map = new Hashtable();

            using (IDbCommand command = request.IDbCommand)
            {
               IDataReader reader = command.ExecuteReader();
               try
                {
                    
                   if (rowDelegate == null)
                    {
                        while (reader.Read())
                        {
                            object obj = _resultStrategy.Process(request, ref reader, null);
                            object key = ObjectProbe.GetMemberValue(obj, keyProperty, request.DataExchangeFactory.AccessorFactory);
                            object value = obj;
                            if (valueProperty != null)
                            {
                                value = ObjectProbe.GetMemberValue(obj, valueProperty, request.DataExchangeFactory.AccessorFactory);
                            }
                            map.Add(key, value);
                        }
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            object obj = _resultStrategy.Process(request, ref reader, null);
                            object key = ObjectProbe.GetMemberValue(obj, keyProperty, request.DataExchangeFactory.AccessorFactory);
                            object value = obj;
                            if (valueProperty != null)
                            {
                                value = ObjectProbe.GetMemberValue(obj, valueProperty, request.DataExchangeFactory.AccessorFactory);
                            }
                            rowDelegate(key, value, parameterObject, map);

                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }
                ExecutePostSelect(request);
            }
            return map;

        }

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
        public virtual IDictionary<K, V> ExecuteQueryForDictionary<K, V>(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty)
        {
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            return RunQueryForDictionary<K, V>(request, session, parameterObject, keyProperty, valueProperty, null);

        }

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
        ///<exception cref="DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
        public virtual IDictionary<K, V> ExecuteQueryForDictionary<K, V>(ISqlMapSession session, object parameterObject, string keyProperty, string valueProperty, DictionaryRowDelegate<K, V> rowDelegate)
        {
            RequestScope request = _statement.Sql.GetRequestScope(this, parameterObject, session);

            if (rowDelegate == null)
            {
                throw new DataMapperException("A null DictionaryRowDelegate was passed to QueryForDictionary.");
            }

            _preparedCommand.Create(request, session, this.Statement, parameterObject);

            return RunQueryForDictionary<K, V>(request, session, parameterObject, keyProperty, valueProperty, rowDelegate);
        }

        /// <summary>
        /// Executes the SQL and retuns all rows selected in a map that is keyed on the property named
        /// in the keyProperty parameter.  The value at each key will be the value of the property specified
        /// in the valueProperty parameter.  If valueProperty is null, the entire result object will be entered.
        /// </summary>
        /// <param name="request">The request scope.</param>
        /// <param name="session">The session used to execute the statement</param>
        /// <param name="parameterObject">The object used to set the parameters in the SQL.</param>
        /// <param name="keyProperty">The property of the result object to be used as the key.</param>
        /// <param name="valueProperty">The property of the result object to be used as the value (or null)</param>
        /// <param name="rowDelegate">A delegate called once per row in the QueryForMapWithRowDelegate method</param>
        /// <returns>A IDictionary of object containing the rows keyed by keyProperty.</returns>
        ///<exception cref="DataMapperException">If a transaction is not in progress, or the database throws an exception.</exception>
        internal IDictionary<K, V> RunQueryForDictionary<K, V>(RequestScope request,
            ISqlMapSession session,
            object parameterObject,
            string keyProperty,
            string valueProperty,
            DictionaryRowDelegate<K, V> rowDelegate)
        {
            IDictionary<K, V> map = new Dictionary<K, V>();

            using (IDbCommand command = request.IDbCommand)
            {
                IDataReader reader = command.ExecuteReader();
                try
                {

                    if (rowDelegate == null)
                    {
                        while (reader.Read())
                        {
                            object obj = _resultStrategy.Process(request, ref reader, null);
                            K key = (K)ObjectProbe.GetMemberValue(obj, keyProperty, request.DataExchangeFactory.AccessorFactory);
                            V value = default(V);
                            if (valueProperty != null)
                            {
                                value = (V)ObjectProbe.GetMemberValue(obj, valueProperty, request.DataExchangeFactory.AccessorFactory);
                            }
                            else
                            {
                                value = (V)obj;
                            }
                            map.Add(key, value);
                        }
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            object obj = _resultStrategy.Process(request, ref reader, null);
                            K key = (K)ObjectProbe.GetMemberValue(obj, keyProperty, request.DataExchangeFactory.AccessorFactory);
                            V value = default(V);
                            if (valueProperty != null)
                            {
                                value = (V)ObjectProbe.GetMemberValue(obj, valueProperty, request.DataExchangeFactory.AccessorFactory);
                            }
                            else
                            {
                                value = (V)obj;
                            }
                            rowDelegate(key, value, parameterObject, map);
                        }
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    reader.Close();
                    reader.Dispose();
                }
                ExecutePostSelect(request);
            }
            return map;

        }
#endif
        
        #endregion


        /// <summary>
        /// Executes the <see cref="PostBindind"/>.
        /// </summary>
        /// <param name="request">The current <see cref="RequestScope"/>.</param>
        private void ExecutePostSelect(RequestScope request)
        {
            while (request.QueueSelect.Count > 0)
            {
                PostBindind postSelect = request.QueueSelect.Dequeue() as PostBindind;

                PostSelectStrategyFactory.Get(postSelect.Method).Execute(postSelect, request);
            }
        }


        /// <summary>
        /// Raise an event ExecuteEventArgs
        /// (Used when a query is executed)
        /// </summary>
        private void RaiseExecuteEvent()
        {
            ExecuteEventArgs e = new ExecuteEventArgs();
            e.StatementName = _statement.Id;
            if (Execute != null)
            {
                Execute(this, e);
            }
        }

        /// <summary>
        /// ToString implementation.
        /// </summary>
        /// <returns>A string that describes the MappedStatement</returns>
        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("\tMappedStatement: " + this.Id);
            buffer.Append(Environment.NewLine);
            if (_statement.ParameterMap != null) buffer.Append(_statement.ParameterMap.Id);

            return buffer.ToString();
        }


        #endregion

    }
}
