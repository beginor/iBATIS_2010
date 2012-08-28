#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-05-18 14:25:03 -0600 (Thu, 18 May 2006) $
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
using System.Collections;
using System.Text;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.Common.Utilities.Objects.Members;

namespace IBatisNet.DataMapper.Proxy
{
    /// <summary>
    /// Implementation of <see cref="ILazyFactory"/> to create proxy for an <see cref="IList"/> element.
    /// </summary>
    public class LazyListFactory : ILazyFactory
    {
        #region ILazyFactory Members

        /// <summary>
        /// Create a new proxy instance.
        /// </summary>
        /// <param name="mappedStatement">The mapped statement.</param>
        /// <param name="param">The param.</param>
        /// <param name="target">The target.</param>
        /// <param name="setAccessor">The set accessor.</param>
        /// <returns>Returns a new proxy.</returns>
        public object CreateProxy(IMappedStatement mappedStatement, object param,
            object target, ISetAccessor setAccessor)
        {
            return new LazyList(mappedStatement, param, target, setAccessor);
        }

        #endregion
    }
}
