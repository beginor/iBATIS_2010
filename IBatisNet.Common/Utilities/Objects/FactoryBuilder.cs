#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-02-19 12:37:22 +0100 (Sun, 19 Feb 2006) $
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
using IBatisNet.Common.Logging;

namespace IBatisNet.Common.Utilities.Objects
{
	/// <summary>
	/// Build IFactory object via IL 
	/// </summary>
	public class FactoryBuilder
	{
		private const BindingFlags VISIBILITY = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private const MethodAttributes CREATE_METHOD_ATTRIBUTES = MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final;
		private static readonly ILog _logger = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );

        private ModuleBuilder _moduleBuilder = null;
       
        /// <summary>
		/// constructor
		/// </summary>
		public FactoryBuilder()
		{
			AssemblyName assemblyName = new AssemblyName();
            assemblyName.Name = "iBATIS.EmitFactory" + HashCodeProvider.GetIdentityHashCode(this).ToString();

			// Create a new assembly with one module
			AssemblyBuilder _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
			_moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name + ".dll");
		}


		/// <summary>
		/// Create a factory which build class of type typeToCreate
		/// </summary>
		/// <param name="typeToCreate">The type instance to build</param>
		/// <param name="types">The types of the constructor arguments</param>
        /// <returns>Returns a new <see cref="IFactory"/> instance.</returns>
		public IFactory CreateFactory(Type typeToCreate, Type[] types)
		{
			if (typeToCreate.IsAbstract)
			{
				if (_logger.IsInfoEnabled)
				{
                    _logger.Info("Create a stub IFactory for abstract type " + typeToCreate.Name);
                }
                return new AbstractFactory(typeToCreate);
			}
			else
			{
				Type innerType = CreateFactoryType(typeToCreate, types);
				ConstructorInfo ctor = innerType.GetConstructor(Type.EmptyTypes);
				return (IFactory) ctor.Invoke(new object[] {});			
			}
		}


        /// <summary>
        /// Creates a <see cref="IFactory"/>.
        /// </summary>
        /// <param name="typeToCreate">The type instance to create.</param>
        /// <param name="types">The types.</param>
        /// <returns>The <see cref="IFactory"/></returns>
		private Type CreateFactoryType(Type typeToCreate, Type[] types)
		{
			string typesName = string.Empty;
			for(int i = 0; i < types.Length; i++)
			{
				typesName += types[i].Name.Replace("[]",string.Empty)+i.ToString();
			}
			TypeBuilder typeBuilder = _moduleBuilder.DefineType("EmitFactoryFor" + typeToCreate.FullName + typesName, TypeAttributes.Public);
			typeBuilder.AddInterfaceImplementation(typeof (IFactory));
			ImplementCreateInstance(typeBuilder, typeToCreate, types);
			return typeBuilder.CreateType();
		}

        /// <summary>
        /// Implements the create instance.
        /// </summary>
        /// <param name="typeBuilder">The type builder.</param>
        /// <param name="typeToCreate">The type to create.</param>
        /// <param name="argumentTypes">The argument types.</param>
		private void ImplementCreateInstance(TypeBuilder typeBuilder, Type typeToCreate, Type[] argumentTypes )
		{
			// object CreateInstance(object[] parameters);
			MethodBuilder meth = typeBuilder.DefineMethod("CreateInstance", CREATE_METHOD_ATTRIBUTES, typeof (object), new Type[]{typeof(object[])} );
			ILGenerator il = meth.GetILGenerator();

			// Add test if contructeur not public
			ConstructorInfo ctor = typeToCreate.GetConstructor(VISIBILITY, null, argumentTypes, null);
			if (ctor==null || !ctor.IsPublic)
			{
				throw new ProbeException(
					string.Format("Unable to optimize create instance. Cause : Could not find public constructor matching specified arguments for type \"{0}\".", typeToCreate.Name));
			}
			// new typeToCreate() or new typeToCreate(... arguments ...)
			EmitArgsIL(il, argumentTypes);
			il.Emit(OpCodes.Newobj, ctor);
			il.Emit(OpCodes.Ret);				
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
				il.Emit(OpCodes.Ldarg_1);   // Argument 1 is argument array.
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
