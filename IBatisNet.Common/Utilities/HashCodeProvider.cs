
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 512878 $
 * $Date: 2007-02-28 10:57:11 -0700 (Wed, 28 Feb 2007) $
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

#region Imports
using System;
using System.Runtime.CompilerServices;

#endregion

namespace IBatisNet.Common.Utilities
{
	using System.Reflection;

	/// <summary>
	/// Summary description for HashCodeProvider.
	/// </summary>
	public sealed class HashCodeProvider
	{
		private static MethodInfo getHashCodeMethodInfo = null;

		static HashCodeProvider()
		{
			Type type = typeof(object);
			getHashCodeMethodInfo = type.GetMethod("GetHashCode");
		}

		/// <summary>
		/// Supplies a hash code for an object.
		/// </summary>
		/// <param name="obj">The object.</param>
		/// <returns>A hash code</returns>
		/// <remarks>
		/// Buggy in .NET V1.0
		/// .NET Fx v1.1 Update: 
		/// As of v1.1 of the framework, there is a method System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(object) that does this as well.
		/// I will not use to Keep compatiblity with .NET V1.0
		/// </remarks>
		public static int GetIdentityHashCode(object obj)
		{
            //#if dotnet2
            //return RuntimeHelpers.GetHashCode(obj);
            //#else
			// call the underlying System.Object.GetHashCode()
			return (int)getHashCodeMethodInfo.Invoke(obj, null);
            //#endif
		}
	}
}
