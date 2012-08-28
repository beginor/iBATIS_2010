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
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security;
using Castle.DynamicProxy;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Logging;

#endregion

namespace IBatisNet.Common.Utilities.Proxy {
	/// <summary>
	/// An ProxyGenerator with cache that uses the Castle.DynamicProxy library.
	/// </summary>
	[CLSCompliant(false)]
	public class CachedProxyGenerator : ProxyGenerator {
		private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		// key = mapped type
		// value = proxy type
		private readonly IDictionary _cachedProxyTypes;

		/// <summary>
		/// Cosntructor
		/// </summary>
		public CachedProxyGenerator() {
			this._cachedProxyTypes = new HybridDictionary();
		}

		public override object CreateClassProxy(Type classToProxy, Type[] additionalInterfacesToProxy, ProxyGenerationOptions options, object[] constructorArguments, params IInterceptor[] interceptors) {
			try {
				if (classToProxy == null) {
					throw new ArgumentNullException("classToProxy");
				}
				if (options == null) {
					throw new ArgumentNullException("options");
				}
				if (!classToProxy.IsClass) {
					throw new ArgumentException("'classToProxy' must be a class", "classToProxy");
				}
				this.CheckNotGenericTypeDefinition(classToProxy, "classToProxy");
				this.CheckNotGenericTypeDefinitions(additionalInterfacesToProxy, "additionalInterfacesToProxy");
				Type classProxyType;

				lock(this._cachedProxyTypes.SyncRoot) {
					classProxyType = this._cachedProxyTypes[classToProxy] as Type;
					if (classProxyType == null) {
						classProxyType = this.CreateClassProxyType(classToProxy, additionalInterfacesToProxy, options);
						this._cachedProxyTypes[classToProxy] = classProxyType;
					}
				}

				List<object> proxyArguments = this.BuildArgumentListForClassProxy(options, interceptors);
				if (constructorArguments != null && constructorArguments.Length != 0) {
					proxyArguments.AddRange(constructorArguments);
				}

				return this.CreateClassProxyInstance(classProxyType, proxyArguments, classToProxy, constructorArguments);
			}
			catch (Exception ex) {
				_log.Error("Castle Dynamic Class-Proxy Generator failed", ex);
				throw new IBatisNetException("Castle Proxy Generator failed", ex);
			}
		}

		public override object CreateClassProxyWithTarget(Type classToProxy, Type[] additionalInterfacesToProxy, object target, ProxyGenerationOptions options, object[] constructorArguments, params IInterceptor[] interceptors) {
			try {
				if (classToProxy == null) {
					throw new ArgumentNullException("classToProxy");
				}
				if (options == null) {
					throw new ArgumentNullException("options");
				}
				if (!classToProxy.IsClass) {
					throw new ArgumentException("'classToProxy' must be a class", "classToProxy");
				}
				this.CheckNotGenericTypeDefinition(classToProxy, "classToProxy");
				this.CheckNotGenericTypeDefinitions(additionalInterfacesToProxy, "additionalInterfacesToProxy");
				Type proxyTypeWithTarget;
				Type type = target.GetType();
				lock(this._cachedProxyTypes.SyncRoot) {
					proxyTypeWithTarget = this._cachedProxyTypes[type] as Type;
					if (proxyTypeWithTarget == null) {
						proxyTypeWithTarget = this.CreateClassProxyTypeWithTarget(classToProxy, additionalInterfacesToProxy, options);
						this._cachedProxyTypes[type] = proxyTypeWithTarget;
					}
				}
				List<object> proxyArguments = this.BuildArgumentListForClassProxyWithTarget(target, options, interceptors);
				if (constructorArguments != null && constructorArguments.Length != 0) {
					proxyArguments.AddRange(constructorArguments);
				}
				return this.CreateClassProxyInstance(proxyTypeWithTarget, proxyArguments, classToProxy, constructorArguments);
			}
			catch (Exception ex) {
				_log.Error("Castle Dynamic Class-Proxy Generator failed", ex);
				throw new IBatisNetException("Castle Proxy Generator failed", ex);
			}
		}

		public override object CreateInterfaceProxyWithoutTarget(Type interfaceToProxy, Type[] additionalInterfacesToProxy, ProxyGenerationOptions options, params IInterceptor[] interceptors) {
			if (interfaceToProxy == null) {
				throw new ArgumentNullException("interfaceToProxy");
			}
			if (interceptors == null) {
				throw new ArgumentNullException("interceptors");
			}
			if (!interfaceToProxy.IsInterface) {
				throw new ArgumentException("Specified type is not an interface", "interfaceToProxy");
			}
			this.CheckNotGenericTypeDefinition(interfaceToProxy, "interfaceToProxy");
			this.CheckNotGenericTypeDefinitions(additionalInterfacesToProxy, "additionalInterfacesToProxy");
			Type proxyType;
			lock(this._cachedProxyTypes.SyncRoot) {
				proxyType = this._cachedProxyTypes[interfaceToProxy] as Type;
				if (proxyType == null) {
					proxyType = this.CreateInterfaceProxyTypeWithoutTarget(interfaceToProxy, additionalInterfacesToProxy, options);
					this._cachedProxyTypes[interfaceToProxy] = proxyType;
				}
			}
			return Activator.CreateInstance(proxyType, this.GetConstructorArguments(null, interceptors, options).ToArray());
		}

		public override object CreateInterfaceProxyWithTarget(Type interfaceToProxy, Type[] additionalInterfacesToProxy, object target, ProxyGenerationOptions options, params IInterceptor[] interceptors) {
			if (interfaceToProxy == null) {
				throw new ArgumentNullException("interfaceToProxy");
			}
			if (target == null) {
				throw new ArgumentNullException("target");
			}
			if (interceptors == null) {
				throw new ArgumentNullException("interceptors");
			}
			if (!interfaceToProxy.IsInterface) {
				throw new ArgumentException("Specified type is not an interface", "interfaceToProxy");
			}
			Type type = target.GetType();
			if (!interfaceToProxy.IsAssignableFrom(type)) {
				throw new ArgumentException("Target does not implement interface " + interfaceToProxy.FullName, "target");
			}
			this.CheckNotGenericTypeDefinition(interfaceToProxy, "interfaceToProxy");
			this.CheckNotGenericTypeDefinitions(additionalInterfacesToProxy, "additionalInterfacesToProxy");
			Type proxyTypeWithTarget;
			lock(this._cachedProxyTypes.SyncRoot) {
				proxyTypeWithTarget = this._cachedProxyTypes[type] as Type;
				if (proxyTypeWithTarget == null) {
					proxyTypeWithTarget = this.CreateInterfaceProxyTypeWithTarget(interfaceToProxy, additionalInterfacesToProxy, type, options);
					this._cachedProxyTypes[type] = proxyTypeWithTarget;
				}
			}
			return Activator.CreateInstance(proxyTypeWithTarget, this.GetConstructorArguments(target, interceptors, options).ToArray());
		}

		[SecuritySafeCritical]
		public override object CreateInterfaceProxyWithTargetInterface(Type interfaceToProxy, Type[] additionalInterfacesToProxy, object target, ProxyGenerationOptions options, params IInterceptor[] interceptors) {
			if (target != null && !interfaceToProxy.IsInstanceOfType(target)) {
				throw new ArgumentException("targetType");
			}
			if (interfaceToProxy == null) {
				throw new ArgumentNullException("interfaceToProxy");
			}
			if (interceptors == null) {
				throw new ArgumentNullException("interceptors");
			}
			if (!interfaceToProxy.IsInterface) {
				throw new ArgumentException("Specified type is not an interface", "interfaceToProxy");
			}
			bool flag = false;
			if (target != null && !interfaceToProxy.IsInstanceOfType(target)) {
				if (RemotingServices.IsTransparentProxy(target)) {
					var remotingTypeInfo = RemotingServices.GetRealProxy(target) as IRemotingTypeInfo;
					if (remotingTypeInfo != null) {
						if (!remotingTypeInfo.CanCastTo(interfaceToProxy, target)) {
							throw new ArgumentException("Target does not implement interface " + interfaceToProxy.FullName, "target");
						}
						flag = true;
					}
				}
				else {
					if (!Marshal.IsComObject(target)) {
						throw new ArgumentException("Target does not implement interface " + interfaceToProxy.FullName, "target");
					}
					Guid guid = interfaceToProxy.GUID;
					if (guid != Guid.Empty) {
						IntPtr iunknownForObject = Marshal.GetIUnknownForObject(target);
						IntPtr ppv = IntPtr.Zero;
						if (Marshal.QueryInterface(iunknownForObject, ref guid, out ppv) == 0 && ppv == IntPtr.Zero) {
							throw new ArgumentException("Target COM object does not implement interface " + interfaceToProxy.FullName, "target");
						}
					}
				}
			}
			this.CheckNotGenericTypeDefinition(interfaceToProxy, "interfaceToProxy");
			this.CheckNotGenericTypeDefinitions(additionalInterfacesToProxy, "additionalInterfacesToProxy");
			Type withTargetInterface;
			var type = target == null ? interfaceToProxy : target.GetType();
			lock (_cachedProxyTypes.SyncRoot) {
				withTargetInterface = _cachedProxyTypes[type] as Type;
				if (withTargetInterface == null) {
					withTargetInterface = this.CreateInterfaceProxyTypeWithTargetInterface(interfaceToProxy, additionalInterfacesToProxy, options);
					_cachedProxyTypes[type] = withTargetInterface;
				}
			}
			List<object> constructorArguments = this.GetConstructorArguments(target, interceptors, options);
			return flag ? withTargetInterface.GetConstructors()[0].Invoke(constructorArguments.ToArray()) : Activator.CreateInstance(withTargetInterface, constructorArguments.ToArray());
		}
	}
}
