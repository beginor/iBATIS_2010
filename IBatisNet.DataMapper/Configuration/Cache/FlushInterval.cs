
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 707150 $
 * $Date: 2008-10-22 11:54:18 -0600 (Wed, 22 Oct 2008) $
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

#region Imports
using System;
using System.Xml.Serialization;
#endregion

namespace IBatisNet.DataMapper.Configuration.Cache
{
	/// <summary>
	/// Summary description for FlushInterval.
	/// </summary>
	[Serializable]
	[XmlRoot("flushInterval")]
	public class FlushInterval
	{
		
		#region Fields 

		private int _hours = 0;
		private int _minutes= 0;
		private int _seconds = 0;
		private int _milliseconds = 0;
		private long _interval = CacheModel.NO_FLUSH_INTERVAL;

		#endregion

		#region Properties
		/// <summary>
		/// Flush interval in hours
		/// </summary>
		[XmlAttribute("hours")]
		public int Hours
		{
			get 
			{
				return _hours;
			}
			set 
			{
				_hours = value;
			}
		}


		/// <summary>
		/// Flush interval in minutes
		/// </summary>
		[XmlAttribute("minutes")]
		public int Minutes
		{
			get 
			{
				return _minutes;
			}
			set 
			{
				_minutes = value;
			}
		}


		/// <summary>
		/// Flush interval in seconds
		/// </summary>
		[XmlAttribute("seconds")]
		public int Seconds
		{
			get 
			{
				return _seconds;
			}
			set 
			{
				_seconds = value;
			}
		}


		/// <summary>
		/// Flush interval in milliseconds
		/// </summary>
		[XmlAttribute("milliseconds")]
		public int Milliseconds
		{
			get 
			{
				return _milliseconds;
			}
			set 
			{
				_milliseconds = value;
			}
		}


		/// <summary>
		/// Get the flush interval value
		/// </summary>
		[XmlIgnoreAttribute]
		public long Interval
		{
			get 
			{
				return _interval;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Calcul the flush interval value in ticks
		/// </summary>
		public void Initialize()
		{
            long interval = 0;
			if (_milliseconds != 0) 
			{
                interval += (_milliseconds * TimeSpan.TicksPerMillisecond);
			}
			if (_seconds != 0) 
			{
                interval += (_seconds * TimeSpan.TicksPerSecond);
			}
			if (_minutes != 0) 
			{
                interval += (_minutes * TimeSpan.TicksPerMinute);
			}
			if (_hours != 0) 
			{
                interval += (_hours * TimeSpan.TicksPerHour);
			}

            if (interval == 0)
			{
                interval = CacheModel.NO_FLUSH_INTERVAL;
			}
		    _interval = interval;
		}
		#endregion

	}
}
