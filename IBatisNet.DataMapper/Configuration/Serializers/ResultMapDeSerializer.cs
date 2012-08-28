#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 470514 $
 * $Date: 2006-11-02 13:46:13 -0700 (Thu, 02 Nov 2006) $
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
	/// Summary description for ResultMapDeSerializer.
	/// </summary>
	public sealed class ResultMapDeSerializer
	{
		/// <summary>
		/// Deserialize a ResultMap object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static ResultMap Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
            ResultMap resultMap = new ResultMap(configScope, prop["id"], prop["class"], prop["extends"], prop["groupBy"]);

			configScope.ErrorContext.MoreInfo = "initialize ResultMap";

			resultMap.Initialize( configScope );

			return resultMap;
		}
	}
}
