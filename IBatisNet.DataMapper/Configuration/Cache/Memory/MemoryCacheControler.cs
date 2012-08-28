
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

namespace IBatisNet.DataMapper.Configuration.Cache.Memory
{
	/// <summary>
	/// Summary description for MemoryCacheControler.
	/// </summary>
	public class MemoryCacheControler : ICacheController	
	{
		#region Fields 
		private MemoryCacheLevel _cacheLevel = MemoryCacheLevel.Weak;
		private Hashtable _cache = null;
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Constructor
		/// </summary>
		public MemoryCacheControler() 
		{
			_cache = Hashtable.Synchronized( new Hashtable() );
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
			object value = null;
			object reference = this[key];
			_cache.Remove(key);
			if (reference != null) 
			{
				if (reference is StrongReference) 
				{
					value = ((StrongReference) reference).Target;
				} 
				else if (reference is WeakReference) {
					value = ((WeakReference) reference).Target;
				}
			}
			return value;
		}

		/// <summary>
		/// Adds an item with the specified key and value into cached data.
		/// Gets a cached object with the specified key.
		/// </summary>
		/// <value>The cached object or <c>null</c></value>
		public object this[object key]
		{
			get
			{
				object value = null;
				object reference = _cache[key];
				if (reference != null) 
				{
					if (reference is StrongReference) 
					{
						value = ((StrongReference) reference).Target;
					} 
					else if (reference is WeakReference) 
					{
						value = ((WeakReference) reference).Target;
					}
				}				
				return value;
			}
			set
			{
				object reference = null;
				if (_cacheLevel.Equals(MemoryCacheLevel.Weak)) 
				{
					reference = new WeakReference(value);
				} 
				else if (_cacheLevel.Equals(MemoryCacheLevel.Strong)) 
				{
					reference = new StrongReference(value);
				}
				_cache[key] = reference;	
			
			}
		}


		/// <summary>
		/// Clears all elements from the cache.
		/// </summary>
		public void Flush()
		{
			lock(this) 
			{
				_cache.Clear();
			}				
		}


		/// <summary>
		/// Configures the cache
		/// </summary>
		public void Configure(IDictionary properties)
		{
			string referenceType = (string)properties["Type"];;
			if (referenceType != null) 
			{
				_cacheLevel = MemoryCacheLevel.GetByRefenceType(referenceType.ToUpper());
			}
		}

		#endregion

		/// <summary>
		/// Class to implement a strong (permanent) reference.
		/// </summary>
		private class StrongReference 
		{
			private object _target = null;

			public StrongReference(object obj) 
			{
				_target = obj;
			}

			/// <summary>
			/// Gets the object (the target) referenced by this instance.
			/// </summary>
			public object Target  
			{
				get
				{
					return _target ;
				}
			}
		}
	}
}
