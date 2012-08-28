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
    /// The <see cref="EmitPropertySetAccessor"/> class provides an IL-based set access   
    /// to a property of a specified target class.
    /// </summary>
    public sealed class EmitPropertySetAccessor : BaseAccessor, ISetAccessor
    {
        /// <summary>
        /// The property name
        /// </summary>
        private string _propertyName = string.Empty;
        /// <summary>
        /// The property type
        /// </summary>
        private Type _propertyType = null;
        /// <summary>
        /// The class parent type
        /// </summary>
        private Type _targetType = null;

        private bool _canWrite = false;
        /// <summary>
        /// The IL emitted ISet
        /// </summary>
        private ISet _emittedSet = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmitPropertySetAccessor"/> class.
        /// Generates the implementation for setter methods.
        /// </summary>
        /// <param name="targetObjectType">Type of the target object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="assemblyBuilder">The <see cref="AssemblyBuilder"/>.</param>
        /// <param name="moduleBuilder">The <see cref="ModuleBuilder"/>.</param>
        public EmitPropertySetAccessor(Type targetObjectType, string propertyName, AssemblyBuilder assemblyBuilder, ModuleBuilder moduleBuilder)
		{
			_targetType = targetObjectType;
			_propertyName = propertyName;

			// deals with Overriding a property using new and reflection
			// http://blogs.msdn.com/thottams/archive/2006/03/17/553376.aspx
			PropertyInfo propertyInfo = _targetType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			if (propertyInfo == null)
			{
				propertyInfo = _targetType.GetProperty(propertyName);
			}
        	
			// Make sure the property exists
			if(propertyInfo == null)
			{
				throw new NotSupportedException(
					string.Format("Property \"{0}\" does not exist for type "
					+ "{1}.", propertyName, _targetType));
			}
			else
			{
				this._propertyType = propertyInfo.PropertyType;
				_canWrite = propertyInfo.CanWrite;
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
            _emittedSet = assemblyBuilder.CreateInstance("SetFor" + _targetType.FullName + _propertyName) as ISet;

            this.nullInternal = this.GetNullInternal(_propertyType);

            if (_emittedSet == null)
            {
                throw new NotSupportedException(
                    string.Format("Unable to create a get propert accessor for '{0}' property on class  '{1}'.", _propertyName, _propertyType.ToString()));
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
			// Define a public class named "PropertyAccessorFor.FullTagetTypeName.PropertyName" in the assembly.
            TypeBuilder typeBuilder = moduleBuilder.DefineType("SetFor" + _targetType.FullName + _propertyName, 
				TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed);

			// Mark the class as implementing IMemberAccessor. 
            typeBuilder.AddInterfaceImplementation(typeof(ISet));

            // Add a constructor
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);	

			#region Emit Set
            // Define a method named "Set" for the set operation (IMemberAccessor).
            Type[] setParamTypes = new Type[] { typeof(object), typeof(object) };
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("Set",
                MethodAttributes.Public | MethodAttributes.Virtual,
                null,
                setParamTypes);

			// Get an ILGenerator and  used to emit the IL that we want.
			// Set(object, value);
			ILGenerator generatorIL = methodBuilder.GetILGenerator();
			if (_canWrite)
            {
                // Emit the IL for the set access. 
				MethodInfo targetSetMethod = _targetType.GetMethod("set_" + _propertyName,BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
				if (targetSetMethod == null)
				{
					targetSetMethod =  _targetType.GetMethod("set_" + _propertyName);
				}
                Type paramType = targetSetMethod.GetParameters()[0].ParameterType;

                generatorIL.DeclareLocal(paramType);
                generatorIL.Emit(OpCodes.Ldarg_1); //Load the first argument (target object)
                generatorIL.Emit(OpCodes.Castclass, _targetType); //Cast to the source type
                generatorIL.Emit(OpCodes.Ldarg_2); //Load the second argument (value object)
                if (paramType.IsValueType)
                {
                    generatorIL.Emit(OpCodes.Unbox, paramType); //Unbox it 	
                    if (typeToOpcode[paramType] != null)
                    {
                        OpCode load = (OpCode)typeToOpcode[paramType];
                        generatorIL.Emit(load); //and load
                    }
                    else
                    {
                        generatorIL.Emit(OpCodes.Ldobj, paramType);
                    }
                }
                else
                {
                    generatorIL.Emit(OpCodes.Castclass, paramType); //Cast class
                }
                generatorIL.EmitCall(OpCodes.Callvirt, targetSetMethod, null); //Set the property value
                generatorIL.Emit(OpCodes.Ret);
            }
			else
			{
				generatorIL.ThrowException(typeof(MissingMethodException));
			}
			#endregion

			// Load the type
			typeBuilder.CreateType();
		}

        #region IAccessor Members

        /// <summary>
        /// Gets the member name.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// Gets the type of this member (field or property).
        /// </summary>
        /// <value></value>
        public Type MemberType
        {
            get { return _propertyType; }
        }

        #endregion

        #region ISet Members

        /// <summary>
        /// Sets the property for the specified target.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <param name="value">Value to set.</param>
        public void Set(object target, object value)
        {
            if (_canWrite)
            {
                object newValue = value;
                if (newValue == null)
                {
                    // If the value to assign is null, assign null internal value
                    newValue = nullInternal;
                }

                _emittedSet.Set(target, newValue);
            }
            else
            {
                throw new NotSupportedException(
                    string.Format("Property \"{0}\" on type "
                    + "{1} doesn't have a set method.", _propertyName, _targetType));
            }
        }

        #endregion
    }
}
