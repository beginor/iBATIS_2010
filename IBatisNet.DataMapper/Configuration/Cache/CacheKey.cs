
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 451064 $
 * $Date: 2006-09-28 17:53:56 -0600 (Thu, 28 Sep 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2005 - iBATIS Team
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

using System.Collections;
using System.Text;

namespace IBatisNet.DataMapper.Configuration.Cache
{
	using IBatisNet.Common.Utilities;

	/// <summary>
	///  Hash value generator for cache keys
	/// </summary>
	public class CacheKey
	{
		private const int DEFAULT_MULTIPLYER = 37;
		private const int DEFAULT_HASHCODE = 17;

		private int _multiplier = DEFAULT_MULTIPLYER;
		private int _hashCode = DEFAULT_HASHCODE;
		private long _checksum = long.MinValue;
		private int _count = 0;
		private IList _paramList = new ArrayList();

		/// <summary>
		/// Default constructor
		/// </summary>
		public CacheKey()
		{
			_hashCode = DEFAULT_HASHCODE;
			_multiplier = DEFAULT_MULTIPLYER;
			_count = 0;
		}

		/// <summary>
		/// Constructor that supplies an initial hashcode
		/// </summary>
		/// <param name="initialNonZeroOddNumber">the hashcode to use</param>
		public CacheKey(int initialNonZeroOddNumber) 
		{
			_hashCode = initialNonZeroOddNumber;
			_multiplier = DEFAULT_MULTIPLYER;
			_count = 0;
		}

		/// <summary>
		/// Constructor that supplies an initial hashcode and multiplier
		/// </summary>
		/// <param name="initialNonZeroOddNumber">the hashcode to use</param>
		/// <param name="multiplierNonZeroOddNumber">the multiplier to use</param>
		public CacheKey(int initialNonZeroOddNumber, int multiplierNonZeroOddNumber) 
		{
			_hashCode = initialNonZeroOddNumber;
			_multiplier = multiplierNonZeroOddNumber;
			_count = 0;
		}

		/// <summary>
		/// Updates this object with new information based on an object
		/// </summary>
		/// <param name="obj">the object</param>
		/// <returns>the cachekey</returns>
		public CacheKey Update(object obj) 
		{
			int baseHashCode = HashCodeProvider.GetIdentityHashCode(obj);

			_count++;
			_checksum += baseHashCode;
			baseHashCode *= _count;

			_hashCode = _multiplier * _hashCode + baseHashCode;

			_paramList.Add(obj);

			return this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj) 
		{
			if (this == obj) return true;
			if (!(obj is CacheKey)) return false;

			CacheKey cacheKey = (CacheKey) obj;

			if (_hashCode != cacheKey._hashCode) return false;
			if (_checksum != cacheKey._checksum) return false;
			if (_count != cacheKey._count) return false;

			int count = _paramList.Count;
			for (int i=0; i < count; i++) 
			{
				object thisParam = _paramList[i];
				object thatParam = cacheKey._paramList[i];
				if(thisParam == null) 
				{
					if (thatParam != null) return false;
				} 
				else 
				{
					if (!thisParam.Equals(thatParam)) return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Get the HashCode for this CacheKey
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode() 
		{
			return _hashCode;
		}

		/// <summary>
		/// ToString implementation.
		/// </summary>
		/// <returns>A string that give the CacheKey HashCode.</returns>
		public override string ToString() 
		{
			return new StringBuilder().Append(_hashCode).Append('|').Append(_checksum).ToString();
		}

	}
}
