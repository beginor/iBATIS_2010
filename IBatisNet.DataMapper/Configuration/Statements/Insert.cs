
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

#region Imports
using System;
using System.Xml.Serialization;

using IBatisNet.DataMapper.Exceptions;
#endregion

namespace IBatisNet.DataMapper.Configuration.Statements
{
	/// <summary>
	/// Represent an insert statement.
	/// </summary>
	[Serializable]
	[XmlRoot("insert", Namespace="http://ibatis.apache.org/mapping")]
	public class Insert : Statement
	{
		#region Fields
		[NonSerialized]
		private SelectKey _selectKey = null;
		[NonSerialized]
		private Generate _generate = null;
		#endregion

		#region Properties
		/// <summary>
		/// Extend statement attribute
		/// </summary>
		[XmlIgnoreAttribute]
		public override string ExtendStatement
		{
			get { return string.Empty;  }
			set {  }
		}

		/// <summary>
		/// The selectKey statement used by an insert statement.
		/// </summary>
		[XmlElement("selectKey",typeof(SelectKey))]
		public SelectKey SelectKey
		{
			get { return _selectKey; }
			set { _selectKey = value; }
		}

		/// <summary>
		/// The Generate tag used by a generated insert statement.
		/// (CRUD operation)
		/// </summary>
		[XmlElement("generate",typeof(Generate))]
		public Generate Generate
		{
			get { return _generate; }
			set { _generate = value; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public Insert():base()
		{
		}
		#endregion


	}
}
