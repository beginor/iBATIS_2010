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
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Xml;
using IBatisNet.DataMapper.Configuration.Statements;
using IBatisNet.DataMapper.Scope;
#endregion 

namespace IBatisNet.DataMapper.Configuration.Serializers
{
	/// <summary>
	/// Summary description for InsertDeSerializer.
	/// </summary>
	public sealed class InsertDeSerializer
	{
		/// <summary>
		/// Deserialize a TypeHandler object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static Insert Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			Insert insert = new Insert();
			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
			
			insert.CacheModelName = NodeUtils.GetStringAttribute(prop, "cacheModel");
			insert.ExtendStatement = NodeUtils.GetStringAttribute(prop, "extends");
			insert.Id = NodeUtils.GetStringAttribute(prop, "id");
			insert.ParameterClassName = NodeUtils.GetStringAttribute(prop, "parameterClass");
			insert.ParameterMapName = NodeUtils.GetStringAttribute(prop, "parameterMap");
			insert.ResultClassName = NodeUtils.GetStringAttribute(prop, "resultClass");
			insert.ResultMapName = NodeUtils.GetStringAttribute(prop, "resultMap");
			insert.AllowRemapping = NodeUtils.GetBooleanAttribute(prop, "remapResults", false); 

			int count = node.ChildNodes.Count;
			for(int i=0;i<count;i++)
			{
				if (node.ChildNodes[i].LocalName=="generate")
				{
					Generate generate = new Generate();
					NameValueCollection props = NodeUtils.ParseAttributes(node.ChildNodes[i], configScope.Properties);
					
					generate.By = NodeUtils.GetStringAttribute(props, "by");
					generate.Table = NodeUtils.GetStringAttribute(props, "table");

					insert.Generate = generate;
				}
				else if (node.ChildNodes[i].LocalName=="selectKey")
				{
					SelectKey selectKey = new SelectKey();
					NameValueCollection props = NodeUtils.ParseAttributes(node.ChildNodes[i], configScope.Properties);
					
					selectKey.PropertyName = NodeUtils.GetStringAttribute(props, "property");
					selectKey.SelectKeyType = InsertDeSerializer.ReadSelectKeyType( props["type"] );
					selectKey.ResultClassName = NodeUtils.GetStringAttribute(props, "resultClass");

					insert.SelectKey = selectKey;
				}
			}
			return insert;
		}

		private static SelectKeyType ReadSelectKeyType(string s) 
		{
			switch (s) 
			{
				case @"pre": return IBatisNet.DataMapper.SelectKeyType.@pre;
				case @"post": return IBatisNet.DataMapper.SelectKeyType.@post;
				default: throw new ConfigurationException("Unknown selectKey type : '"+s+"'");
			}
		}
	}
}
