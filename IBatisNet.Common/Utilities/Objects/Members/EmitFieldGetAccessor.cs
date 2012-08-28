#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-04-30 18:01:40 +0200 (dim., 30 avr. 2006) $
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
    /// The <see cref="EmitFieldGetAccessor"/> class provides an IL-based get access   
    /// to a field of a specified target class.
    /// </summary>
    /// <remarks>Will Throw FieldAccessException on private field</remarks>
    public sealed class EmitFieldGetAccessor : BaseAccessor, IGetAccessor
    {
        private const BindingFlags VISIBILITY = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// The field name
        /// </summary>
        private string _fieldName = string.Empty;        
        /// <summary>
        /// The class parent type
        /// </summary>
        private Type _fieldType = null;
        /// <summary>
        /// The IL emitted IGet
        /// </summary>
        private IGet _emittedGet = null;
        private Type _targetType = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmitFieldGetAccessor"/> class.
        /// </summary>
        /// <param name="targetObjectType">Type of the target object.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="assemblyBuilder">The assembly builder.</param>
        /// <param name="moduleBuilder">The module builder.</param>
        public EmitFieldGetAccessor(Type targetObjectType, string fieldName, AssemblyBuilder assemblyBuilder, ModuleBuilder moduleBuilder)
		{
            _targetType = targetObjectType;
			_fieldName = fieldName;

            FieldInfo fieldInfo = _targetType.GetField(fieldName, VISIBILITY);

			// Make sure the field exists
			if(fieldInfo == null)
			{
				throw new NotSupportedException(
					string.Format("Field \"{0}\" does not exist for type "
					+ "{1}.", fieldName, targetObjectType));
			}
			else
			{
				_fieldType = fieldInfo.FieldType;
                this.EmitIL(assemblyBuilder, moduleBuilder);
			}
		}

        /// <summary>
        /// This method create a new type oject for the the property accessor class 
        /// that will provide dynamic access.
        /// </summary>
        /// <param name="assemblyBuilder">The assembly builder.</param>
        /// <param name="moduleBuilder">The module builder.</param>
        private void EmitIL(AssemblyBuilder assemblyBuilder, ModuleBuilder moduleBuilder)
        {
            // Create a new type object for the the field accessor class.
            EmitType(moduleBuilder);

            // Create a new instance
            _emittedGet = assemblyBuilder.CreateInstance("GetFor" + _targetType.FullName + _fieldName) as IGet;

            this.nullInternal = this.GetNullInternal(_fieldType);

            if (_emittedGet == null)
            {
                throw new NotSupportedException(
                    string.Format("Unable to create a get field accessor for '{0}' field on class  '{0}'.", _fieldName, _fieldType));
            }
        }

        /// <summary>
        /// Create an type that will provide the set access method.
        /// </summary>
        /// <remarks>
        ///  new ReflectionPermission(PermissionState.Unrestricted).Assert();
        ///  CodeAccessPermission.RevertAssert();
        /// </remarks>
        /// <param name="moduleBuilder">The module builder.</param>
        private void EmitType(ModuleBuilder moduleBuilder)
        {
            // Define a public class named "GetFor.FullTagetTypeName.FieldName" in the assembly.
            TypeBuilder typeBuilder = moduleBuilder.DefineType("GetFor" + _targetType.FullName + _fieldName, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed);

            // Mark the class as implementing IMemberAccessor. 
            typeBuilder.AddInterfaceImplementation(typeof(IGet));

            // Add a constructor
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            #region Emit Get
            // Define a method named "Get" for the get operation (IMemberAccessor). 
            Type[] getParamTypes = new Type[] { typeof(object) };
            MethodBuilder getMethod = typeBuilder.DefineMethod("Get",
                MethodAttributes.Public | MethodAttributes.Virtual,
                typeof(object),
                getParamTypes);

            // Get an ILGenerator and used it to emit the IL that we want.
            ILGenerator getIL = getMethod.GetILGenerator();

            FieldInfo targetField = _targetType.GetField(_fieldName, VISIBILITY);

            // Emit the IL for get access. 
            if (targetField != null)
            {
                // We need a reference to the current instance (stored in local argument index 1) 
                // so Ldfld can load from the correct instance (this one).
                getIL.Emit(OpCodes.Ldarg_1);
                getIL.Emit(OpCodes.Ldfld, targetField);
                if (_fieldType.IsValueType)
                {
                    // Now, we execute the box opcode, which pops the value of field 'x',
                    // returning a reference to the filed value boxed as an object.
                    getIL.Emit(OpCodes.Box, targetField.FieldType);
                }
                getIL.Emit(OpCodes.Ret);
            }
            else
            {
                getIL.ThrowException(typeof(MissingMethodException));
            }
            #endregion

            // Load the type
            typeBuilder.CreateType();
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

        #region IGet Members

        /// <summary>
        /// Gets the value stored in the field for the specified target.
        /// </summary>
        /// <param name="target">Object to retrieve the field from.</param>
        /// <returns>The value.</returns>
        public object Get(object target)
        {
            return _emittedGet.Get(target);
        }

        #endregion
    }
}
