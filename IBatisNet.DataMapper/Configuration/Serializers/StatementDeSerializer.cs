#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 469233 $
 * $Date: 2006-10-30 12:09:11 -0700 (Mon, 30 Oct 2006) $
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
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.Scope;

#endregion 

namespace IBatisNet.DataMapper.Configuration.Serializers
{
	/// <summary>
	/// Summary description for StatementDeSerializer.
	/// </summary>
	public sealed class StatementDeSerializer
	{
		/// <summary>
		/// Deserialize a Procedure object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static Statement Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			Statement statement = new Statement();
			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
						
			statement.CacheModelName = NodeUtils.GetStringAttribute(prop, "cacheModel");
			statement.ExtendStatement = NodeUtils.GetStringAttribute(prop, "extends");
			statement.Id = NodeUtils.GetStringAttribute(prop, "id");
			statement.ListClassName = NodeUtils.GetStringAttribute(prop, "listClass");
			statement.ParameterClassName = NodeUtils.GetStringAttribute(prop, "parameterClass");
			statement.ParameterMapName = NodeUtils.GetStringAttribute(prop, "parameterMap");
			statement.ResultClassName = NodeUtils.GetStringAttribute(prop, "resultClass");
			statement.ResultMapName = NodeUtils.GetStringAttribute(prop, "resultMap");
			statement.AllowRemapping = NodeUtils.GetBooleanAttribute(prop, "remapResults", false); 

			return statement;
		}
	}
}
