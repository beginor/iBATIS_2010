#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-05-23 14:42:23 -0600 (Tue, 23 May 2006) $
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
	/// A IDataExchange implemtation for working with .NET object
	/// </summary>
	public sealed class ComplexDataExchange : BaseDataExchange
	{

		/// <summary>
		/// Cosntructor
		/// </summary>
		/// <param name="dataExchangeFactory"></param>
		public ComplexDataExchange(DataExchangeFactory dataExchangeFactory):base(dataExchangeFactory)
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
            if (parameterObject!=null)
            {
 			    if (this.DataExchangeFactory.TypeHandlerFactory.IsSimpleType(parameterObject.GetType()))
			    {
				    return parameterObject;
			    }
			    else
			    {
				    return ObjectProbe.GetMemberValue(parameterObject, mapping.PropertyName, this.DataExchangeFactory.AccessorFactory);
			    }               
            }
		    else
            {
                return null;
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
			ObjectProbe.SetMemberValue(target, mapping.PropertyName, dataBaseValue, 
				this.DataExchangeFactory.ObjectFactory,
				this.DataExchangeFactory.AccessorFactory);
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
			ObjectProbe.SetMemberValue(target, mapping.PropertyName, dataBaseValue, 
				this.DataExchangeFactory.ObjectFactory,
				this.DataExchangeFactory.AccessorFactory);
		}

		#endregion
	}
}
