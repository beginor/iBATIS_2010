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

namespace IBatisNet.Common.Utilities.Objects
{
	/// <summary>
    /// A <see cref="IObjectFactory"/> implementation that can create objects 
	/// via Activator.CreateInstance
	/// </summary>
	public class ActivatorObjectFactory : IObjectFactory
	{

		#region IObjectFactory members

		/// <summary>
        /// Create a new see <see cref="IObjectFactory"/> instance for a given type
		/// </summary>
		/// <param name="typeToCreate">The type instance to build</param>
		/// <param name="types">The types of the constructor arguments</param>
        /// <returns>Returns a new <see cref="IObjectFactory"/> instance.</returns>
		public IFactory CreateFactory(Type typeToCreate, Type[] types)
		{
			return new ActivatorFactory( typeToCreate );
		}

		#endregion
	}
}
