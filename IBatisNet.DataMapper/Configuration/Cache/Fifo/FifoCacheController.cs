
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

#endregion

namespace IBatisNet.DataMapper.Configuration.Cache.Fifo
{
	/// <summary>
	/// Summary description for FifoCacheController.
	/// </summary>
	public class FifoCacheController : ICacheController
	{
		#region Fields 
		private int _cacheSize = 0;
		private Hashtable _cache = null;
		private IList _keyList = null;
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// 
		/// </summary>
		public FifoCacheController() 
		{
			_cacheSize = 100;
			_cache = Hashtable.Synchronized( new Hashtable() );
			_keyList = ArrayList.Synchronized( new ArrayList() );
		}
		#endregion

		#region ICacheController Members

		/// <summary>
		/// Remove an object from a cache model
		/// </summary>
		/// <param name="key">the key to the object</param>
		/// <returns>the removed object(?)</returns>
		public object Remove(object key)
		{
			object o = this[key];

			_keyList.Remove(key);
			_cache.Remove(key);
			return o;
		}

		/// <summary>
		/// Clears all elements from the cache.
		/// </summary>
		public void Flush()
		{
				_cache.Clear();
				_keyList.Clear();
		}


		/// <summary>
		/// Adds an item with the specified key and value into cached data.
		/// Gets a cached object with the specified key.
		/// </summary>
		/// <value>The cached object or <c>null</c></value>
		public object this [object key] 
		{
			get
			{
				return _cache[key];
			}
			set
			{
				_cache[key] = value;
				_keyList.Add(key);
				if (_keyList.Count > _cacheSize) 
				{
					object oldestKey = _keyList[0];
					_keyList.Remove(0);
					_cache.Remove(oldestKey);
				}		
			}
		}


		/// <summary>
		/// Configures the cache
		/// </summary>
		public void Configure(IDictionary properties)
		{
			string size = (string)properties["CacheSize"];;
			if (size != null) 
			{
				_cacheSize = Convert.ToInt32(size);		
			}
		}
		
		#endregion

	}
}
