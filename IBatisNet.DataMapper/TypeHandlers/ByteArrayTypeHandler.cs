
#region Apache Notice
/*****************************************************************************
 * $Revision: 652259 $
 * $LastChangedDate: 2008-04-29 22:53:42 -0600 (Tue, 29 Apr 2008) $
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

#region using
using System;
using System.Data;
using System.Globalization;
using System.IO;

using IBatisNet.DataMapper.Configuration.ResultMapping;
using IBatisNet.DataMapper.Exceptions;
#endregion

namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Description résumée de ByteArrayTypeHandler.
	/// </summary>
    public sealed class ByteArrayTypeHandler : BaseTypeHandler
	{

        /// <summary>
        /// Gets a column value by the name
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="dataReader"></param>
        /// <returns></returns>
		public override object GetValueByName(ResultProperty mapping, IDataReader dataReader)
		{
			int index = dataReader.GetOrdinal(mapping.ColumnName);

            if (dataReader.IsDBNull(index) || dataReader.GetBytes(index, 0, null, 0, 0) == 0)
			{
				return DBNull.Value;
			}
			else
			{
				return GetValueByIndex(index, dataReader);
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
            if (dataReader.IsDBNull(mapping.ColumnIndex) || dataReader.GetBytes(mapping.ColumnIndex, 0, null, 0, 0) == 0)
            {
				return DBNull.Value;
			}
			else
			{
				return GetValueByIndex(mapping.ColumnIndex, dataReader);
			}
		}


        /// <summary>
        /// Gets the index of the value by.
        /// </summary>
        /// <param name="columnIndex">Index of the column.</param>
        /// <param name="dataReader">The data reader.</param>
        /// <returns></returns>
		private byte[] GetValueByIndex(int columnIndex, IDataReader dataReader) 
		{
			// determine the buffer size
			int bufferLength = (int) dataReader.GetBytes(columnIndex, 0, null, 0, 0);

			// initialize it
			byte[] byteArray = new byte[bufferLength];

			// fill it
			dataReader.GetBytes(columnIndex, 0, byteArray, 0, bufferLength);

			return byteArray;
		}


        /// <summary>
        /// Converts the String to the type that this handler deals with
        /// </summary>
        /// <param name="type">the tyepe of the property (used only for enum conversion)</param>
        /// <param name="s">the String value</param>
        /// <returns>the converted value</returns>
		public override object ValueOf(Type type, string s)
		{
			return System.Text.Encoding.Default.GetBytes(s);
		}

        /// <summary>
        /// Retrieve ouput database value of an output parameter
        /// </summary>
        /// <param name="outputValue">ouput database value</param>
        /// <param name="parameterType">type used in EnumTypeHandler</param>
        /// <returns></returns>
		public override object GetDataBaseValue(object outputValue, Type parameterType )
		{
			throw new DataMapperException("NotSupportedException");
		}

        /// <summary>
        /// Tell us if ot is a 'primitive' type
        /// </summary>
        /// <value></value>
        /// <returns></returns>
		public override bool IsSimpleType
		{
			get { return true; }
		}

        //public override object NullValue
        //{
        //    get { return null; }
        //}
	}
}
