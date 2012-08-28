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
#if dotnet2
using System.Collections.Generic;
#endif
using System.Data;
//using System.Reflection;
//using IBatisNet.Common;
//using IBatisNet.DataMapper.Commands;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.Scope;

namespace IBatisNet.DataMapper.MappedStatements.ArgumentStrategy
{
	/// <summary>
	/// <see cref="IArgumentStrategy"/> implementation when a 'select' attribute exists
	/// on a <see cref="ArgumentProperty"/>
	/// </summary>
	public class SelectStrategy : IArgumentStrategy
	{
        private IArgumentStrategy _selectStrategy = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectStrategy"/> class.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <param name="selectArrayStrategy">The select array strategy.</param>
        /// <param name="selectGenericListStrategy">The select generic list strategy.</param>
        /// <param name="selectListStrategy">The select list strategy.</param>
        /// <param name="selectObjectStrategy">The select object strategy.</param>
		public SelectStrategy(ResultProperty mapping,
            IArgumentStrategy selectArrayStrategy,
            IArgumentStrategy selectGenericListStrategy,
            IArgumentStrategy selectListStrategy,
            IArgumentStrategy selectObjectStrategy)
		{
			// Collection object or .NET object			
			if (mapping.MemberType.BaseType == typeof(Array))
			{
                _selectStrategy = selectArrayStrategy;
			}
#if dotnet2
            else if (mapping.MemberType.IsGenericType &&
                 typeof(IList<>).IsAssignableFrom(mapping.MemberType.GetGenericTypeDefinition())) 
            {
                _selectStrategy = selectGenericListStrategy;
            }
#endif				
            // Check if the object to Map implement 'IList' or is IList type
			// If yes the ResultProperty is map to a IList object
			else if ( typeof(IList).IsAssignableFrom(mapping.MemberType) )
			{
                _selectStrategy = selectListStrategy;
			}

			else // The ResultProperty is map to a .Net object
			{
                _selectStrategy = selectObjectStrategy;
			}
		}

		#region IArgumentStrategy Members

		/// <summary>
		/// Gets the value of an argument constructor.
		/// </summary>
		/// <param name="request">The current <see cref="RequestScope"/>.</param>
		/// <param name="mapping">The <see cref="ResultProperty"/> with the argument infos.</param>
		/// <param name="reader">The current <see cref="IDataReader"/>.</param>
		/// <param name="selectKeys">The keys</param>
		/// <returns>The paremeter value.</returns>
		public object GetValue(RequestScope request, ResultProperty mapping, 
		                       ref IDataReader reader, object selectKeys)
		{
			string paramString = mapping.ColumnName;
			object keys = null;
			bool wasNull = false;

			#region Finds the select keys.
			if (paramString.IndexOf(',')>0 || paramString.IndexOf('=')>0) // composite parameters key
			{
				IDictionary keyMap = new Hashtable();
				keys = keyMap;
				// define which character is seperating fields
				char[] splitter  = {'=',','};

				string[] paramTab = paramString.Split(splitter);
				if (paramTab.Length % 2 != 0) 
				{
					throw new DataMapperException("Invalid composite key string format in '"+mapping.PropertyName+". It must be: property1=column1,property2=column2,..."); 
				}
				IEnumerator enumerator = paramTab.GetEnumerator();
				while (!wasNull && enumerator.MoveNext()) 
				{
					string hashKey = ((string)enumerator.Current).Trim();
                    if (paramString.Contains("="))// old 1.x style multiple params
                    {
                        enumerator.MoveNext();
                    }
				    object hashValue = reader.GetValue( reader.GetOrdinal(((string)enumerator.Current).Trim()) );

					keyMap.Add(hashKey, hashValue );
					wasNull = (hashValue == DBNull.Value);
				}
			} 
			else // single parameter key
			{
				keys = reader.GetValue(reader.GetOrdinal(paramString));
				wasNull = reader.IsDBNull(reader.GetOrdinal(paramString));
			}
			#endregion

			if (wasNull) 
			{
				return null;
			} 
			else // Collection object or .Net object
			{
				// lazyLoading is not permit for argument constructor
				return _selectStrategy.GetValue(request, mapping, ref reader, keys);
			}	
		}

		#endregion
	}
}
