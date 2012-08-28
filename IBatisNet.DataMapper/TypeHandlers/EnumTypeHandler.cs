
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
using System.Data;
using IBatisNet.DataMapper.Configuration.ResultMapping;

#endregion 

namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Summary description for EnumTypeHandler.
	/// </summary>
    public sealed class EnumTypeHandler : BaseTypeHandler
	{

		/// <summary>
		///  Sets a parameter on a IDbCommand
		/// </summary>
		/// <param name="dataParameter">the parameter</param>
		/// <param name="parameterValue">the parameter value</param>
		/// <param name="dbType">the dbType of the parameter</param>
		public override void SetParameter(IDataParameter dataParameter, object parameterValue, string dbType)
		{
            if (parameterValue != null)
            {
			    dataParameter.Value =  Convert.ChangeType( parameterValue, Enum.GetUnderlyingType( parameterValue.GetType() ) );
            }
            else
            {
                // When sending a null parameter value to the server,
                // the user must specify DBNull, not null. 
                dataParameter.Value = DBNull.Value;
            }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="dataReader"></param>
		/// <returns></returns>
		public override object GetValueByName(ResultProperty mapping, IDataReader dataReader)
		{
			int index = dataReader.GetOrdinal(mapping.ColumnName);

			if (dataReader.IsDBNull(index) == true)
			{
				return DBNull.Value;
			}
			else
			{  
				return Enum.Parse(mapping.MemberType, dataReader.GetValue(index).ToString());
			}
		}

        /// <summary>
        /// Gets a column value by the index
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="dataReader"></param>
        /// <returns></returns>
		public override object GetValueByIndex(ResultProperty mapping, IDataReader dataReader) 
		{
			if (dataReader.IsDBNull(mapping.ColumnIndex) == true)
			{
				return DBNull.Value;
			}
			else
			{
				return Enum.Parse(mapping.MemberType, dataReader.GetValue(mapping.ColumnIndex).ToString());
			}
		}


        /// <summary>
        /// Converts the String to the type that this handler deals with
        /// </summary>
        /// <param name="type">the tyepe of the property (used only for enum conversion)</param>
        /// <param name="s">the String value</param>
        /// <returns>the converted value</returns>
		public override object ValueOf(Type type, string s)
		{
			return Enum.Parse(type, s);
		}

        /// <summary>
        /// Retrieve ouput database value of an output parameter
        /// </summary>
        /// <param name="outputValue">ouput database value</param>
        /// <param name="parameterType">type used in EnumTypeHandler</param>
        /// <returns></returns>
		public override object GetDataBaseValue(object outputValue, Type parameterType )
		{
			return Enum.Parse(parameterType, outputValue.ToString());
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

        //public override object NullValue
        //{
        //    get { throw new InvalidCastException("EnumTypeHandler, could not cast a null value in Enum field."); }
        //}
	}
}
