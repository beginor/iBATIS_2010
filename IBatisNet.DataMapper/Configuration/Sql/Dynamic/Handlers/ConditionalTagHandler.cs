
#region Apache Notice
/*****************************************************************************
 * $Revision: 405046 $
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

#region Imports
using System;
using System.Text;
using IBatisNet.Common.Utilities.Objects.Members;
using IBatisNet.DataMapper.Exceptions;
using IBatisNet.DataMapper.Configuration.Sql.Dynamic.Elements;
using IBatisNet.Common.Utilities.Objects;

#endregion

namespace IBatisNet.DataMapper.Configuration.Sql.Dynamic.Handlers
{
	/// <summary>
	/// Description résumée de ConditionalTagHandler.
	/// </summary>
	public abstract class ConditionalTagHandler : BaseTagHandler 
	{

		#region Const
		/// <summary>
		/// 
		/// </summary>
		public const long NOT_COMPARABLE = long.MinValue;
		#endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionalTagHandler"/> class.
        /// </summary>
        /// <param name="accessorFactory">The accessor factory.</param>
        public ConditionalTagHandler(AccessorFactory accessorFactory)
            : base(accessorFactory)
		{
		}

		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="tag"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		public abstract bool IsCondition(SqlTagContext ctx, SqlTag tag, object parameterObject);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="tag"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		public override int DoStartFragment(SqlTagContext ctx, SqlTag tag, Object parameterObject) 
		{
			if (IsCondition(ctx, tag, parameterObject)) 
			{
				return BaseTagHandler.INCLUDE_BODY;
			} 
			else 
			{
				return BaseTagHandler.SKIP_BODY;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="tag"></param>
		/// <param name="parameterObject"></param>
		/// <param name="bodyContent"></param>
		/// <returns></returns>
		public override int DoEndFragment(SqlTagContext ctx, SqlTag tag, Object parameterObject, StringBuilder bodyContent) 
		{
			return BaseTagHandler.INCLUDE_BODY;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="sqlTag"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		protected long Compare(SqlTagContext ctx, SqlTag sqlTag, object parameterObject)
		{
			Conditional tag = (Conditional)sqlTag;
			string propertyName = tag.Property;
			string comparePropertyName = tag.CompareProperty;
			string compareValue = tag.CompareValue;

			object value1 = null;
			Type type = null;
			if (propertyName != null && propertyName.Length > 0) 
			{
				value1 = ObjectProbe.GetMemberValue(parameterObject, propertyName, this.AccessorFactory);
				type = value1.GetType();
			} 
			else 
			{
				value1 = parameterObject;
				if (value1 != null) 
				{
					type = parameterObject.GetType();
				} 
				else 
				{
					type = typeof(object);
				}
			}
			if (comparePropertyName != null && comparePropertyName.Length > 0) 
			{
                object value2 = ObjectProbe.GetMemberValue(parameterObject, comparePropertyName, this.AccessorFactory);
				return CompareValues(type, value1, value2);
			} 
			else if (compareValue != null && compareValue != "") 
			{
				return CompareValues(type, value1, compareValue);
			} 
			else 
			{
				throw new DataMapperException("Error comparing in conditional fragment.  Uknown 'compare to' values.");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="value1"></param>
		/// <param name="value2"></param>
		/// <returns></returns>
		protected long CompareValues(Type type, object value1, object value2) 
		{
			long result = NOT_COMPARABLE;

			if (value1 == null || value2 == null) 
			{
				result = value1 == value2 ? 0 : NOT_COMPARABLE;
			} 
			else 
			{
				if (value2.GetType() != type) 
				{
					value2 = ConvertValue(type, value2.ToString());
				}
				if (value2 is string && type != typeof(string)) 
				{
					value1 = value1.ToString();
				}
				if (!(value1 is IComparable && value2 is IComparable)) 
				{
					value1 = value1.ToString();
					value2 = value2.ToString();
				}
				result = ((IComparable) value1).CompareTo(value2);
			}

			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected object ConvertValue(Type type, string value) 
		{

			if (type == typeof(String)) 
			{
				return value;
			} 
			else if (type == typeof(bool)) 
			{
				return System.Convert.ToBoolean(value);
			} 
			else if (type == typeof(Byte)) 
			{
				return System.Convert.ToByte(value);
			} 
			else if (type == typeof(Char)) 
			{
				return System.Convert.ToChar(value.Substring(0,1));//new Character(value.charAt(0));
			}			
			else if (type == typeof(DateTime)) 
			{
				try 
				{
					return System.Convert.ToDateTime(value);
				} 
				catch (Exception e) 
				{
					throw new DataMapperException("Error parsing date. Cause: " + e.Message, e);
				}
			} 
			else if (type == typeof(Decimal)) 
			{
				return System.Convert.ToDecimal(value);
			} 
			else if (type == typeof(Double))  
			{
				return System.Convert.ToDouble(value);
			} 
			else if (type == typeof(Int16)) 
			{
				return System.Convert.ToInt16(value);
			} 
			else if (type == typeof(Int32)) 
			{
				return System.Convert.ToInt32(value);
			} 
			else if (type == typeof(Int64)) 
			{
				return System.Convert.ToInt64(value);
			} 
			else if (type == typeof(Single)) 
			{
				return System.Convert.ToSingle(value);
			} 
			else 
			{
				return value;
			}

		}
		#endregion

	}
}
