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
	/// <see cref="IPropertyStrategy"/> contract to set value object on <see cref="ResultProperty"/>.
	/// </summary>
	public interface IPropertyStrategy
	{
		/// <summary>
		/// Sets value of the specified <see cref="ResultProperty"/> on the target object.
		/// </summary>
		/// <param name="request">The request.</param>
		/// <param name="resultMap">The result map.</param>
		/// <param name="mapping">The ResultProperty.</param>
		/// <param name="target">The target.</param>
		/// <param name="reader">The reader.</param>
		/// <param name="keys">The keys</param>
		void Set(RequestScope request, IResultMap resultMap, 
		         ResultProperty mapping, ref object target, 
		         IDataReader reader, object keys);


        /// <summary>
        /// Gets the value of the specified <see cref="ResultProperty"/> that must be set on the target object.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="resultMap">The result map.</param>
        /// <param name="mapping">The mapping.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="target">The target.</param>
        object Get(RequestScope request, IResultMap resultMap, ResultProperty mapping, ref object target, IDataReader reader);
	}
}
