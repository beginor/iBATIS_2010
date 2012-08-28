#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-10-30 12:09:11 -0700 (Mon, 30 Oct 2006) $
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

namespace IBatisNet.DataMapper.Configuration.ParameterMapping
{
	/// <summary>
	/// A ParameterProperty Collection.
	/// </summary>
	public class ParameterPropertyCollection
	{
		private const int DEFAULT_CAPACITY = 4;
		private const int CAPACITY_MULTIPLIER = 2;
		private int _count = 0;
		private ParameterProperty[] _innerList = null;


		/// <summary>
		/// Read-only property describing how many elements are in the Collection.
		/// </summary>
		public int Count 
		{
			get { return _count; }
		}


		/// <summary>
		/// Constructs a ParameterProperty collection. The list is initially empty and has a capacity
		/// of zero. Upon adding the first element to the list the capacity is
		/// increased to 8, and then increased in multiples of two as required.
		/// </summary>
		public ParameterPropertyCollection() 
		{
			_innerList = new ParameterProperty[DEFAULT_CAPACITY];
			_count = 0;
		}

		/// <summary>
		///  Constructs a ParameterPropertyCollection with a given initial capacity. 
		///  The list is initially empty, but will have room for the given number of elements
		///  before any reallocations are required.
		/// </summary>
		/// <param name="capacity">The initial capacity of the list</param>
		public ParameterPropertyCollection(int capacity) 
		{
			if (capacity < 0)
			{
				throw new ArgumentOutOfRangeException("Capacity", "The size of the list must be >0.");
			}
			_innerList = new ParameterProperty[capacity];
		}

		/// <summary>
		/// Length of the collection
		/// </summary>
		public int Length
		{
			get{ return _innerList.Length; }
		}

 
		/// <summary>
		/// Sets or Gets the ParameterProperty at the given index.
		/// </summary>
		public ParameterProperty this[int index] 
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
		/// Add an ParameterProperty
		/// </summary>
		/// <param name="value"></param>
		/// <returns>Index</returns>
		public int Add(ParameterProperty value) 
		{
			Resize(_count + 1);
			int index = _count++;
			_innerList[index] = value;

			return index;
		}

 
		/// <summary>
		/// Add a list of ParameterProperty to the collection
		/// </summary>
		/// <param name="value"></param>
		public void AddRange(ParameterProperty[] value) 
		{
			for (int i = 0;   i < value.Length; i++) 
			{
				Add(value[i]);
			}
		}

 
		/// <summary>
		/// Add a list of ParameterProperty to the collection
		/// </summary>
		/// <param name="value"></param>
		public void AddRange(ParameterPropertyCollection value) 
		{
			for (int i = 0;   i < value.Count; i++) 
			{
				Add(value[i]);
			}
		}
 

		/// <summary>
		/// Indicate if a ParameterProperty is in the collection
		/// </summary>
		/// <param name="value">A ParameterProperty</param>
		/// <returns>True fi is in</returns>
		public bool Contains(ParameterProperty value) 
		{
			for (int i = 0;   i < _count; i++) 
			{
				if(_innerList[i].PropertyName==value.PropertyName)
				{
					return true;
				}
			}
			return false;
		}
      

		/// <summary>
		/// Insert a ParameterProperty in the collection.
		/// </summary>
		/// <param name="index">Index where to insert.</param>
		/// <param name="value">A ParameterProperty</param>
		public void Insert(int index, ParameterProperty value) 
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
		/// Remove a ParameterProperty of the collection.
		/// </summary>
		public void Remove(ParameterProperty value) 
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
		/// Removes a ParameterProperty at the given index. The size of the list is
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
				ParameterProperty[] oldEntries = _innerList;
				int newSize = oldEntries.Length * CAPACITY_MULTIPLIER;
		
				if (newSize < minSize) 
				{
					newSize = minSize;
				}
				_innerList = new ParameterProperty[newSize];
				Array.Copy(oldEntries, 0, _innerList, 0, _count);
			}
		}
	}

}

