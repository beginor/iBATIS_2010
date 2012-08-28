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
	/// Summary description for SelectDeSerializer.
	/// </summary>
	public sealed class SelectDeSerializer
	{
		/// <summary>
		/// Deserialize a Procedure object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static Select Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			Select select = new Select();
			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
						
			select.CacheModelName = NodeUtils.GetStringAttribute(prop, "cacheModel");
			select.ExtendStatement = NodeUtils.GetStringAttribute(prop, "extends");
			select.Id = NodeUtils.GetStringAttribute(prop, "id");
			select.ListClassName = NodeUtils.GetStringAttribute(prop, "listClass");
			select.ParameterClassName = NodeUtils.GetStringAttribute(prop, "parameterClass");
			select.ParameterMapName = NodeUtils.GetStringAttribute(prop, "parameterMap");
			select.ResultClassName = NodeUtils.GetStringAttribute(prop, "resultClass");
			select.ResultMapName = NodeUtils.GetStringAttribute(prop, "resultMap");
			select.AllowRemapping = NodeUtils.GetBooleanAttribute(prop, "remapResults", false); 

			int count = node.ChildNodes.Count;
			for(int i=0;i<count;i++)
			{
				if (node.ChildNodes[i].LocalName=="generate")
				{
					Generate generate = new Generate();
					NameValueCollection props = NodeUtils.ParseAttributes(node.ChildNodes[i], configScope.Properties);
					
					generate.By = NodeUtils.GetStringAttribute(props, "by");
					generate.Table = NodeUtils.GetStringAttribute(props, "table");

					select.Generate = generate;
				}
			}
			return select;
		}
	}
}
