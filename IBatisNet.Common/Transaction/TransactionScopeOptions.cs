
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 638571 $
 * $Date: 2008-03-18 15:11:57 -0600 (Tue, 18 Mar 2008) $
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
#endregion

namespace IBatisNet.Common.Transaction
{
	/// <summary>
	/// Describes how a transaction scope is associated with a transaction.
	/// </summary>
	public enum TransactionScopeOptions : int
	{
		/// <summary>
		/// The transaction scope must be associated with a transaction.
		/// If we are in a transaction scope join it. If we aren't, create a new one.
		/// </summary>
		Required  = 0,
		/// <summary>
		/// Always creates a new transaction scope.
		/// </summary>
		RequiresNew  = 1, 
		/// <summary>
		/// Don't need a transaction scope, but if we are in a transaction scope then join it.
		/// </summary>
		Supported  = 2,
		/// <summary>
		/// Means that cannot cannot be associated with a transaction scope.
		/// </summary>
		NotSupported  = 3,
		/// <summary>
		/// The transaction scope must be associated with an existing transaction scope.
		/// </summary>
		Mandatory  = 4
	}
}
