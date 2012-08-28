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


using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.Configuration.Statements;

namespace IBatisNet.DataMapper.MappedStatements
{
	/// <summary>
	/// Summary description for SelectMappedStatement.
	/// </summary>
    public sealed class SelectMappedStatement : MappedStatement
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sqlMap">An SqlMap</param>
		/// <param name="statement">An SQL statement</param>
        internal SelectMappedStatement(ISqlMapper sqlMap, IStatement statement)
            : base(sqlMap, statement)
		{ }


		#region ExecuteInsert

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		public override object ExecuteInsert(ISqlMapSession session, object parameterObject )
		{
			throw new DataMapperException("Update statements cannot be executed as a query insert.");
		}

		#endregion

		#region ExecuteUpdate

		/// <summary>
		/// 
		/// </summary>
		/// <param name="session"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
        public override int ExecuteUpdate(ISqlMapSession session, object parameterObject)
		{
			throw new DataMapperException("Insert statements cannot be executed as a update query.");
		}

		#endregion
	}
}
