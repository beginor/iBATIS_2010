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
using IBatisNet.DataMapper.MappedStatements.PropertyStrategy;
using IBatisNet.DataMapper.Scope;

namespace IBatisNet.DataMapper.MappedStatements.ResultStrategy
{
	/// <summary>
	/// <see cref="IResultStrategy"/> implementation when 
	/// a 'resultClass' attribute is specified and
	/// the type of the result object is primitive.
	/// </summary>
    public sealed class SimpleTypeStrategy : IResultStrategy
    {
        #region IResultStrategy Members

        /// <summary>
        /// Processes the specified <see cref="IDataReader"/> 
        /// when a ResultClass is specified on the statement and
        /// the ResultClass is a SimpleType.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="resultObject">The result object.</param>
        public object Process(RequestScope request, ref IDataReader reader, object resultObject)
        {
            object outObject = resultObject;
            AutoResultMap resultMap = request.CurrentResultMap as AutoResultMap;
            
            if (outObject == null) 
            {
                outObject = resultMap.CreateInstanceOfResultClass();
            }

            if (!resultMap.IsInitalized)
            { 
                lock(resultMap)
                {
                    if (!resultMap.IsInitalized)
                    {
                        // Create a ResultProperty
                        ResultProperty property = new ResultProperty();
                        property.PropertyName = "value";
                        property.ColumnIndex = 0;
                        property.TypeHandler = request.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(outObject.GetType());
                        property.PropertyStrategy = PropertyStrategyFactory.Get(property);

                        resultMap.Properties.Add(property);
                        resultMap.DataExchange = request.DataExchangeFactory.GetDataExchangeForClass(typeof(int));// set the PrimitiveDataExchange
                        resultMap.IsInitalized = true;
                    }
                }
            }

            resultMap.Properties[0].PropertyStrategy.Set(request, resultMap, resultMap.Properties[0], ref outObject, reader, null); 
      
			return outObject;
		}

        #endregion
    }
}
