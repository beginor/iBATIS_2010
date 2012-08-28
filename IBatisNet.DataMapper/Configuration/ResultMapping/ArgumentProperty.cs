
#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-05-20 15:56:36 -0600 (Sat, 20 May 2006) $
 * $LastChangedBy: gbayon $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006/2005 - The Apache Software Foundation
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
#if dotnet2
using System.Configuration;
#endif
using System.Reflection;
using System.Xml.Serialization;

using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;
using IBatisNet.DataMapper.MappedStatements.ArgumentStrategy;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.TypeHandlers;

#endregion


namespace IBatisNet.DataMapper.Configuration.ResultMapping
{
	/// <summary>
	/// Summary description for ArgumentProperty.
	/// </summary>
	[Serializable]
	[XmlRoot("argument", Namespace="http://ibatis.apache.org/mapping")]
	public class ArgumentProperty : ResultProperty
	{

		#region Fields
		[NonSerialized]
		private string _argumentName = string.Empty;
		[NonSerialized]
		private Type _argumentType = null;
		[NonSerialized]
		private IArgumentStrategy _argumentStrategy = null;
		#endregion

		#region Properties

		/// <summary>
		/// Sets or gets the <see cref="IArgumentStrategy"/> used to fill the object property.
		/// </summary>
		[XmlIgnore]
		public override IArgumentStrategy ArgumentStrategy
		{
			set { _argumentStrategy = value ; }
			get { return _argumentStrategy ; }
		}

		/// <summary>
		/// Specify the constructor argument name.
		/// </summary>
		[XmlAttribute("argumentName")]
		public string ArgumentName
		{
			get { return _argumentName; }
			set
			{
				if ((value == null) || (value.Length < 1))
				{
					throw new ArgumentNullException("The name attribute is mandatory in a argument tag.");				
				}
				_argumentName = value;
			}
		}

		/// <summary>
		/// Tell us if we must lazy load this property..
		/// </summary>
		[XmlAttribute("lazyLoad")]
		public override bool IsLazyLoad
		{
			get { return false; }
			set { throw new InvalidOperationException("Argument property cannot be lazy load."); }
		}

		/// <summary>
		/// Get the argument type
		/// </summary>
		[XmlIgnore]
		public override Type MemberType
		{
			get { return _argumentType; }
		}

		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
		/// Do not use direclty, only for serialization.
		/// </summary>
		public ArgumentProperty()
		{
		}
		#endregion

		#region Methods

		/// <summary>
		/// Initialize the argument property.
		/// </summary>
		/// <param name="constructorInfo"></param>
		/// <param name="configScope"></param>
		public void Initialize( ConfigurationScope configScope, ConstructorInfo constructorInfo )
		{
            // Search argument by his name to set his type
			ParameterInfo[] parameters = constructorInfo.GetParameters();

			bool found = false;
			for(int i =0; i< parameters.Length; i++)
			{
				found = (parameters[ i ].Name == _argumentName);
				if( found )
				{
					_argumentType = parameters[ i ].ParameterType;
					break;
				}
			}
			if (this.CallBackName!=null && this.CallBackName.Length >0)
			{
				configScope.ErrorContext.MoreInfo = "Argument property ("+_argumentName+"), check the typeHandler attribute '" + this.CallBackName + "' (must be a ITypeHandlerCallback implementation).";
				try 
				{
					Type type = configScope.SqlMapper.TypeHandlerFactory.GetType(this.CallBackName);
					ITypeHandlerCallback typeHandlerCallback = (ITypeHandlerCallback) Activator.CreateInstance( type );
					this.TypeHandler = new CustomTypeHandler(typeHandlerCallback);
				}
				catch (Exception e) 
				{
#if dotnet2
                    throw new ConfigurationErrorsException("Error occurred during custom type handler configuration.  Cause: " + e.Message, e);
#else       
					throw new ConfigurationException("Error occurred during custom type handler configuration.  Cause: " + e.Message, e);
#endif
				}
			}
			else
			{
				configScope.ErrorContext.MoreInfo = "Argument property ("+_argumentName+") set the typeHandler attribute.";	
				this.TypeHandler = this.ResolveTypeHandler(configScope, _argumentType, this.CLRType, this.DbType);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="configScope"></param>
		/// <param name="argumenType">The argument type</param>
		/// <param name="clrType"></param>
		/// <param name="dbType"></param>
		/// <returns></returns>
		public ITypeHandler ResolveTypeHandler(ConfigurationScope configScope, Type argumenType, string clrType, string dbType)
		{
			ITypeHandler handler = null;
			if (argumenType==null)
			{
				handler = configScope.DataExchangeFactory.TypeHandlerFactory.GetUnkownTypeHandler();
			}
			else if (typeof(IDictionary).IsAssignableFrom(argumenType)) 
			{
				// IDictionary
				if (clrType ==null ||clrType.Length == 0) 
				{
					handler = configScope.DataExchangeFactory.TypeHandlerFactory.GetUnkownTypeHandler(); 
				} 
				else 
				{
					try 
					{
                        Type type = TypeUtils.ResolveType(clrType);
						handler = configScope.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(type, dbType);
					} 
					catch (Exception e) 
					{
#if dotnet2
                        throw new ConfigurationErrorsException("Error. Could not set TypeHandler.  Cause: " + e.Message, e);
#else       
						throw new ConfigurationException("Error. Could not set TypeHandler.  Cause: " + e.Message, e);
#endif
					}
				}
			}
			else if (configScope.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(argumenType, dbType) != null) 
			{
				// Primitive
				handler = configScope.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(argumenType, dbType);
			}
			else 
			{
				// .NET object
				if (clrType ==null || clrType.Length == 0) 
				{
					handler =  configScope.DataExchangeFactory.TypeHandlerFactory.GetUnkownTypeHandler(); 
				} 
				else 
				{
					try 
					{
                        Type type = TypeUtils.ResolveType(clrType);
						handler = configScope.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(type, dbType);
					} 
					catch (Exception e) 
					{
#if dotnet2
                        throw new ConfigurationErrorsException("Error. Could not set TypeHandler.  Cause: " + e.Message, e);
#else       
						throw new ConfigurationException("Error. Could not set TypeHandler.  Cause: " + e.Message, e);
#endif
					}
				}
			}

			return handler;
		}
		#endregion
	}
}
