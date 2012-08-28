#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-04-09 20:24:53 +0200 (dim., 09 avr. 2006) $
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
using System.Reflection.Emit;

namespace IBatisNet.Common.Utilities.Objects.Members
{
    /// <summary>
    /// The <see cref="DelegateFieldSetAccessor"/> class defines a field get accessor and
    /// provides <c>Reflection.Emit</c>-generated <see cref="ISet"/> 
    /// via the new DynamicMethod (.NET V2).
    /// </summary>
    public sealed class DelegateFieldSetAccessor : BaseAccessor, ISetAccessor
    {
        private delegate void SetValue(object instance, object value);

        private SetValue _set = null;

        /// <summary>
        /// The field name
        /// </summary>
        private string _fieldName = string.Empty;
        /// <summary>
        /// The class parent type
        /// </summary>
        private Type _fieldType = null;

                 /// <summary>
        /// Initializes a new instance of the <see cref="T:DelegateFieldSetAccessor"/> class
        /// for field get access via DynamicMethod.
        /// </summary>
        /// <param name="targetObjectType">Type of the target object.</param>
        /// <param name="fieldName">Name of the field.</param>
        public DelegateFieldSetAccessor(Type targetObjectType, string fieldName)
        {
           // this.targetType = targetObjectType;
            _fieldName = fieldName;

            FieldInfo fieldInfo = targetObjectType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            // Make sure the field exists
            if (fieldInfo == null)
            {
                throw new NotSupportedException(
                    string.Format("Field \"{0}\" does not exist for type "
                    + "{1}.", fieldName, targetObjectType));
            }
            else
            {
                _fieldType = fieldInfo.FieldType;
                this.nullInternal = this.GetNullInternal(_fieldType);

                // Emit the IL for set access. 
                DynamicMethod dynamicMethodSet = new DynamicMethod("SetImplementation", null, new Type[] { typeof(object), typeof(object) }, this.GetType().Module, false);
                ILGenerator ilgen = dynamicMethodSet.GetILGenerator();

                ilgen = dynamicMethodSet.GetILGenerator();

                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldarg_1);
                UnboxIfNeeded(fieldInfo.FieldType, ilgen);
                ilgen.Emit(OpCodes.Stfld, fieldInfo);
                ilgen.Emit(OpCodes.Ret);

                _set = (SetValue)dynamicMethodSet.CreateDelegate(typeof(SetValue));
            }
		}

        #region IAccessor Members
        
        /// <summary>
        /// Gets the field's name.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return _fieldName; }
        }

        /// <summary>
        /// Gets the field's type.
        /// </summary>
        /// <value></value>
        public Type MemberType
        {
            get { return _fieldType; }
        }

        #endregion

        #region ISet Members

        /// <summary>
        /// Sets the field for the specified target.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <param name="value">Value to set.</param>
        public void Set(object target, object value)
        {
            object newValue = value;
            if (newValue == null)
            {
                // If the value to assign is null, assign null internal value
                newValue = nullInternal;
            }
            _set(target, newValue);
        }

        #endregion

        private static void UnboxIfNeeded(Type type, ILGenerator generator)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Unbox_Any, type);
            }
        }

    }
}
