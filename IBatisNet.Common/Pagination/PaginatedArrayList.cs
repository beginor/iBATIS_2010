
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


namespace IBatisNet.Common.Pagination
{
	/// <summary>
	/// Summary description for PaginatedArrayList.
	/// </summary>
	public class PaginatedArrayList: IPaginatedList
	{
		
		#region Fields

		private static ArrayList _emptyList = new ArrayList();

		private int _pageSize = 0;
		private int _pageIndex = 0;
		private IList _list = null;
		private IList _page = null;
		#endregion

		#region Properties
		/// <summary>
		/// 
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				return (_page.Count == 0);
			}
		}
		#endregion

		#region Constructor (s) / Destructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pageSize"></param>
		public PaginatedArrayList(int pageSize) 
		{
			_pageSize = pageSize;
			_pageIndex = 0;
			_list = new ArrayList();
			Repaginate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="initialCapacity"></param>
		/// <param name="pageSize"></param>
		public PaginatedArrayList(int initialCapacity, int pageSize) 
		{
			_pageSize = pageSize;
			_pageIndex = 0;
			_list = new ArrayList(initialCapacity);
			Repaginate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="c"></param>
		/// <param name="pageSize"></param>
		public PaginatedArrayList(ICollection c, int pageSize) 
		{
			_pageSize = pageSize;
			_pageIndex = 0;
			_list = new ArrayList(c);
			Repaginate();
		}
		#endregion 

		#region Methods

		/// <summary>
		/// 
		/// </summary>
		private void Repaginate() 
		{
			if (_list.Count==0) 
			{
				_page = _emptyList;
			} 
			else 
			{
				int start = _pageIndex * _pageSize;
				int end = start + _pageSize - 1;
				if (end >= _list.Count) 
				{
					end = _list.Count - 1;
				}
				if (start >= _list.Count) 
				{
					_pageIndex = 0;
					Repaginate();
				} 
				else if (start < 0) 
				{
					_pageIndex = _list.Count / _pageSize;
					if (_list.Count % _pageSize == 0) 
					{
						_pageIndex--;
					}
					Repaginate();
				} 
				else 
				{
					_page = SubList(_list, start, end + 1);
				}
			}
		}


		/// <summary>
		/// Provides a view of the IList pramaeter 
		/// from the specified position <paramref name="fromIndex"/> 
		/// to the specified position <paramref name="toIndex"/>. 
		/// </summary>
		/// <param name="list">The IList elements.</param>
		/// <param name="fromIndex">Starting position for the view of elements. </param>
		/// <param name="toIndex">Ending position for the view of elements. </param>
		/// <returns> A view of list.
		/// </returns>
		/// <remarks>
		/// The list that is returned is just a view, it is still backed
		/// by the orignal list.  Any changes you make to it will be 
		/// reflected in the orignal list.
		/// </remarks>
		private IList SubList(IList list, int fromIndex, int toIndex)
		{
			return ((ArrayList)list).GetRange(fromIndex, toIndex-fromIndex);
		}
		#endregion

		#region IPaginatedList Members

		/// <summary>
		/// 
		/// </summary>
		public int PageSize
		{
			get
			{
				return _pageSize;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsFirstPage
		{
			get
			{
				return (_pageIndex == 0);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsMiddlePage
		{
			get
			{
				return !(this.IsFirstPage || this.IsLastPage);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsLastPage
		{
			get
			{
				return _list.Count - ((_pageIndex + 1) * _pageSize) < 1;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsNextPageAvailable
		{
			get
			{
				return !this.IsLastPage;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsPreviousPageAvailable
		{
			get
			{
				return !this.IsFirstPage;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool NextPage()
		{
			if (this.IsNextPageAvailable) 
			{
				_pageIndex++;
				Repaginate();
				return true;
			} 
			else 
			{
				return false;
			}		
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool PreviousPage()
		{
			if (this.IsPreviousPageAvailable) 
			{
				_pageIndex--;
				Repaginate();
				return true;
			} 
			else 
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pageIndex"></param>
		public void GotoPage(int pageIndex)
		{
			_pageIndex = pageIndex;
			Repaginate();		
		}

		/// <summary>
		/// 
		/// </summary>
		public int PageIndex
		{
			get
			{
				return _pageIndex;
			}
		}

		#endregion

		#region IList Members

		/// <summary>
		/// 
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return _list.IsReadOnly;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public object this[int index]
		{
			get
			{
				return _page[index];
			}
			set
			{
				_list[index] = value;
				Repaginate();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			_list.RemoveAt(index);
			Repaginate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert(int index, object value)
		{
			_list.Insert(index, value);
			Repaginate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		public void Remove(object value)
		{
			_list.Remove(value);
			Repaginate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(object value)
		{
			return _page.Contains(value);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			_list.Clear();
			Repaginate();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(object value)
		{
			return _page.IndexOf(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add(object value)
		{
			int i = _list.Add(value);
			Repaginate();
			return i;
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsFixedSize
		{
			get
			{
				return _list.IsFixedSize;
			}
		}

		#endregion

		#region ICollection Members

		/// <summary>
		/// 
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				return _page.IsSynchronized;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get
			{
				return _page.Count;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(Array array, int index)
		{
			_page.CopyTo(array, index);
		}

		/// <summary>
		/// 
		/// </summary>
		public object SyncRoot
		{
			get
			{
				return _page.SyncRoot;
			}
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return _page.GetEnumerator();
		}

		#endregion

		#region IEnumerator Members

		/// <summary>
		/// Sets the enumerator to its initial position, 
		/// which is before the first element in the collection.
		/// </summary>
		public void Reset()
		{
			_page.GetEnumerator().Reset();
		}

		/// <summary>
		/// Gets the current element in the page.
		/// </summary>
		public object Current
		{
			get
			{
				return _page.GetEnumerator().Current;
			}
		}

		/// <summary>
		/// Advances the enumerator to the next element of the collection.
		/// </summary>
		/// <returns>
		/// true if the enumerator was successfully advanced to the next element; 
		/// false if the enumerator has passed the end of the collection.
		/// </returns>
		public bool MoveNext()
		{
			return _page.GetEnumerator().MoveNext();
		}

		#endregion
	}
}
