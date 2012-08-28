#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 513429 $
 * $Date: 2007-03-01 11:32:25 -0700 (Thu, 01 Mar 2007) $
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

using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;

namespace IBatisNet.DataMapper
{
	/// <summary>
	/// A singleton class to access the default SqlMapper defined by the SqlMap.Config
	/// </summary>
	public sealed class Mapper
	{
		#region Fields
        private static volatile ISqlMapper _mapper = null;
		#endregion

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		public static void Configure (object obj)
		{
			_mapper = null;
		}

		/// <summary>
		/// Init the 'default' SqlMapper defined by the SqlMap.Config file.
		/// </summary>
		public static void InitMapper()
		{
			ConfigureHandler handler = new ConfigureHandler (Configure);
			DomSqlMapBuilder builder = new DomSqlMapBuilder();
			_mapper = builder.ConfigureAndWatch (handler);		}

		/// <summary>
		/// Get the instance of the SqlMapper defined by the SqlMap.Config file.
		/// </summary>
		/// <returns>A SqlMapper initalized via the SqlMap.Config file.</returns>
        public static ISqlMapper Instance()
		{
			if (_mapper == null)
			{
				lock (typeof (SqlMapper))
				{
					if (_mapper == null) // double-check
					{	
						InitMapper();
					}
				}
			}
			return _mapper;
		}
		
		/// <summary>
		/// Get the instance of the SqlMapper defined by the SqlMap.Config file. (Convenience form of Instance method.)
		/// </summary>
		/// <returns>A SqlMapper initalized via the SqlMap.Config file.</returns>
        public static ISqlMapper Get()
		{
			return Instance();
		}
	}
}
