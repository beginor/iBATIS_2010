#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2007-02-21 13:23:49 -0700 (Wed, 21 Feb 2007) $
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

namespace IBatisNet.DataMapper.Configuration.ResultMapping
{
	/// <summary>
	/// A ResultProperty Collection.
	/// </summary>
	public class ResultPropertyCollection
	{
		private const int DEFAULT_CAPACITY = 4;
		private const int CAPACITY_MULTIPLIER = 2;
		private int _count = 0;
		private ResultProperty[] _innerList = null;


		/// <summary>
		/// Read-only property describing how many elements are in the Collection.
		/// </summary>
		public int Count 
		{
			get { return _count; }
		}


		/// <summary>
		/// Constructs a ResultPropertyCollection. The list is initially empty and has a capacity
		/// of zero. Upon adding the first element to the list the capacity is
		/// increased to 8, and then increased in multiples of two as required.
		/// </summary>
		public ResultPropertyCollection() 
		{
            this.Clear();
		}

        /// <summary>
        /// Removes all items from the collection.
        /// </summary>
        public void Clear()
        {
            _innerList = new ResultProperty[DEFAULT_CAPACITY];
            _count = 0;
        }
	    
		/// <summary>
		///  Constructs a ResultPropertyCollection with a given initial capacity. 
		///  The list is initially empty, but will have room for the given number of elements
		///  before any reallocations are required.
		/// </summary>
		/// <param name="capacity">The initial capacity of the list</param>
		public ResultPropertyCollection(int capacity) 
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("Capacity", "The size of the list must be >0.");
			}
			_innerList = new ResultProperty[capacity];
		}

		/// <summary>
		/// Length of the collection
		/// </summary>
		public int Length
		{
			get{ return _innerList.Length; }
		}

 
		/// <summary>
		/// Sets or Gets the ResultProperty at the given index.
		/// </summary>
		public ResultProperty this[int index] 
		{
			get
			{
				if (index < 0 || index >= _count) 
				{
					throw new ArgumentOutOfRangeException("index");
				}
				return _innerList[index];
			}
			set
			{
				if (index < 0 || index >= _count) 
				{
					throw new ArgumentOutOfRangeException("index");
				}
				_innerList[index] = value;
			}
		}

        /// <summary>
        /// Finds a property by his name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public ResultProperty FindByPropertyName(string propertyName)
        {
            ResultProperty resultProperty = null;
            for (int i = 0; i < _count; i++)
            {
                if (_innerList[i].PropertyName == propertyName)
                {
                    resultProperty = _innerList[i];
                    break;
                }
            }
            return resultProperty;
        }


        /// <summary>
        /// Add an ResultProperty
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Index</returns>
        public int Add(ResultProperty value) 
		{
			Resize(_count + 1);
			int index = _count++;
			_innerList[index] = value;

			return index;
		}

 
		/// <summary>
		/// Add a list of ResultProperty to the collection
		/// </summary>
		/// <param name="value"></param>
		public void AddRange(ResultProperty[] value) 
		{
			for (int i = 0;   i < value.Length; i++) 
			{
				Add(value[i]);
			}
		}

 
		/// <summary>
		/// Add a list of ResultProperty to the collection
		/// </summary>
		/// <param name="value"></param>
		public void AddRange(ResultPropertyCollection value) 
		{
			for (int i = 0;   i < value.Count; i++) 
			{
				Add(value[i]);
			}
		}
 

		/// <summary>
		/// Indicate if a ResultProperty is in the collection
		/// </summary>
		/// <param name="value">A ResultProperty</param>
		/// <returns>True fi is in</returns>
		public bool Contains(ResultProperty value) 
		{
		    return Contains(value.PropertyName);
		}

        /// <summary>
        /// Indicate if a ResultProperty is in the collection
        /// </summary>
        /// <param name="propertyName">A property name</param>
        /// <returns>True fi is in</returns>
        public bool Contains(string propertyName)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_innerList[i].PropertyName == propertyName)
                {
                    return true;
                }
            }
            return false;
        }

		/// <summary>
		/// Insert a ResultProperty in the collection.
		/// </summary>
		/// <param name="index">Index where to insert.</param>
		/// <param name="value">A ResultProperty</param>
		public void Insert(int index, ResultProperty value) 
		{
			if (index < 0 || index > _count) 
			{
				throw new ArgumentOutOfRangeException("index");
			}
			
			Resize(_count + 1);
			Array.Copy(_innerList, index, _innerList, index + 1, _count - index);
			_innerList[index] = value;
			_count++;
		}
            

		/// <summary>
		/// Remove a ResultProperty of the collection.
		/// </summary>
		public void Remove(ResultProperty value) 
		{
			for(int i = 0; i < _count; i++) 
			{
				if(_innerList[i].PropertyName==value.PropertyName)
				{
					RemoveAt(i);
					return;
				}
			}

		}

		/// <summary>
		/// Removes a ResultProperty at the given index. The size of the list is
		/// decreased by one.
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index) 
		{
			if (index < 0 || index >= _count) 
			{
				throw new ArgumentOutOfRangeException("index");
			}
			
			int remaining = _count - index - 1;
						
			if (remaining > 0) 
			{
				Array.Copy(_innerList, index + 1, _innerList, index, remaining);
			}
			
			_count--;
			_innerList[_count] = null;
		}

		/// <summary>
		/// Ensures that the capacity of this collection is at least the given minimum
		/// value. If the currect capacity of the list is less than min, the
		/// capacity is increased to twice the current capacity.
		/// </summary>
		/// <param name="minSize"></param>
		private void Resize(int minSize) 
		{
			int oldSize = _innerList.Length;

			if (minSize > oldSize) 
			{
				ResultProperty[] oldEntries = _innerList;
				int newSize = oldEntries.Length * CAPACITY_MULTIPLIER;
		
				if (newSize < minSize) 
				{
					newSize = minSize;
				}
				_innerList = new ResultProperty[newSize];
				Array.Copy(oldEntries, 0, _innerList, 0, _count);
			}
		}
	}
}
