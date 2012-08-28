
#region Apache Notice
/*****************************************************************************
 * $Revision: 469233 $
 * $LastChangedDate: 2006-10-30 12:09:11 -0700 (Mon, 30 Oct 2006) $
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
using System.Collections.Generic;
#endif
using System.Data;
using System.Xml.Serialization;
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Configuration.Sql;
using IBatisNet.DataMapper.DataExchange;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.Scope;

#endregion

namespace IBatisNet.DataMapper.Configuration.Statements
{
    /// <summary>
    /// Summary description for Statement.
    /// </summary>
    [Serializable]
    [XmlRoot("statement", Namespace = "http://ibatis.apache.org/mapping")]
    public class Statement : IStatement
    {

        #region Fields

        [NonSerialized]
        private bool _allowRemapping = false;
        [NonSerialized]
        private string _id = string.Empty;
        // ResultMap
        [NonSerialized]
        private string _resultMapName = string.Empty;
        [NonSerialized]
        private ResultMapCollection _resultsMap = new ResultMapCollection();
        // ParameterMap
        [NonSerialized]
        private string _parameterMapName = string.Empty;
        [NonSerialized]
        private ParameterMap _parameterMap = null;
        // Result Class
        [NonSerialized]
        private string _resultClassName = string.Empty;
        [NonSerialized]
        private Type _resultClass = null;
        // Parameter Class
        [NonSerialized]
        private string _parameterClassName = string.Empty;
        [NonSerialized]
        private Type _parameterClass = null;
        // List Class
        [NonSerialized]
        private string _listClassName = string.Empty;
        [NonSerialized]
        private Type _listClass = null;
        // CacheModel
        [NonSerialized]
        private string _cacheModelName = string.Empty;
        [NonSerialized]
        private CacheModel _cacheModel = null;
        [NonSerialized]
        private ISql _sql = null;
        [NonSerialized]
        private string _extendStatement = string.Empty;
        [NonSerialized]
        private IFactory _listClassFactory = null;

        #endregion

        #region Properties

        /// <summary>
        /// Allow remapping of dynamic SQL
        /// </summary>
        [XmlAttribute("remapResults")]
        public bool AllowRemapping
        {
            get { return _allowRemapping; }
            set { _allowRemapping = value; }
        }

        /// <summary>
        /// Extend statement attribute
        /// </summary>
        [XmlAttribute("extends")]
        public virtual string ExtendStatement
        {
            get { return _extendStatement; }
            set { _extendStatement = value; }
        }

        /// <summary>
        /// The CacheModel name to use.
        /// </summary>
        [XmlAttribute("cacheModel")]
        public string CacheModelName
        {
            get { return _cacheModelName; }
            set { _cacheModelName = value; }
        }

        /// <summary>
        /// Tell us if a cacheModel is attached to this statement.
        /// </summary>
        [XmlIgnore]
        public bool HasCacheModel
        {
            get { return _cacheModelName.Length > 0; }
        }

        /// <summary>
        /// The CacheModel used by this statement.
        /// </summary>
        [XmlIgnore]
        public CacheModel CacheModel
        {
            get { return _cacheModel; }
            set { _cacheModel = value; }
        }

        /// <summary>
        /// The list class name to use for strongly typed collection.
        /// </summary>
        [XmlAttribute("listClass")]
        public string ListClassName
        {
            get { return _listClassName; }
            set { _listClassName = value; }
        }


        /// <summary>
        /// The list class type to use for strongly typed collection.
        /// </summary>
        [XmlIgnore]
        public Type ListClass
        {
            get { return _listClass; }
        }

        /// <summary>
        /// The result class name to used.
        /// </summary>
        [XmlAttribute("resultClass")]
        public string ResultClassName
        {
            get { return _resultClassName; }
            set { _resultClassName = value; }
        }

        /// <summary>
        /// The result class type to used.
        /// </summary>
        [XmlIgnore]
        public Type ResultClass
        {
            get { return _resultClass; }
        }

        /// <summary>
        /// The parameter class name to used.
        /// </summary>
        [XmlAttribute("parameterClass")]
        public string ParameterClassName
        {
            get { return _parameterClassName; }
            set { _parameterClassName = value; }
        }

        /// <summary>
        /// The parameter class type to used.
        /// </summary>
        [XmlIgnore]
        public Type ParameterClass
        {
            get { return _parameterClass; }
        }

        /// <summary>
        /// Name used to identify the statement amongst the others.
        /// </summary>
        [XmlAttribute("id")]
        public string Id
        {
            get { return _id; }
            set
            {
                if ((value == null) || (value.Length < 1))
                    throw new DataMapperException("The id attribute is required in a statement tag.");

                _id = value;
            }
        }


        /// <summary>
        /// The sql statement
        /// </summary>
        [XmlIgnore]
        public ISql Sql
        {
            get { return _sql; }
            set
            {
                if (value == null)
                    throw new DataMapperException("The sql statement query text is required in the statement tag " + _id);

                _sql = value;
            }
        }


        /// <summary>
        /// The ResultMaps name used by the statement.
        /// </summary>
        [XmlAttribute("resultMap")]
        public string ResultMapName
        {
            get { return _resultMapName; }
            set { _resultMapName = value; }
        }

        /// <summary>
        /// The ParameterMap name used by the statement.
        /// </summary>
        [XmlAttribute("parameterMap")]
        public string ParameterMapName
        {
            get { return _parameterMapName; }
            set { _parameterMapName = value; }
        }

        /// <summary>
        /// The ResultMap used by the statement.
        /// </summary>
        [XmlIgnore]
        public ResultMapCollection ResultsMap
        {
            get { return _resultsMap; }
        }

        /// <summary>
        /// The parameterMap used by the statement.
        /// </summary>
        [XmlIgnore]
        public ParameterMap ParameterMap
        {
            get { return _parameterMap; }
            set { _parameterMap = value; }
        }

        /// <summary>
        /// The type of the statement (text or procedure)
        /// Default Text.
        /// </summary>
        /// <example>Text or StoredProcedure</example>
        [XmlIgnore]
        public virtual CommandType CommandType
        {
            get { return CommandType.Text; }
        }
        #endregion


        #region Methods
        /// <summary>
        /// Initialize an statement for the sqlMap.
        /// </summary>
        /// <param name="configurationScope">The scope of the configuration</param>
        internal virtual void Initialize(ConfigurationScope configurationScope)
        {
            if (_resultMapName.Length > 0)
            {
                string[] names = _resultMapName.Split(',');
                for (int i = 0; i < names.Length; i++)
                {
                    string name = configurationScope.ApplyNamespace(names[i].Trim());
                    _resultsMap.Add( configurationScope.SqlMapper.GetResultMap(name) );
                }
            }
            if (_parameterMapName.Length > 0)
            {
                _parameterMap = configurationScope.SqlMapper.GetParameterMap(_parameterMapName);
            }
            if (_resultClassName.Length > 0)
            {
                string[] classNames = _resultClassName.Split(',');
                for (int i = 0; i < classNames.Length; i++)
                {
                    _resultClass = configurationScope.SqlMapper.TypeHandlerFactory.GetType(classNames[i].Trim());
                    IFactory resultClassFactory = null;
                    if (Type.GetTypeCode(_resultClass) == TypeCode.Object &&
                        (_resultClass.IsValueType == false))
                    {
                        resultClassFactory = configurationScope.SqlMapper.ObjectFactory.CreateFactory(_resultClass, Type.EmptyTypes);
                    }
                    IDataExchange dataExchange = configurationScope.DataExchangeFactory.GetDataExchangeForClass(_resultClass);
                    IResultMap autoMap = new AutoResultMap(_resultClass, resultClassFactory, dataExchange);
                    _resultsMap.Add(autoMap);
                }
                

            }
            if (_parameterClassName.Length > 0)
            {
                _parameterClass = configurationScope.SqlMapper.TypeHandlerFactory.GetType(_parameterClassName);
            }
            if (_listClassName.Length > 0)
            {
                _listClass = configurationScope.SqlMapper.TypeHandlerFactory.GetType(_listClassName);
                _listClassFactory = configurationScope.SqlMapper.ObjectFactory.CreateFactory(_listClass, Type.EmptyTypes);
            }
        }


        /// <summary>
        /// Create an instance of 'IList' class.
        /// </summary>
        /// <returns>An object which implment IList.</returns>
        public IList CreateInstanceOfListClass()
        {
            return (IList)_listClassFactory.CreateInstance(null); 
        }
#if dotnet2
        /// <summary>
        /// Create an instance of a generic 'IList' class.
        /// </summary>
        /// <returns>An object which implment IList.</returns>
        public IList<T> CreateInstanceOfGenericListClass<T>()
        {
            return (IList<T>)_listClassFactory.CreateInstance(null); 
        }
#endif
        #endregion

    }
}
