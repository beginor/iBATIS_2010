
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 638571 $
 * $Date: 2008-03-18 15:11:57 -0600 (Tue, 18 Mar 2008) $
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
using System.Data;
using System.Xml.Serialization;

namespace IBatisNet.DataMapper.Configuration.Statements
{
	/// <summary>
	/// Represent a generate tag element.
	/// The generation would happen at the point where the 
	/// SqlMapClient instance is built.
	/// </summary>
	[Serializable]
	[XmlRoot("generate", Namespace="http://ibatis.apache.org/mapping")]
	public class Generate : Statement
	{
		#region Fields

		[NonSerialized]
		private string _table = string.Empty;
		[NonSerialized]
		private string _by = string.Empty;

		#endregion

		/// <summary>
		/// The table name used to build the SQL query. 
		/// </summary>
		/// <remarks>
		/// Will be used to get the metadata to build the SQL if needed.
		/// </remarks>
		[XmlAttribute("table")]
		public string Table
		{
			get { return _table; }
			set { _table = value; }
		}

		/// <summary>
		/// The by attribute is used to generate the where clause.
		/// </summary>
		/// <remarks>The by="" attribute can support multiple colums.</remarks>
		/// <example> 
		///		&lt; delete ...&gt;
		///			&lt;generate table="EMPLOYEE" by="EMPLOYEE_ID, LAST_MOD_DATE" /&gt;
		///		&lt;/delete&gt;
		/// </example>
		[XmlAttribute("by")]
		public string By
		{
			get { return _by; }
			set { _by = value; }
		}

		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public Generate():base(){}


	}
}
