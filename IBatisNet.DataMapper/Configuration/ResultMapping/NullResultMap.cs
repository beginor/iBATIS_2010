#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2007-02-21 13:23:49 -0700 (Wed, 21 Feb 2007) $
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

using System;
using System.Collections.Specialized;
using IBatisNet.DataMapper.DataExchange;

namespace IBatisNet.DataMapper.Configuration.ResultMapping
{
    /// <summary>
    /// 
    /// </summary>
    public class NullResultMap : IResultMap
    {
 
        #region Fields
        [NonSerialized]
        private StringCollection _groupByPropertyNames = new StringCollection();
        [NonSerialized]
        private ResultPropertyCollection _properties = new ResultPropertyCollection();
        [NonSerialized]
        private ResultPropertyCollection _parameters = new ResultPropertyCollection();
        [NonSerialized]
        private ResultPropertyCollection _groupByProperties = new ResultPropertyCollection();

        #endregion

        #region Properties

        /// <summary>
        /// The GroupBy Properties.
        /// </summary>
        public StringCollection GroupByPropertyNames
        {
            get { return _groupByPropertyNames; }
        }

        /// <summary>
        /// The GroupBy Properties.
        /// </summary>
        public ResultPropertyCollection GroupByProperties
        {
            get { return _groupByProperties; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is initalized.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is initalized; otherwise, <c>false</c>.
        /// </value>
        public bool IsInitalized
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// The discriminator used to choose the good SubMap
        /// </summary>
        public Discriminator Discriminator
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// The collection of ResultProperty.
        /// </summary>
        public ResultPropertyCollection Properties
        {
            get { return _properties; }
        }

        /// <summary>
        /// The collection of constructor parameters.
        /// </summary>
        public ResultPropertyCollection Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Identifier used to identify the resultMap amongst the others.
        /// </summary>
        /// <example>GetProduct</example>
        public string Id
        {
            get { return "NullResultMap.Id"; }
        }

        /// <summary>
        /// Extend ResultMap attribute
        /// </summary>
        public string ExtendMap
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// The output type class of the resultMap.
        /// </summary>
        public Type Class
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }


        /// <summary>
        /// Sets the IDataExchange
        /// </summary>
        public IDataExchange DataExchange
        {
            set { throw new Exception("The method or operation is not implemented."); }
        }
        #endregion

        /// <summary>
        /// Create an instance Of result.
        /// </summary>
        /// <param name="parameters">An array of values that matches the number, order and type
        /// of the parameters for this constructor.</param>
        /// <returns>An object.</returns>
        public object CreateInstanceOfResult(object[] parameters)
        {
            return null;
        }

        /// <summary>
        /// Set the value of an object property.
        /// </summary>
        /// <param name="target">The object to set the property.</param>
        /// <param name="property">The result property to use.</param>
        /// <param name="dataBaseValue">The database value to set.</param>
        public void SetValueOfProperty(ref object target, ResultProperty property, object dataBaseValue)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns></returns>
        public IResultMap ResolveSubMap(System.Data.IDataReader dataReader)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
