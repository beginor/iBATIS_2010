#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 378715 $
 * $Date: 2006-11-19 09:07:45 -0700 (Sun, 19 Nov 2006) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2006 - Apache Fondation
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

namespace IBatisNet.DataMapper.SessionStore
{

	/// <summary>
	/// Provides the contract for implementors who want to store session.
	/// </summary>
	public interface ISessionStore
	{
		/// <summary>
		/// Get the local session
		/// </summary>
        ISqlMapSession LocalSession
		{
			get; 
		}

		/// <summary>
		/// Store the specified session.
		/// </summary>
		/// <param name="session">The session to store</param>
        void Store(ISqlMapSession session);

		/// <summary>
		/// Remove the local session from the storage.
		/// </summary>
		void Dispose();
	}
}
