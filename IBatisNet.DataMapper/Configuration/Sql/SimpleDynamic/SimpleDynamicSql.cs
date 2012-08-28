
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

using System.Collections;
using System.Text;

using IBatisNet.DataMapper.Configuration.Sql;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.DataExchange;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.Common.Utilities;
using IBatisNet.Common.Utilities.Objects;
#endregion


namespace IBatisNet.DataMapper.Configuration.Sql.SimpleDynamic
{
	/// <summary>
	/// Summary description for SimpleDynamicSql.
	/// </summary>
	internal sealed class SimpleDynamicSql : ISql
	{

		#region private

		private const string ELEMENT_TOKEN = "$";

		private string _simpleSqlStatement = string.Empty;
		private IStatement _statement = null ;
		private DataExchangeFactory _dataExchangeFactory = null;

		#endregion

		#region Constructor (s) / Destructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDynamicSql"/> class.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="sqlStatement">The SQL statement.</param>
        /// <param name="statement">The statement.</param>
		internal SimpleDynamicSql(IScope scope,
			string sqlStatement, 
			IStatement statement)
		{
			_simpleSqlStatement = sqlStatement;
			_statement = statement;

			_dataExchangeFactory = scope.DataExchangeFactory;
		}
		#endregion
		
		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		public string GetSql(object parameterObject)
		{
			return ProcessDynamicElements(parameterObject);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sqlStatement"></param>
		/// <returns></returns>
		public static bool IsSimpleDynamicSql(string sqlStatement) 
		{
			return ( (sqlStatement != null) && (sqlStatement.IndexOf(ELEMENT_TOKEN) > -1) );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		private string ProcessDynamicElements(object parameterObject) 
		{
			// define which character is seperating fields

			StringTokenizer parser = new StringTokenizer(_simpleSqlStatement, ELEMENT_TOKEN, true);

			StringBuilder newSql = new StringBuilder();

			string token = null;
			string lastToken = null;
			
			IEnumerator enumerator = parser.GetEnumerator();

			while (enumerator.MoveNext()) 
			{
				token = ((string)enumerator.Current);

				if (ELEMENT_TOKEN.Equals(lastToken)) 
				{
					if (ELEMENT_TOKEN.Equals(token)) 
					{
						newSql.Append(ELEMENT_TOKEN);
						token = null;
					} 
					else 
					{
						object value = null;
						if (parameterObject != null) 
						{
							if ( _dataExchangeFactory.TypeHandlerFactory.IsSimpleType( parameterObject.GetType() ) == true) 
							{
								value = parameterObject;
							} 
							else 
							{
                                value = ObjectProbe.GetMemberValue(parameterObject, token, _dataExchangeFactory.AccessorFactory);
							}
						}
						if (value != null) 
						{
							newSql.Append(value.ToString());
						}

						enumerator.MoveNext();
						token = ((string)enumerator.Current);

						if (!ELEMENT_TOKEN.Equals(token)) 
						{
							throw new DataMapperException("Unterminated dynamic element in sql (" + _simpleSqlStatement + ").");
						}
						token = null;
					}
				} 
				else 
				{
					if (!ELEMENT_TOKEN.Equals(token)) 
					{
						newSql.Append(token);
					}
				}

				lastToken = token;
			}

			return newSql.ToString();
		}


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
			string sqlStatement = ProcessDynamicElements(parameterObject);
			
			RequestScope request = new RequestScope( _dataExchangeFactory, session, _statement);

			request.PreparedStatement = BuildPreparedStatement(session, request, sqlStatement);
			request.MappedStatement = mappedStatement;

			return request;
		}

		/// <summary>
		/// Build the PreparedStatement
		/// </summary>
		/// <param name="session"></param>
		/// <param name="request"></param>
		/// <param name="sqlStatement"></param>
		private PreparedStatement BuildPreparedStatement(ISqlMapSession session, RequestScope request, string sqlStatement)
		{
			PreparedStatementFactory factory = new PreparedStatementFactory( session, request, _statement, sqlStatement);
			return factory.Prepare();
		}
		#endregion

		#endregion

	}
}
