#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-04-01 11:53:15 -0700 (Sat, 01 Apr 2006) $
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

using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;

namespace IBatisNet.DataMapper.DataExchange
{
	/// <summary>
	/// Summary description for BaseDataExchange.
	/// </summary>
	public abstract class BaseDataExchange : IDataExchange
	{
		private DataExchangeFactory _dataExchangeFactory = null;

		/// <summary>
		/// Getter for the factory that created this object
		/// </summary>
		public DataExchangeFactory DataExchangeFactory
		{
			get{ return _dataExchangeFactory; }
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dataExchangeFactory"></param>
		public BaseDataExchange(DataExchangeFactory dataExchangeFactory)
		{
			_dataExchangeFactory = dataExchangeFactory;
		}

		#region IDataExchange Members

		/// <summary>
		/// Gets the data to be set into a IDataParameter.
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="parameterObject"></param>
		public abstract object GetData(ParameterProperty mapping, object parameterObject);

		/// <summary>
		/// Sets the value to the result property.
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="target"></param>
		/// <param name="dataBaseValue"></param>
		public abstract void SetData(ref object target, ResultProperty mapping, object dataBaseValue);

		/// <summary>
		/// Sets the value to the parameter property.
		/// </summary>
		/// <remarks>Use to set value on output parameter</remarks>
		/// <param name="mapping"></param>
		/// <param name="target"></param>
		/// <param name="dataBaseValue"></param>
		public abstract void SetData(ref object target, ParameterProperty mapping, object dataBaseValue);

		#endregion
	}
}
