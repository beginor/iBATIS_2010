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
using System.Reflection;
using Castle.DynamicProxy;
using IBatisNet.Common.Logging;
using IBatisNet.Common.Utilities.Objects.Members;
using IBatisNet.Common.Utilities.Proxy;
using IBatisNet.DataMapper.MappedStatements;

namespace IBatisNet.DataMapper.Proxy
{
	/// <summary>
    /// This class is responsible of create lazy load proxies for a concrete class with virtual method.
	/// </summary>
	public class LazyLoadProxyFactory : ILazyFactory
	{
		#region Fields
		private static readonly ILog _logger = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );
		#endregion

        #region ILazyFactory Members
        /// <summary>
		/// Builds the specified lazy load proxy for a concrete class with virtual method.
		/// </summary>
        /// <param name="selectStatement">The mapped statement used to build the lazy loaded object.</param>
		/// <param name="param">The parameter object used to build lazy loaded object.</param>
		/// <param name="target">The target object which contains the property proxydied..</param>
        /// <param name="setAccessor">The proxified member accessor.</param>
		/// <returns>Return a proxy object</returns>
        public object CreateProxy(IMappedStatement selectStatement, object param, 
			object target, ISetAccessor setAccessor)
		{
			Type typeProxified = setAccessor.MemberType;

            if (_logger.IsDebugEnabled)
            {
                _logger.Debug(string.Format("Statement '{0}', create proxy for member {1}.", selectStatement.Id, setAccessor.MemberType));
            }

            // Build the proxy
            IInterceptor handler = new LazyLoadInterceptor(selectStatement, param, target, setAccessor);

            // if you want to proxy concrete classes, there are also 2 main requirements : 
            // the class can not be sealed and only virtual methods can be intercepted. 
            // The reason is that DynamicProxy will create a subclass of your class overriding all methods 
            // so it can dispatch the invocations to the interceptor.

            // The proxified type must also have an empty constructor
            object proxy = ProxyGeneratorFactory.GetProxyGenerator().CreateClassProxy(typeProxified, handler);
                    
			return proxy;
        }

        #endregion
    }
}
