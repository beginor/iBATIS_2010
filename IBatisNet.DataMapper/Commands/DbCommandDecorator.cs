#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 383115 $
 * $Date: 2006-11-15 13:22:00 -0700 (Wed, 15 Nov 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2005 - Gilles Bayon
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
using System.Data;
using IBatisNet.DataMapper.Scope;

namespace IBatisNet.DataMapper.Commands
{
    /// <summary>
    /// Decorate an <see cref="System.Data.IDbCommand"></see>
    /// to auto move to next ResultMap on ExecuteReader call. 
    /// </summary>
    public class DbCommandDecorator : IDbCommand
    {
        private IDbCommand _innerDbCommand = null;
        private RequestScope _request = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbCommandDecorator"/> class.
        /// </summary>
        /// <param name="dbCommand">The db command.</param>
        /// <param name="request">The request scope</param>
        public DbCommandDecorator(IDbCommand dbCommand, RequestScope request)
        {
            _request = request;
            _innerDbCommand = dbCommand;
        }


        #region IDbCommand Members

        /// <summary>
        /// Attempts to cancels the execution of an <see cref="System.Data.IDbCommand"></see>.
        /// </summary>
        void IDbCommand.Cancel()
        {
            _innerDbCommand.Cancel();
        }

        /// <summary>
        /// Gets or sets the text command to run against the data source.
        /// </summary>
        /// <value></value>
        /// <returns>The text command to execute. The default value is an empty string ("").</returns>
        string IDbCommand.CommandText
        {
            get { return _innerDbCommand.CommandText; }
            set {  _innerDbCommand.CommandText = value; }
        }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        /// <value></value>
        /// <returns>The time (in seconds) to wait for the command to execute. The default value is 30 seconds.</returns>
        /// <exception cref="System.ArgumentException">The property value assigned is less than 0. </exception>
        int IDbCommand.CommandTimeout
        {
            get { return _innerDbCommand.CommandTimeout; }
            set { _innerDbCommand.CommandTimeout = value; }
        }

        /// <summary>
        /// Indicates or specifies how the <see cref="P:System.Data.IDbCommand.CommandText"></see> property is interpreted.
        /// </summary>
        /// <value></value>
        /// <returns>One of the <see cref="System.Data.CommandType"></see> values. The default is Text.</returns>
        CommandType IDbCommand.CommandType
        {
            get { return _innerDbCommand.CommandType; }
            set { _innerDbCommand.CommandType = value; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Data.IDbConnection"></see> used by this instance of the <see cref="System.Data.IDbCommand"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The connection to the data source.</returns>
        IDbConnection IDbCommand.Connection
        {
            get { return _innerDbCommand.Connection; }
            set { _innerDbCommand.Connection = value; }
        }

        /// <summary>
        /// Creates a new instance of an <see cref="System.Data.IDbDataParameter"></see> object.
        /// </summary>
        /// <returns>An IDbDataParameter object.</returns>
        IDbDataParameter IDbCommand.CreateParameter()
        {
            return _innerDbCommand.CreateParameter();
        }

        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        /// <exception cref="T:System.InvalidOperationException">The connection does not exist.-or- The connection is not open. </exception>
        int IDbCommand.ExecuteNonQuery()
        {
            _request.Session.OpenConnection();
            return _innerDbCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes the <see cref="P:System.Data.IDbCommand.CommandText"></see> against the <see cref="P:System.Data.IDbCommand.Connection"></see>, and builds an <see cref="System.Data.IDataReader"></see> using one of the <see cref="System.Data.CommandBehavior"></see> values.
        /// </summary>
        /// <param name="behavior">One of the <see cref="System.Data.CommandBehavior"></see> values.</param>
        /// <returns>
        /// An <see cref="System.Data.IDataReader"></see> object.
        /// </returns>
        IDataReader IDbCommand.ExecuteReader(CommandBehavior behavior)
        {
            _request.Session.OpenConnection();
            return _innerDbCommand.ExecuteReader(behavior);
        }

        /// <summary>
        /// Executes the <see cref="P:System.Data.IDbCommand.CommandText"></see> against the <see cref="P:System.Data.IDbCommand.Connection"></see> and builds an <see cref="System.Data.IDataReader"></see>.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Data.IDataReader"></see> object.
        /// </returns>
        IDataReader IDbCommand.ExecuteReader()
        {
            _request.Session.OpenConnection();
            _request.MoveNextResultMap();
            return new DataReaderDecorator(_innerDbCommand.ExecuteReader(), _request);
            
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        /// <returns>
        /// The first column of the first row in the resultset.
        /// </returns>
        object IDbCommand.ExecuteScalar()
        {
            _request.Session.OpenConnection();
            return _innerDbCommand.ExecuteScalar();
        }

        /// <summary>
        /// Gets the <see cref="System.Data.IDataParameterCollection"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The parameters of the SQL statement or stored procedure.</returns>
        IDataParameterCollection IDbCommand.Parameters
        {
            get { return _innerDbCommand.Parameters; }
        }

        /// <summary>
        /// Creates a prepared (or compiled) version of the command on the data source.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The <see cref="P:System.Data.OleDb.OleDbCommand.Connection"></see> is not set.-or- The <see cref="System.Data.OleDb.OleDbCommand.Connection"></see> is not <see cref="System.Data.OleDb.OleDbConnection.Open"></see>. </exception>
        void IDbCommand.Prepare()
        {
            _innerDbCommand.Prepare();
        }

        /// <summary>
        /// Gets or sets the transaction within which the Command object of a .NET Framework data provider executes.
        /// </summary>
        /// <value></value>
        /// <returns>the Command object of a .NET Framework data provider executes. The default value is null.</returns>
        IDbTransaction IDbCommand.Transaction
        {
            get { return _innerDbCommand.Transaction; }
            set { _innerDbCommand.Transaction = value; }
        }

        /// <summary>
        /// Gets or sets how command results are applied to the <see cref="System.Data.DataRow"></see> when used by the <see cref="M:System.Data.IDataAdapter.Update(System.Data.DataSet)"></see> method of a <see cref="System.Data.Common.DbDataAdapter"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>One of the <see cref="System.Data.UpdateRowSource"></see> values. The default is Both unless the command is automatically generated. Then the default is None.</returns>
        /// <exception cref="System.ArgumentException">The value entered was not one of the <see cref="System.Data.UpdateRowSource"></see> values. </exception>
        UpdateRowSource IDbCommand.UpdatedRowSource
        {
            get { return _innerDbCommand.UpdatedRowSource; }
            set { _innerDbCommand.UpdatedRowSource = value; }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
           _innerDbCommand.Dispose();
        }

        #endregion
    }
}
