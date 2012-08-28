#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 707150 $
 * $Date: 2008-10-22 11:54:18 -0600 (Wed, 22 Oct 2008) $
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
using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.Scope;
#endregion 


namespace IBatisNet.DataMapper.Configuration.Serializers
{
	/// <summary>
	/// Summary description for CacheModelDeSerializer.
	/// </summary>
	public sealed class CacheModelDeSerializer
	{
		/// <summary>
		/// Deserialize a CacheModel object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static CacheModel Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			CacheModel model = new CacheModel();

			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
			model.Id = NodeUtils.GetStringAttribute(prop, "id");
			model.Implementation = NodeUtils.GetStringAttribute(prop, "implementation");
			model.Implementation = configScope.SqlMapper.TypeHandlerFactory.GetTypeAlias(model.Implementation).Class.AssemblyQualifiedName;
			model.IsReadOnly = NodeUtils.GetBooleanAttribute(prop, "readOnly", true);
			model.IsSerializable = NodeUtils.GetBooleanAttribute(prop, "serialize", false);

			int count = node.ChildNodes.Count;
			for(int i=0;i<count;i++)
			{
				if (node.ChildNodes[i].LocalName=="flushInterval")
				{
					FlushInterval flush = new FlushInterval();
					NameValueCollection props = NodeUtils.ParseAttributes(node.ChildNodes[i], configScope.Properties);
					flush.Hours = NodeUtils.GetIntAttribute(props, "hours", 0);
					flush.Milliseconds = NodeUtils.GetIntAttribute(props, "milliseconds", 0);
					flush.Minutes = NodeUtils.GetIntAttribute(props, "minutes", 0);
					flush.Seconds = NodeUtils.GetIntAttribute(props, "seconds", 0);
					
					model.FlushInterval = flush;
				}
			}

			return model;
		}
	}
}
