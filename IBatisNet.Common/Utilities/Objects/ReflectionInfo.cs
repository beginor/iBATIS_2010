
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 511513 $
 * $Date: 2007-02-25 06:46:57 -0700 (Sun, 25 Feb 2007) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Gilles Bayon
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

using IBatisNet.Common.Exceptions;

namespace IBatisNet.Common.Utilities.Objects
{
	/// <summary>
	/// This class represents a cached set of class definition information that
	/// allows for easy mapping between property names and get/set methods.
	/// </summary>
    public sealed class ReflectionInfo
	{
		/// <summary>
		/// 
		/// </summary>
		public static BindingFlags BINDING_FLAGS_PROPERTY
			= BindingFlags.Public
            | BindingFlags.NonPublic
			| BindingFlags.Instance
			;


		/// <summary>
		/// 
		/// </summary>
		public static BindingFlags BINDING_FLAGS_FIELD
            = BindingFlags.Public
            | BindingFlags.NonPublic 
			| BindingFlags.Instance 

			;

		private static readonly string[] _emptyStringArray = new string[0];
		private static ArrayList _simleTypeMap = new ArrayList();
		private static Hashtable _reflectionInfoMap = Hashtable.Synchronized(new Hashtable());

		private string _className = string.Empty;
		private string[] _readableMemberNames = _emptyStringArray;
		private string[] _writeableMemberNames = _emptyStringArray;
		// (memberName, MemberInfo)
		private Hashtable _setMembers = new Hashtable();
        // (memberName, MemberInfo)
		private Hashtable _getMembers = new Hashtable();
		// (memberName, member type)
		private Hashtable _setTypes = new Hashtable();
		// (memberName, member type)
		private Hashtable _getTypes = new Hashtable();

		/// <summary>
		/// 
		/// </summary>
		public string ClassName 
		{
			get { return _className; }
		}

		/// <summary>
		/// 
		/// </summary>
		static ReflectionInfo()
		{
			_simleTypeMap.Add(typeof(string));
			_simleTypeMap.Add(typeof(byte));
			_simleTypeMap.Add(typeof(char));
			_simleTypeMap.Add(typeof(bool));
			_simleTypeMap.Add(typeof(Guid));
			_simleTypeMap.Add(typeof(Int16));
			_simleTypeMap.Add(typeof(Int32));
			_simleTypeMap.Add(typeof(Int64));
			_simleTypeMap.Add(typeof(Single));
			_simleTypeMap.Add(typeof(Double));
			_simleTypeMap.Add(typeof(Decimal));
			_simleTypeMap.Add(typeof(DateTime));
			_simleTypeMap.Add(typeof(TimeSpan));
			_simleTypeMap.Add(typeof(Hashtable));
			_simleTypeMap.Add(typeof(SortedList));
			_simleTypeMap.Add(typeof(ListDictionary));
			_simleTypeMap.Add(typeof(HybridDictionary));


			//			_simleTypeMap.Add(Class.class);
			//			_simleTypeMap.Add(Collection.class);
			//			_simleTypeMap.Add(HashMap.class);
			//			_simleTypeMap.Add(TreeMap.class);
			_simleTypeMap.Add(typeof(ArrayList));
			//			_simleTypeMap.Add(HashSet.class);
			//			_simleTypeMap.Add(TreeSet.class);
			//			_simleTypeMap.Add(Vector.class);
			_simleTypeMap.Add(typeof(IEnumerator));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		private ReflectionInfo(Type type) 
		{
			_className = type.Name;
			AddMembers(type);

			string[] getArray = new string[_getMembers.Count];
			_getMembers.Keys.CopyTo(getArray,0);
			_readableMemberNames = getArray;

			string[] setArray = new string[_setMembers.Count];
			_setMembers.Keys.CopyTo(setArray,0);
			_writeableMemberNames = setArray;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		private void AddMembers(Type type) 
		{
			#region Properties
            PropertyInfo[] properties = type.GetProperties(BINDING_FLAGS_PROPERTY);
			for (int i = 0; i < properties.Length; i++) 
			{
                if (properties[i].CanWrite)
                {
                    string name = properties[i].Name;
                    _setMembers[name] = properties[i];
                    _setTypes[name] = properties[i].PropertyType;
                }
                if (properties[i].CanRead)
                {
                    string name = properties[i].Name;
                    _getMembers[name] = properties[i];
                    _getTypes[name] = properties[i].PropertyType;
                }
            }
            
			#endregion

			#region Fields
			FieldInfo[] fields = type.GetFields(BINDING_FLAGS_FIELD) ;
			for (int i = 0; i < fields.Length; i++) 
			{
				string name = fields[i].Name;
				_setMembers[name] = fields[i];
				_setTypes[name] = fields[i].FieldType;
				_getMembers[name] = fields[i];
				_getTypes[name] = fields[i].FieldType;
			}
			#endregion

            // Fix for problem with interfaces inheriting other interfaces
            if (type.IsInterface)
            {
                // Loop through interfaces for the type and add members from
                // these types too
                foreach (Type interf in type.GetInterfaces())
                {
                    AddMembers(interf);
                }
            }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="memberName"></param>
		/// <returns></returns>
		public MemberInfo GetSetter(string memberName) 
		{
			MemberInfo memberInfo = (MemberInfo) _setMembers[memberName];

			if (memberInfo == null) 
			{
				throw new ProbeException("There is no Set member named '" + memberName + "' in class '" + _className + "'");
			}				

			return memberInfo;
		}


        /// <summary>
        /// Gets the <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="memberName">Member's name.</param>
        /// <returns>The <see cref="MemberInfo"/></returns>
		public MemberInfo GetGetter(string memberName) 
		{
			MemberInfo memberInfo = _getMembers[memberName] as MemberInfo;
			if (memberInfo == null) 
			{
				throw new ProbeException("There is no Get member named '" + memberName + "' in class '" + _className + "'");
			}
			return memberInfo;
		}


        /// <summary>
        /// Gets the type of the member.
        /// </summary>
        /// <param name="memberName">Member's name.</param>
        /// <returns></returns>
		public Type GetSetterType(string memberName) 
		{
			Type type = (Type) _setTypes[memberName];
			if (type == null) 
			{
				throw new ProbeException("There is no Set member named '" + memberName + "' in class '" + _className + "'");
			}
			return type;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="memberName"></param>
		/// <returns></returns>
		public Type GetGetterType(string memberName) 
		{
			Type type = (Type) _getTypes[memberName];
			if (type == null) 
			{
				throw new ProbeException("There is no Get member named '" + memberName + "' in class '" + _className + "'");
			}
			return type;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string[] GetReadableMemberNames() 
		{
			return _readableMemberNames;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public string[] GetWriteableMemberNames() 
		{
			return _writeableMemberNames;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="memberName"></param>
		/// <returns></returns>
		public bool HasWritableMember(string memberName) 
		{
			return _setMembers.ContainsKey(memberName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="memberName"></param>
		/// <returns></returns>
		public bool HasReadableMember(string memberName) 
		{
			return _getMembers.ContainsKey(memberName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsKnownType(Type type) 
		{
			if (_simleTypeMap.Contains(type)) 
			{
				return true;
			} 
			else if (typeof(IList).IsAssignableFrom(type)) 
			{
				return true;
			} 
			else if (typeof(IDictionary).IsAssignableFrom(type)) 
			{
				return true;
			} 
			else if (typeof(IEnumerator).IsAssignableFrom(type)) 
			{
				return true;
			} 
			else 
			{
				return false;
			}
		}

		/// <summary>
		/// Gets an instance of ReflectionInfo for the specified type.
		/// </summary>summary>
		/// <param name="type">The type for which to lookup the method cache.</param>
		/// <returns>The properties cache for the type</returns>
		public static ReflectionInfo GetInstance(Type type) 
		{
			lock (type) 
			{
				ReflectionInfo cache = (ReflectionInfo) _reflectionInfoMap[type];
				if (cache == null) 
				{
					cache = new ReflectionInfo(type);
					_reflectionInfoMap.Add(type, cache);
				}
				return cache;
			}
		}

																									
	}
	
}
