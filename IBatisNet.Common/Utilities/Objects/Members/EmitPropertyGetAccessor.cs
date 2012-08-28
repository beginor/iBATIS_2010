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
    /// The <see cref="EmitPropertyGetAccessor"/> class provides an IL-based get access   
    /// to a property of a specified target class.
    /// </summary>
    public sealed class EmitPropertyGetAccessor : BaseAccessor, IGetAccessor
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
        private bool _canRead = false;
        /// <summary>
        /// The IL emitted IGet
        /// </summary>
        private IGet _emittedGet = null;


        /// <summary>
        /// Initializes a new instance of the <see cref="EmitPropertyGetAccessor"/> class.
        /// </summary>
        /// <param name="targetObjectType">Type of the target object.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="assemblyBuilder">The <see cref="AssemblyBuilder"/>.</param>
        /// <param name="moduleBuilder">The <see cref="ModuleBuilder"/>.</param>
        public EmitPropertyGetAccessor(Type targetObjectType, string propertyName, AssemblyBuilder assemblyBuilder, ModuleBuilder moduleBuilder)
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
                _canRead = propertyInfo.CanRead;
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
            _emittedGet = assemblyBuilder.CreateInstance("GetFor" + _targetType.FullName + _propertyName) as IGet;

            this.nullInternal = this.GetNullInternal(_propertyType);

            if (_emittedGet == null)
            {
                throw new NotSupportedException(
                    string.Format("Unable to create a get property accessor for \"{0}\".", _propertyType));
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
            TypeBuilder typeBuilder = moduleBuilder.DefineType("GetFor" + _targetType.FullName + _propertyName, 
				TypeAttributes.Class | TypeAttributes.Public | TypeAttributes.Sealed);

			// Mark the class as implementing IMemberAccessor. 
            typeBuilder.AddInterfaceImplementation(typeof(IGet));

            // Add a constructor
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);

			#region Emit Get
            // Define a method named "Get" for the get operation (IGet). 
			Type[] getParamTypes = new Type[] { typeof(object) };
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("Get",
				MethodAttributes.Public | MethodAttributes.Virtual, typeof(object), getParamTypes);
            // Get an ILGenerator and used it to emit the IL that we want.
            ILGenerator generatorIL = methodBuilder.GetILGenerator();

			if (_canRead)
            {
                // Emit the IL for get access. 
				MethodInfo targetGetMethod = _targetType.GetMethod("get_" + _propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
				if (targetGetMethod == null)
				{
					targetGetMethod =  _targetType.GetMethod("get_" + _propertyName);
				}                	

                generatorIL.DeclareLocal(typeof(object));
                generatorIL.Emit(OpCodes.Ldarg_1);	//Load the first argument,(target object)
                generatorIL.Emit(OpCodes.Castclass, _targetType);	//Cast to the source type
                generatorIL.EmitCall(OpCodes.Call, targetGetMethod, null); //Get the property value
                if (targetGetMethod.ReturnType.IsValueType)
                {
                    generatorIL.Emit(OpCodes.Box, targetGetMethod.ReturnType); //Box if necessary
                }
                generatorIL.Emit(OpCodes.Stloc_0); //Store it
                generatorIL.Emit(OpCodes.Ldloc_0);
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
        /// Gets the property's name.
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return _propertyName; }
        }

        /// <summary>
        /// Gets the property's type.
        /// </summary>
        /// <value></value>
        public Type MemberType
        {
            get { return _propertyType; }
        }

        #endregion

        #region IGet Members

        /// <summary>
        /// Gets the property value from the specified target.
        /// </summary>
        /// <param name="target">Target object.</param>
        /// <returns>Property value.</returns>
        public object Get(object target)
        {
            if (_canRead)
            {
                return _emittedGet.Get(target);
            }
            else
            {
                throw new NotSupportedException(
                    string.Format("Property \"{0}\" on type "
                    + "{1} doesn't have a get method.", _propertyName, _targetType));
            }
        }

        #endregion
    }
}
