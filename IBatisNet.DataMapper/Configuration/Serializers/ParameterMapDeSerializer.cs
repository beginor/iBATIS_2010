#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 410610 $
 * $Date: 2006-05-31 11:40:07 -0600 (Wed, 31 May 2006) $
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
	/// Summary description for ParameterMapDeSerializer.
	/// </summary>
	public sealed class ParameterMapDeSerializer
	{
		/// <summary>
		/// Deserialize a ParameterMap object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static ParameterMap Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			ParameterMap parameterMap = new ParameterMap(configScope.DataExchangeFactory);
			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);

			configScope.ErrorContext.MoreInfo = "ParameterMap DeSerializer";

			parameterMap.ExtendMap = NodeUtils.GetStringAttribute(prop, "extends");
			parameterMap.Id =  NodeUtils.GetStringAttribute(prop, "id");
			parameterMap.ClassName = NodeUtils.GetStringAttribute(prop, "class");

			configScope.ErrorContext.MoreInfo = "Initialize ParameterMap";
			configScope.NodeContext = node;
			parameterMap.Initialize( configScope.DataSource.DbProvider.UsePositionalParameters, configScope );
			parameterMap.BuildProperties(configScope);
			configScope.ErrorContext.MoreInfo = string.Empty;

			return parameterMap;
		}
	}
}
