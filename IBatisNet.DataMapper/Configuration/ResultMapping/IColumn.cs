#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-04-26 14:05:17 -0600 (Wed, 26 Apr 2006) $
 * $LastChangedBy: gbayon $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006/2005 - The Apache Software Foundation
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

using System.Xml.Serialization;
using IBatisNet.Common.Utilities.Objects.Members;

namespace IBatisNet.DataMapper.Configuration.ResultMapping
{
	/// <summary>
	/// Summary description for IColumn.
	/// </summary>
	public interface IColumn
	{
		/// <summary>
		/// Column Index
		/// </summary>
		[XmlAttribute("columnIndex")]
		int ColumnIndex { get; set; }

		/// <summary>
		/// Column Name
		/// </summary>
		[XmlAttribute("column")]
		string ColumnName { get; set; }


		/// <summary>
		/// Defines a field/property accessor
		/// </summary>
		[XmlIgnore]
		IMemberAccessor MemberAccessor{ get; }
	}
}