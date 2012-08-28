#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 474141 $
 * $Date: 2006-11-12 23:43:37 -0500 (Sun, 12 Nov 2006) $
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

namespace IBatisNet.Common.Logging.Impl
{
	/// <summary>
	/// Base class that implements the ILog interface.
	/// </summary>
	public abstract class AbstractLogger : ILog
	{
		/// <summary>
		/// Concrete classes should override this method to perform the actual logging.
		/// </summary>
		/// <param name="logLevel"></param>
		/// <param name="message"></param>
		/// <param name="e"></param>
		protected abstract void Write(LogLevel logLevel, object message, Exception e);

		/// <summary>
		/// Concrete classes should override this method to determine if a particular <see cref="LogLevel" />
		/// is enabled.
		/// </summary>
		/// <param name="logLevel"></param>
		/// <returns></returns>
		protected abstract bool IsLevelEnabled(LogLevel logLevel);
		
		#region ILog Members

		/// <summary>
		/// Log a <see cref="LogLevel.Debug" /> message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		public void Debug(object message)
		{
			Debug( message, null );
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Debug" /> message with an optional <see cref="Exception" />.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">
		/// The	<see cref="Exception" /> associated with the message. If there isn't any
		/// <see cref="Exception" /> associated with the message, pass <see langword="null" />.
		/// </param>
		public void Debug(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Debug ) )
			{
				Write( LogLevel.Debug, message, e );	
			}
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Error" /> message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		public void Error(object message)
		{
			Error( message, null );
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Error" /> message with an optional <see cref="Exception" />.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">
		/// The	<see cref="Exception" /> associated with the message. If there isn't any
		/// <see cref="Exception" /> associated with the message, pass <see langword="null" />.
		/// </param>
		public void Error(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Error ) )
			{
				Write( LogLevel.Error, message, e );	
			}
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Fatal" /> message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		public void Fatal(object message)
		{
			Fatal( message, null );
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Fatal" /> message with an optional <see cref="Exception" />.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">
		/// The	<see cref="Exception" /> associated with the message. If there isn't any
		/// <see cref="Exception" /> associated with the message, pass <see langword="null" />.
		/// </param>
		public void Fatal(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Fatal ) )
			{
				Write( LogLevel.Fatal, message, e );
			}
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Info" /> message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		public void Info(object message)
		{
			Info( message, null );
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Info" /> message with an optional <see cref="Exception" />.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">
		/// The	<see cref="Exception" /> associated with the message. If there isn't any
		/// <see cref="Exception" /> associated with the message, pass <see langword="null" />.
		/// </param>
		public void Info(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Info ) )
			{
				Write( LogLevel.Info, message, e );
			}
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Warn" /> message.
		/// </summary>
		/// <param name="message">The message to log.</param>
		public void Warn(object message)
		{
			Warn( message, null );
		}

		/// <summary>
		/// Log a <see cref="LogLevel.Warn" /> message with an optional <see cref="Exception" />.
		/// </summary>
		/// <param name="message">The message to log.</param>
		/// <param name="e">
		/// The	<see cref="Exception" /> associated with the message. If there isn't any
		/// <see cref="Exception" /> associated with the message, pass <see langword="null" />.
		/// </param>
		public void Warn(object message, Exception e)
		{
			if ( IsLevelEnabled( LogLevel.Warn ) )
			{
				Write( LogLevel.Warn, message, e );
			}
		}

		/// <summary>
		/// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
		/// equal to <see cref="LogLevel.Debug" />. If it is, all messages will be sent to <see cref="Console.Out" />.
		/// </summary>
		public bool IsDebugEnabled
		{
			get { return IsLevelEnabled( LogLevel.Debug ); }
		}

		/// <summary>
		/// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
		/// equal to <see cref="LogLevel.Error" />. If it is, only messages with a <see cref="LogLevel" /> of
		/// <see cref="LogLevel.Error" /> and <see cref="LogLevel.Fatal" /> will be sent to <see cref="Console.Out" />.
		/// </summary>
		public bool IsErrorEnabled
		{
			get { return IsLevelEnabled( LogLevel.Error ); }
		}

		/// <summary>
		/// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
		/// equal to <see cref="LogLevel.Fatal" />. If it is, only messages with a <see cref="LogLevel" /> of
		/// <see cref="LogLevel.Fatal" /> will be sent to <see cref="Console.Out" />.
		/// </summary>
		public bool IsFatalEnabled
		{
			get { return IsLevelEnabled( LogLevel.Fatal ); }
		}

		/// <summary>
		/// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
		/// equal to <see cref="LogLevel.Info" />. If it is, only messages with a <see cref="LogLevel" /> of
		/// <see cref="LogLevel.Info" />, <see cref="LogLevel.Warn" />, <see cref="LogLevel.Error" />, and 
		/// <see cref="LogLevel.Fatal" /> will be sent to <see cref="Console.Out" />.
		/// </summary>
		public bool IsInfoEnabled
		{
			get { return IsLevelEnabled( LogLevel.Info ); }
		}


		/// <summary>
		/// Returns <see langword="true" /> if the current <see cref="LogLevel" /> is greater than or
		/// equal to <see cref="LogLevel.Warn" />. If it is, only messages with a <see cref="LogLevel" /> of
		/// <see cref="LogLevel.Warn" />, <see cref="LogLevel.Error" />, and <see cref="LogLevel.Fatal" /> 
		/// will be sent to <see cref="Console.Out" />.
		/// </summary>
		public bool IsWarnEnabled
		{
			get { return IsLevelEnabled( LogLevel.Warn ); }
		}

		#endregion
	}
}
