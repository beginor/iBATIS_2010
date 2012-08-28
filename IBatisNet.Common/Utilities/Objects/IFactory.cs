
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

namespace IBatisNet.Common.Utilities.Objects
{
	/// <summary>
	/// Factory to create object for a given type
	/// </summary>
	public interface IFactory
	{
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
		object CreateInstance(object[] parameters);
	}
}
