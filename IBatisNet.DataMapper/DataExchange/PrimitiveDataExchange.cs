#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-05-08 07:21:44 -0600 (Mon, 08 May 2006) $
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

using IBatisNet.Common.Utilities.Objects;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;

namespace IBatisNet.DataMapper.DataExchange
{
	/// <summary>
	/// DataExchange implementation for "primitive" objects.
	/// </summary>
	/// <remarks>
	/// The primitive types are Boolean, Byte, SByte, Int16, UInt16, Int32,
	/// UInt32, Int64, UInt64, Char, Double, and Single + string, Guid, Decimal, DateTime
	/// </remarks>
	public sealed class PrimitiveDataExchange : BaseDataExchange
	{
		/// <summary>
		/// Cosntructor
		/// </summary>
		/// <param name="dataExchangeFactory"></param>
		public PrimitiveDataExchange(DataExchangeFactory dataExchangeFactory):base(dataExchangeFactory)
		{
		}

		#region IDataExchange Members

		/// <summary>
		/// Gets the data to be set into a IDataParameter.
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="parameterObject"></param>
		public override object GetData(ParameterProperty mapping, object parameterObject)
		{
			if (mapping.IsComplexMemberName)
			{
				return ObjectProbe.GetMemberValue(parameterObject, mapping.PropertyName, 
					this.DataExchangeFactory.AccessorFactory);
			}
			else
			{
				return parameterObject;
			}
		}

		/// <summary>
		/// Sets the value to the result property.
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="target"></param>
		/// <param name="dataBaseValue"></param>
		public override void SetData(ref object target, ResultProperty mapping, object dataBaseValue)
		{
			target = dataBaseValue;
		}

		/// <summary>
		/// Sets the value to the parameter property.
		/// </summary>
		/// <remarks>Use to set value on output parameter</remarks>
		/// <param name="mapping"></param>
		/// <param name="target"></param>
		/// <param name="dataBaseValue"></param>
		public override void SetData(ref object target, ParameterProperty mapping, object dataBaseValue)
		{
			target = dataBaseValue;
		}

		#endregion
	}
}
