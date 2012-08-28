
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 501527 $
 * $Date: 2007-01-30 12:32:11 -0700 (Tue, 30 Jan 2007) $
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
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using IBatisNet.Common.Logging;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.Configuration.Alias;
using IBatisNet.DataMapper.Exceptions;
#if dotnet2
using IBatisNet.DataMapper.TypeHandlers.Nullables;
#endif
#endregion 

namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Not much of a suprise, this is a factory class for TypeHandler objects.
	/// </summary>
	public class TypeHandlerFactory
	{

		#region Fields
		
		private static readonly ILog _logger = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );
		private IDictionary _typeHandlerMap = new HybridDictionary();
		private ITypeHandler _unknownTypeHandler = null;
		private const string NULL = "_NULL_TYPE_";
		//(typeAlias name, type alias)
		private IDictionary _typeAliasMaps = new HybridDictionary();
		#endregion 

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public TypeHandlerFactory() 
		{
			ITypeHandler handler = null;

			handler = new DBNullTypeHandler();
			this.Register(typeof(DBNull), handler);

			handler = new BooleanTypeHandler();
			this.Register(typeof(bool), handler); // key= "System.Boolean"

			handler = new ByteTypeHandler();
			this.Register(typeof(Byte), handler);

			handler = new CharTypeHandler();
			this.Register(typeof(Char), handler);

			handler = new DateTimeTypeHandler();
			this.Register(typeof(DateTime), handler);

			handler = new DecimalTypeHandler();
			this.Register(typeof(Decimal), handler);

			handler = new DoubleTypeHandler();
			this.Register(typeof(Double), handler);

			handler = new Int16TypeHandler();
			this.Register(typeof(Int16), handler);

			handler = new Int32TypeHandler();
			this.Register(typeof(Int32), handler);

			handler = new Int64TypeHandler();
			this.Register(typeof(Int64), handler);

			handler = new SingleTypeHandler();
			this.Register(typeof(Single), handler);

			handler = new StringTypeHandler();
			this.Register(typeof(String), handler);

			handler = new GuidTypeHandler();
			this.Register(typeof(Guid), handler);

			handler = new TimeSpanTypeHandler();
			this.Register(typeof(TimeSpan), handler);

			handler = new ByteArrayTypeHandler();
			this.Register(typeof(Byte[]), handler);

			handler = new ObjectTypeHandler();
			this.Register(typeof(object), handler);

			handler = new EnumTypeHandler();
			this.Register( typeof(System.Enum), handler);

            handler = new UInt16TypeHandler();
            this.Register(typeof(UInt16), handler);

            handler = new UInt32TypeHandler();
            this.Register(typeof(UInt32), handler);

            handler = new UInt64TypeHandler();
            this.Register(typeof(UInt64), handler);

            handler = new SByteTypeHandler();
            this.Register(typeof(SByte), handler);
		    
#if dotnet2
            handler = new NullableBooleanTypeHandler();
            this.Register(typeof(bool?), handler);

            handler = new NullableByteTypeHandler();
            this.Register(typeof(byte?), handler);

            handler = new NullableCharTypeHandler();
            this.Register(typeof(char?), handler);

            handler = new NullableDateTimeTypeHandler();
            this.Register(typeof(DateTime?), handler);

            handler = new NullableDecimalTypeHandler();
            this.Register(typeof(decimal?), handler);

            handler = new NullableDoubleTypeHandler();
            this.Register(typeof(double?), handler);

            handler = new NullableGuidTypeHandler();
            this.Register(typeof(Guid?), handler);

            handler = new NullableInt16TypeHandler();
            this.Register(typeof(Int16?), handler);
            
            handler = new NullableInt32TypeHandler();
            this.Register(typeof(Int32?), handler);

            handler = new NullableInt64TypeHandler();
            this.Register(typeof(Int64?), handler);

            handler = new NullableSingleTypeHandler();
            this.Register(typeof(Single?), handler);

            handler = new NullableUInt16TypeHandler();
            this.Register(typeof(UInt16?), handler);

            handler = new NullableUInt32TypeHandler();
            this.Register(typeof(UInt32?), handler);

            handler = new NullableUInt64TypeHandler();
            this.Register(typeof(UInt64?), handler);

            handler = new NullableSByteTypeHandler();
            this.Register(typeof(SByte?), handler);

            handler = new NullableTimeSpanTypeHandler();
            this.Register(typeof(TimeSpan?), handler);

#endif

            _unknownTypeHandler = new UnknownTypeHandler(this);

		}

		#endregion 

		#region Methods

		/// <summary>
		/// Get a TypeHandler for a Type
		/// </summary>
		/// <param name="type">the Type you want a TypeHandler for</param>
		/// <returns>the handler</returns>
		public ITypeHandler GetTypeHandler(Type type)
		{
			return GetTypeHandler(type, null);
		}

		/// <summary>
		/// Get a TypeHandler for a type
		/// </summary>
		/// <param name="type">the type you want a TypeHandler for</param>
		/// <param name="dbType">the database type</param>
		/// <returns>the handler</returns>
		public ITypeHandler GetTypeHandler(Type type, string dbType) 
		{
			if (type.IsEnum)
			{
				return this.GetPrivateTypeHandler(typeof(System.Enum), dbType);
			}
			else
			{
				return this.GetPrivateTypeHandler(type, dbType);
			}
		}

		/// <summary>
		///  Get a TypeHandler for a type and a dbType type
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="dbType">the dbType type</param>
		/// <returns>the handler</returns>
		private ITypeHandler GetPrivateTypeHandler(Type type, string dbType) 
		{
            IDictionary dbTypeHandlerMap = (IDictionary)_typeHandlerMap[type];
			ITypeHandler handler = null;

			if (dbTypeHandlerMap != null) 
			{
				if (dbType==null)
				{
					handler = (ITypeHandler) dbTypeHandlerMap[ NULL ];
				}
				else
				{
					handler = (ITypeHandler) dbTypeHandlerMap[ dbType ];
					if (handler == null) 
					{
						handler = (ITypeHandler) dbTypeHandlerMap[ NULL ];
					}					
				}
				if (handler==null)
				{
					throw new DataMapperException(String.Format("Type handler for {0} not registered.",type.Name));
				}
			}

			return handler;
		}


		/// <summary>
		/// Register (add) a type handler for a type
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="handler">the handler instance</param>
		public void Register(Type type, ITypeHandler handler) 
		{
			this.Register(type, null, handler);
		}

		/// <summary>
		/// Register (add) a type handler for a type and dbType
		/// </summary>
		/// <param name="type">the type</param>
		/// <param name="dbType">the dbType (optional, if dbType is null the handler will be used for all dbTypes)</param>
		/// <param name="handler">the handler instance</param>
		public void Register(Type type, string dbType, ITypeHandler handler) 
		{
			HybridDictionary map = (HybridDictionary) _typeHandlerMap[ type ];
			if (map == null) 
			{
				map = new HybridDictionary();
				_typeHandlerMap.Add(type, map)  ;
			}
			if (dbType==null)
			{
				if (_logger.IsInfoEnabled)
				{
					// notify the user that they are no longer using one of the built-in type handlers
					ITypeHandler oldTypeHandler = (ITypeHandler)map[NULL];

					if (oldTypeHandler != null)
					{
						// the replacement will always(?) be a CustomTypeHandler
						CustomTypeHandler customTypeHandler = handler as CustomTypeHandler;
						
						string replacement = string.Empty;
						
						if (customTypeHandler != null)
						{
							// report the underlying type
							replacement = customTypeHandler.Callback.ToString();
						}
						else
						{
							replacement = handler.ToString();
						}

						// should oldTypeHandler be checked if its a CustomTypeHandler and if so report the Callback property ???
						_logger.Info("Replacing type handler [" + oldTypeHandler.ToString() + "] with [" + replacement + "].");
					}
				}

				map[NULL] = handler;
			}
			else
			{
				map.Add(dbType, handler);
			}
		}

		/// <summary>
		/// When in doubt, get the "unknown" type handler
		/// </summary>
		/// <returns>if I told you, it would not be unknown, would it?</returns>
		public ITypeHandler GetUnkownTypeHandler() 
		{
			return _unknownTypeHandler;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool IsSimpleType(Type type) 
		{
			bool result = false;
			if (type != null) 
			{
				ITypeHandler handler = this.GetTypeHandler(type, null);
				if (handler != null) 
				{
					result = handler.IsSimpleType;
				}
			}
			return result;
		}

		#endregion

		/// <summary>
		/// Gets a named TypeAlias from the list of available TypeAlias
		/// </summary>
		/// <param name="name">The name of the TypeAlias.</param>
		/// <returns>The TypeAlias.</returns>
		internal TypeAlias GetTypeAlias(string name) 
		{
			if (_typeAliasMaps.Contains(name) == true) 
			{
				return (TypeAlias) _typeAliasMaps[name];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Gets the type object from the specific class name.
		/// </summary>
		/// <param name="className">The supplied class name.</param>
		/// <returns>The correpsonding type.
		/// </returns>
		internal Type GetType(string className) 
		{
			Type type = null;
			TypeAlias typeAlias = this.GetTypeAlias(className) as TypeAlias;

			if (typeAlias != null)
			{
				type = typeAlias.Class;
			}
			else
			{
                type = TypeUtils.ResolveType(className);
			}

			return type;
		}

		/// <summary>
		/// Adds a named TypeAlias to the list of available TypeAlias.
		/// </summary>
		/// <param name="key">The key name.</param>
		/// <param name="typeAlias"> The TypeAlias.</param>
		internal void AddTypeAlias(string key, TypeAlias typeAlias) 
		{
			if (_typeAliasMaps.Contains(key) == true) 
			{
				throw new DataMapperException(" Alias name conflict occurred.  The type alias '" + key + "' is already mapped to the value '"+typeAlias.ClassName+"'.");
			}
			_typeAliasMaps.Add(key, typeAlias);
		}
	}
}
