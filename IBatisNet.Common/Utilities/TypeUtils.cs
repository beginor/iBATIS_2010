#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-03-22 22:39:21 +0100 (mer., 22 mars 2006) $
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
#if dotnet2
using System.Collections.Generic;
#endif
using IBatisNet.Common.Utilities.TypesResolver;

namespace IBatisNet.Common.Utilities
{
    /// <summary>
    ///  Helper methods with regard to type.
    /// </summary>
    /// <remarks>
    /// <p>
    /// Mainly for internal use within the framework.
    /// </p>
    /// </remarks>
    public sealed class TypeUtils
    {
        #region Fields

        private static readonly ITypeResolver _internalTypeResolver = new CachedTypeResolver(new TypeResolver());

        #endregion

        #region Constructor (s) / Destructor

		/// <summary>
        /// Creates a new instance of the <see cref="IBatisNet.Common.Utilities.TypeUtils"/> class.
		/// </summary>
		/// <remarks>
		/// <p>
		/// This is a utility class, and as such exposes no public constructors.
		/// </p>
		/// </remarks>
        private TypeUtils()
		{
		}

		#endregion

        /// <summary>
        /// Resolves the supplied type name into a <see cref="System.Type"/>
        /// instance.
        /// </summary>
        /// <param name="typeName">
        /// The (possibly partially assembly qualified) name of a
        /// <see cref="System.Type"/>.
        /// </param>
        /// <returns>
        /// A resolved <see cref="System.Type"/> instance.
        /// </returns>
        /// <exception cref="System.TypeLoadException">
        /// If the type cannot be resolved.
        /// </exception>
        public static Type ResolveType(string typeName)
        {
            Type type = TypeRegistry.ResolveType(typeName);
            if (type == null)
            {
                type = _internalTypeResolver.Resolve(typeName);
            }
            return type;
        }

        /// <summary>
        /// Instantiate a 'Primitive' Type.
        /// </summary>
        /// <param name="typeCode">a typeCode.</param>
        /// <returns>An object.</returns>
        public static object InstantiatePrimitiveType(TypeCode typeCode)
        {
            object resultObject = null;

            switch (typeCode)
            {
                case TypeCode.Boolean:
                    resultObject = new Boolean();
                    break;
                case TypeCode.Byte:
                    resultObject = new Byte();
                    break;
                case TypeCode.Char:
                    resultObject = new Char();
                    break;
                case TypeCode.DateTime:
                    resultObject = new DateTime();
                    break;
                case TypeCode.Decimal:
                    resultObject = new Decimal();
                    break;
                case TypeCode.Double:
                    resultObject = new Double();
                    break;
                case TypeCode.Int16:
                    resultObject = new Int16();
                    break;
                case TypeCode.Int32:
                    resultObject = new Int32();
                    break;
                case TypeCode.Int64:
                    resultObject = new Int64();
                    break;
                case TypeCode.SByte:
                    resultObject = new SByte();
                    break;
                case TypeCode.Single:
                    resultObject = new Single();
                    break;
                case TypeCode.String:
                    resultObject = "";
                    break;
                case TypeCode.UInt16:
                    resultObject = new UInt16();
                    break;
                case TypeCode.UInt32:
                    resultObject = new UInt32();
                    break;
                case TypeCode.UInt64:
                    resultObject = new UInt64();
                    break;
            }
            return resultObject;
        }

#if dotnet2
        /// <summary>
        /// Instantiate a Nullable Type.
        /// </summary>
        /// <param name="type">The nullable type.</param>
        /// <returns>An object.</returns>
        public static object InstantiateNullableType(Type type)
        {
            object resultObject = null;

            if (type== typeof(bool?))
            {
                resultObject = new Nullable<bool>(false);
            }
            else if (type== typeof(byte?))
            {
                resultObject = new Nullable<byte>(byte.MinValue);
            }               
            else if (type== typeof(char?))
            {
                resultObject = new Nullable<char>(char.MinValue);
            }
            else if (type == typeof(DateTime?))
            {
                resultObject = new Nullable<DateTime>(DateTime.MinValue);
            }
            else if (type == typeof(decimal?))
            {
                resultObject = new Nullable<decimal>(decimal.MinValue);
            }
            else if (type == typeof(double?))
            {
                resultObject = new Nullable<double>(double.MinValue);
            }
            else if (type == typeof(Int16?))
            {
                resultObject = new Nullable<Int16>(Int16.MinValue);
            }
            else if (type == typeof(Int32?))
            {
                resultObject = new Nullable<Int32>(Int32.MinValue);
            }
            else if (type == typeof(Int64?))
            {
                resultObject = new Nullable<Int64>(Int64.MinValue);
            }
            else if (type == typeof(SByte?))
            {
                resultObject = new Nullable<SByte>(SByte.MinValue);
            }
            else if (type == typeof(Single?))
            {
                resultObject = new Nullable<Single>(Single.MinValue);
            }
            else if (type == typeof(UInt16?))
            {
                resultObject = new Nullable<UInt16>(UInt16.MinValue);
            }
            else if (type == typeof(UInt32?))
            {
                resultObject = new Nullable<UInt32>(UInt32.MinValue);
            }
            else if (type == typeof(UInt64?))
            {
                resultObject = new Nullable<UInt64>(UInt64.MinValue);
            }              

            return resultObject;
        }

        /// <summary>
        /// Determines whether the specified type is implement generic Ilist interface.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// 	<c>true</c> if the specified type is implement generic Ilist interface; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsImplementGenericIListInterface(Type type)
        {
            bool ret = false;

            if (!type.IsGenericType)
            {
                ret = false;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
            {
                return true;
            }
            else // check if one of the derived interfaces is IList<>
            {
                Type[] interfaceTypes = type.GetInterfaces();
                foreach (Type interfaceType in interfaceTypes)
                {
                    ret = IsImplementGenericIListInterface(interfaceType);
                    if (ret)
                    {
                        break;
                    }
                }
            }
            return ret;
        } 
#endif
    }
}
