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

using System;
using System.Collections.Generic;
using System.Reflection;

using IBatisNet.Common;
using IBatisNet.DataMapper.Scope;

namespace IBatisNet.DataMapper.MappedStatements.PostSelectStrategy
{
    /// <summary>
    /// <see cref="IPostSelectStrategy"/> implementation to exceute a query generic list.
    /// </summary>
    public sealed class GenericListStrategy : IPostSelectStrategy
    {
        #region IPostSelectStrategy Members

        /// <summary>
        /// Executes the specified <see cref="PostBindind"/>.
        /// </summary>
        /// <param name="postSelect">The <see cref="PostBindind"/>.</param>
        /// <param name="request">The <see cref="RequestScope"/></param>
        public void Execute(PostBindind postSelect, RequestScope request)
        {
            // How to: Examine and Instantiate Generic Types with Reflection  
            // http://msdn2.microsoft.com/en-us/library/b8ytshk6.aspx

            Type[] typeArgs = postSelect.ResultProperty.SetAccessor.MemberType.GetGenericArguments();
            Type genericList = typeof(IList<>);
            Type constructedType = genericList.MakeGenericType(typeArgs);
            Type elementType = postSelect.ResultProperty.SetAccessor.MemberType.GetGenericArguments()[0];

            Type mappedStatementType = postSelect.Statement.GetType();

            Type[] typeArguments = { typeof(SqlMapSession), typeof(object) };

            MethodInfo[] mis = mappedStatementType.GetMethods(BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance);
            MethodInfo mi = null;
            foreach (MethodInfo m in mis)
            {
                if (m.IsGenericMethod &&
                    m.Name == "ExecuteQueryForList" &&
                    m.GetParameters().Length == 2)
                {
                    mi = m;
                    break;
                }
            }

            MethodInfo miConstructed = mi.MakeGenericMethod(elementType);

            // Invoke the method.
            object[] args = { request.Session, postSelect.Keys };
            object values = miConstructed.Invoke(postSelect.Statement, args);

            postSelect.ResultProperty.SetAccessor.Set(postSelect.Target, values);
        }

        #endregion
    }
}
