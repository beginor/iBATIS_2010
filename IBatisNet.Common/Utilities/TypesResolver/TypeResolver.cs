
#region Apache Notice
/*****************************************************************************
 * $Revision: 663728 $
 * $LastChangedDate: 2008-06-05 14:40:05 -0600 (Thu, 05 Jun 2008) $
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

#region Remarks
// Inpspired from Spring.NET
#endregion

#region Using

using System;

#if dotnet2
using System.Collections.Generic;
#endif
using System.Reflection;

using System.IO;
using System.Text;
using System.Text.RegularExpressions;
#endregion

namespace IBatisNet.Common.Utilities.TypesResolver
{
	/// <summary>
	/// Resolves a <see cref="System.Type"/> by name.
	/// </summary>
	/// <remarks>
	/// <p>
	/// The rationale behind the creation of this class is to centralise the
	/// resolution of type names to <see cref="System.Type"/> instances beyond that
	/// offered by the plain vanilla System.Type.GetType method call.
	/// </p>
	/// </remarks>
	/// <version>$Id: TypeResolver.cs,v 1.5 2004/09/28 07:51:47 springboy Exp $</version>
    public class TypeResolver : ITypeResolver
	{
        private const string NULLABLE_TYPE = "System.Nullable";

        #region ITypeResolver Members
        /// <summary>
        /// Resolves the supplied <paramref name="typeName"/> to a
        /// <see cref="System.Type"/> instance.
        /// </summary>
        /// <param name="typeName">
        /// The unresolved name of a <see cref="System.Type"/>.
        /// </param>
        /// <returns>
        /// A resolved <see cref="System.Type"/> instance.
        /// </returns>
        /// <exception cref="System.TypeLoadException">
        /// If the supplied <paramref name="typeName"/> could not be resolved
        /// to a <see cref="System.Type"/>.
        /// </exception>
        public virtual Type Resolve(string typeName)
        {
#if dotnet2
            Type type = ResolveGenericType(typeName.Replace(" ", string.Empty));
            if (type == null)
            {
                type = ResolveType(typeName.Replace(" ", string.Empty));
            }
            return type;
#else
            return ResolveType(typeName.Replace(" ", string.Empty));
#endif
        }
        #endregion

#if dotnet2
        /// <summary>
        /// Resolves the supplied generic <paramref name="typeName"/>,
        /// substituting recursively all its type parameters., 
        /// to a <see cref="System.Type"/> instance.
        /// </summary>
        /// <param name="typeName">
        /// The (possibly generic) name of a <see cref="System.Type"/>.
        /// </param>
        /// <returns>
        /// A resolved <see cref="System.Type"/> instance.
        /// </returns>
        /// <exception cref="System.TypeLoadException">
        /// If the supplied <paramref name="typeName"/> could not be resolved
        /// to a <see cref="System.Type"/>.
        /// </exception>
        private Type ResolveGenericType(string typeName)
        {
            #region Sanity Check
			if (typeName ==  null || typeName.Trim().Length==0)
			{
                throw BuildTypeLoadException(typeName);
			}
			#endregion

            if (typeName.StartsWith(NULLABLE_TYPE))
            {
                return null;
            }
            else
            {
                GenericArgumentsInfo genericInfo = new GenericArgumentsInfo(typeName);
                Type type = null;
                try
                {
                    if (genericInfo.ContainsGenericArguments)
                    {
                        type = TypeUtils.ResolveType(genericInfo.GenericTypeName);
                        if (!genericInfo.IsGenericDefinition)
                        {
                            string[] unresolvedGenericArgs = genericInfo.GetGenericArguments();
                            Type[] genericArgs = new Type[unresolvedGenericArgs.Length];
                            for (int i = 0; i < unresolvedGenericArgs.Length; i++)
                            {
                                genericArgs[i] = TypeUtils.ResolveType(unresolvedGenericArgs[i]);
                            }
                            type = type.MakeGenericType(genericArgs);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex is TypeLoadException)
                    {
                        throw;
                    }
                    throw BuildTypeLoadException(typeName, ex);
                }
                return type;
            }
        }
#endif

        /// <summary>
        /// Resolves the supplied <paramref name="typeName"/> to a
        /// <see cref="System.Type"/>
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
        /// If the supplied <paramref name="typeName"/> could not be resolved
        /// to a <see cref="System.Type"/>.
        /// </exception>
        private Type ResolveType(string typeName)
        {
            #region Sanity Check
            if (typeName == null || typeName.Trim().Length == 0)
            {
                throw BuildTypeLoadException(typeName);
            }
            #endregion

            TypeAssemblyInfo typeInfo = new TypeAssemblyInfo(typeName);
            Type type = null;
            try
            {
                type = (typeInfo.IsAssemblyQualified) ?
                     LoadTypeDirectlyFromAssembly(typeInfo) :
                     LoadTypeByIteratingOverAllLoadedAssemblies(typeInfo);
            }
            catch (Exception ex)
            {
                throw BuildTypeLoadException(typeName, ex);
            }
            if (type == null)
            {
                throw BuildTypeLoadException(typeName);
            }
            return type;
        }

        /// <summary>
        /// Uses <see cref="System.Reflection.Assembly.LoadWithPartialName(string)"/>
        /// to load an <see cref="System.Reflection.Assembly"/> and then the attendant
        /// <see cref="System.Type"/> referred to by the <paramref name="typeInfo"/>
        /// parameter.
        /// </summary>
        /// <remarks>
        /// <p>
        /// <see cref="System.Reflection.Assembly.LoadWithPartialName(string)"/> is
        /// deprecated in .NET 2.0, but is still used here (even when this class is
        /// compiled for .NET 2.0);
        /// <see cref="System.Reflection.Assembly.LoadWithPartialName(string)"/> will
        /// still resolve (non-.NET Framework) local assemblies when given only the
        /// display name of an assembly (the behaviour for .NET Framework assemblies
        /// and strongly named assemblies is documented in the docs for the
        /// <see cref="System.Reflection.Assembly.LoadWithPartialName(string)"/> method).
        /// </p>
        /// </remarks>
        /// <param name="typeInfo">
        /// The assembly and type to be loaded.
        /// </param>
        /// <returns>
        /// A <see cref="System.Type"/>, or <see lang="null"/>.
        /// </returns>
        /// <exception cref="System.Exception">
        /// <see cref="System.Reflection.Assembly.LoadWithPartialName(string)"/>
        /// </exception>
        private static Type LoadTypeDirectlyFromAssembly(TypeAssemblyInfo typeInfo)
        {
            Type type = null;
            // assembly qualified... load the assembly, then the Type
            Assembly assembly = null;

#if dotnet2
            assembly = Assembly.Load(typeInfo.AssemblyName);
#else
            assembly = Assembly.LoadWithPartialName (typeInfo.AssemblyName);
#endif

            if (assembly != null)
            {
                type = assembly.GetType(typeInfo.TypeName, true, true);
            }
            return type;
        }

        /// <summary>
        /// Check all assembly
        /// to load the attendant <see cref="System.Type"/> referred to by 
        /// the <paramref name="typeInfo"/> parameter.
        /// </summary>
        /// <param name="typeInfo">
        /// The type to be loaded.
        /// </param>
        /// <returns>
        /// A <see cref="System.Type"/>, or <see lang="null"/>.
        /// </returns>
        private static Type LoadTypeByIteratingOverAllLoadedAssemblies(TypeAssemblyInfo typeInfo)
        {
            Type type = null;
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                type = assembly.GetType(typeInfo.TypeName, false, false);
                if (type != null)
                {
                    break;
                }
            }
            return type;
        }

        private static TypeLoadException BuildTypeLoadException(string typeName)
        {
            return new TypeLoadException("Could not load type from string value '" + typeName + "'.");
        }

        private static TypeLoadException BuildTypeLoadException(string typeName, Exception ex)
        {
            return new TypeLoadException("Could not load type from string value '" + typeName + "'.", ex);
        }

#if dotnet2
        #region Inner Class : GenericArgumentsInfo

        /// <summary>
        /// Holder for the generic arguments when using type parameters.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Type parameters can be applied to classes, interfaces, 
        /// structures, methods, delegates, etc...
        /// </p>
        /// </remarks>
        internal class GenericArgumentsInfo
        {
            #region Constants

            /// <summary>
            /// The generic arguments prefix.
            /// </summary>
            public const string GENERIC_ARGUMENTS_PREFIX = "[[";

            /// <summary>
            /// The generic arguments suffix.
            /// </summary>
            public const string GENERIC_ARGUMENTS_SUFFIX = "]]";

            /// <summary>
            /// The character that separates a list of generic arguments.
            /// </summary>
            public const string GENERIC_ARGUMENTS_SEPARATOR = "],[";
            
            #endregion

            #region Fields

            private string _unresolvedGenericTypeName = string.Empty;
            private string[] _unresolvedGenericArguments = null;
            private readonly static Regex generic = new Regex(@"`\d*\[\[", RegexOptions.Compiled); 
            #endregion

            #region Constructor (s) / Destructor

            /// <summary>
            /// Creates a new instance of the GenericArgumentsInfo class.
            /// </summary>
            /// <param name="value">
            /// The string value to parse looking for a generic definition
            /// and retrieving its generic arguments.
            /// </param>
            public GenericArgumentsInfo(string value)
            {
                ParseGenericArguments(value);
            }

            #endregion

            #region Properties

            /// <summary>
            /// The (unresolved) generic type name portion 
            /// of the original value when parsing a generic type.
            /// </summary>
            public string GenericTypeName
            {
                get { return _unresolvedGenericTypeName; }
            }


            /// <summary>
            /// Is the string value contains generic arguments ?
            /// </summary>
            /// <remarks>
            /// <p>
            /// A generic argument can be a type parameter or a type argument.
            /// </p>
            /// </remarks>
            public bool ContainsGenericArguments
            {
                get
                {
                    return (_unresolvedGenericArguments != null &&
                        _unresolvedGenericArguments.Length > 0);
                }
            }

            /// <summary>
            /// Is generic arguments only contains type parameters ?
            /// </summary>
            public bool IsGenericDefinition
            {
                get
                {
                    if (_unresolvedGenericArguments == null)
                        return false;

                    foreach (string arg in _unresolvedGenericArguments)
                    {
                        if (arg.Length > 0)
                            return false;
                    }
                    return true;
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// Returns an array of unresolved generic arguments types.
            /// </summary>
            /// <remarks>
            /// <p>
            /// A empty string represents a type parameter that 
            /// did not have been substituted by a specific type.
            /// </p>
            /// </remarks>
            /// <returns>
            /// An array of strings that represents the unresolved generic 
            /// arguments types or an empty array if not generic.
            /// </returns>
            public string[] GetGenericArguments()
            {
                if (_unresolvedGenericArguments == null)
                {
                    return new string[] { };
                }

                return _unresolvedGenericArguments;
            }

            private void ParseGenericArguments(string originalString)
            {
                // Check for match
                bool isMatch = generic.IsMatch(originalString); 
                if (!isMatch)
                {
                    _unresolvedGenericTypeName = originalString;
                }
                else
                {
                    int argsStartIndex = originalString.IndexOf(GENERIC_ARGUMENTS_PREFIX);
                    int argsEndIndex = originalString.LastIndexOf(GENERIC_ARGUMENTS_SUFFIX);
                    if (argsEndIndex != -1)
                    {
                        SplitGenericArguments(originalString.Substring(
                            argsStartIndex + 1, argsEndIndex - argsStartIndex));

                        _unresolvedGenericTypeName = originalString.Remove(argsStartIndex, argsEndIndex - argsStartIndex + 2);
                    }
                }
            }

            private void SplitGenericArguments(string originalArgs)
            {
                IList<string> arguments = new List<string>();

                if (originalArgs.Contains(GENERIC_ARGUMENTS_SEPARATOR))
                {
                    arguments = Parse(originalArgs);
                }
                else
                {
                    string argument = originalArgs.Substring(1, originalArgs.Length - 2).Trim();
                    arguments.Add(argument);
                }
                _unresolvedGenericArguments = new string[arguments.Count];
                 arguments.CopyTo(_unresolvedGenericArguments, 0);
           }

            private IList<string> Parse(string args)
            {
                StringBuilder argument = new StringBuilder();
                IList<string> arguments = new List<string>();

                TextReader input = new StringReader(args);
                int nbrOfRightDelimiter = 0;
                bool findRight = false;
                do
                {
                    char ch = (char)input.Read();
                    if (ch == '[')
                    {
                        nbrOfRightDelimiter++;
                        findRight = true;
                    }
                    else if (ch == ']')
                    {
                        nbrOfRightDelimiter--;
                    }
                    argument.Append(ch);
                    
                    //Find one argument
                    if (findRight && nbrOfRightDelimiter == 0)
                    {
                        string arg = argument.ToString();
                        arg = arg.Substring(1, arg.Length - 2);
                        arguments.Add(arg);
                        input.Read();
                        argument = new StringBuilder();
                    }
                }
                while (input.Peek() != -1);

                return arguments;
            }
            #endregion
        }

        #endregion
#endif

        #region Inner Class : TypeAssemblyInfo

        /// <summary>
        /// Holds data about a <see cref="System.Type"/> and it's
        /// attendant <see cref="System.Reflection.Assembly"/>.
        /// </summary>
        internal class TypeAssemblyInfo
        {
            #region Constants

            /// <summary>
            /// The string that separates a <see cref="System.Type"/> name
            /// from the name of it's attendant <see cref="System.Reflection.Assembly"/>
            /// in an assembly qualified type name.
            /// </summary>
            public const string TYPE_ASSEMBLY_SEPARATOR = ",";
            public const string NULLABLE_TYPE = "System.Nullable";
            public const string NULLABLE_TYPE_ASSEMBLY_SEPARATOR = "]],";
            #endregion

            #region Fields

            private string _unresolvedAssemblyName = string.Empty;
            private string _unresolvedTypeName = string.Empty;

            #endregion

            #region Constructor (s) / Destructor

            /// <summary>
            /// Creates a new instance of the TypeAssemblyInfo class.
            /// </summary>
            /// <param name="unresolvedTypeName">
            /// The unresolved name of a <see cref="System.Type"/>.
            /// </param>
            public TypeAssemblyInfo(string unresolvedTypeName)
            {
                SplitTypeAndAssemblyNames(unresolvedTypeName);
            }

            #endregion

            #region Properties

            /// <summary>
            /// The (unresolved) type name portion of the original type name.
            /// </summary>
            public string TypeName
            {
                get { return _unresolvedTypeName; }
            }

            /// <summary>
            /// The (unresolved, possibly partial) name of the attandant assembly.
            /// </summary>
            public string AssemblyName
            {
                get { return _unresolvedAssemblyName; }
            }

            /// <summary>
            /// Is the type name being resolved assembly qualified?
            /// </summary>
            public bool IsAssemblyQualified
            {
                get { return HasText(AssemblyName); }
            }

            #endregion

            #region Methods

            private bool HasText(string target)
            {
                if (target == null)
                {
                    return false;
                }
                else
                {
                    return HasLength(target.Trim());
                }
            }

            private bool HasLength(string target)
            {
                return (target != null && target.Length > 0);
            }

            private void SplitTypeAndAssemblyNames(string originalTypeName)
            {
                if (originalTypeName.StartsWith(NULLABLE_TYPE))
                {
                    int typeAssemblyIndex = originalTypeName.IndexOf(NULLABLE_TYPE_ASSEMBLY_SEPARATOR);
                    if (typeAssemblyIndex < 0)
                    {
                        _unresolvedTypeName = originalTypeName;
                    }
                    else
                    {
                        _unresolvedTypeName = originalTypeName.Substring(0, typeAssemblyIndex + 2).Trim();
                        _unresolvedAssemblyName = originalTypeName.Substring(typeAssemblyIndex + 3).Trim();
                    }
                }
                else
                {
                    int typeAssemblyIndex = originalTypeName.IndexOf(TYPE_ASSEMBLY_SEPARATOR);
                    if (typeAssemblyIndex < 0)
                    {
                        _unresolvedTypeName = originalTypeName;
                    }
                    else
                    {
                        _unresolvedTypeName = originalTypeName.Substring(0, typeAssemblyIndex).Trim();
                        _unresolvedAssemblyName = originalTypeName.Substring(typeAssemblyIndex + 1).Trim();
                    }
                }
            }

            #endregion
        }

        #endregion
    }
}
