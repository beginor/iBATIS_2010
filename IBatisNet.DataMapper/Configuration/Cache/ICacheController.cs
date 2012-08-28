
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 638571 $
 * $Date: 2008-03-18 15:11:57 -0600 (Tue, 18 Mar 2008) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
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

#region Imports
using System;
using System.Collections;
using System.Collections.Specialized;
#endregion

namespace IBatisNet.DataMapper.Configuration.Cache
{
	/// <summary>
	/// Summary description for ICacheController.
	/// </summary>
	public interface ICacheController
	{
		#region Properties
		/// <summary>
		/// Adds an item with the specified key and value into cached data.
		/// Gets a cached object with the specified key.
		/// </summary>
		/// <value>The cached object or <c>null</c></value>
		object this [object key] 
		{
			get;
			set;
		}
		#endregion

		#region Methods
	
		/// <summary>
		/// Remove an object from a cache model
		/// </summary>
		/// <param name="key">the key to the object</param>
		/// <returns>the removed object(?)</returns>
		object Remove(object key);

		/// <summary>
		/// Clears all elements from the cache.
		/// </summary>
		void Flush ();

		/// <summary>
		/// Configures the CacheController
		/// </summary>
		/// <param name="properties"></param>
		void Configure(IDictionary properties);
		#endregion

	}
}
