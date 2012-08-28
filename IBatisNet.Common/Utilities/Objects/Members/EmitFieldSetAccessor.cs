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
    /// The <see cref="EmitFieldSetAccessor"/> class provides an IL-based set access   
    /// to a field of a specified target class.
    /// </summary>
    /// <remarks>Will Throw FieldAccessException on private field</remarks>
    public sealed class EmitFieldSetAccessor : BaseAccessor, ISetAccessor
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
        /// The IL emitted ISet
        /// </summary>
        private ISet _emittedSet = null;
        private Type _targetType = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmitFieldGetAccessor"/> class.
        /// </summary>
        /// <param name="targetObjectType">Type of the target object.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="assemblyBuilder">The assembly builder.</param>
        /// <param name="moduleBuilder">The module builder.</param>
        public EmitFieldSetAccessor(Type targetObjectType, string fieldName, AssemblyBuilder assemblyBuilder, ModuleBuilder moduleBuilder)
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
            _emittedSet = assemblyBuilder.CreateInstance("SetFor" + _targetType.FullName + _fieldName) as ISet;

            this.nullInternal = this.GetNullInternal(_fieldType);

            if (_emittedSet == null)
            {
                throw new NotSupportedException(
                    string.Format("Unable to create a set field accessor for '{0}' field on class  '{0}'.", _fieldName, _fieldType));
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
            // Define a public class named "SetFor.FullTargetTypeName.FieldName" in the assembly.
            TypeBuilder typeBuilder = moduleBuilder.DefineType("SetFor" + _targetType.FullName + _fieldName, TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed);

            // Mark the class as implementing ISet. 
            typeBuilder.AddInterfaceImplementation(typeof(ISet));

            // Add a constructor
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

            #region Emit Set
            // Define a method named "Set" for the set operation (IMemberAccessor).
            Type[] setParamTypes = new Type[] { typeof(object), typeof(object) };
            MethodBuilder setMethod = typeBuilder.DefineMethod("Set",
                MethodAttributes.Public | MethodAttributes.Virtual,
                null,
                setParamTypes);

            // Get an ILGenerator and used to emit the IL that we want.
            ILGenerator setIL = setMethod.GetILGenerator();

            FieldInfo targetField = _targetType.GetField(_fieldName, VISIBILITY);

            // Emit the IL for the set access. 
            if (targetField != null)
            {
                setIL.Emit(OpCodes.Ldarg_1);//Load the first argument (target object)
                setIL.Emit(OpCodes.Castclass, _targetType); //Cast to the source type
                setIL.Emit(OpCodes.Ldarg_2);//Load the second argument (value object)
                if (_fieldType.IsValueType)
                {
                    setIL.Emit(OpCodes.Unbox, _fieldType); //Unbox it 	
                    if (typeToOpcode[_fieldType] != null)
                    {
                        OpCode load = (OpCode)typeToOpcode[_fieldType];
                        setIL.Emit(load); //and load
                    }
                    else
                    {
                        setIL.Emit(OpCodes.Ldobj, _fieldType);
                    }
                    setIL.Emit(OpCodes.Stfld, targetField); //Set the field value
                }
                else
                {
                   // setIL.Emit(OpCodes.Castclass, _fieldType); //Cast class
                	setIL.Emit(OpCodes.Stfld, targetField);
                }
            }
            else
            {
                setIL.ThrowException(typeof(MissingMethodException));
            }
            setIL.Emit(OpCodes.Ret);
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
            _emittedSet.Set(target, newValue);
        }

        #endregion
    }
}
