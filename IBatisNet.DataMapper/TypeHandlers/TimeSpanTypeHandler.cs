
#region Apache Notice
/*****************************************************************************
 * $Revision: 474910 $
 * $LastChangedDate: 2006-11-14 11:33:12 -0700 (Tue, 14 Nov 2006) $
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
using System.Globalization;

using IBatisNet.DataMapper.Configuration.ResultMapping;
#endregion 

namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Description résumée de TimespanTypeHandler.
	/// </summary>
    public sealed class TimeSpanTypeHandler : BaseTypeHandler
	{

        /// <summary>
        ///  Sets a parameter on a IDbCommand
        /// </summary>
        /// <param name="dataParameter">the parameter</param>
        /// <param name="parameterValue">the parameter value</param>
        /// <param name="dbType">the dbType of the parameter</param>
        public override void SetParameter(IDataParameter dataParameter, object parameterValue, string dbType)
        {
            dataParameter.Value = ((TimeSpan)parameterValue).Ticks;
        }


        /// <summary>
        /// Gets a column value by the name
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="dataReader"></param>
        /// <returns></returns>
		public override object GetValueByName(ResultProperty mapping, IDataReader dataReader)
		{
            int index = dataReader.GetOrdinal(mapping.ColumnName);

            if (dataReader.IsDBNull(index) == true)
            {
                return System.DBNull.Value;
            }
            else
            {
                return new TimeSpan( Convert.ToInt64(dataReader.GetValue(index)) );
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
                return System.DBNull.Value;
            }
            else
            {
                return new TimeSpan( Convert.ToInt64(dataReader.GetValue(mapping.ColumnIndex)) );
            }
		}

        /// <summary>
        /// Retrieve ouput database value of an output parameter
        /// </summary>
        /// <param name="outputValue">ouput database value</param>
        /// <param name="parameterType">type used in EnumTypeHandler</param>
        /// <returns></returns>
		public override object GetDataBaseValue(object outputValue, Type parameterType )
		{
			return  new TimeSpan(Convert.ToInt64(outputValue));
		}

        /// <summary>
        /// Converts the String to the type that this handler deals with
        /// </summary>
        /// <param name="type">the tyepe of the property (used only for enum conversion)</param>
        /// <param name="s">the String value</param>
        /// <returns>the converted value</returns>
		public override object ValueOf(Type type, string s)
		{
            return TimeSpan.Parse(s);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override bool IsSimpleType
		{
			get { return true; }
		}

        //public override object NullValue
        //{
        //    get { throw new InvalidCastException("TimeSpanTypeHandler could not cast a null value in TimeSpan field."); }
        //}
	}
}
