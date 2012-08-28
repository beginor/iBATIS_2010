
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
	/// TypeAlias.
	/// </summary>
	[Serializable]
	[XmlRoot("typeAlias", Namespace="http://ibatis.apache.org/dataMapper")]
	public class TypeAlias
	{

		#region Fields
		[NonSerialized]
		private string _name = string.Empty;
		[NonSerialized]
		private string _className = string.Empty;
		[NonSerialized]
		private Type _class = null;
		#endregion

		#region Properties
		/// <summary>
		/// Name used to identify the typeAlias amongst the others.
		/// </summary>
		/// <example> Account</example>
		[XmlAttribute("alias")]
		public string Name
		{
			get { return _name; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
				{
					throw new ArgumentNullException("The name attribute is mandatory in the typeAlias ");
				}
				_name = value; 
			}
		}


		/// <summary>
		/// The type class for the typeAlias
		/// </summary>
		[XmlIgnore]
		public Type Class
		{
			get { return _class; }
		}
	

		/// <summary>
		/// The class name to identify the typeAlias.
		/// </summary>
		/// <example>Com.Site.Domain.Product</example>
		[XmlAttribute("type")]
		public string ClassName
		{
			get { return _className; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
				{
					throw new ArgumentNullException("The class attribute is mandatory in the typeAlias " + _name);
				}
				_className = value; 
			}
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public TypeAlias()
		{}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="type">a type.</param>
		public TypeAlias(Type type)
		{
			_class = type;
		}
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
