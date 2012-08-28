
#region Apache Notice
/*****************************************************************************
 * $Revision: 595821 $
 * $LastChangedDate: 2007-11-16 14:00:04 -0700 (Fri, 16 Nov 2007) $
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
using IBatisNet.DataMapper.Configuration.ResultMapping;

#endregion 

namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Summary description for BaseTypeHandler.
	/// </summary>
	public abstract class BaseTypeHandler : ITypeHandler
	{
		/// <summary>
		/// Gets a column value by the name
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		public abstract object GetValueByName(ResultProperty mapping, IDataReader dataReader);

		/// <summary>
		/// Gets a column value by the index
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		public abstract object GetValueByIndex(ResultProperty mapping, IDataReader dataReader);

		/// <summary>
		/// Retrieve ouput database value of an output parameter
		/// </summary>
		/// <param name="outputValue">ouput database value</param>
		/// <param name="parameterType">type used in EnumTypeHandler</param>
		/// <returns></returns>
		public abstract object GetDataBaseValue(object outputValue, Type parameterType );

        /// <summary>
        /// Gets a value indicating whether this instance is simple type.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is simple type; otherwise, <c>false</c>.
        /// </value>
		public abstract bool IsSimpleType{ get; }

		/// <summary>
		/// Converts the String to the type that this handler deals with
		/// </summary>
		/// <param name="type">the tyepe of the property (used only for enum conversion)</param>
		/// <param name="s">the String value</param>
		/// <returns>the converted value</returns>
		public abstract object ValueOf(Type type, string s);

        /// <summary>
        /// The null value for this type
        /// </summary>
        public virtual object NullValue 
        {
            get { return null; }
        }

		/// <summary>
		///  Sets a parameter on a IDbCommand
		/// </summary>
		/// <param name="dataParameter">the parameter</param>
		/// <param name="parameterValue">the parameter value</param>
		/// <param name="dbType">the dbType of the parameter</param>
		public virtual void SetParameter(IDataParameter dataParameter, object parameterValue, string dbType)
		{
			if (parameterValue != null) 
			{
				dataParameter.Value = parameterValue;
			}
			else
			{
				// When sending a null parameter value to the server,
				// the user must specify DBNull, not null. 
				dataParameter.Value = DBNull.Value;
			}
		}

		/// <summary>
		///  Compares two values (that this handler deals with) for equality
		/// </summary>
		/// <param name="obj">one of the objects</param>
		/// <param name="str">the other object as a String</param>
		/// <returns>true if they are equal</returns>
		public virtual bool Equals(object obj, string str) 
		{
			if (obj == null || str == null) 
			{
				return (string)obj == str;
			} 
			else 
			{
				object castedObject = ValueOf(obj.GetType(), str);
				return obj.Equals(castedObject);
			}
		}
	}
}
