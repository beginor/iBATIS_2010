
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 383115 $
 * $Date: 2006-03-04 07:21:51 -0700 (Sat, 04 Mar 2006) $
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
using System.Data;
using Castle.DynamicProxy;

#endregion

namespace IBatisNet.Common.Logging
{
	/// <summary>
	/// Summary description for IDbConnectionProxy.
	/// </summary>
	/// <remarks>Not used.</remarks>
	[CLSCompliant(false)]
	public class IDbConnectionProxy : IInterceptor
	{

		#region Fields
		private IDbConnection _connection = null;
		private Provider _provider = null;
		private static ArrayList _passthroughMethods = new ArrayList();
		private static readonly ILog _logger = LogManager.GetLogger( "System.Data.IDbConnection" );

		#endregion 

		#region Constructors

		static IDbConnectionProxy()
		{
			_passthroughMethods.Add("GetType");
			_passthroughMethods.Add("ToString");
		}

		/// <summary>
		/// Constructor for a connection proxy
		/// </summary>
		/// <param name="connection">The connection which been proxified.</param>
		/// <param name="provider">The provider used</param>
		internal IDbConnectionProxy(IDbConnection connection, Provider provider)
		{
			_connection = connection;
			_provider = provider;
		}
		#endregion 

		#region Methods

		/// <summary>
		/// Static constructor
		/// </summary>
		/// <param name="connection">The connection which been proxified.</param>
		/// <param name="provider">The provider used</param>
		/// <returns>A proxy</returns>
		internal static IDbConnection NewInstance(IDbConnection connection, Provider provider)
		{
			object proxyConnection = null;
			IInterceptor handler = new IDbConnectionProxy(connection, provider);

			ProxyGenerator proxyGenerator = new ProxyGenerator();

			proxyConnection = proxyGenerator.CreateProxy(typeof(IDbConnection), handler, connection);

			return (IDbConnection) proxyConnection;
		}
		#endregion 

		#region IInterceptor Members

		/// <summary>
		/// 
		/// </summary>
		/// <param name="invocation"></param>
		/// <param name="arguments"></param>
		/// <returns></returns>
		public object Intercept(IInvocation invocation, params object[] arguments)
		{
			object returnValue = null;

			if (invocation.Method.Name=="Open")
			{
				_connection.Open();
				if (_logger.IsDebugEnabled)
				{
					_logger.Debug( string.Format("Open Connection \"{0}\" to \"{1}\".", _connection.GetHashCode().ToString(), _provider.Description) );
				}				 
			}
			else if (invocation.Method.Name=="Close")
			{
				_connection.Close();
				if (_logger.IsDebugEnabled)
				{
					_logger.Debug( string.Format("Close Connection \"{0}\" to \"{1}\".", _connection.GetHashCode().ToString(), _provider.Description) );
				}				 
			}
			else //if ( !_passthroughMethods.Contains(invocation.Method.Name) )
			{
				returnValue = invocation.Method.Invoke( _connection, arguments);
			}

			return returnValue;
		}

		#endregion
	}
}
