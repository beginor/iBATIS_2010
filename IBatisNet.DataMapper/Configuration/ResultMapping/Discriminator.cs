
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 469233 $
 * $Date: 2006-10-30 12:09:11 -0700 (Mon, 30 Oct 2006) $
 * Author : Gilles Bayon
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Apache Fondation
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
using System.Xml.Serialization;
using IBatisNet.DataMapper.MappedStatements.PropertyStrategy;
using IBatisNet.DataMapper.Scope;

#endregion

namespace IBatisNet.DataMapper.Configuration.ResultMapping
{
	/// <summary>
	/// Summary description for Discriminator.
	/// </summary>
	[Serializable]
	[XmlRoot("discriminator", Namespace="http://ibatis.apache.org/mapping")]
	public class Discriminator
	{

		#region Fields
		[NonSerialized]
		private ResultProperty _mapping = null;
		/// <summary>
		/// (discriminatorValue (string), ResultMap)
		/// </summary>
		[NonSerialized]
		private HybridDictionary _resultMaps = null;
		/// <summary>
		/// The subMaps name who used this discriminator
		/// </summary>
		[NonSerialized]
		private ArrayList _subMaps = null;

		[NonSerialized]
		private string _nullValue = string.Empty;
		[NonSerialized]
		private string _columnName = string.Empty;
		[NonSerialized]
		private int _columnIndex = ResultProperty.UNKNOWN_COLUMN_INDEX;
		[NonSerialized]
		private string _dbType = string.Empty;
		[NonSerialized]
		private string _clrType = string.Empty;
		[NonSerialized]
		private string _callBackName= string.Empty;
		#endregion 

		#region Properties

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
		/// Specify the CLR type of the result.
		/// </summary>
		/// <remarks>
		/// The type attribute is used to explicitly specify the property type of the property to be set.
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
		/// Column Index
		/// </summary>
		[XmlAttribute("columnIndex")]
		public int ColumnIndex
		{
			get { return _columnIndex; }
			set { _columnIndex = value; }
		}

		/// <summary>
		/// Column Name
		/// </summary>
		[XmlAttribute("column")]
		public string ColumnName
		{
			get { return _columnName; }
			set { _columnName = value; }
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
		/// Th underlying ResultProperty
		/// </summary>
		[XmlIgnore]
		public ResultProperty ResultProperty
		{
			get { return _mapping; }
		}
		#endregion 

		#region Constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public Discriminator()
		{
			_resultMaps = new HybridDictionary();
			_subMaps = new ArrayList();
		}
		#endregion 

		#region Methods

		/// <summary>
		/// Initilaize the underlying mapping
		/// </summary>
		/// <param name="configScope"></param>
		/// <param name="resultClass"></param>
		public void SetMapping(ConfigurationScope configScope, Type resultClass)
		{
			configScope.ErrorContext.MoreInfo = "Initialize discriminator mapping";
			_mapping = new ResultProperty();
			_mapping.ColumnName =  _columnName;
			_mapping.ColumnIndex = _columnIndex;
			_mapping.CLRType = _clrType;
			_mapping.CallBackName = _callBackName;
			_mapping.DbType = _dbType;
			_mapping.NullValue = _nullValue;

			_mapping.Initialize( configScope, resultClass );
		}

		/// <summary>
		/// Initialize the Discriminator
		/// </summary>
		/// <param name="configScope"></param>
		public void Initialize(ConfigurationScope configScope)
		{
			// Set the ResultMaps
			int count = _subMaps.Count;
			for(int index=0; index<count; index++)
			{
				SubMap subMap = _subMaps[index] as SubMap;
				_resultMaps.Add(subMap.DiscriminatorValue, configScope.SqlMapper.GetResultMap( subMap.ResultMapName ) );
			}
		}

		/// <summary>
		/// Add a subMap that the discrimator must treat
		/// </summary>
		/// <param name="subMap">A subMap</param>
		public void Add(SubMap subMap)
		{
			_subMaps.Add(subMap);
		}

		/// <summary>
		/// Find the SubMap to use.
		/// </summary>
		/// <param name="discriminatorValue">the discriminator value</param>
		/// <returns>The find ResultMap</returns>
		public IResultMap GetSubMap(string discriminatorValue)
		{
			return _resultMaps[discriminatorValue] as ResultMap;
		}

		#endregion 


	}
}
