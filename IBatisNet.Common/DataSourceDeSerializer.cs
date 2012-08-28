#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 512878 $
 * $Date: 2007-02-28 10:57:11 -0700 (Wed, 28 Feb 2007) $
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

using System.Collections.Specialized;
using System.Xml;
using IBatisNet.Common.Xml;
#endregion 

namespace IBatisNet.Common
{
	/// <summary>
	/// Summary description for DataSourceDeSerializer.
	/// </summary>
	public sealed class DataSourceDeSerializer
	{
		/// <summary>
		/// Deserialize a DataSource object
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public static DataSource Deserialize(XmlNode node)
		{
			DataSource dataSource = new DataSource();
			NameValueCollection prop = NodeUtils.ParseAttributes(node);

			dataSource.ConnectionString = prop["connectionString"];
			dataSource.Name = prop["name"];
			
			return dataSource;
		}
	}
}
