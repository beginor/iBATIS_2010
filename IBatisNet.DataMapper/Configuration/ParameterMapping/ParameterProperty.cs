
#region Apache Notice
/*****************************************************************************
 * $Revision: 575902 $
 * $LastChangedDate: 2007-09-15 04:40:19 -0600 (Sat, 15 Sep 2007) $
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
using System.Data;
using System.Reflection;
using System.Xml.Serialization;
using IBatisNet.Common.Exceptions;
using IBatisNet.Common.Utilities;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.Common.Utilities.Objects.Members;
using IBatisNet.DataMapper.Scope;
using IBatisNet.DataMapper.TypeHandlers;

#endregion

namespace IBatisNet.DataMapper.Configuration.ParameterMapping
{
	/// <summary>
	/// Summary description for ParameterProperty.
	/// </summary>
	[Serializable]
	[XmlRoot("parameter", Namespace="http://ibatis.apache.org/mapping")]
	public class ParameterProperty
	{

		#region Fields
		[NonSerialized]
		private string _nullValue = null;//string.Empty;//null;
		[NonSerialized]
		private string _propertyName = string.Empty;
		[NonSerialized]
		private ParameterDirection _direction = ParameterDirection.Input;
		[NonSerialized]
		private string _directionAttribute = string.Empty;
		[NonSerialized]
		private string _dbType = null;
		[NonSerialized]
		private int _size = -1;
		[NonSerialized]
		private byte _scale= 0;
		[NonSerialized]
		private byte _precision = 0;
		[NonSerialized]
		private string _columnName = string.Empty; // used only for store procedure
		[NonSerialized]
		private ITypeHandler _typeHandler = null;
		[NonSerialized]
		private string _clrType = string.Empty;
		[NonSerialized]
		private string _callBackName= string.Empty;
		[NonSerialized]
		private IGetAccessor _getAccessor = null;
		[NonSerialized]
		private bool _isComplexMemberName = false;

		#endregion

		#region Properties

		/// <summary>
		/// Indicate if we have a complex member name as [avouriteLineItem.Id]
		/// </summary>
		public bool IsComplexMemberName
		{
			get { return _isComplexMemberName; }
		}

		/// <summary>
		/// Specify the custom type handlers to used.
		/// </summary>
		/// <remarks>Will be an alias to a class wchic implement ITypeHandlerCallback</remarks>
		[XmlAttribute("typeHandler")]
		public string CallBackName
		{
			get { return _callBackName; }
			set { _callBackName = value; }
		}

		/// <summary>
		/// Specify the CLR type of the parameter.
		/// </summary>
		/// <remarks>
		/// The type attribute is used to explicitly specify the property type to be read.
		/// Normally this can be derived from a property through reflection, but certain mappings such as
		/// HashTable cannot provide the type to the framework.
		/// </remarks>
		[XmlAttribute("type")]
		public string CLRType
		{
			get { return _clrType; }
			set { _clrType = value; }
		}

		/// <summary>
		/// The typeHandler used to work with the parameter.
		/// </summary>
		[XmlIgnore]
		public ITypeHandler TypeHandler
		{
			get { return _typeHandler; }
			set { _typeHandler = value; }
		}

		/// <summary>
		/// Column Name for output parameter 
		/// in store proccedure.
		/// </summary>
		[XmlAttribute("column")]
		public string ColumnName
		{
			get { return _columnName; }
			set { _columnName = value; }
		}

		/// <summary>
		/// Column size.
		/// </summary>
		[XmlAttribute("size")]
		public int Size
		{
			get { return _size; }
			set { _size = value; }
		}

		/// <summary>
		/// Column Scale.
		/// </summary>
		[XmlAttribute("scale")]
		public byte Scale
		{
			get { return _scale; }
			set { _scale = value; }
		}

		/// <summary>
		/// Column Precision.
		/// </summary>
		[XmlAttribute("precision")]
		public byte Precision
		{
			get { return _precision; }
			set { _precision = value; }
		}
		/// <summary>
		/// Give an entry in the 'DbType' enumeration
		/// </summary>
		/// <example >
		/// For Sql Server, give an entry of SqlDbType : Bit, Decimal, Money...
		/// <br/>
		/// For Oracle, give an OracleType Enumeration : Byte, Int16, Number...
		/// </example>
		[XmlAttribute("dbType")]
		public string DbType
		{
			get { return _dbType; }
			set { _dbType = value; }
		}

		/// <summary>
		/// The direction attribute of the XML parameter.
		/// </summary>
		/// <example> Input, Output, InputOutput</example>
		[XmlAttribute("direction")]
		public string DirectionAttribute
		{
			get { return _directionAttribute; }
			set { _directionAttribute = value; }
		}

		/// <summary>
		/// Indicate the direction of the parameter.
		/// </summary>
		/// <example> Input, Output, InputOutput</example>
		[XmlIgnore]
		public ParameterDirection Direction
		{
			get { return _direction; }
			set 
			{ 
				_direction = value;
				_directionAttribute = _direction.ToString();
			}
		}

		/// <summary>
		/// Property name used to identify the property amongst the others.
		/// </summary>
		/// <example>EmailAddress</example>
		[XmlAttribute("property")]
		public string PropertyName
		{
			get { return _propertyName; }
			set 
			{ 
				if ((value == null) || (value.Length < 1))
					throw new ArgumentNullException("The property attribute is mandatory in a paremeter property.");

				_propertyName = value; 
				if (_propertyName.IndexOf('.')<0)
				{
					_isComplexMemberName = false;
				}
				else // complex member name FavouriteLineItem.Id
				{
					_isComplexMemberName = true;
				}
			}
		}

		/// <summary>
		/// Tell if a nullValue is defined._nullValue!=null
		/// </summary>
		[XmlIgnore]
		public bool HasNullValue
		{
			get { return (_nullValue!=null); }
		}

		/// <summary>
		/// Null value replacement.
		/// </summary>
		/// <example>"no_email@provided.com"</example>
		[XmlAttribute("nullValue")]
		public string NullValue
		{
			get { return _nullValue; }
			set { _nullValue = value; }
		}

		/// <summary>
		/// Defines a field/property get accessor
		/// </summary>
		[XmlIgnore]
        public IGetAccessor GetAccessor
		{
            get { return _getAccessor; }
		}

		#endregion

		#region Methods

        /// <summary>
        /// Initializes the parameter property
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="parameterClass">The parameter class.</param>
		public void Initialize(IScope scope, Type parameterClass)
		{

			if(_directionAttribute.Length >0)
			{
				_direction = (ParameterDirection)Enum.Parse( typeof(ParameterDirection), _directionAttribute, true );
			}

			if (!typeof(IDictionary).IsAssignableFrom(parameterClass) // Hashtable parameter map
				&& parameterClass !=null // value property
				&& !scope.DataExchangeFactory.TypeHandlerFactory.IsSimpleType(parameterClass) ) // value property
			{
				if (!_isComplexMemberName)
				{
                    IGetAccessorFactory getAccessorFactory = scope.DataExchangeFactory.AccessorFactory.GetAccessorFactory;
                    _getAccessor = getAccessorFactory.CreateGetAccessor(parameterClass, _propertyName);
				}
				else // complex member name FavouriteLineItem.Id
				{
				    string memberName = _propertyName.Substring( _propertyName.LastIndexOf('.')+1);
                    string parentName = _propertyName.Substring(0,_propertyName.LastIndexOf('.'));
                    Type parentType = ObjectProbe.GetMemberTypeForGetter(parameterClass, parentName);

                    IGetAccessorFactory getAccessorFactory = scope.DataExchangeFactory.AccessorFactory.GetAccessorFactory;
                    _getAccessor = getAccessorFactory.CreateGetAccessor(parentType, memberName);
				}
			}

			scope.ErrorContext.MoreInfo = "Check the parameter mapping typeHandler attribute '" + this.CallBackName + "' (must be a ITypeHandlerCallback implementation).";
			if (this.CallBackName.Length >0)
			{
				try 
				{
                    Type type = scope.DataExchangeFactory.TypeHandlerFactory.GetType(this.CallBackName);
					ITypeHandlerCallback typeHandlerCallback = (ITypeHandlerCallback) Activator.CreateInstance( type );
					_typeHandler = new CustomTypeHandler(typeHandlerCallback);
				}
				catch (Exception e) 
				{
					throw new ConfigurationException("Error occurred during custom type handler configuration.  Cause: " + e.Message, e);
				}
			}
			else
			{
				if (this.CLRType.Length == 0 )  // Unknown
				{
                    if (_getAccessor!= null &&
                        scope.DataExchangeFactory.TypeHandlerFactory.IsSimpleType(_getAccessor.MemberType)) 
					{
						// Primitive
                        _typeHandler = scope.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(_getAccessor.MemberType, _dbType);
					}
					else
					{
	                    _typeHandler = scope.DataExchangeFactory.TypeHandlerFactory.GetUnkownTypeHandler();
					}
				}
				else // If we specify a CLR type, use it
				{
                    Type type = TypeUtils.ResolveType(this.CLRType);

                    if (scope.DataExchangeFactory.TypeHandlerFactory.IsSimpleType(type)) 
					{
						// Primitive
                        _typeHandler = scope.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(type, _dbType);
					}
					else
					{
						// .NET object
						type = ObjectProbe.GetMemberTypeForGetter(type, this.PropertyName);
						_typeHandler = scope.DataExchangeFactory.TypeHandlerFactory.GetTypeHandler(type, _dbType);
					}
				}
			}
		}




        /// <summary>
        /// Determines whether the specified <see cref="System.Object"></see> is equal to the current <see cref="System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"></see> to compare with the current <see cref="System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="System.Object"></see> is equal to the current <see cref="System.Object"></see>; otherwise, false.
        /// </returns>
		public override bool Equals(object obj) 
		{
			//Check for null and compare run-time types.
			if (obj == null || GetType() != obj.GetType()) return false;
			ParameterProperty p = (ParameterProperty)obj;
			return (this.PropertyName == p.PropertyName);
		}


        /// <summary>
        /// Serves as a hash function for a particular type. <see cref="System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="System.Object"></see>.
        /// </returns>
		public override int GetHashCode() 
		{
			return _propertyName.GetHashCode();
		}
		#endregion

        #region ICloneable Members

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>An <see cref="ParameterProperty"/></returns>
        public ParameterProperty Clone()
        {
            ParameterProperty property = new ParameterProperty();

            property.CallBackName = this.CallBackName;
            property.CLRType = this.CLRType;
            property.ColumnName = this.ColumnName;
            property.DbType = this.DbType;
            property.DirectionAttribute = this.DirectionAttribute;
            property.NullValue = this.NullValue;
            property.PropertyName = this.PropertyName;
            property.Precision = this.Precision;
            property.Scale = this.Scale;
            property.Size = this.Size;

            return property;
        }
        #endregion

    }
}
