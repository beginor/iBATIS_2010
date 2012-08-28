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

using System.Data;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Scope;

namespace IBatisNet.DataMapper.MappedStatements.ArgumentStrategy
{
	/// <summary>
	/// <see cref="IArgumentStrategy"/> implementation when a 'resultMapping' attribute exists
	/// on a <see cref="ArgumentProperty"/>.
	/// </summary>
	public sealed class ResultMapStrategy : BaseStrategy, IArgumentStrategy
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
			object[] parameters = null;
			bool isParameterFound = false;

		    IResultMap resultMapping = mapping.NestedResultMap.ResolveSubMap(reader);

            if (resultMapping.Parameters.Count > 0)
			{
                parameters = new object[resultMapping.Parameters.Count];
				// Fill parameters array
                for (int index = 0; index < resultMapping.Parameters.Count; index++)
				{
                    ResultProperty property = resultMapping.Parameters[index];
                    parameters[index] = property.ArgumentStrategy.GetValue(request, property, ref reader, null);
					request.IsRowDataFound = request.IsRowDataFound || (parameters[index] != null);
					isParameterFound = isParameterFound || (parameters[index] != null);
				}
			}

			object obj = null;
			// If I have a constructor tag and all argumments values are null, the obj is null
            if (resultMapping.Parameters.Count > 0 && isParameterFound == false)
			{
				obj = null;
			}
			else
			{
                obj = resultMapping.CreateInstanceOfResult(parameters);
                if (FillObjectWithReaderAndResultMap(request, reader, resultMapping, ref obj) == false)
				{
					obj = null;
				}
			}

			return obj;
		}

		#endregion
	}
}
