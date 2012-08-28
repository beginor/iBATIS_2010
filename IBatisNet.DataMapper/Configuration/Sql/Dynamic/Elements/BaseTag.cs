
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
#endregion

namespace IBatisNet.DataMapper.Configuration.Sql.Dynamic.Elements
{
	/// <summary>
	/// Summary description for BaseTag.
	/// </summary>
	[Serializable]
	public abstract class BaseTag : SqlTag
	{
		#region Fields
		
		[NonSerialized]
		private string _property = string.Empty;

		#endregion

		/// <summary>
		/// Property attribute
		/// </summary>
		[XmlAttribute("property")]
		public string Property
		{
			get
			{
				return _property;
			}
			set
			{
				_property = value;
			}
		}
	}
}
