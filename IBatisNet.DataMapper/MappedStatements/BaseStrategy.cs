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
using System.Text;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Scope;

namespace IBatisNet.DataMapper.MappedStatements
{
	/// <summary>
	/// BaseStrategy.
	/// </summary>
	public abstract class BaseStrategy
	{
 	    /// <summary>
	    /// Used by N+1 Select solution
	    /// </summary>
        public static object SKIP = new object();
	    
        private const string KEY_SEPARATOR = "\002";

    
	    /// <summary>
        /// Calculte a unique key which identify the resukt object build by this <see cref="IResultMap"/>
	    /// </summary>
	    /// <param name="resultMap"></param>
	    /// <param name="request"></param>
	    /// <param name="reader"></param>
	    /// <returns></returns>
        protected string GetUniqueKey(IResultMap resultMap, RequestScope request, IDataReader reader)
        {
            if (resultMap.GroupByProperties.Count > 0)
            {
                StringBuilder keyBuffer = new StringBuilder();

                for (int i = 0; i < resultMap.GroupByProperties.Count; i++)
                {
                    ResultProperty resultProperty = resultMap.GroupByProperties[i];
                    keyBuffer.Append(resultProperty.GetDataBaseValue(reader));
                    keyBuffer.Append('-');
                }

                if (keyBuffer.Length < 1)
                {
                    // we should never go here
                    return null;
                }
                else
                {
                    // separator value not likely to appear in a database
                    keyBuffer.Append(KEY_SEPARATOR);
                    return keyBuffer.ToString();
                }
            }
            else
            {
                // we should never go here
                return null;
            }
        }
	    
		/// <summary>
		/// Fills the object with reader and result map.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="reader">The reader.</param>
		/// <param name="resultMap">The result map.</param>
		/// <param name="resultObject">The result object.</param>
		/// <returns>Indicates if we have found a row.</returns>
		protected bool FillObjectWithReaderAndResultMap(RequestScope request,IDataReader reader,
                                                        IResultMap resultMap, ref object resultObject)
		{
			bool dataFound = false;

            if (resultMap.Properties.Count>0)
            {
 			    // For each Property in the ResultMap, set the property in the object 
			    for(int index=0; index< resultMap.Properties.Count; index++)
			    {
				    request.IsRowDataFound = false;
				    ResultProperty property = resultMap.Properties[index];
				    property.PropertyStrategy.Set(request, resultMap, property, ref resultObject, reader, null);
				    dataFound = dataFound || request.IsRowDataFound;
			    }

			    request.IsRowDataFound = dataFound;
			    return dataFound;
		    }
		    else
            {
                return true;
            }
        }

	}
}
