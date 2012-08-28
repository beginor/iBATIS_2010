
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 383115 $
 * $Date: 2006-03-04 07:21:51 -0700 (Sat, 04 Mar 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
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
using System.Data;
using Castle.DynamicProxy;

namespace IBatisNet.Common.Logging
{
	/// <summary>
	/// Requires at least v1.1.5.0 of Castle.DynamicProxy
	/// </summary>
	/// <remarks>
	/// It is common for the user to expect a DataReader with zero or one DataRecords:
	/// 
	/// <code>
	/// if (dr.Read())
	/// {
	///  processDataReader(dr);
	/// }
	/// </code>
	///  
	/// There is no way to know when the user is done processing the DataReader. There may be a lot more
	/// code executed before 'dr' is Disposed() or Closed(). When Read() is called, we print out the
	/// column headers as well as the data from the first DataRecord.
	/// 
	/// DataReaders with 2 or more DataRecords benefit from a simple cache in which the code to generate the
	/// log message does not make a call to the underlying _dataReader if the user has already requested the
	/// value. If a DataRecord contains 50 columns and the user accessed 45 of those, the code to produce the
	/// log message for the DataRecord will only need to make 5 calls to GetValue(i) for the missing columns
	/// rather than iterating through all the columns and calling GetValue(i) to retrieve their value. One
	/// could argue that unaccessed columns should not printed to the log. Since IBatisNet now supports
	/// discriminator nodes in resultMaps, its important to display everything in the DataRecord
	/// or else log messages may contain column values that are mis-aligned with column headers (i.e. some
	/// log messages may have more column values than others when discriminators are in use).
	///
	/// _isFirstRow and _isSecondRow could be replaced with:
	///
	/// <code>
	/// private int _dataRecordsProcessed = 0;
	/// </code>
	/// 				
	/// We don't need to maintain a counter for the number of rows processed. We are only concerned with
	/// the first and second DataRecords.
	/// </remarks>
	public class IDataReaderProxy : IInterceptor
	{
		private IDataReader _dataReader = null;
		private bool _isFirstRow = true;
		private bool _isSecondRow = false;
		private object[] _columnValuesCache = null;
		private static readonly ILog _logger = LogManager.GetLogger("System.Data.IDataReader");
		private static readonly ArrayList _getters;

		static IDataReaderProxy()
		{
			// the following methods have a 0th argument of type int
			_getters = new ArrayList(15);
			_getters.Add("GetInt32");
			_getters.Add("GetValue");
			_getters.Add("GetBytes");
			_getters.Add("GetByte");
			_getters.Add("GetDecimal");
			_getters.Add("GetInt64");
			_getters.Add("GetDouble");
			_getters.Add("GetBoolean");
			_getters.Add("GetGuid");
			_getters.Add("GetDateTime");
			_getters.Add("GetFloat");
			_getters.Add("GetChars");
			_getters.Add("GetString");
			_getters.Add("GetChar");
			_getters.Add("GetIn16");
		}
	
		internal IDataReaderProxy(IDataReader dataReader)
		{
			_dataReader = dataReader;
		}

		public static IDataReader NewInstance(IDataReader dataReader)
		{
			IInterceptor handler = new IDataReaderProxy(dataReader);

			object proxyDataReader = new ProxyGenerator().CreateProxy(typeof(IDataReader), handler, dataReader);

			return (IDataReader)proxyDataReader;
		}

		public object Intercept(IInvocation invocation, params object[] arguments)
		{
			if (invocation.Method.Name == "Read")
			{
				#region Read()
				if (_isFirstRow)
				{
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
				
					for (int i=0;i<_dataReader.FieldCount;i++)
					{
						sb.Append(_dataReader.GetName(i) + ", ");
					}

					_logger.Debug("Headers for \"" + _dataReader.GetHashCode() + "\": [" + sb.ToString(0, sb.Length - 2) + "]");

					_isFirstRow = false;

					// advance to the next DataRecord
					// (bool)invocation.Method.Invoke(_dataReader, arguments);
					bool read = _dataReader.Read();

					if (read == true)
					{
						// log values of the first (non-header) DataRecord
						_columnValuesCache = new object[_dataReader.FieldCount];
						logColumnValues();

						// if the user has chosen the 'while (dr.Read())' syntax, tell the second DataRecord
						// that its contents have already been logged
						_isSecondRow = true;
					}

					return read;
				}
				else
				{
					if (_isSecondRow)
					{
						// we've already called logColumnValues()
						_isSecondRow = false;
					}
					else
					{
						// the user is most likely iterating through the records using the 'while (dr.Read())' notation
						logColumnValues();
					}
					
					_columnValuesCache = new object[_dataReader.FieldCount];

					return invocation.Method.Invoke(_dataReader, arguments);
				}
				#endregion
			}
			else if (_getters.Contains(invocation.Method.Name))
			{
				#region GetInt32(int), GetString(int), GetDateTime(int), etc.
				// Cache repeated lookups: checking for null is probably faster than calling Invoke ???
				object value = _columnValuesCache[(int)arguments[0]];
				if (value == null)
				{
					value = invocation.Method.Invoke(_dataReader, arguments);
					_columnValuesCache[(int)arguments[0]] = value;
				}
				else
				{
					// If someone wrote a custom type handler that makes repeated calls to GetXXXXX(int), we only make one call to Invoke
					// _logger.Debug("Using cached value for column: [" + (int)arguments[0] + "]");
				}
				return value;
				#endregion
			}
			else
			{
				return invocation.Method.Invoke(_dataReader, arguments);
			}
		}

		private void logColumnValues()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			for (int i=0;i<_columnValuesCache.Length;i++)
			{
				object value = _columnValuesCache[i];

				if (value == null)
				{
					// _logger.Debug("Could not find existing value for column [" + i + "]");

					value = _dataReader.GetValue(i);

					if (value == DBNull.Value)
					{
						value = "NULL";
					}
				}

				sb.Append(value + ", ");
			}

			// remove trailing comma and space
			if (sb.Length > 2)
			{
				sb.Length -= 2;
			}

			_logger.Debug("Results for \"" + _dataReader.GetHashCode() + "\": [" + sb.ToString() + "]");
		}
	}
}