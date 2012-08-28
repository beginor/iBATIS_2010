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
using IBatisNet.DataMapper.Configuration.Sql.Dynamic.Elements;
using IBatisNet.DataMapper.Scope;
#endregion 



namespace IBatisNet.DataMapper.Configuration.Serializers
{
	/// <summary>
	/// Summary description for IsEmptyDeSerializer.
	/// </summary>
	public sealed class IsEmptyDeSerializer : IDeSerializer
	{

		private ConfigurationScope _configScope = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="configScope"></param>
		public IsEmptyDeSerializer(ConfigurationScope configScope)
		{
			_configScope = configScope;
		}

		#region IDeSerializer Members

		/// <summary>
		/// Deserialize a IsNotNull object
		/// </summary>
		/// <param name="node"></param>
		/// <returns></returns>
		public SqlTag Deserialize(XmlNode node)
		{
			IsEmpty isEmpty = new IsEmpty(_configScope.DataExchangeFactory.AccessorFactory);

			NameValueCollection prop = NodeUtils.ParseAttributes(node, _configScope.Properties);
			isEmpty.Prepend = NodeUtils.GetStringAttribute(prop, "prepend");
			isEmpty.Property = NodeUtils.GetStringAttribute(prop, "property");

			return isEmpty;
		}

		#endregion
	}
}
