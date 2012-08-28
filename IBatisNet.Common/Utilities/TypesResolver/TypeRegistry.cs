
#region Apache Notice
/*****************************************************************************
 * $Revision: 408164 $
 * $LastChangedDate: 2006-05-21 06:27:09 -0600 (Sun, 21 May 2006) $
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

#region Imports

using System;
using System.Collections;
using System.Collections.Specialized;

#endregion

namespace IBatisNet.Common.Utilities.TypesResolver
{
	/// <summary> 
	/// Provides access to a central registry of aliased <see cref="System.Type"/>s.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Simplifies configuration by allowing aliases to be used instead of
	/// fully qualified type names.
	/// </p>
	/// <p>
	/// Comes 'pre-loaded' with a number of convenience alias' for the more
	/// common types; an example would be the '<c>int</c>' (or '<c>Integer</c>'
	/// for Visual Basic.NET developers) alias for the <see cref="System.Int32"/>
	/// type.
	/// </p>
	/// </remarks>
	public class TypeRegistry
	{
		#region Constants

		/// <summary>
		/// The alias around the 'list' type.
		/// </summary>
		public const string ArrayListAlias1 = "arraylist";
		/// <summary>
		/// Another alias around the 'list' type.
		/// </summary>
		public const string ArrayListAlias2 = "list";

		/// <summary>
		/// Another alias around the 'bool' type.
		/// </summary>
		public const string BoolAlias = "bool";
		/// <summary>
		/// The alias around the 'bool' type.
		/// </summary>
		public const string BooleanAlias = "boolean";

		/// <summary>
		/// The alias around the 'byte' type.
		/// </summary>
		public const string ByteAlias = "byte";

		/// <summary>
		/// The alias around the 'char' type.
		/// </summary>
		public const string CharAlias = "char";

		/// <summary>
		/// The alias around the 'DateTime' type.
		/// </summary>
		public const string DateAlias1 = "datetime";
		/// <summary>
		/// Another alias around the 'DateTime' type.
		/// </summary>
		public const string DateAlias2 = "date";

		/// <summary>
		/// The alias around the 'decimal' type.
		/// </summary>
		public const string DecimalAlias = "decimal";

		/// <summary>
		/// The alias around the 'double' type.
		/// </summary>
		public const string DoubleAlias = "double";


		/// <summary>
		/// The alias around the 'float' type.
		/// </summary>
		public const string FloatAlias = "float";
		/// <summary>
		/// Another alias around the 'float' type.
		/// </summary>
		public const string SingleAlias = "single";

		/// <summary>
		/// The alias around the 'guid' type.
		/// </summary>
		public const string GuidAlias = "guid";

		/// <summary>
		/// The alias around the 'Hashtable' type.
		/// </summary>
		public const string HashtableAlias1 = "hashtable";
		/// <summary>
		/// Another alias around the 'Hashtable' type.
		/// </summary>
		public const string HashtableAlias2 = "map";
		/// <summary>
		/// Another alias around the 'Hashtable' type.
		/// </summary>
		public const string HashtableAlias3 = "hashmap";

		/// <summary>
		/// The alias around the 'short' type.
		/// </summary>
		public const string Int16Alias1 = "int16";
		/// <summary>
		/// Another alias around the 'short' type.
		/// </summary>
		public const string Int16Alias2 = "short";


		/// <summary>
		/// The alias around the 'int' type.
		/// </summary>
		public const string Int32Alias1 = "int32";
		/// <summary>
		/// Another alias around the 'int' type.
		/// </summary>
		public const string Int32Alias2 = "int";
		/// <summary>
		/// Another alias around the 'int' type.
		/// </summary>
		public const string Int32Alias3 = "integer";

		/// <summary>
		/// The alias around the 'long' type.
		/// </summary>
		public const string Int64Alias1 = "int64";
		/// <summary>
		/// Another alias around the 'long' type.
		/// </summary>
		public const string Int64Alias2 = "long";

		/// <summary>
		/// The alias around the 'unsigned short' type.
		/// </summary>
		public const string UInt16Alias1 = "uint16";
		/// <summary>
		/// Another alias around the 'unsigned short' type.
		/// </summary>
		public const string UInt16Alias2 = "ushort";

		/// <summary>
		/// The alias around the 'unsigned int' type.
		/// </summary>
		public const string UInt32Alias1 = "uint32";
		/// <summary>
		/// Another alias around the 'unsigned int' type.
		/// </summary>
		public const string UInt32Alias2 = "uint";

		/// <summary>
		/// The alias around the 'unsigned long' type.
		/// </summary>
		public const string UInt64Alias1 = "uint64";
		/// <summary>
		/// Another alias around the 'unsigned long' type.
		/// </summary>
		public const string UInt64Alias2 = "ulong";

		/// <summary>
		/// The alias around the 'SByte' type.
		/// </summary>
		public const string SByteAlias = "sbyte";

		/// <summary>
		/// The alias around the 'string' type.
		/// </summary>
		public const string StringAlias = "string";

		/// <summary>
		/// The alias around the 'TimeSpan' type.
		/// </summary>
		public const string TimeSpanAlias = "timespan";

#if dotnet2
        /// <summary>
        /// The alias around the 'int?' type.
        /// </summary>
        public const string NullableInt32Alias = "int?";

        /// <summary>
        /// The alias around the 'int?[]' array type.
        /// </summary>
        public const string NullableInt32ArrayAlias = "int?[]";

        /// <summary>
        /// The alias around the 'decimal?' type.
        /// </summary>
        public const string NullableDecimalAlias = "decimal?";

        /// <summary>
        /// The alias around the 'decimal?[]' array type.
        /// </summary>
        public const string NullableDecimalArrayAlias = "decimal?[]";

        /// <summary>
        /// The alias around the 'char?' type.
        /// </summary>
        public const string NullableCharAlias = "char?";

        /// <summary>
        /// The alias around the 'char?[]' array type.
        /// </summary>
        public const string NullableCharArrayAlias = "char?[]";

        /// <summary>
        /// The alias around the 'long?' type.
        /// </summary>
        public const string NullableInt64Alias = "long?";

        /// <summary>
        /// The alias around the 'long?[]' array type.
        /// </summary>
        public const string NullableInt64ArrayAlias = "long?[]";

        /// <summary>
        /// The alias around the 'short?' type.
        /// </summary>
        public const string NullableInt16Alias = "short?";

        /// <summary>
        /// The alias around the 'short?[]' array type.
        /// </summary>
        public const string NullableInt16ArrayAlias = "short?[]";

        /// <summary>
        /// The alias around the 'unsigned int?' type.
        /// </summary>
        public const string NullableUInt32Alias = "uint?";

        /// <summary>
        /// The alias around the 'unsigned long?' type.
        /// </summary>
        public const string NullableUInt64Alias = "ulong?";

        /// <summary>
        /// The alias around the 'ulong?[]' array type.
        /// </summary>
        public const string NullableUInt64ArrayAlias = "ulong?[]";

        /// <summary>
        /// The alias around the 'uint?[]' array type.
        /// </summary>
        public const string NullableUInt32ArrayAlias = "uint?[]";

        /// <summary>
        /// The alias around the 'unsigned short?' type.
        /// </summary>
        public const string NullableUInt16Alias = "ushort?";

        /// <summary>
        /// The alias around the 'ushort?[]' array type.
        /// </summary>
        public const string NullableUInt16ArrayAlias = "ushort?[]";

        /// <summary>
        /// The alias around the 'double?' type.
        /// </summary>
        public const string NullableDoubleAlias = "double?";

        /// <summary>
        /// The alias around the 'double?[]' array type.
        /// </summary>
        public const string NullableDoubleArrayAlias = "double?[]";

        /// <summary>
        /// The alias around the 'float?' type.
        /// </summary>
        public const string NullableFloatAlias = "float?";

        /// <summary>
        /// The alias around the 'float?[]' array type.
        /// </summary>
        public const string NullableFloatArrayAlias = "float?[]";

        /// <summary>
        /// The alias around the 'bool?' type.
        /// </summary>
        public const string NullableBoolAlias = "bool?";

        /// <summary>
        /// The alias around the 'bool?[]' array type.
        /// </summary>
        public const string NullableBoolArrayAlias = "bool?[]";
#endif
		#endregion

		#region Fields
        private static IDictionary _types = new Hashtable();
		#endregion

		#region Constructor (s) / Destructor
		/// <summary>
        /// Creates a new instance of the <see cref="TypeRegistry"/> class.
		/// </summary>
		/// <remarks>
		/// <p>
		/// This is a utility class, and as such has no publicly visible
		/// constructors.
		/// </p>
		/// </remarks>
		private TypeRegistry() {}

		/// <summary>
		/// Initialises the static properties of the TypeAliasResolver class.
		/// </summary>
        static TypeRegistry()
		{
			// Initialize a dictionary with some fully qualifiaed name 
			_types[ArrayListAlias1] = typeof (ArrayList);
			_types[ArrayListAlias2] = typeof (ArrayList);

			_types[BoolAlias] = typeof (bool);
			_types[BooleanAlias] = typeof (bool);

			_types[ByteAlias] = typeof (byte);

			_types[CharAlias] = typeof (char);

			_types[DateAlias1] = typeof (DateTime);
			_types[DateAlias2] = typeof (DateTime);

			_types[DecimalAlias] = typeof (decimal);

			_types[DoubleAlias] = typeof (double);

			_types[FloatAlias] = typeof (float);
			_types[SingleAlias] = typeof (float);

			_types[GuidAlias] = typeof (Guid);

			_types[HashtableAlias1] = typeof (Hashtable);
			_types[HashtableAlias2] = typeof (Hashtable);
			_types[HashtableAlias3] = typeof (Hashtable);

			_types[Int16Alias1] = typeof (short);
			_types[Int16Alias2] = typeof (short);

			_types[Int32Alias1] = typeof (int);
			_types[Int32Alias2] = typeof (int);
			_types[Int32Alias3] = typeof (int);

			_types[Int64Alias1] = typeof (long);
			_types[Int64Alias2] = typeof (long);

			_types[UInt16Alias1] = typeof (ushort);
			_types[UInt16Alias2] = typeof (ushort);

			_types[UInt32Alias1] = typeof (uint);
			_types[UInt32Alias2] = typeof (uint);

			_types[UInt64Alias1] = typeof (ulong);
			_types[UInt64Alias2] = typeof (ulong);

			_types[SByteAlias] = typeof (sbyte);

			_types[StringAlias] = typeof (string);

			_types[TimeSpanAlias] = typeof (string);

#if dotnet2
            _types[NullableInt32Alias] = typeof(int?);
            _types[NullableInt32ArrayAlias] = typeof(int?[]);

            _types[NullableDecimalAlias] = typeof(decimal?);
            _types[NullableDecimalArrayAlias] = typeof(decimal?[]);

            _types[NullableCharAlias] = typeof(char?);
            _types[NullableCharArrayAlias] = typeof(char?[]);

            _types[NullableInt64Alias] = typeof(long?);
            _types[NullableInt64ArrayAlias] = typeof(long?[]);

            _types[NullableInt16Alias] = typeof(short?);
            _types[NullableInt16ArrayAlias] = typeof(short?[]);

            _types[NullableUInt32Alias] = typeof(uint?);
            _types[NullableUInt32ArrayAlias] = typeof(uint?[]);

            _types[NullableUInt64Alias] = typeof(ulong?);
            _types[NullableUInt64ArrayAlias] = typeof(ulong?[]);

            _types[NullableUInt16Alias] = typeof(ushort?);
            _types[NullableUInt16ArrayAlias] = typeof(ushort?[]);

            _types[NullableDoubleAlias] = typeof(double?);
            _types[NullableDoubleArrayAlias] = typeof(double?[]);

            _types[NullableFloatAlias] = typeof(float?);
            _types[NullableFloatArrayAlias] = typeof(float?[]);

            _types[NullableBoolAlias] = typeof(bool?);
            _types[NullableBoolArrayAlias] = typeof(bool?[]);
#endif
        }
		#endregion

		#region Methods

        /// <summary> 
        /// Resolves the supplied <paramref name="alias"/> to a <see cref="System.Type"/>. 
        /// </summary> 
        /// <param name="alias">
        /// The alias to resolve.
        /// </param>
        /// <returns>
        /// The <see cref="System.Type"/> the supplied <paramref name="alias"/> was
        /// associated with, or <see lang="null"/> if no <see cref="System.Type"/> 
        /// was previously registered for the supplied <paramref name="alias"/>.
        /// </returns>
        /// <remarks>The alis name will be convert in lower character before the resolution.</remarks>
        /// <exception cref="System.ArgumentNullException">
        /// If the supplied <paramref name="alias"/> is <see langword="null"/> or
        /// contains only whitespace character(s).
        /// </exception>
        public static Type ResolveType(string alias)
        {
            return (Type)_types[alias.ToLower()];
        }

		#endregion

	}
}
