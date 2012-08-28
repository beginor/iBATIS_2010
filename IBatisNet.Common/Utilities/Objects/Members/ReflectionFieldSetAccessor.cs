#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-04-05 22:23:27 +0200 (mer., 05 avr. 2006) $
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
using System.Reflection;

namespace IBatisNet.Common.Utilities.Objects.Members
{
    /// <summary>
    /// The <see cref="ReflectionFieldSetAccessor"/> class provides an reflection set access   
    /// to a field of a specified target class.
    /// </summary>
    public sealed class ReflectionFieldSetAccessor : ISetAccessor
    {
                private FieldInfo _fieldInfo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionFieldSetAccessor"/> class.
        /// </summary>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="fieldName">Name of the field.</param>
        public ReflectionFieldSetAccessor(Type targetType, string fieldName)
		{
			ReflectionInfo reflectionCache = ReflectionInfo.GetInstance( targetType );
			_fieldInfo = (FieldInfo)reflectionCache.GetGetter(fieldName);
        }

        #region ISetAccessor Members

        /// <summary>
        /// Gets the member name.
        /// </summary>
        public string Name
        {
            get { return _fieldInfo.Name; }
        }

        /// <summary>
        /// Gets the type of this member, such as field, property.
        /// </summary>
        public Type MemberType
        {
            get { return _fieldInfo.FieldType; }
        }

        /// <summary>
        /// Sets the value for the field of the specified target.
        /// </summary>
        /// <param name="target">Object to set the property on.</param>
        /// <param name="value">Property value.</param>
        public void Set(object target, object value)
        {
            _fieldInfo.SetValue(target, value);
        }

        #endregion
    }
}
