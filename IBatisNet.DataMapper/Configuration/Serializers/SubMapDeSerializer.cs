#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 576082 $
 * $Date: 2007-09-16 06:04:01 -0600 (Sun, 16 Sep 2007) $
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
	/// Summary description for SubMapDeSerializer.
	/// </summary>
	public sealed class SubMapDeSerializer
	{
		/// <summary>
		/// Deserialize a ResultMap object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static SubMap Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
			string discriminatorValue = NodeUtils.GetStringAttribute(prop, "value");
            string resultMapName = configScope.ApplyNamespace(NodeUtils.GetStringAttribute(prop, "resultMapping"));

            return new SubMap(discriminatorValue, resultMapName);
		}
	}
}
