
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

#region Using

using System;
using System.Collections;
using System.Collections.Specialized;
using Castle.DynamicProxy;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Logging;

#endregion

namespace IBatisNet.Common.Utilities.Proxy
{
	/// <summary>
	/// An ProxyGenerator with cache that uses the Castle.DynamicProxy library.
	/// </summary>
	[CLSCompliant(false)]
	public class CachedProxyGenerator : ProxyGenerator
	{
		private static readonly ILog _log = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		// key = mapped type
		// value = proxy type
		private IDictionary _cachedProxyTypes =null;

		/// <summary>
		/// Cosntructor
		/// </summary>
		public CachedProxyGenerator()
		{
			_cachedProxyTypes = new HybridDictionary();
		}

		/// <summary>
		/// Generates a proxy implementing all the specified interfaces and
		/// redirecting method invocations to the specifed interceptor.
		/// </summary>
		/// <param name="theInterface">Interface to be implemented</param>
		/// <param name="interceptor">instance of <see cref="IInterceptor"/></param>
		/// <param name="target">The target object.</param>
		/// <returns>Proxy instance</returns>
		public object CreateProxy(Type theInterface, IInterceptor interceptor, object target)
		{
			return CreateProxy(new Type[] {theInterface}, interceptor, target);
		}

		/// <summary>
		/// Generates a proxy implementing all the specified interfaces and
		/// redirecting method invocations to the specifed interceptor.
		/// </summary>
		/// <param name="interfaces">Array of interfaces to be implemented</param>
		/// <param name="interceptor">instance of <see cref="IInterceptor"/></param>
		/// <param name="target">The target object.</param>
		/// <returns>Proxy instance</returns>
		public object CreateProxy(Type[] interfaces, IInterceptor interceptor, object target)
		{
			/*try
			{
				System.Type proxyType = null;
				System.Type targetType = target.GetType();

				lock( _cachedProxyTypes.SyncRoot )
				{
					proxyType = _cachedProxyTypes[ targetType ] as System.Type;

					if( proxyType == null )
					{
						proxyType = ProxyBuilder.CreateInterfaceProxy(interfaces, targetType );
						_cachedProxyTypes[ targetType ] = proxyType;
					}
				}
				return base.CreateProxyInstance( proxyType, interceptor, target );
			}
			catch( Exception e )
			{
				_log.Error( "Castle Dynamic Proxy Generator failed", e );
				throw new IBatisNetException( "Castle Proxy Generator failed", e );
			}*/
			throw new NotImplementedException();
		}


		
		/// <summary>
		/// Generates a proxy implementing all the specified interfaces and
		/// redirecting method invocations to the specifed interceptor.
		/// This proxy is for object different from IList or ICollection
		/// </summary>
		/// <param name="targetType">The target type</param>
		/// <param name="interceptor">The interceptor.</param>
		/// <param name="argumentsForConstructor">The arguments for constructor.</param>
		/// <returns></returns>
		public object CreateClassProxy(Type targetType, IInterceptor interceptor, params object[] argumentsForConstructor)
		{
			//try
			//{
			//   System.Type proxyType = null;

			//   lock( _cachedProxyTypes.SyncRoot )
			//   {
			//      proxyType = _cachedProxyTypes[ targetType ] as System.Type;

			//      if( proxyType == null )
			//      {
			//         proxyType = ProxyBuilder.CreateClassProxy(targetType);
			//         _cachedProxyTypes[ targetType ] = proxyType;
			//      }
			//   }
			//   return CreateClassProxyInstance( proxyType, interceptor, argumentsForConstructor ); 
			//}
			//catch( Exception e )
			//{
			//   _log.Error( "Castle Dynamic Class-Proxy Generator failed", e );
			//   throw new IBatisNetException( "Castle Proxy Generator failed", e );
			//}
			throw new NotImplementedException();
		}
	}
}
