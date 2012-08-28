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
	/// Summary description for UpdateDeSerializer.
	/// </summary>
	public sealed class UpdateDeSerializer
	{
		/// <summary>
		/// Deserialize a Procedure object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static Update Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			Update update = new Update();
			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
						
			update.CacheModelName = NodeUtils.GetStringAttribute(prop, "cacheModel");
			update.ExtendStatement = NodeUtils.GetStringAttribute(prop, "extends");
			update.Id = NodeUtils.GetStringAttribute(prop, "id");
			update.ParameterClassName = NodeUtils.GetStringAttribute(prop, "parameterClass");
			update.ParameterMapName = NodeUtils.GetStringAttribute(prop, "parameterMap");
			update.AllowRemapping = NodeUtils.GetBooleanAttribute(prop, "remapResults", false); 

			int count = node.ChildNodes.Count;
			for(int i=0;i<count;i++)
			{
				if (node.ChildNodes[i].LocalName=="generate")
				{
					Generate generate = new Generate();
					NameValueCollection props = NodeUtils.ParseAttributes(node.ChildNodes[i], configScope.Properties);
					
					generate.By = NodeUtils.GetStringAttribute(props, "by");
					generate.Table = NodeUtils.GetStringAttribute(props, "table");

					update.Generate = generate;
				}
			}
			return update;
		}
	}
}
