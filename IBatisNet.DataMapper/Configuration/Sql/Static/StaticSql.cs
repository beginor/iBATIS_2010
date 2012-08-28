
#region Apache Notice
/*****************************************************************************
 * $Revision: 476843 $
 * $LastChangedDate: 2006-11-19 09:07:45 -0700 (Sun, 19 Nov 2006) $
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

#region Imports

using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.DataExchange;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;

#endregion

namespace IBatisNet.DataMapper.Configuration.Sql.Static
{
	/// <summary>
	/// Summary description for StaticSql.
	/// </summary>
	public sealed class StaticSql : ISql
	{

		#region Fields

		private IStatement _statement = null ;
		private PreparedStatement _preparedStatement = null ;
		private DataExchangeFactory _dataExchangeFactory = null;

		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="statement">The statement.</param>
		/// <param name="scope"></param>
		public StaticSql(IScope scope, IStatement statement)
		{
			_statement = statement;

			_dataExchangeFactory = scope.DataExchangeFactory;
		}
		#endregion

		#region ISql Members

		/// <summary>
		/// Builds a new <see cref="RequestScope"/> and the sql command text to execute.
		/// </summary>
		/// <param name="parameterObject">The parameter object (used in DynamicSql)</param>
		/// <param name="session">The current session</param>
		/// <param name="mappedStatement">The <see cref="IMappedStatement"/>.</param>
		/// <returns>A new <see cref="RequestScope"/>.</returns>
		public RequestScope GetRequestScope(IMappedStatement mappedStatement,
			object parameterObject, ISqlMapSession session)
		{
			RequestScope request = new RequestScope(_dataExchangeFactory, session, _statement);

			request.PreparedStatement = _preparedStatement;
			request.MappedStatement = mappedStatement;

			return request;
		}

		/// <summary>
		/// Build the PreparedStatement
		/// </summary>
		/// <param name="session"></param>
		/// <param name="sqlStatement"></param>
		public void BuildPreparedStatement(ISqlMapSession session, string sqlStatement)
		{
			RequestScope request = new RequestScope( _dataExchangeFactory, session, _statement);

			PreparedStatementFactory factory = new PreparedStatementFactory( session, request, _statement, sqlStatement);
			_preparedStatement = factory.Prepare();
		}

		#endregion
	}
}
