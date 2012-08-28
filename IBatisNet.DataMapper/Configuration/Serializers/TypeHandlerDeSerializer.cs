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

using System;
using System.Collections.Specialized;
using System.Xml;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;
using IBatisNet.Common.Xml;
using IBatisNet.DataMapper.Configuration.Alias;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.TypeHandlers;

#endregion 

namespace IBatisNet.DataMapper.Configuration.Serializers
{
	/// <summary>
	/// Summary description for TypeHandlerDeSerializer.
	/// </summary>
	public sealed class TypeHandlerDeSerializer
	{
		/// <summary>
		/// Deserialize a TypeHandler object
		/// </summary>
		/// <param name="node"></param>
		/// <param name="configScope"></param>
		/// <returns></returns>
		public static void Deserialize(XmlNode node, ConfigurationScope configScope)
		{
			TypeHandler handler = new TypeHandler();

			NameValueCollection prop = NodeUtils.ParseAttributes(node, configScope.Properties);
			handler.CallBackName = NodeUtils.GetStringAttribute(prop, "callback");
			handler.ClassName = NodeUtils.GetStringAttribute(prop, "type");
			handler.DbType = NodeUtils.GetStringAttribute(prop, "dbType");

			handler.Initialize();

			configScope.ErrorContext.MoreInfo = "Check the callback attribute '" + handler.CallBackName + "' (must be a classname).";
			ITypeHandler typeHandler = null;
			Type type = configScope.SqlMapper.TypeHandlerFactory.GetType(handler.CallBackName);
			object impl = Activator.CreateInstance( type );
			if (impl is ITypeHandlerCallback) 
			{
				typeHandler = new CustomTypeHandler((ITypeHandlerCallback) impl);
			} 
			else if (impl is ITypeHandler) 
			{
				typeHandler = (ITypeHandler) impl;
			} 
			else 
			{
				throw new ConfigurationException("The callBack type is not a valid implementation of ITypeHandler or ITypeHandlerCallback");
			}

			// 
			configScope.ErrorContext.MoreInfo = "Check the type attribute '" + handler.ClassName + "' (must be a class name) or the dbType '" + handler.DbType + "' (must be a DbType type name).";
			if (handler.DbType!= null && handler.DbType.Length > 0) 
			{
                configScope.DataExchangeFactory.TypeHandlerFactory.Register(TypeUtils.ResolveType(handler.ClassName), handler.DbType, typeHandler);
			} 
			else 
			{
                configScope.DataExchangeFactory.TypeHandlerFactory.Register(TypeUtils.ResolveType(handler.ClassName), typeHandler);
			}
		}
	}
}
