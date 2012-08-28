#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 408164 $
 * $Date: 2006-05-21 06:27:09 -0600 (Sun, 21 May 2006) $
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
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Scope;
#endregion 

namespace IBatisNet.DataMapper.Configuration.Serializers
{
	/// <summary>
	/// Summary description for DiscriminatorDeSerializer.
	/// </summary>
	public sealed class DiscriminatorDeSerializer
	{
		/// <summary>
		/// Deserialize a ResultMap object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static Discriminator Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			Discriminator discriminator = new Discriminator();

			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
			discriminator.CallBackName = NodeUtils.GetStringAttribute(prop, "typeHandler");
			discriminator.CLRType = NodeUtils.GetStringAttribute(prop, "type");
			discriminator.ColumnIndex = NodeUtils.GetIntAttribute( prop, "columnIndex", ResultProperty.UNKNOWN_COLUMN_INDEX  );
			discriminator.ColumnName = NodeUtils.GetStringAttribute(prop, "column");
			discriminator.DbType = NodeUtils.GetStringAttribute(prop, "dbType");
			discriminator.NullValue = prop["nullValue"];

			return discriminator;
		}
	}
}
