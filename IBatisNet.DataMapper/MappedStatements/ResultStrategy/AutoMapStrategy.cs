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

namespace IBatisNet.DataMapper.MappedStatements.ResultStrategy
{
	/// <summary>
	/// <see cref="IResultStrategy"/> implementation used when implicit 'ResultMap'.
	/// </summary>
    public sealed class AutoMapStrategy : IResultStrategy
    {
		/// <summary>
		/// Auto-map the reader to the result object.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="reader">The reader.</param>
		/// <param name="resultObject">The result object.</param>
        /// <returns>The AutoResultMap use to map the resultset.</returns>
        private AutoResultMap InitializeAutoResultMap(RequestScope request, ref IDataReader reader, ref object resultObject) 
		{
		    AutoResultMap resultMap  = request.CurrentResultMap as AutoResultMap;
		    
			if (request.Statement.AllowRemapping)
			{
                resultMap = resultMap.Clone();
			    
                ResultPropertyCollection properties = ReaderAutoMapper.Build(
                    request.DataExchangeFactory,
                    reader,
                    ref resultObject);

                resultMap.Properties.AddRange(properties);
			}
			else
			{
                if (!resultMap.IsInitalized)
				{
                    lock (resultMap) 
					{
                        if (!resultMap.IsInitalized)
						{
                            ResultPropertyCollection properties = ReaderAutoMapper.Build(
                               request.DataExchangeFactory,
                               reader,
                               ref resultObject);

                            resultMap.Properties.AddRange(properties);
                            resultMap.IsInitalized = true;
						}
					}
				}

			}
		    
		    return resultMap;
		}


        #region IResultStrategy Members

        /// <summary>
        /// Processes the specified <see cref="IDataReader"/> 
        /// a an auto result map is used.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="resultObject">The result object.</param>
        public object Process(RequestScope request, ref IDataReader reader, object resultObject)
        {
			object outObject = resultObject; 

			if (outObject == null) 
			{
                outObject = (request.CurrentResultMap as AutoResultMap).CreateInstanceOfResultClass();
			}

            AutoResultMap resultMap = InitializeAutoResultMap(request, ref reader, ref outObject);

            // En configuration initialiser des AutoResultMap (IResultMap) avec uniquement leur class name et class et les mettres
			// ds Statement.ResultsMap puis ds AutoMapStrategy faire comme AutoResultMap ds Java
			// tester si la request.CurrentResultMap [AutoResultMap (IResultMap)] est initialisée 
			// [if (allowRemapping || getResultMappings() == null) {initialize(rs);] java
			// si ( request.Statement.AllowRemapping || (request.CurrentResultMap as AutoResultMap).IsInitalized) ....

            for (int index = 0; index < resultMap.Properties.Count; index++)
            {
                ResultProperty property = resultMap.Properties[index];
                resultMap.SetValueOfProperty(ref outObject, property,
                                             property.GetDataBaseValue(reader));
            }
            
			return outObject;
        }

        #endregion
    }
}
