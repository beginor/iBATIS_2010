
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 452570 $
 * $Date: 2006-10-03 11:00:01 -0600 (Tue, 03 Oct 2006) $
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

using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using IBatisNet.Common.Logging;

namespace IBatisNet.Common.Utilities
{

	/// <summary>
	/// Represents the method that handles calls from Configure.
	/// </summary>
	/// <remarks>
	/// obj is a null object in a DaoManager context.
	/// obj is the reconfigured sqlMap in a SqlMap context.
	/// </remarks>
	public delegate void ConfigureHandler(object obj);

	/// <summary>
	/// 
	/// </summary>
	public struct StateConfig
	{
		/// <summary>
		/// Master Config File name.
		/// </summary>
		public string FileName;
		/// <summary>
		/// Delegate called when a file is changed, use it to rebuild.
		/// </summary>
		public ConfigureHandler ConfigureHandler;
	}

	/// <summary>
	/// Class used to watch config files.
	/// </summary>
	/// <remarks>
	/// Uses the <see cref="FileSystemWatcher"/> to monitor
	/// changes to a specified file. Because multiple change notifications
	/// may be raised when the file is modified, a timer is used to
	/// compress the notifications into a single event. The timer
	/// waits for the specified time before delivering
	/// the event notification. If any further <see cref="FileSystemWatcher"/>
	/// change notifications arrive while the timer is waiting it
	/// is reset and waits again for the specified time to
	/// elapse.
	/// </remarks>
	public sealed class ConfigWatcherHandler
	{
		#region Fields
		private static readonly ILog _logger = LogManager.GetLogger( System.Reflection.MethodBase.GetCurrentMethod().DeclaringType );

		/// <summary>
		/// The timer used to compress the notification events.
		/// </summary>
		private Timer _timer = null;

		/// <summary>
		/// A list of configuration files to watch.
		/// </summary>
		private static ArrayList _filesToWatch = new ArrayList();

		/// <summary>
		/// The list of FileSystemWatcher.
		/// </summary>
		private static ArrayList _filesWatcher = new ArrayList();

		/// <summary>
		/// The default amount of time to wait after receiving notification
		/// before reloading the config file.
		/// </summary>
		private const int TIMEOUT_MILLISECONDS = 500;
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		///-
		/// </summary>
		/// <param name="state">
		/// Represent the call context of the SqlMap or DaoManager ConfigureAndWatch method call.
		/// </param>
		/// <param name="onWhatchedFileChange"></param>
		public ConfigWatcherHandler(TimerCallback onWhatchedFileChange, StateConfig state)
		{
			for(int index = 0; index < _filesToWatch.Count; index++)
			{
				FileInfo configFile = (FileInfo)_filesToWatch[index];

                AttachWatcher(configFile);
                
				// Create the timer that will be used to deliver events. Set as disabled
                // callback  : A TimerCallback delegate representing a method to be executed. 
                // state : An object containing information to be used by the callback method, or a null reference 
                // dueTime : The amount of time to delay before callback is invoked, in milliseconds. Specify Timeout.Infinite to prevent the timer from starting. Specify zero (0) to start the timer immediately
                // period : The time interval between invocations of callback, in milliseconds. Specify Timeout.Infinite to disable periodic signaling
			    _timer = new Timer(onWhatchedFileChange, state, Timeout.Infinite, Timeout.Infinite);
			}
		}
		#endregion

		#region Methods

        private void AttachWatcher(FileInfo configFile)
	    {				
            // Create a new FileSystemWatcher and set its properties.
            FileSystemWatcher watcher = new FileSystemWatcher();

            watcher.Path = configFile.DirectoryName;
            watcher.Filter = configFile.Name;

            // Set the notification filters
            watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.LastWrite | NotifyFilters.FileName;

            // Add event handlers. OnChanged will do for all event handlers that fire a FileSystemEventArgs
            watcher.Changed += new FileSystemEventHandler(ConfigWatcherHandler_OnChanged);
            watcher.Created += new FileSystemEventHandler(ConfigWatcherHandler_OnChanged);
            watcher.Deleted += new FileSystemEventHandler(ConfigWatcherHandler_OnChanged);
            watcher.Renamed += new RenamedEventHandler(ConfigWatcherHandler_OnRenamed);

            // Begin watching.
            watcher.EnableRaisingEvents = true;

            _filesWatcher.Add(watcher);
	    }
	    
		/// <summary>
		/// Add a file to be monitored.
		/// </summary>
		/// <param name="configFile"></param>
		public static void AddFileToWatch(FileInfo configFile)
		{
			if (_logger.IsDebugEnabled)
			{
				// TODO: remove Path.GetFileName?
				_logger.Debug("Adding file [" + Path.GetFileName(configFile.FullName) + "] to list of watched files.");
			}

			_filesToWatch.Add( configFile );
		}

		/// <summary>
		/// Reset the list of files being monitored.
		/// </summary>
		public static void ClearFilesMonitored()
		{
			_filesToWatch.Clear();
			
			// Kill all FileSystemWatcher
			for(int index = 0; index < _filesWatcher.Count; index++)
			{
				FileSystemWatcher fileWatcher = (FileSystemWatcher)_filesWatcher[index];

				fileWatcher.EnableRaisingEvents = false;
				fileWatcher.Dispose();
			}
		}

		/// <summary>
		/// Event handler used by <see cref="ConfigWatcherHandler"/>.
		/// </summary>
		/// <param name="source">The <see cref="FileSystemWatcher"/> firing the event.</param>
		/// <param name="e">The argument indicates the file that caused the event to be fired.</param>
		/// <remarks>
		/// This handler reloads the configuration from the file when the event is fired.
		/// </remarks>
		private void ConfigWatcherHandler_OnChanged(object source, FileSystemEventArgs e)
		{
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("ConfigWatcherHandler : "+e.ChangeType+" [" + e.Name + "]");
			}

			// timer will fire only once
			_timer.Change(TIMEOUT_MILLISECONDS, Timeout.Infinite);
		}

		/// <summary>
		/// Event handler used by <see cref="ConfigWatcherHandler"/>.
		/// </summary>
		/// <param name="source">The <see cref="FileSystemWatcher"/> firing the event.</param>
		/// <param name="e">The argument indicates the file that caused the event to be fired.</param>
		/// <remarks>
		/// This handler reloads the configuration from the file when the event is fired.
		/// </remarks>
		private void ConfigWatcherHandler_OnRenamed(object source, RenamedEventArgs e)
		{
			if (_logger.IsDebugEnabled)
			{
				_logger.Debug("ConfigWatcherHandler : " + e.ChangeType + " [" + e.OldName + "/" +e.Name +"]");
			}

			// timer will fire only once
			_timer.Change(TIMEOUT_MILLISECONDS, Timeout.Infinite);
		}
		#endregion

	}
}
