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
using IBatisNet.Common.Exceptions;

namespace IBatisNet.Common.Utilities.Objects
{
    /// <summary>
    /// A <see cref="IFactory"/> implementation that builds object via DynamicMethod.
    /// </summary>
    public sealed class DelegateFactory : IFactory
    {
        private const BindingFlags VISIBILITY = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private delegate object Create(object[] parameters);

        private Create _create = null;

        #region IFactory members

        /// <summary>
        /// Create a new instance with the specified parameters
        /// </summary>
        /// <param name="parameters">
        /// An array of values that matches the number, order and type 
        /// of the parameters for this constructor. 
        /// </param>
        /// <remarks>
        /// If you call a constructor with no parameters, pass null. 
        /// Anyway, what you pass will be ignore.
        /// </remarks>
        /// <returns>A new instance</returns>
        public object CreateInstance(object[] parameters)
        {
            return _create(parameters);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateFactory"/> class.
        /// </summary>
        /// <param name="typeToCreate">The instance type to create.</param>
        /// <param name="argumentTypes">The types argument.</param>
        public DelegateFactory(Type typeToCreate, Type[] argumentTypes)
        {
            DynamicMethod dynamicMethod = new DynamicMethod("CreateImplementation", typeof(object), new Type[] { typeof(object[]) }, this.GetType().Module, false);
            ILGenerator generatorIL = dynamicMethod.GetILGenerator();
            
            // Emit the IL for Create method. 
            // Add test if contructeur not public
            ConstructorInfo constructorInfo = typeToCreate.GetConstructor(VISIBILITY, null, argumentTypes, null);
            if (constructorInfo==null || !constructorInfo.IsPublic)
            {
                throw new ProbeException(
                    string.Format("Unable to optimize create instance. Cause : Could not find public constructor matching specified arguments for type \"{0}\".", typeToCreate.Name));
            }
            // new typeToCreate() or new typeToCreate(... arguments ...)
            EmitArgsIL(generatorIL, argumentTypes);
            generatorIL.Emit(OpCodes.Newobj, constructorInfo);
            generatorIL.Emit(OpCodes.Ret);

            _create = (Create)dynamicMethod.CreateDelegate(typeof(Create));
        }

        /// <summary>   
        /// Emit parameter IL for a method call.   
        /// </summary>   
        /// <param name="il">IL generator.</param>   
        /// <param name="argumentTypes">Arguments type defined for a the constructor.</param>   
        private void EmitArgsIL(ILGenerator il, Type[] argumentTypes)
        {
            // Add args. Since all args are objects, value types are unboxed. 
            // Refs to value types are to be converted to values themselves.   
            for (int i = 0; i < argumentTypes.Length; i++)
            {
                // Push args array reference on the stack , followed by the index.   
                // Ldelem will resolve them to args[i].   
                il.Emit(OpCodes.Ldarg_0);   // Argument 1 is argument array.
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);

                // If param is a primitive/value type then we need to unbox it.   
                Type paramType = argumentTypes[i];
                if (paramType.IsValueType)
                {
                    if (paramType.IsPrimitive || paramType.IsEnum)
                    {
                        il.Emit(OpCodes.Unbox, paramType);
                        il.Emit(BoxingOpCodes.GetOpCode(paramType));
                    }
                    else if (paramType.IsValueType)
                    {
                        il.Emit(OpCodes.Unbox, paramType);
                        il.Emit(OpCodes.Ldobj, paramType);
                    }
                }
            }
        }  
    }
}
