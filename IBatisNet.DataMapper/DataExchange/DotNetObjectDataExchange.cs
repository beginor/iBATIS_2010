#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2008-10-11 15:26:43 -0600 (Sat, 11 Oct 2008) $
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
using IBatisNet.Common.Utilities.Objects;
using IBatisNet.Common.Utilities.Objects.Members;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.ResultMapping;

namespace IBatisNet.DataMapper.DataExchange
{
	/// <summary>
	/// IDataExchange implementation for .NET object
	/// </summary>
	public sealed class DotNetObjectDataExchange : BaseDataExchange
	{

        private Type _parameterClass = null;

		/// <summary>
		/// Cosntructor
		/// </summary>
		/// <param name="dataExchangeFactory"></param>
        /// <param name="parameterClass"></param>
        public DotNetObjectDataExchange(Type parameterClass, DataExchangeFactory dataExchangeFactory)
            : base(dataExchangeFactory)
		{
            _parameterClass = parameterClass;
		}

		#region IDataExchange Members

		/// <summary>
		/// Gets the data to be set into a IDataParameter.
		/// </summary>
		/// <param name="mapping"></param>
		/// <param name="parameterObject"></param>
		public override object GetData(ParameterProperty mapping, object parameterObject)
		{
            if (mapping.IsComplexMemberName || _parameterClass!=parameterObject.GetType())
			{
				return ObjectProbe.GetMemberValue(parameterObject, mapping.PropertyName,
					this.DataExchangeFactory.AccessorFactory);
			}
			else
			{
				return mapping.GetAccessor.Get(parameterObject);
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
            Type targetType = target.GetType();
            if ((targetType != _parameterClass)
                && !targetType.IsSubclassOf(_parameterClass)
                && !_parameterClass.IsAssignableFrom(targetType))
            {
                throw new ArgumentException("Could not set value in class '" + target.GetType() + "' for property '" + mapping.PropertyName + "' of type '" + mapping.MemberType + "'");
            }
			if ( mapping.IsComplexMemberName)
			{
				ObjectProbe.SetMemberValue(target, mapping.PropertyName, dataBaseValue, 
					this.DataExchangeFactory.ObjectFactory,
					this.DataExchangeFactory.AccessorFactory);
			}
			else
			{
                mapping.SetAccessor.Set(target, dataBaseValue);
			}
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
			if (mapping.IsComplexMemberName)
			{	
				ObjectProbe.SetMemberValue(target, mapping.PropertyName, dataBaseValue, 
					this.DataExchangeFactory.ObjectFactory,
					this.DataExchangeFactory.AccessorFactory);
			}
			else
			{
                ISetAccessorFactory setAccessorFactory = this.DataExchangeFactory.AccessorFactory.SetAccessorFactory;
                ISetAccessor _setAccessor = setAccessorFactory.CreateSetAccessor(_parameterClass, mapping.PropertyName);

                _setAccessor.Set(target, dataBaseValue);
			}
		}

		#endregion
	}
}
