#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-04-26 14:12:57 -0600 (Wed, 26 Apr 2006) $
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
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using IBatisNet.Common;
using IBatisNet.DataMapper.Exceptions;

namespace IBatisNet.DataMapper.Commands
{
	/// <summary>
	/// An implementation of <see cref="IDataReader"/> that will copy the contents
	/// of the an open <see cref="IDataReader"/> to an in-memory <see cref="InMemoryDataReader"/> if the 
	/// session <see cref="IDbProvider"/> doesn't allow multiple open <see cref="IDataReader"/> with
	/// the same <see cref="IDbConnection"/>.
	/// </summary>
	public class InMemoryDataReader : IDataReader
	{
		private int _currentRowIndex = 0;
		private int _currentResultIndex = 0;

		private bool _isClosed = false;

		private InMemoryResultSet[ ] _results = null;

	
		/// <summary>
		///  Creates an InMemoryDataReader from a <see cref="IDataReader" />
		/// </summary>
		/// <param name="reader">The <see cref="IDataReader" /> which holds the records from the Database.</param>
		public InMemoryDataReader(IDataReader reader)
		{
			ArrayList resultList = new ArrayList();

			try
			{
				_currentResultIndex = 0;
				_currentRowIndex = 0;

				resultList.Add( new InMemoryResultSet( reader, true ) );

				while( reader.NextResult() )
				{
					resultList.Add( new InMemoryResultSet( reader, false ) );
				}

				_results = ( InMemoryResultSet[ ] ) resultList.ToArray( typeof( InMemoryResultSet ) );
			}
			catch( Exception e )
			{
				throw new DataMapperException( "There was a problem converting an IDataReader to an InMemoryDataReader", e );
			}
			finally
			{
				reader.Close();
				reader.Dispose();
			}
		}


		#region IDataReader Members

		/// <summary>
		/// Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
		/// </summary>
		public int RecordsAffected
		{
			get { throw new NotImplementedException( "InMemoryDataReader only used for select IList statements !" ); }
		}

		/// <summary>
		/// Gets a value indicating whether the data reader is closed.
		/// </summary>
		public bool IsClosed
		{
			get { return _isClosed; }
		}

		/// <summary>
		/// Advances the data reader to the next result, when reading the results of batch SQL statements.
		/// </summary>
		/// <returns></returns>
		public bool NextResult()
		{
			_currentResultIndex++;
			if( _currentResultIndex >= _results.Length )
			{
				_currentResultIndex--;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Closes the IDataReader 0bject.
		/// </summary>
		public void Close()
		{
			_isClosed = true;
		}

		/// <summary>
		/// Advances the IDataReader to the next record.
		/// </summary>
		/// <returns>true if there are more rows; otherwise, false.</returns>
		public bool Read()
		{
			_currentRowIndex++;
			if( _currentRowIndex >= _results[ _currentResultIndex ].RecordCount )
			{
				_currentRowIndex--;
				return false;
			}
			return true;
		}

		/// <summary>
		/// Gets a value indicating the depth of nesting for the current row.
		/// </summary>
		public int Depth
		{
			get { return _currentResultIndex; }
		}

		/// <summary>
		/// Returns a DataTable that describes the column metadata of the IDataReader.
		/// </summary>
		/// <returns></returns>
		public DataTable GetSchemaTable()
		{
			throw new NotImplementedException( "GetSchemaTable() is not implemented, cause not use." );
		}

		#endregion

		#region IDisposable Members

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			_isClosed = true;
			_results = null;
		}

		#endregion

		#region IDataRecord Members

		/// <summary>
		/// Gets the 32-bit signed integer value of the specified field.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public int GetInt32(int fieldIndex)
		{
			return (int) GetValue(fieldIndex); 
		}

		/// <summary>
		/// Gets the column with the specified name.
		/// </summary>
		public object this[string name]
		{
			get { return this [GetOrdinal(name)]; }
		}

		/// <summary>
		/// Gets the column located at the specified index.
		/// </summary>
		public object this[int fieldIndex]
		{
			get { return GetValue( fieldIndex ); }
		}

		/// <summary>
		/// Return the value of the specified field.
		/// </summary>
		/// <param name="fieldIndex">The index of the field to find. </param>
		/// <returns>The object which will contain the field value upon return.</returns>
		public object GetValue(int fieldIndex)
		{
			return this.CurrentResultSet.GetValue( _currentRowIndex, fieldIndex );
		}

		/// <summary>
		/// Return whether the specified field is set to null.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public bool IsDBNull(int fieldIndex)
		{
			return (GetValue(fieldIndex) == DBNull.Value);
		}

		/// <summary>
		/// Reads a stream of bytes from the specified column offset into the buffer as an array, 
		/// starting at the given buffer offset.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <param name="dataIndex">The index within the field from which to begin the read operation. </param>
		/// <param name="buffer">The buffer into which to read the stream of bytes. </param>
		/// <param name="bufferIndex">The index for buffer to begin the read operation.</param>
		/// <param name="length">The number of bytes to read. </param>
		/// <returns>The actual number of bytes read.</returns>
		public long GetBytes(int fieldIndex, long dataIndex, byte[] buffer, int bufferIndex, int length)
		{
			object value = GetValue(fieldIndex);
			if (!(value is byte []))
			{
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}
                                                                                                   
			if ( buffer == null ) 
			{
				// Return length of data
				return ((byte []) value).Length;
			}
			else 
			{
				// Copy data into buffer
				int availableLength = (int) ( ( (byte []) value).Length - dataIndex);
				if (availableLength < length)
				{
					length = availableLength;
				}

				Array.Copy ((byte []) value, (int) dataIndex, buffer, bufferIndex, length);
				return length; 
			}
		}


		/// <summary>
		/// Gets the 8-bit unsigned integer value of the specified column.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public byte GetByte(int fieldIndex)
		{
			return (byte) GetValue(fieldIndex);
		}

		/// <summary>
		/// Gets the Type information corresponding to the type of Object that would be returned from GetValue.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public Type GetFieldType(int fieldIndex)
		{
			return this.CurrentResultSet.GetFieldType(fieldIndex);
		}

		/// <summary>
		/// Gets the fixed-position numeric value of the specified field.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public decimal GetDecimal(int fieldIndex)
		{
			return (decimal) GetValue(fieldIndex);
		}

		/// <summary>
		/// Gets all the attribute fields in the collection for the current record.
		/// </summary>
		/// <param name="values"></param>
		/// <returns></returns>
		public int GetValues(object[] values)
		{
			return this.CurrentResultSet.GetValues( _currentRowIndex, values );
		}

		/// <summary>
		/// Gets the name for the field to find.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public string GetName(int fieldIndex)
		{
			return this.CurrentResultSet.GetName( fieldIndex );
		}

		/// <summary>
		/// Indicates the number of fields within the current record. This property is read-only.
		/// </summary>
		public int FieldCount
		{
			get { return this.CurrentResultSet.FieldCount; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public long GetInt64(int fieldIndex)
		{
			return (long) GetValue(fieldIndex); 
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public double GetDouble(int fieldIndex)
		{
			return (double) GetValue(fieldIndex);
		}

		/// <summary>
		/// Gets the value of the specified column as a Boolean.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public bool GetBoolean(int fieldIndex)
		{
			return (bool) GetValue(fieldIndex);
		}

		/// <summary>
		/// Returns the GUID value of the specified field.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public Guid GetGuid(int fieldIndex)
		{
			return (Guid) GetValue(fieldIndex);
		}

		/// <summary>
		/// Returns the value of the specified column as a DateTime object.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public DateTime GetDateTime(int fieldIndex)
		{
			return (DateTime) GetValue(fieldIndex);
		}

		/// <summary>
		/// Returns the column ordinal, given the name of the column.
		/// </summary>
		/// <param name="colName">The name of the column. </param>
		/// <returns>The value of the column.</returns>
		public int GetOrdinal(string colName)
		{
			return this.CurrentResultSet.GetOrdinal(colName);
		}

		/// <summary>
		/// Gets the database type information for the specified field.
		/// </summary>
		/// <param name="fieldIndex">The index of the field to find.</param>
		/// <returns>The database type information for the specified field.</returns>
		public string GetDataTypeName(int fieldIndex)
		{
			return this.CurrentResultSet.GetDataTypeName(fieldIndex);
		}

		/// <summary>
		/// Returns the value of the specified column as a single-precision floating point number.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public float GetFloat(int fieldIndex)
		{
			return (float) GetValue(fieldIndex);
		}

		/// <summary>
		/// Gets an IDataReader to be used when the field points to more remote structured data.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public IDataReader GetData(int fieldIndex)
		{
			throw new NotImplementedException( "GetData(int) is not implemented, cause not use." );
		}

		/// <summary>
		/// Reads a stream of characters from the specified column offset into the buffer as an array, 
		/// starting at the given buffer offset.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <param name="dataIndex">The index within the row from which to begin the read operation.</param>
		/// <param name="buffer">The buffer into which to read the stream of bytes.</param>
		/// <param name="bufferIndex">The index for buffer to begin the read operation. </param>
		/// <param name="length">The number of bytes to read.</param>
		/// <returns>The actual number of characters read.</returns>
		public long GetChars(int fieldIndex, long dataIndex, char[] buffer, int bufferIndex, int length)
		{
			object value = GetValue(fieldIndex);
			char [] valueBuffer = null;
                                                                                                    
			if (value is char[])
			{
				valueBuffer = (char[])value;
			}
			else if (value is string)
			{
				valueBuffer = ((string)value).ToCharArray();
			}
			else
			{
				throw new InvalidCastException ("Type is " + value.GetType ().ToString ());
			}

			if ( buffer == null ) 
			{
				// Return length of data
				return valueBuffer.Length;
			}
			else 
			{
				// Copy data into buffer
				Array.Copy (valueBuffer, (int) dataIndex, buffer, bufferIndex, length);
				return valueBuffer.Length - dataIndex;
			}

		}

		/// <summary>
		/// Gets the string value of the specified field.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public string GetString(int fieldIndex)
		{
			return (string) GetValue(fieldIndex);
		}

		/// <summary>
		/// Gets the character value of the specified column.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public char GetChar(int fieldIndex)
		{
			return (char) GetValue(fieldIndex);
		}

		/// <summary>
		/// Gets the 16-bit signed integer value of the specified field.
		/// </summary>
		/// <param name="fieldIndex">The zero-based column ordinal. </param>
		/// <returns>The value of the column.</returns>
		public short GetInt16(int fieldIndex)
		{
			return (short)GetValue (fieldIndex); 
		}

		#endregion


        /// <summary>
        /// Gets the current result set.
        /// </summary>
        /// <value>The current result set.</value>
		private InMemoryResultSet CurrentResultSet
		{
			get {return _results[ _currentResultIndex ];} 
		}

		/// <summary>
		/// Represent an in-memory result set
		/// </summary>
		private class InMemoryResultSet
		{
			// [row][column]
			private readonly object[ ][ ] _records = null;
			private int _fieldCount = 0;

			private string[] _fieldsName = null;
			private Type[] _fieldsType = null;
			StringDictionary _fieldsNameLookup = new StringDictionary();
			private string[] _dataTypeName = null;

			/// <summary>
			///  Creates an in-memory ResultSet from a <see cref="IDataReader" />
			/// </summary>
			/// <param name="isMidstream">
			/// <c>true</c> if the <see cref="IDataReader"/> is already positioned on the record
			/// to start reading from.
			/// </param>
			/// <param name="reader">The <see cref="IDataReader" /> which holds the records from the Database.</param>
			public InMemoryResultSet(IDataReader reader, bool isMidstream )
			{
				// [record index][ columns values=object[ ] ]
				ArrayList recordsList = new ArrayList();

				_fieldCount = reader.FieldCount;
				_fieldsName = new string[_fieldCount];
				_fieldsType = new Type[_fieldCount];
				_dataTypeName = new string[_fieldCount];


				bool firstRow = true;

				// if we are in the middle of processing the reader then don't bother
				// to move to the next record - just use the current one.
				// Copy the records in memory
				while( isMidstream || reader.Read())
				{
					if( firstRow )
					{
						for( int fieldIndex = 0; fieldIndex < reader.FieldCount; fieldIndex++ )
						{
							string fieldName = reader.GetName( fieldIndex );

							_fieldsName[ fieldIndex ] = fieldName;
							if (!_fieldsNameLookup.ContainsKey(fieldName))
							{
								_fieldsNameLookup.Add(fieldName, fieldIndex.ToString() );
							}
							
							_fieldsType[fieldIndex] = reader.GetFieldType( fieldIndex ) ;
							_dataTypeName[ fieldIndex ] = reader.GetDataTypeName( fieldIndex );
						}
					}

					firstRow = false;

					object[ ] columnsValues = new object[_fieldCount];
					reader.GetValues( columnsValues );
					recordsList.Add( columnsValues );

					isMidstream = false;
				}
				_records = ( object[ ][ ] ) recordsList.ToArray( typeof( object[ ] ) );

			}

            /// <summary>
            /// Gets the number of columns in the current row.
            /// </summary>
            /// <value>The number of columns in the current row.</value>
			public int FieldCount
			{
				get {return _fieldCount;}
			}

			/// <summary>
			/// Get a column value in a row
			/// </summary>
			/// <param name="rowIndex">The row index</param>
			/// <param name="colIndex">The column index</param>
			/// <returns>The column value</returns>
			public object GetValue( int rowIndex, int colIndex )
			{
				return _records[ rowIndex ][ colIndex ];
			}

			/// <summary>
			/// The number of record contained in the ResultSet.
			/// </summary>
			public int RecordCount
			{
				get { return _records.Length; }
			}


            /// <summary>
            /// Gets the type of the field.
            /// </summary>
            /// <param name="colIndex">Index of the col.</param>
            /// <returns>The type of the field.</returns>
			public Type GetFieldType( int colIndex )
			{
				return _fieldsType[ colIndex ];
			}

            /// <summary>
            /// Gets the name of the field.
            /// </summary>
            /// <param name="colIndex">Index of the col.</param>
            /// <returns>The name of the field.</returns>
			public string GetName( int colIndex )
			{
				return _fieldsName[ colIndex ];
			}

            /// <summary>
            /// Gets the ordinal.
            /// </summary>
            /// <param name="colName">Name of the column.</param>
            /// <returns>The ordinal of the column</returns>
			public int GetOrdinal( string colName )
			{
				if( _fieldsNameLookup.ContainsKey(colName) )
				{
					return  Convert.ToInt32(_fieldsNameLookup[ colName ]);
				}
				else
				{
					throw new IndexOutOfRangeException( String.Format( "No column with the specified name was found: {0}.", colName ) );
				}
			}

            /// <summary>
            /// Gets the name of the database type.
            /// </summary>
            /// <param name="colIndex">Index of the col.</param>
            /// <returns>The name of the database type</returns>
			public string GetDataTypeName( int colIndex )
			{
				return _dataTypeName[ colIndex ];
			}


            /// <summary>
            /// Gets the values.
            /// </summary>
            /// <param name="rowIndex">Index of the row.</param>
            /// <param name="values">The values.</param>
            /// <returns></returns>
			public int GetValues( int rowIndex, object[ ] values )
			{
				Array.Copy( _records[ rowIndex ], 0, values, 0, _fieldCount );
				return _fieldCount;
			}
		}

		
	}
}
