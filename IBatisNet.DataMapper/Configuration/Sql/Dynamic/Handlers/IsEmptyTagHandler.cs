
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
using System.Collections;
using IBatisNet.Common.Utilities.Objects.Members;
using IBatisNet.DataMapper.Configuration.Sql.Dynamic.Elements;
using IBatisNet.Common.Utilities.Objects;

#endregion


namespace IBatisNet.DataMapper.Configuration.Sql.Dynamic.Handlers
{
	/// <summary>
	/// IsEmptyTagHandler represent a isEmpty tag element in a dynamic mapped statement.
	/// </summary>
	public class IsEmptyTagHandler : ConditionalTagHandler 
	{

        /// <summary>
        /// Initializes a new instance of the <see cref="IsEmptyTagHandler"/> class.
        /// </summary>
        /// <param name="accessorFactory">The accessor factory.</param>
        public IsEmptyTagHandler(AccessorFactory accessorFactory)
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
		public override bool IsCondition(SqlTagContext ctx, SqlTag tag, object parameterObject)
		{
			if (parameterObject == null) 
			{
				return true;
			} 
			else 
			{
				string propertyName = ((BaseTag)tag).Property;
				object value = null;
				if (propertyName != null && propertyName.Length>0) 
				{
					value = ObjectProbe.GetMemberValue(parameterObject, propertyName, this.AccessorFactory);
				} 
				else 
				{
					value = parameterObject;
				}
				if (value is ICollection) 
				{
					return ((value == null) || (((ICollection) value).Count< 1));
				} 
				else if (value != null && typeof(Array).IsAssignableFrom(value.GetType())) //value.GetType().IsArray
				{
					return ((Array) value).GetLength(0) == 0;
				}
				else 
				{
					return ((value == null) || (System.Convert.ToString(value).Equals("")));
				}
			}
		}
		#endregion

	}
}
