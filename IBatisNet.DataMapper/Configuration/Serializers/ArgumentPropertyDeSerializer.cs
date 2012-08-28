#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-05-21 06:27:09 -0600 (Sun, 21 May 2006) $
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

#region Using

using System.Collections.Specialized;
using System.Xml;
using IBatisNet.Common.Xml;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Scope;
#endregion 

namespace IBatisNet.DataMapper.Configuration.Serializers
{
	/// <summary>
	/// Summary description for ArgumentPropertyDeSerializer.
	/// </summary>
	public sealed class ArgumentPropertyDeSerializer
	{
		/// <summary>
		/// Deserialize a ResultProperty object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static ArgumentProperty Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			ArgumentProperty argumentProperty = new ArgumentProperty();

			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
			argumentProperty.CLRType = NodeUtils.GetStringAttribute(prop, "type");
			argumentProperty.CallBackName = NodeUtils.GetStringAttribute(prop, "typeHandler");
			argumentProperty.ColumnIndex = NodeUtils.GetIntAttribute( prop, "columnIndex", ResultProperty.UNKNOWN_COLUMN_INDEX  );
			argumentProperty.ColumnName = NodeUtils.GetStringAttribute(prop, "column");
			argumentProperty.DbType = NodeUtils.GetStringAttribute(prop, "dbType");
			argumentProperty.NestedResultMapName = NodeUtils.GetStringAttribute(prop, "resultMapping");
			argumentProperty.NullValue = prop["nullValue"];
			argumentProperty.ArgumentName = NodeUtils.GetStringAttribute(prop, "argumentName");
			argumentProperty.Select = NodeUtils.GetStringAttribute(prop, "select");

			return argumentProperty;
		}
	}
}
