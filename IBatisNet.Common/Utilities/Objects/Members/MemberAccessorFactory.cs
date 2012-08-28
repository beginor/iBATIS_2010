#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
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

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using IBatisNet.Common.Exceptions;

namespace IBatisNet.Common.Utilities.Objects.Members
{
	/// <summary>
	/// A factory to build IMemberAccessor for a type.
	/// </summary>
	public class MemberAccessorFactory : IMemberAccessorFactory
	{
		private delegate IMemberAccessor CreateMemberPropertyAccessor(Type targetType, string propertyName);
		private delegate IMemberAccessor CreateMemberFieldAccessor(Type targetType, string fieldName);
		
		private CreateMemberPropertyAccessor _createPropertyAccessor = null;
		private CreateMemberFieldAccessor _createFieldAccessor = null;

		private IDictionary _cachedIMemberAccessor = new HybridDictionary();
		private AssemblyBuilder _assemblyBuilder = null;
		private ModuleBuilder _moduleBuilder = null;
		private object _syncObject = new object();


        /// <summary>
        /// Initializes a new instance of the <see cref="T:MemberAccessorFactory"/> class.
        /// </summary>
        /// <param name="allowCodeGeneration">if set to <c>true</c> [allow code generation].</param>
		public MemberAccessorFactory(bool allowCodeGeneration)
		{
			if (allowCodeGeneration)
            {
                // Detect runtime environment and create the appropriate factory
				if (Environment.Version.Major >= 2)
				{
#if dotnet2
                    _createPropertyAccessor = new CreateMemberPropertyAccessor(CreateDelegatePropertyAccessor);
                    _createFieldAccessor = new CreateMemberFieldAccessor(CreateDynamicFieldAccessor);
#endif
				}
				else
				{
                AssemblyName assemblyName = new AssemblyName();
				assemblyName.Name = "iBATIS.FastPropertyAccessor"+HashCodeProvider.GetIdentityHashCode(this).ToString();

				// Create a new assembly with one module
				_assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
				_moduleBuilder = _assemblyBuilder.DefineDynamicModule(assemblyName.Name + ".dll");
				
                _createPropertyAccessor = new CreateMemberPropertyAccessor(CreateEmitPropertyAccessor);
                _createFieldAccessor = new CreateMemberFieldAccessor(CreateFieldAccessor);
				}
			}
			else
			{
				_createPropertyAccessor = new CreateMemberPropertyAccessor(CreateReflectionPropertyAccessor);
				_createFieldAccessor = new CreateMemberFieldAccessor(CreateReflectionFieldAccessor);
			}

		}

		/// <summary>
		/// Generate an IMemberAccessor object
		/// </summary>
		/// <param name="targetType">Target object type.</param>
		/// <param name="name">Field or Property name.</param>
		/// <returns>null if the generation fail</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public IMemberAccessor CreateMemberAccessor(Type targetType, string name)
		{
			string key = targetType.FullName+name;
			
			if (_cachedIMemberAccessor.Contains(key))
			{
				return (IMemberAccessor)_cachedIMemberAccessor[key];
			}
			else
			{
				IMemberAccessor memberAccessor = null;
				lock (_syncObject)
				{
					if (!_cachedIMemberAccessor.Contains(key))
					{
						// Property
						PropertyInfo propertyInfo = targetType.GetProperty(name);

						if (propertyInfo!=null)
						{
							memberAccessor = _createPropertyAccessor(targetType, name);
							_cachedIMemberAccessor[key] = memberAccessor;
						}
						else 
						{
							// Field
							FieldInfo fieldInfo = targetType.GetField(name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

							if (fieldInfo!=null)
							{
								memberAccessor = _createFieldAccessor(targetType, name);
								_cachedIMemberAccessor[key] = memberAccessor;
							}
							else
							{
								throw new ProbeException(
									string.Format("No property or field named \"{0}\" exists for type "
									+ "{1}.",name, targetType));
							}
						}
					}
					else
					{
						memberAccessor = (IMemberAccessor)_cachedIMemberAccessor[key];
					}
				}
				return memberAccessor;
			}
		}

#if dotnet2
        /// <summary>
        /// Create a Dynamic IMemberAccessor instance for a property
        /// </summary>
        /// <param name="targetType">Target object type.</param>
        /// <param name="propertyName">Property name.</param>
        /// <returns>null if the generation fail</returns>
        private IMemberAccessor CreateDelegatePropertyAccessor(Type targetType, string propertyName)
        {
            return new DelegatePropertyAccessor(targetType, propertyName);
        }

        /// <summary>
        /// Create a Dynamic IMemberAccessor instance for a field
        /// </summary>
        /// <param name="targetType">Target object type.</param>
        /// <param name="fieldName">Property name.</param>
        /// <returns>null if the generation fail</returns>
        private IMemberAccessor CreateDynamicFieldAccessor(Type targetType, string fieldName)
        {
            FieldInfo fieldInfo = targetType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            if (fieldInfo.IsPublic)
            {
                return new DelegateFieldAccessor(targetType, fieldName);
            }
            else
            {
                return new ReflectionFieldAccessor(targetType, fieldName);
            }
        }
#endif

		/// <summary>
        /// Create an IL IMemberAccessor instance for a property
		/// </summary>
		/// <param name="targetType">Target object type.</param>
		/// <param name="propertyName">Property name.</param>
		/// <returns>null if the generation fail</returns>
		private IMemberAccessor CreateEmitPropertyAccessor(Type targetType, string propertyName)
		{
			return new EmitPropertyAccessor(targetType, propertyName, _assemblyBuilder, _moduleBuilder);
		}

		/// <summary>
        /// Create a IMemberAccessor instance for a fiedl
		/// </summary>
		/// <param name="targetType">Target object type.</param>
		/// <param name="fieldName">Field name.</param>
		/// <returns>null if the generation fail</returns>
        private IMemberAccessor CreateFieldAccessor(Type targetType, string fieldName)
		{
            FieldInfo fieldInfo = targetType.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            if (fieldInfo.IsPublic)
            {
			    return new EmitFieldAccessor(targetType, fieldName, _assemblyBuilder, _moduleBuilder);
            }
            else
            {
                return new ReflectionFieldAccessor(targetType, fieldName);
            }
		}

		/// <summary>
        /// Create a Reflection IMemberAccessor instance for a property
		/// </summary>
		/// <param name="targetType">Target object type.</param>
		/// <param name="propertyName">Property name.</param>
		/// <returns>null if the generation fail</returns>
		private IMemberAccessor CreateReflectionPropertyAccessor(Type targetType, string propertyName)
		{
			return new ReflectionPropertyAccessor(targetType, propertyName);
		}

		/// <summary>
        /// Create Reflection IMemberAccessor instance for a field
		/// </summary>
		/// <param name="targetType">Target object type.</param>
		/// <param name="fieldName">field name.</param>
		/// <returns>null if the generation fail</returns>
		private IMemberAccessor CreateReflectionFieldAccessor(Type targetType, string fieldName)
		{
			return new ReflectionFieldAccessor(targetType, fieldName);
		}
	}
}
