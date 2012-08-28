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

namespace IBatisNet.DataMapper.MappedStatements.PropertyStrategy
{
	/// <summary>
	/// <see cref="IPropertyStrategy"/> implementation when a 'resultMapping' attribute exists
	/// on a <see cref="ResultProperty"/>.
	/// </summary>
    public sealed class ResultMapStrategy : BaseStrategy, IPropertyStrategy
	{
		#region IPropertyStrategy Members

		/// <summary>
		/// Sets value of the specified <see cref="ResultProperty"/> on the target object
		/// when a 'resultMapping' attribute exists
		/// on the <see cref="ResultProperty"/>.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="resultMap">The result map.</param>
		/// <param name="mapping">The ResultProperty.</param>
		/// <param name="target">The target.</param>
		/// <param name="reader">The reader.</param>
		/// <param name="keys">The keys</param>
		public void Set(RequestScope request, IResultMap resultMap, 
			ResultProperty mapping, ref object target, IDataReader reader, object keys)
		{
            object obj = Get(request, resultMap, mapping, ref target, reader);
			// Sets created object on the property
			resultMap.SetValueOfProperty( ref target, mapping, obj );		
		}

	    /// <summary>
        /// Gets the value of the specified <see cref="ResultProperty"/> that must be set on the target object.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="resultMap">The result map.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="reader">The reader.</param>
		/// <param name="target">The target object</param>
		public object Get(RequestScope request, IResultMap resultMap, ResultProperty mapping, ref object target, IDataReader reader)
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
                    ResultProperty resultProperty = resultMapping.Parameters[index];
                    parameters[index] = resultProperty.ArgumentStrategy.GetValue(request, resultProperty, ref reader, null);
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

                // Fills properties on the new object
                if (this.FillObjectWithReaderAndResultMap(request, reader, resultMapping, ref obj) == false)
                {
                    obj = null;
                }
            }
	        
	        return obj;
        }
		#endregion
	}
}
