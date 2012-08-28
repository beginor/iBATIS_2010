
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
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.MappedStatements;
using IBatisNet.DataMapper.Scope;

#endregion

namespace IBatisNet.DataMapper.Configuration.Statements
{
	/// <summary>
	/// Represent a SelectKey tag element.
	/// </summary>
	[Serializable]
	[XmlRoot("selectKey", Namespace="http://ibatis.apache.org/mapping")]
	public class SelectKey : Statement 
	{

		#region Fields

		[NonSerialized]
		private SelectKeyType _selectKeyType = SelectKeyType.post;
		[NonSerialized]
		private string _property = string.Empty;

		#endregion

		#region Properties
		/// <summary>
		/// Extend statement attribute
		/// </summary>
		[XmlIgnore]
		public override string ExtendStatement
		{
			get { return string.Empty;  }
			set {  }
		}

		/// <summary>
		/// The property name object to fill with the key.
		/// </summary>
		[XmlAttribute("property")]
		public string PropertyName
		{
			get { return _property; }
			set { _property = value; }
		}

		/// <summary>
		/// The type of the selectKey tag : 'Pre' or 'Post'
		/// </summary>
		[XmlAttribute("type")]
		public SelectKeyType SelectKeyType
		{
			get { return _selectKeyType; }
			set { _selectKeyType = value; }
		}


		/// <summary>
		/// True if it is a post-generated key.
		/// </summary>
		[XmlIgnore]
		public bool isAfter
		{
			get { return _selectKeyType == SelectKeyType.post; }
		}
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public SelectKey():base()
		{
		}
		#endregion

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="configurationScope">The scope of the configuration</param>
		override internal void Initialize(ConfigurationScope configurationScope)
		{
			// the propertyName attribute on the selectKey node is optional
			if (PropertyName.Length > 0)
			{
				// Id is equal to the parent <select> node's "id" attribute
				IMappedStatement insert = configurationScope.SqlMapper.GetMappedStatement(Id);

				Type insertParameterClass = insert.Statement.ParameterClass;

				// make sure the PropertyName is a valid settable property of the <insert> node's parameterClass
				if (insertParameterClass != null && 
					ObjectProbe.IsSimpleType(insertParameterClass) == false)
				{
					configurationScope.ErrorContext.MoreInfo = String.Format("Looking for settable property named '{0}' on type '{1}' for selectKey node of statement id '{2}'.",
						PropertyName, // 0
						insert.Statement.ParameterClass.Name, // 1
						Id); // 2

					// we expect this to throw an exception if the property cannot be found; GetSetter is
					// called instead of HasWriteableProperty becuase we want the same wording for 
					// property not found exceptions; GetSetter and HasWritableProperty both use the 
					// same internal cache for looking up the ProperyInfo object
					ReflectionInfo.GetInstance(insert.Statement.ParameterClass).GetSetter(PropertyName);
				}
			}

			base.Initialize(configurationScope);
		}
		#endregion

	}
}
