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

using System.Collections;
using System.Data;
using IBatisNet.DataMapper.Scope;

namespace IBatisNet.DataMapper.MappedStatements.ResultStrategy
{
	/// <summary>
	/// <see cref="IResultStrategy"/> implementation when 
	/// a 'resultClass' attribute is specified.
	/// </summary>
    public sealed class ResultClassStrategy : IResultStrategy
	{
		private static IResultStrategy _simpleTypeStrategy = null;
		private static IResultStrategy _dictionaryStrategy = null;
		private static IResultStrategy _listStrategy = null;
		private static IResultStrategy _autoMapStrategy = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="ResultClassStrategy"/> class.
		/// </summary>
		public ResultClassStrategy()
		{
			_simpleTypeStrategy = new SimpleTypeStrategy();
			_dictionaryStrategy = new DictionaryStrategy();
			_listStrategy = new ListStrategy();
			_autoMapStrategy = new AutoMapStrategy();
		}

        #region IResultStrategy Members

		/// <summary>
		/// Processes the specified <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="reader">The reader.</param>
		/// <param name="resultObject">The result object.</param>
        public object Process(RequestScope request, ref IDataReader reader, object resultObject)
		{

  			// Check if the ResultClass is a 'primitive' Type
            if (request.DataExchangeFactory.TypeHandlerFactory.IsSimpleType(request.CurrentResultMap.Class))
			{
                return _simpleTypeStrategy.Process(request, ref reader, resultObject);
			}
            else if (typeof(IDictionary).IsAssignableFrom(request.CurrentResultMap.Class)) 
			{
                return _dictionaryStrategy.Process(request, ref reader, resultObject);
			}
            else if (typeof(IList).IsAssignableFrom(request.CurrentResultMap.Class)) 
			{
                return _listStrategy.Process(request, ref reader, resultObject);
			}
			else
			{
                return _autoMapStrategy.Process(request, ref reader, resultObject);
			}
		}

		#endregion
	}
}
