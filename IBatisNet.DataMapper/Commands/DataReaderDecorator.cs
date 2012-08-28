#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 383115 $
 * $Date: 2006-10-30 12:09:11 -0700 (Mon, 30 Oct 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2005 - Gilles Bayon
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
using System.Data;
using IBatisNet.DataMapper.Scope;

namespace IBatisNet.DataMapper.Commands
{
    /// <summary>
    /// Decorate an <see cref="System.Data.IDataReader"></see>
    /// to auto move to next ResultMap on NextResult call. 
    /// </summary>
    public class DataReaderDecorator : IDataReader
    {
        private IDataReader _innerDataReader = null;
        private RequestScope _request = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReaderDecorator"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader.</param>
		/// <param name="request">The request scope</param>
		public DataReaderDecorator(IDataReader dataReader, RequestScope request)
        {
            _innerDataReader = dataReader;
            _request = request;
        }
        
        #region IDataReader Members

        /// <summary>
        /// Closes the <see cref="System.Data.IDataReader"></see> 0bject.
        /// </summary>
        void IDataReader.Close()
        {
            _innerDataReader.Close();
        }

        /// <summary>
        /// Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        /// <value></value>
        /// <returns>The level of nesting.</returns>
        int IDataReader.Depth
        {
            get { return _innerDataReader.Depth; }
        }

        /// <summary>
        /// Returns a <see cref="System.Data.DataTable"></see> that describes the column metadata of the <see cref="System.Data.IDataReader"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="System.Data.DataTable"></see> that describes the column metadata.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">The <see cref="System.Data.IDataReader"></see> is closed. </exception>
        DataTable IDataReader.GetSchemaTable()
        {
            return _innerDataReader.GetSchemaTable();
        }

        /// <summary>
        /// Gets a value indicating whether the data reader is closed.
        /// </summary>
        /// <value></value>
        /// <returns>true if the data reader is closed; otherwise, false.</returns>
        bool IDataReader.IsClosed
        {
            get { return _innerDataReader.IsClosed; }
        }

        /// <summary>
        /// Advances the data reader to the next result, when reading the results of batch SQL statements.
        /// </summary>
        /// <returns>
        /// true if there are more rows; otherwise, false.
        /// </returns>
        bool IDataReader.NextResult()
        {
            _request.MoveNextResultMap();
            return _innerDataReader.NextResult();
        }

        /// <summary>
        /// Advances the <see cref="System.Data.IDataReader"></see> to the next record.
        /// </summary>
        /// <returns>
        /// true if there are more rows; otherwise, false.
        /// </returns>
        bool IDataReader.Read()
        {
            return _innerDataReader.Read();
        }

        int IDataReader.RecordsAffected
        {
            get { return _innerDataReader.RecordsAffected; }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        void IDisposable.Dispose()
        {
            _innerDataReader.Dispose();
        }

        #endregion

        #region IDataRecord Members

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <value></value>
        /// <returns>When not positioned in a valid recordset, 0; otherwise the number of columns in the current record. The default is -1.</returns>
        int IDataRecord.FieldCount
        {
            get { return _innerDataReader.FieldCount; }
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>The value of the column.</returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        bool IDataRecord.GetBoolean(int i)
        {
            return _innerDataReader.GetBoolean(i);
        }

        /// <summary>
        /// Gets the 8-bit unsigned integer value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>
        /// The 8-bit unsigned integer value of the specified column.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        byte IDataRecord.GetByte(int i)
        {
            return _innerDataReader.GetByte(i);
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldOffset">The index within the field from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of bytes read.</returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        long IDataRecord.GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return _innerDataReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        /// <summary>
        /// Gets the character value of the specified column.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <returns>
        /// The character value of the specified column.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        char IDataRecord.GetChar(int i)
        {
            return _innerDataReader.GetChar(i);
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array, starting at the given buffer offset.
        /// </summary>
        /// <param name="i">The zero-based column ordinal.</param>
        /// <param name="fieldoffset">The index within the row from which to start the read operation.</param>
        /// <param name="buffer">The buffer into which to read the stream of bytes.</param>
        /// <param name="bufferoffset">The index for buffer to start the read operation.</param>
        /// <param name="length">The number of bytes to read.</param>
        /// <returns>The actual number of characters read.</returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        long IDataRecord.GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return _innerDataReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        /// <summary>
        /// Gets an <see cref="System.Data.IDataReader"></see> to be used when the field points to more remote structured data.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// An <see cref="System.Data.IDataReader"></see> to be used when the field points to more remote structured data.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        IDataReader IDataRecord.GetData(int i)
        {
            return _innerDataReader.GetData(i);
        }

        /// <summary>
        /// Gets the data type information for the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// The data type information for the specified field.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        string IDataRecord.GetDataTypeName(int i)
        {
            return _innerDataReader.GetDataTypeName(i);
        }

        /// <summary>
        /// Gets the date and time data value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// The date and time data value of the spcified field.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        DateTime IDataRecord.GetDateTime(int i)
        {
            return _innerDataReader.GetDateTime(i);
        }

        /// <summary>
        /// Gets the fixed-position numeric value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// The fixed-position numeric value of the specified field.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        decimal IDataRecord.GetDecimal(int i)
        {
            return _innerDataReader.GetDecimal(i);
        }

        /// <summary>
        /// Gets the double-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// The double-precision floating point number of the specified field.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        double IDataRecord.GetDouble(int i)
        {
            return _innerDataReader.GetDouble(i);
        }

        /// <summary>
        /// Gets the <see cref="System.Type"></see> information corresponding to the type of <see cref="System.Object"></see> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)"></see>.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// The <see cref="System.Type"></see> information corresponding to the type of <see cref="System.Object"></see> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)"></see>.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        Type IDataRecord.GetFieldType(int i)
        {
            return _innerDataReader.GetFieldType(i);
        }

        /// <summary>
        /// Gets the single-precision floating point number of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// The single-precision floating point number of the specified field.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        float IDataRecord.GetFloat(int i)
        {
            return _innerDataReader.GetFloat(i);
        }

        /// <summary>
        /// Returns the GUID value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The GUID value of the specified field.</returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        Guid IDataRecord.GetGuid(int i)
        {
            return _innerDataReader.GetGuid(i);
        }

        /// <summary>
        /// Gets the 16-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// The 16-bit signed integer value of the specified field.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        short IDataRecord.GetInt16(int i)
        {
            return _innerDataReader.GetInt16(i);
        }

        /// <summary>
        /// Gets the 32-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// The 32-bit signed integer value of the specified field.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        int IDataRecord.GetInt32(int i)
        {
            return _innerDataReader.GetInt32(i);
        }

        /// <summary>
        /// Gets the 64-bit signed integer value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// The 64-bit signed integer value of the specified field.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        long IDataRecord.GetInt64(int i)
        {
            return _innerDataReader.GetInt64(i);
        }

        /// <summary>
        /// Gets the name for the field to find.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// The name of the field or the empty string (""), if there is no value to return.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        string IDataRecord.GetName(int i)
        {
            return _innerDataReader.GetName(i);
        }

        /// <summary>
        /// Return the index of the named field.
        /// </summary>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>The index of the named field.</returns>
        int IDataRecord.GetOrdinal(string name)
        {
            return _innerDataReader.GetOrdinal(name);
        }

        /// <summary>
        /// Gets the string value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>The string value of the specified field.</returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        string IDataRecord.GetString(int i)
        {
            return _innerDataReader.GetString(i);
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// The <see cref="System.Object"></see> which will contain the field value upon return.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        object IDataRecord.GetValue(int i)
        {
            return _innerDataReader.GetValue(i);
        }

        int IDataRecord.GetValues(object[] values)
        {
            return _innerDataReader.GetValues(values);
        }

        /// <summary>
        /// Return whether the specified field is set to null.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        /// true if the specified field is set to null. Otherwise, false.
        /// </returns>
        /// <exception cref="System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"></see>. </exception>
        bool IDataRecord.IsDBNull(int i)
        {
            return _innerDataReader.IsDBNull(i);
        }

        /// <summary>
        /// Gets the <see cref="Object"/> with the specified name.
        /// </summary>
        /// <value></value>
        object IDataRecord.this[string name]
        {
            get { return _innerDataReader[name]; }
        }

        /// <summary>
        /// Gets the <see cref="Object"/> with the specified i.
        /// </summary>
        /// <value></value>
        object IDataRecord.this[int i]
        {
            get { return _innerDataReader[i]; }
        }

        #endregion
    }
}
