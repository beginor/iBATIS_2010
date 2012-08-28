#region Apache Notice
/*****************************************************************************
 * $Revision: 408164 $
 * $LastChangedDate: 2006-05-21 06:27:09 -0600 (Sun, 21 May 2006) $
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

using System.Data;

#endregion 

namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// Description résumée de ResultGetterImpl.
	/// </summary>
    public sealed class ResultGetterImpl : IResultGetter
	{

		#region Fields

		private int _columnIndex = int.MinValue;
		private string _columnName = string.Empty;
		private object _outputValue = null;

		private IDataReader _dataReader = null;
		#endregion 

		#region Constructors

		/// <summary>
		/// Creates an instance for a IDataReader and column index
		/// </summary>
		/// <param name="dataReader">The dataReader</param>
		/// <param name="columnIndex">the column index</param>
		public ResultGetterImpl(IDataReader dataReader, int columnIndex)
		{
			_columnIndex = columnIndex;
			_dataReader = dataReader;
		}

		/// <summary>
		/// Creates an instance for a IDataReader and column name
		/// </summary>
		/// <param name="dataReader">The dataReader</param>
		/// <param name="columnName">the column name</param>
		public ResultGetterImpl(IDataReader dataReader, string columnName)
		{
			_columnName= columnName;
			_dataReader = dataReader;
		}

		/// <summary>
		/// Creates an instance for an output parameter
		/// </summary>
		/// <param name="outputValue">value of an output parameter (store procedure)</param>
		public ResultGetterImpl(object outputValue)
		{
			_outputValue = outputValue;
		}
		#endregion 

		#region IResultGetter members

		/// <summary>
		/// Returns the underlying IDataReader
		/// </summary>
		/// <remarks>Null for an output parameter</remarks>
		public IDataReader DataReader
		{
			get { return _dataReader; }
		}

		/// <summary>
		/// Get the parameter value
		/// </summary>
		public object Value
		{
			get
			{
				if (_columnName.Length >0)
				{
					int index = _dataReader.GetOrdinal(_columnName);
					return _dataReader.GetValue(index);
				}
				else if (_columnIndex>=0)
				{
					return _dataReader.GetValue(_columnIndex);
				}
				else
				{
					return _outputValue;
				}
			}
		}
		#endregion 

	}
}
