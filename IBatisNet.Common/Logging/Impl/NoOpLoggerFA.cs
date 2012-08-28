
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 474141 $
 * $Date: 2006-11-12 21:43:37 -0700 (Sun, 12 Nov 2006) $
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

using System;
using System.Collections.Specialized;

namespace IBatisNet.Common.Logging.Impl
{
	/// <summary>
	/// Factory for creating "no operation" loggers that do nothing and whose Is*Enabled properties always 
	/// return false.
	/// </summary>
	/// <remarks>
	/// This factory creates a single instance of <see cref="NoOpLogger" /> and always returns that 
	/// instance whenever an <see cref="ILog" /> instance is requested.
	/// </remarks>
	public sealed class NoOpLoggerFA : ILoggerFactoryAdapter
	{
		private ILog _nopLogger = new NoOpLogger();

		/// <summary>
		/// Constructor
		/// </summary>
		public NoOpLoggerFA()
		{
			// empty
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public NoOpLoggerFA(NameValueCollection properties)
		{
			// empty
		}

		#region ILoggerFactoryAdapter Members

		/// <summary>
		/// Get a ILog instance by type 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public ILog GetLogger(Type type)
		{
			return _nopLogger;
		}

		/// <summary>
		/// Get a ILog instance by type name 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		ILog ILoggerFactoryAdapter.GetLogger(string name)
		{
			return _nopLogger;

		}

		#endregion
	}
}
