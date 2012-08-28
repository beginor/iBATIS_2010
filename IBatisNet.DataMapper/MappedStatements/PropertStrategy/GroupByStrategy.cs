#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2007-09-15 08:29:07 -0600 (Sat, 15 Sep 2007) $
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
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.MappedStatements.PropertyStrategy;
using IBatisNet.DataMapper.Scope;

namespace IBatisNet.DataMapper.MappedStatements.PropertStrategy
{
    /// <summary>
    /// <see cref="IPropertyStrategy"/> implementation when a 'resultMapping' attribute exists
    /// on a <see cref="ResultProperty"/> ant the resulMap have a groupBy attribute.
    /// </summary>
    public sealed class GroupByStrategy : BaseStrategy, IPropertyStrategy
    {
        private static IPropertyStrategy _resultMapStrategy = null;
		
        /// <summary>
		/// Initializes the <see cref="PropertyStrategyFactory"/> class.
		/// </summary>
        static GroupByStrategy()
		{
		    _resultMapStrategy = new ResultMapStrategy();
		}
        
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
            Get(request, resultMap, mapping, ref target, reader);
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
            // The property is a IList
            IList list = null;
            
            // Get the IList property
            object property = ObjectProbe.GetMemberValue(target, mapping.PropertyName,
                                       request.DataExchangeFactory.AccessorFactory);
            
            if (property == null)// Create the list if need
            {
                property = mapping.ListFactory.CreateInstance(null);
                mapping.SetAccessor.Set(target, property);
            }
            list = (IList)property;

            object result = null;
            IResultMap propertyRresultMap = mapping.NestedResultMap.ResolveSubMap(reader);

            if (propertyRresultMap.GroupByProperties.Count > 0)
            {
                 string uniqueKey = GetUniqueKey(propertyRresultMap, request, reader);

                // Gets the [key, result object] already build
                IDictionary buildObjects = request.GetUniqueKeys(propertyRresultMap);

                if (buildObjects != null && buildObjects.Contains(uniqueKey))
                {
                    // Unique key is already known, so get the existing result object and process additional results.
                    result = buildObjects[uniqueKey];

                    //In some cases (nested groupings) our object may be null, so there is
                    //no point in going on
                    if (result != null)
                    {
                        // process resulMapping attribute which point to a groupBy attribute
                        for (int index = 0; index < propertyRresultMap.Properties.Count; index++)
                        {
                            ResultProperty resultProperty = propertyRresultMap.Properties[index];
                            if (resultProperty.PropertyStrategy is PropertStrategy.GroupByStrategy)
                            {
                                resultProperty.PropertyStrategy.Set(request, propertyRresultMap, resultProperty, ref result, reader, null);
                            }
                        }
                    }
                    result = SKIP;
                }
                else if (uniqueKey == null || buildObjects == null || !buildObjects.Contains(uniqueKey))
                {
                    // Unique key is NOT known, so create a new result object and then process additional results.
                    result = _resultMapStrategy.Get(request, resultMap, mapping, ref target, reader);

                    if (buildObjects == null)
                    {
                        buildObjects = new Hashtable();
                        request.SetUniqueKeys(propertyRresultMap, buildObjects);
                    }
                    buildObjects[uniqueKey] = result;                       
                }               
            }
            else // Last resultMap have no groupBy attribute
            {
                result = _resultMapStrategy.Get(request, resultMap, mapping, ref target, reader);
            }                

            
            if ((result != null) && (result != SKIP))
            {
                list.Add(result);
            }

            return result;
        }
        #endregion
    }
}
