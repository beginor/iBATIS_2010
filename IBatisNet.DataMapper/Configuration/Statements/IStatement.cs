
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

using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.Cache;
using IBatisNet.DataMapper.Configuration.Sql;
#endregion

namespace IBatisNet.DataMapper.Configuration.Statements
{
    /// <summary>
    /// Summary description for ISql.
    /// </summary>
    public interface IStatement
    {

        #region Properties

        /// <summary>
        /// Allow remapping of dynamic SQL
        /// </summary>
        bool AllowRemapping
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier used to identify the statement amongst the others.
        /// </summary>
        string Id
        {
            get;
            set;
        }

        /// <summary>
        /// The type of the statement (text or procedure).
        /// </summary>
        CommandType CommandType
        {
            get;
        }

        /// <summary>
        /// Extend statement attribute
        /// </summary>
        string ExtendStatement
        {
            get;
            set;
        }

        /// <summary>
        /// The sql statement to execute.
        /// </summary>
        ISql Sql
        {
            get;
            set;
        }

        /// <summary>
        /// The ResultMaps used by the statement.
        /// </summary>
        ResultMapCollection ResultsMap
        {
            get;
        }


        /// <summary>
        /// The parameterMap used by the statement.
        /// </summary>
        ParameterMap ParameterMap
        {
            get;
            set;
        }

        /// <summary>
        /// The CacheModel used by this statement.
        /// </summary>
        CacheModel CacheModel
        {
            get;
            set;
        }

        /// <summary>
        /// The CacheModel name to use.
        /// </summary>
        string CacheModelName
        {
            get;
            set;
        }

        /// <summary>
        /// The list class type to use for strongly typed collection.
        /// </summary>
        Type ListClass
        {
            get;
        }

        /// <summary>
        /// The result class type to used.
        /// </summary>
        Type ResultClass
        {
            get;
        }

        /// <summary>
        /// The parameter class type to used.
        /// </summary>
        Type ParameterClass
        {
            get;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Create an instance of 'IList' class.
        /// </summary>
        /// <returns>An object which implement IList.</returns>
        IList CreateInstanceOfListClass();
#if dotnet2
        /// <summary>
        /// Create an instance of a generic 'IList' class.
        /// </summary>
        /// <returns>An object which implement IList.</returns>
        IList<T> CreateInstanceOfGenericListClass<T>();
#endif
        #endregion

    }
}
