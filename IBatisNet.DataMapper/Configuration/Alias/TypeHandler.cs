
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 408099 $
 * $Date: 2006-05-20 15:56:36 -0600 (Sat, 20 May 2006) $
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
using System.Xml.Serialization;
using IBatisNet.Common.Utilities;

#endregion

namespace IBatisNet.DataMapper.Configuration.Alias
{
	/// <summary>
	/// Summary description for TypeHandler.
	/// </summary>
	[Serializable]
	[XmlRoot("typeHandler", Namespace="http://ibatis.apache.org/dataMapper")]
	public class TypeHandler
	{
		#region Fields
		[NonSerialized]
		private string _className = string.Empty;
		[NonSerialized]
		private Type _class = null;
		[NonSerialized]
		private string _dbType = string.Empty;
		[NonSerialized]
		private string _callBackName = string.Empty;
		#endregion

		#region Properties
		/// <summary>
		/// CLR type
		/// </summary>
		[XmlAttribute("type")]
		public string ClassName
		{
			get { return _className; }
			set {_className = value; }
		}

		/// <summary>
		/// The type class for the TypeName
		/// </summary>
		[XmlIgnore]
		public Type Class
		{
			get { return _class; }
		}
	
		/// <summary>
		/// dbType name
		/// </summary>
		[XmlAttribute("dbType")]
		public string DbType
		{
			get { return _dbType; }
			set {_dbType = value; }
		}


		/// <summary>
		/// callback alias name
		/// </summary>
		[XmlAttribute("callback")]
		public string CallBackName
		{
			get { return _callBackName; }
			set {_callBackName = value; }
		}

	
		#endregion

		#region Constructors

		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public TypeHandler()
		{}
		#endregion 

		#region Methods
		/// <summary>
		/// Initialize the object, 
		/// try to idenfify the .Net type class from the corresponding name.
		/// </summary>
		public void Initialize()
		{
            _class = TypeUtils.ResolveType(_className);
		}
		#endregion

	}
}
