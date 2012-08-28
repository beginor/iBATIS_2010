#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 638571 $
 * $Date: 2008-03-18 15:11:57 -0600 (Tue, 18 Mar 2008) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004-2005 - Apache Foundation
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
using Castle.DynamicProxy;
using IBatisNet.Common.Logging;

namespace IBatisNet.Common.Utilities.Proxy
{
	/// <summary>
	///  A Factory for getting the ProxyGenerator.
	/// </summary>
	[CLSCompliant(false)]
	public sealed class ProxyGeneratorFactory
	{
		private static readonly ILog log = LogManager.GetLogger( typeof( ProxyGeneratorFactory ) );

		private static ProxyGenerator _generator = new CachedProxyGenerator();

		private ProxyGeneratorFactory()
		{
			// should not be created.	
		}

		/// <summary></summary>
		public static ProxyGenerator GetProxyGenerator()
		{
			//TODO: make this read from a configuration file!!!  At this point anybody
			// could substitue in their own IProxyGenerator and LazyInitializer.
			return _generator;
		}
	}
}
