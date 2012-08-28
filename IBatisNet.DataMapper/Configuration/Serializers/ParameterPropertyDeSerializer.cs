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
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Scope;
#endregion 

namespace IBatisNet.DataMapper.Configuration.Serializers
{
	/// <summary>
	/// Summary description for ParameterPropertyDeSerializer.
	/// </summary>
	public sealed class ParameterPropertyDeSerializer
	{
		/// <summary>
		/// Deserialize a ResultMap object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static ParameterProperty Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			ParameterProperty property = new ParameterProperty();
			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);

			configScope.ErrorContext.MoreInfo = "ParameterPropertyDeSerializer";

			property.CallBackName = NodeUtils.GetStringAttribute(prop, "typeHandler");
			property.CLRType =  NodeUtils.GetStringAttribute(prop, "type");
			property.ColumnName =  NodeUtils.GetStringAttribute(prop, "column");
			property.DbType =  NodeUtils.GetStringAttribute(prop, "dbType", null);
			property.DirectionAttribute =  NodeUtils.GetStringAttribute(prop, "direction");
			property.NullValue =  prop["nullValue"];
			property.PropertyName =  NodeUtils.GetStringAttribute(prop, "property");
			property.Precision = NodeUtils.GetByteAttribute(prop, "precision", 0);
			property.Scale = NodeUtils.GetByteAttribute(prop, "scale", 0);
			property.Size = NodeUtils.GetIntAttribute(prop, "size", -1);

			return property;
		}
	}
}
