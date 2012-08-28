#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-04-25 19:40:27 +0200 (mar., 25 avr. 2006) $
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

using System;
using System.Collections;
using System.Data;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.Commands;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Scope;

namespace IBatisNet.DataMapper.MappedStatements.ArgumentStrategy
{
	/// <summary>
	/// <see cref="IArgumentStrategy"/> implementation when a 'select' attribute exists
	/// on a <see cref="IList"/> <see cref="ArgumentProperty"/>
	/// </summary>
    public sealed class SelectListStrategy : IArgumentStrategy
	{
		#region IArgumentStrategy Members

		/// <summary>
		/// Gets the value of an argument constructor.
		/// </summary>
		/// <param name="request">The current <see cref="RequestScope"/>.</param>
		/// <param name="mapping">The <see cref="ResultProperty"/> with the argument infos.</param>
		/// <param name="reader">The current <see cref="IDataReader"/>.</param>
		/// <param name="keys">The keys</param>
		/// <returns>The paremeter value.</returns>
		public object GetValue(RequestScope request, ResultProperty mapping, 
		                       ref IDataReader reader, object keys)
		{
			// Get the select statement
			IMappedStatement selectStatement = request.MappedStatement.SqlMap.GetMappedStatement(mapping.Select);

			if (mapping.MemberType == typeof(IList))
			{
				reader = DataReaderTransformer.Transform(reader, request.Session.DataSource.DbProvider);
				return selectStatement.ExecuteQueryForList(request.Session, keys); 
			}
			else // Strongly typed List
			{
				reader = DataReaderTransformer.Transform(reader, request.Session.DataSource.DbProvider);
				IFactory factory = request.DataExchangeFactory.ObjectFactory.CreateFactory(mapping.MemberType, Type.EmptyTypes);
				object values = factory.CreateInstance(null);
				selectStatement.ExecuteQueryForList(request.Session, keys, (IList)values);
				return values;
			}
		}

		#endregion
	}
}
