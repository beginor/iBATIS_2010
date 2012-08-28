#region Apache Notice
/*****************************************************************************
 * $Revision: 383115 $
 * $LastChangedDate: 2006-03-04 07:21:51 -0700 (Sat, 04 Mar 2006) $
 * $LastChangedBy: gbayon $
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
using IBatisNet.Common.Logging;
#endregion 

namespace IBatisNet.Common.Logging.Impl
{
	/// <remarks>
	/// Log4net is capable of outputting extended debug information about where the current 
	/// message was generated: class name, method name, file, line, etc. Log4net assumes that the location
	/// information should be gathered relative to where Debug() was called. In IBatisNet, 
	/// Debug() is called in IBatisNet.Common.Logging.Impl.Log4NetLogger. This means that
	/// the location information will indicate that IBatisNet.Common.Logging.Impl.Log4NetLogger always made
	/// the call to Debug(). We need to know where IBatisNet.Common.Logging.ILog.Debug()
	/// was called. To do this we need to use the log4net.ILog.Logger.Log method and pass in a Type telling
	/// log4net where in the stack to begin looking for location information.
	/// </remarks>
	public class Log4NetLogger : ILog
	{
		#region Fields

		private log4net.Core.ILogger _logger = null;
		private readonly static Type declaringType = typeof(Log4NetLogger);

		#endregion 

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="log"></param>
		internal Log4NetLogger(log4net.ILog log )
		{
			_logger = log.Logger;
		}

		#region ILog Members

		/// <summary>
		/// 
		/// </summary>
		public bool IsInfoEnabled
		{
			get { return _logger.IsEnabledFor(log4net.Core.Level.Info); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsWarnEnabled
		{
			get { return _logger.IsEnabledFor(log4net.Core.Level.Warn); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsErrorEnabled
		{
			get { return _logger.IsEnabledFor(log4net.Core.Level.Error); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsFatalEnabled
		{
			get { return _logger.IsEnabledFor(log4net.Core.Level.Fatal); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsDebugEnabled
		{
			get { return _logger.IsEnabledFor(log4net.Core.Level.Debug); }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsTraceEnabled
		{
			get { return _logger.IsEnabledFor(log4net.Core.Level.Trace); }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Info(object message, Exception e)
		{
			_logger.Log(declaringType, log4net.Core.Level.Info, message, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Info(object message)
		{
			_logger.Log(declaringType, log4net.Core.Level.Info, message, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Debug(object message, Exception e)
		{
			_logger.Log(declaringType, log4net.Core.Level.Debug, message, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Debug(object message)
		{
			_logger.Log(declaringType, log4net.Core.Level.Debug, message, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Warn(object message, Exception e)
		{
			_logger.Log(declaringType, log4net.Core.Level.Warn, message, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Warn(object message)
		{
			_logger.Log(declaringType, log4net.Core.Level.Warn, message, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Trace(object message, Exception e)
		{
			_logger.Log(declaringType, log4net.Core.Level.Trace, message, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Trace(object message)
		{
			_logger.Log(declaringType, log4net.Core.Level.Trace, message, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Fatal(object message, Exception e)
		{
			_logger.Log(declaringType, log4net.Core.Level.Fatal, message, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Fatal(object message)
		{
			_logger.Log(declaringType, log4net.Core.Level.Fatal, message, null);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public void Error(object message, Exception e)
		{
			_logger.Log(declaringType, log4net.Core.Level.Error, message, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public void Error(object message)
		{
			_logger.Log(declaringType, log4net.Core.Level.Error, message, null);
		}

		#endregion
	}
}