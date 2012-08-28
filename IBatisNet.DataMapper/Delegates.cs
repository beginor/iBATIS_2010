
#region Apache Notice
/*****************************************************************************
 * $Revision: 430536 $
 * $LastChangedDate: 2006-08-10 23:24:45 +0200 (jeu., 10 août 2006) $
 * $LastChangedBy: rgrabowski $
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

using System.Collections;
#if dotnet2
using System.Collections.Generic;
#endif


namespace IBatisNet.DataMapper
{
    
    /// <summary>
    /// A delegate called once per row in the QueryWithRowDelegate method
    /// </summary>
    /// <param name="obj">The object currently being processed.</param>
    /// <param name="parameterObject">The optional parameter object passed into the QueryWithRowDelegate method.</param>
    /// <param name="list">The IList that will be returned to the caller.</param>
    public delegate void RowDelegate(object obj, object parameterObject, IList list);

#if dotnet2
    /// <summary>
    /// A delegate called once per row in the QueryWithRowDelegate method
    /// </summary>
    /// <param name="obj">The object currently being processed.</param>
    /// <param name="parameterObject">The optional parameter object passed into the QueryWithRowDelegate method.</param>
    /// <param name="list">The IList that will be returned to the caller.</param>
    public delegate void RowDelegate<T>(object obj, object parameterObject, IList<T> list);

    /// <summary>
    /// A delegate called once per row in the QueryForMapWithRowDelegate method
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="parameterObject">The optional parameter object passed into the QueryForMapWithRowDelegate method.</param>
    /// <param name="dictionary">The IDictionary that will be returned to the caller.</param>
    public delegate void DictionaryRowDelegate<K, V>(K key, V value, object parameterObject, IDictionary<K, V> dictionary);
#endif

    /// <summary>
    /// A delegate called once per row in the QueryForMapWithRowDelegate method
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="parameterObject">The optional parameter object passed into the QueryForMapWithRowDelegate method.</param>
    /// <param name="dictionary">The IDictionary that will be returned to the caller.</param>
    public delegate void DictionaryRowDelegate(object key, object value, object parameterObject, IDictionary dictionary);

}
