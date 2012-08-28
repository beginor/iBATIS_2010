#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-11-09 13:02:26 -0700 (Thu, 09 Nov 2006) $
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
using System.Collections.Generic;
using System.Collections;
using System.Reflection;

using IBatisNet.Common.Utilities.Objects.Members;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.Common.Logging;

namespace IBatisNet.DataMapper.Proxy
{
    /// <summary>
    /// A lazy generic list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class LazyListGeneric<T> : IList<T>, IList where T: class

    {
        #region Fields
        private object _param = null;
        private object _target = null;
        private ISetAccessor _setAccessor = null;
        private ISqlMapper _sqlMap = null;
        private string _statementId = string.Empty;
        private bool _loaded = false;
        private object _loadLock = new object();
        private IList<T> _list = null;

        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        /// <summary>
        /// Initializes a new instance of the LazyListGeneric class.
        /// </summary>
        /// <param name="mappedSatement">The mapped satement.</param>
        /// <param name="param">The param.</param>
        /// <param name="target">The target.</param>
        /// <param name="setAccessor">The set accessor.</param>
        public LazyListGeneric(IMappedStatement mappedSatement, object param,
            object target, ISetAccessor setAccessor)
        {
            _param = param;
            _statementId = mappedSatement.Id;
            _sqlMap = mappedSatement.SqlMap;
            _target = target;
            _setAccessor = setAccessor;
            _list = new List<T>();
        }

        /// <summary>
        /// Resolve the lazy loading.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        private void Load(string methodName)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("Proxyfying call to " + methodName);
            }

            lock (_loadLock)
            {
                if (_loaded == false)
                {
                    if (_logger.IsDebugEnabled)
                    {
                        _logger.Debug("Proxyfying call, query statement " + _statementId);
                    }

                    _list = _sqlMap.QueryForList<T>(_statementId, _param);
                    _loaded = true;
                    _setAccessor.Set(_target, _list);
                }
            }

            if (_logger.IsDebugEnabled)
            {
                _logger.Debug("End of proxyfied call to " + methodName);
            }
        }

        #region IList<T> Members

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"></see>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        /// <returns>
        /// The index of item if found in the list; otherwise, -1.
        /// </returns>
        public int IndexOf(T item)
        {
            Load("IndexOf");
            return _list.IndexOf(item);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"></see> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        public void Insert(int index, T item)
        {
            Load("Insert");
            _list.Insert(index, item);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        public void RemoveAt(int index)
        {
            Load("RemoveAt");
            _list.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the object at the specified index.
        /// </summary>
        /// <value></value>
        public T this[int index]
        {
            get
            {
                Load("this");
                return _list[index];
            }
            set
            {
                Load("this");
                _list[index] = value;
            }
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public void Add(T item)
        {
            Load("Add");
            _list.Add(item);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
        public void Clear()
        {
            Load("Clear");
            _list.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item is found in the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            Load("Contains");
            return _list.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"></see> to an <see cref="T:System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="T:System.ArgumentNullException">array is null.</exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or-arrayIndex is equal to or greater than the length of array.-or-The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1"></see> is greater than the available space from arrayIndex to the end of the destination array.-or-Type T cannot be cast automatically to the type of the destination array.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            Load("CopyTo");
            _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</returns>
        public int Count
        {
            get
            {
                Load("Count");
                return _list.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.</param>
        /// <returns>
        /// true if item was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"></see>; otherwise, false. This method also returns false if item is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.</exception>
        public bool Remove(T item)
        {
            Load("Remove");
            return _list.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Collections.Generic.IEnumerator"></see> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            Load("GetEnumerator<T>");
            return _list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            Load("GetEnumerator");
            return (System.Collections.IEnumerator)_list.GetEnumerator();
        }

        #endregion

        #region IList Members

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.IList"></see>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"></see> to add to the <see cref="T:System.Collections.IList"></see>.</param>
        /// <returns>
        /// The position into which the new element was inserted.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
        int IList.Add(object value)
        {
            Load("Add");
            return ((IList)_list).Add(value);
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"></see>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only. </exception>
        void IList.Clear()
        {
            this.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.IList"></see> contains a specific value.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
        /// <returns>
        /// true if the <see cref="T:System.Object"></see> is found in the <see cref="T:System.Collections.IList"></see>; otherwise, false.
        /// </returns>
        bool IList.Contains(object value)
        {
            Load("Contains");
            return ((IList)_list).Contains(value);
        }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:System.Collections.IList"></see>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"></see> to locate in the <see cref="T:System.Collections.IList"></see>.</param>
        /// <returns>
        /// The index of value if found in the list; otherwise, -1.
        /// </returns>
        int IList.IndexOf(object value)
        {
            Load("IndexOf");
            return ((IList)_list).IndexOf(value);
        }

        /// <summary>
        /// Inserts an item to the <see cref="T:System.Collections.IList"></see> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to insert into the <see cref="T:System.Collections.IList"></see>.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.IList"></see>. </exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
        /// <exception cref="T:System.NullReferenceException">value is null reference in the <see cref="T:System.Collections.IList"></see>.</exception>
        void IList.Insert(int index, object value)
        {
            Load("IndexOf");
            ((IList)_list).Insert(index, value);
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.IList"></see> has a fixed size.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.IList"></see> has a fixed size; otherwise, false.</returns>
        bool IList.IsFixedSize
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"></see> is read-only; otherwise, false.</returns>
        bool IList.IsReadOnly
        {
            get { return false; }
        }


        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.IList"></see>.
        /// </summary>
        /// <param name="value">The <see cref="T:System.Object"></see> to remove from the <see cref="T:System.Collections.IList"></see>.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.IList"></see> is read-only.-or- The <see cref="T:System.Collections.IList"></see> has a fixed size. </exception>
        void IList.Remove(object value)
        {
            Load("Remove");
            ((IList)_list).Remove(value);
        }

        /// <summary>
        /// Removes the <see cref="T:System.Collections.Generic.IList`1"></see> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"></see> is read-only.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"></see>.</exception>
        void IList.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the <see cref="Object"/> at the specified index.
        /// </summary>
        /// <value></value>
        object IList.this[int index]
        {
            get
            {
                Load("this");
                return this[index];
            }
            set
            {
                Load("this");
                ((IList)_list)[index] = value;
            }
        }

        #endregion

        #region ICollection Members

        /// <summary>
        /// Copies the elements of the <see cref="System.Collections.ICollection"></see> to an <see cref="System.Array"></see>, starting at a particular <see cref="T:System.Array"></see> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"></see> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection"></see>. The <see cref="T:System.Array"></see> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">array is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">array is multidimensional.-or- index is equal to or greater than the length of array.-or- The number of elements in the source <see cref="T:System.Collections.ICollection"></see> is greater than the available space from index to the end of the destination array. </exception>
        /// <exception cref="T:System.InvalidCastException">The type of the source <see cref="System.Collections.ICollection"></see> cannot be cast automatically to the type of the destination array. </exception>
        void ICollection.CopyTo(Array array, int index)
        {
            Load("CopyTo");
            ((IList)_list).CopyTo(array, index);
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="System.Collections.Generic.ICollection"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>The number of elements contained in the <see cref="System.Collections.Generic.ICollection"></see>.</returns>
        int ICollection.Count
        {
            get { return this.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether access to the <see cref="System.Collections.ICollection"></see> is synchronized (thread safe).
        /// </summary>
        /// <value></value>
        /// <returns>true if access to the <see cref="T:System.Collections.ICollection"></see> is synchronized (thread safe); otherwise, false.</returns>
        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="System.Collections.ICollection"></see>.
        /// </summary>
        /// <value></value>
        /// <returns>An object that can be used to synchronize access to the <see cref="System.Collections.ICollection"></see>.</returns>
        object ICollection.SyncRoot
        {
            get { return this; }
        }

        #endregion
    }


}
