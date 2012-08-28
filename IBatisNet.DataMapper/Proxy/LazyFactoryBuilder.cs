#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2007-01-04 14:28:10 -0700 (Thu, 04 Jan 2007) $
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
using System.Collections.Specialized;
#if dotnet2
using System.Collections.Generic;
#endif
using IBatisNet.DataMapper.Exceptions;

namespace IBatisNet.DataMapper.Proxy
{
    /// <summary>
    /// Gets <see cref="ILazyFactory"/> instance.
    /// </summary>
    public class LazyFactoryBuilder
    {
        private IDictionary _factory = new HybridDictionary();

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyFactoryBuilder"/> class.
        /// </summary>
        public LazyFactoryBuilder()
        {
            _factory[typeof(IList)] = new LazyListFactory();
#if dotnet2
            _factory[typeof(IList<>)] = new LazyListGenericFactory();
#endif
        }

        
        /// <summary>
        /// Register (add) a lazy load Proxy for a type and member type
        /// </summary>
        /// <param name="type">The target type which contains the member proxyfied</param>
        /// <param name="memberName">The member name the proxy must emulate</param>
        /// <param name="factory">The <see cref="ILazyFactory"/>.</param>
        public void Register(Type type, string memberName, ILazyFactory factory)
        {
            // To use for further used, support for custom proxy
        }

        /// <summary>
        /// Get a ILazyLoadProxy for a type, member name
        /// </summary>
        /// <param name="type">The target type which contains the member proxyfied</param>
        /// <returns>Return the ILazyLoadProxy instance</returns>
        public ILazyFactory GetLazyFactory(Type type)
        {
            if (type.IsInterface)
            {
#if dotnet2
                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IList<>)) )
                {
                    return _factory[ type.GetGenericTypeDefinition() ] as ILazyFactory;
                }
                else 
#endif				
				if (type == typeof(IList))
                {
                    return _factory[type] as ILazyFactory;
                }
                else
                {
                    throw new DataMapperException("Cannot proxy others interfaces than IList or IList<>.");
                }
            }
            else
            {
                // if you want to proxy concrete classes, there are also two requirements: 
                // the class can not be sealed and only virtual methods can be intercepted. 
                // The reason is that DynamicProxy will create a subclass of your class overriding all methods 
                // so it can dispatch the invocations to the interceptor.
                return new LazyLoadProxyFactory();
            }
        }
    }
}
