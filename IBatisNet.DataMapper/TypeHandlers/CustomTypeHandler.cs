#region Apache Notice
/*****************************************************************************
 * $Revision: 476843 $
 * $LastChangedDate: 2006-11-19 09:07:45 -0700 (Sun, 19 Nov 2006) $
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
using System.Data;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;
#endregion 

namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Custom type handler for adding a TypeHandlerCallback
	/// </summary>
    public sealed class CustomTypeHandler : BaseTypeHandler
	{
		private ITypeHandlerCallback _callback = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTypeHandler"/> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
		public CustomTypeHandler(ITypeHandlerCallback callback)
		{
			_callback = callback;
		}

        /// <summary>
        /// Gets or sets the callback.
        /// </summary>
        /// <value>The callback.</value>
		public ITypeHandlerCallback Callback
		{
			get { return _callback; }
			set { /* nop */ }
		}

		/// <summary>
		/// Performs processing on a value before it is used to set
		/// the parameter of a IDbCommand.
		/// </summary>
		/// <param name="dataParameter"></param>
		/// <param name="parameterValue">The value to be set</param>
		/// <param name="dbType">Data base type</param>
		public override void SetParameter(IDataParameter dataParameter, object parameterValue, string dbType)
		{
			IParameterSetter setter = new ParameterSetterImpl(dataParameter);
			_callback.SetParameter(setter, parameterValue);
		}

        /// <summary>
        /// Gets a column value by the name
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="dataReader"></param>
        /// <returns></returns>
		public override object GetValueByName(ResultProperty mapping, IDataReader dataReader)
		{
			IResultGetter getter = new ResultGetterImpl(dataReader, mapping.ColumnName);
			return _callback.GetResult(getter);
		}

        /// <summary>
        /// Gets a column value by the index
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="dataReader"></param>
        /// <returns></returns>
		public override object GetValueByIndex(ResultProperty mapping, IDataReader dataReader)
		{
			IResultGetter getter = new ResultGetterImpl(dataReader, mapping.ColumnIndex);
			return _callback.GetResult(getter);		
		}

        /// <summary>
        /// Converts the String to the type that this handler deals with
        /// </summary>
        /// <param name="type">the tyepe of the property (used only for enum conversion)</param>
        /// <param name="s">the String value</param>
        /// <returns>the converted value</returns>
		public override object ValueOf(Type type, string s)
		{
			return _callback.ValueOf(s);
		}

        /// <summary>
        /// Retrieve ouput database value of an output parameter
        /// </summary>
        /// <param name="outputValue">ouput database value</param>
        /// <param name="parameterType">type used in EnumTypeHandler</param>
        /// <returns></returns>
		public override object GetDataBaseValue(object outputValue, Type parameterType)
		{
			IResultGetter getter = new ResultGetterImpl(outputValue);
			return _callback.GetResult(getter);	
		}

        /// <summary>
        /// Gets a value indicating whether this instance is simple type.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is simple type; otherwise, <c>false</c>.
        /// </value>
		public override bool IsSimpleType
		{
			get { return true; }
		}

        /// <summary>
        /// The null value for this type
        /// </summary>
        /// <value></value>
        public override object NullValue
        {
            get { return _callback.NullValue; }
        }
	}
}
