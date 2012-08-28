
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

using System;
using System.Collections;

namespace IBatisNet.Common.Pagination
{
	/// <summary>
	/// Summary description for IPaginatedList.
	/// </summary>
	public interface IPaginatedList : IList, IEnumerator 
	{

		/// <summary>
		/// The maximum number of items per page.
		/// </summary>
		int PageSize
		{
			get;
		}


		/// <summary>
		/// Is the current page the first page ?
		/// True if the current page is the first page or if only
		/// a single page exists.
		/// </summary>
		bool IsFirstPage
		{
			get;
		}


		/// <summary>
		/// Is the current page a middle page (i.e. not first or last) ?
		/// Return True if the current page is not the first or last page,
		/// and more than one page exists (always returns false if only a
		/// single page exists).
		/// </summary>
		bool IsMiddlePage
		{
			get;
		}


		/// <summary>
		/// Is the current page the last page ?
		/// Return True if the current page is the last page or if only
		/// a single page exists.
		/// </summary>
		bool IsLastPage
		{
			get;
		}


		/// <summary>
		/// Is a page available after the current page ?
		/// Return True if the next page is available
		/// </summary>
		bool IsNextPageAvailable
		{
			get;
		}


		/// <summary>
		/// Is a page available before the current page ?
		/// Return True if the previous page is available
		/// </summary>
		bool IsPreviousPageAvailable
		{
			get;
		}


		/// <summary>
		/// Moves to the next page after the current page.  If the current
		/// page is the last page, wrap to the first page.
		/// </summary>
		/// <returns></returns>
		bool NextPage();


		/// <summary>
		/// Moves to the page before the current page.  If the current
		/// page is the first page, wrap to the last page.
		/// </summary>
		/// <returns></returns>
		bool PreviousPage();


		/// <summary>
		/// Moves to a specified page.  If the specified
		/// page is beyond the last page, wrap to the first page.
		/// If the specified page is before the first page, wrap
		/// to the last page.
		/// </summary>
		/// <param name="pageIndex">The index of the specified page.</param>
		void GotoPage(int pageIndex);


		/// <summary>
		/// Returns the current page index, which is a zero based integer.
		/// All paginated list implementations should know what index they are
		/// on, even if they don't know the ultimate boundaries (min/max)
		/// </summary>
		int PageIndex
		{
			get;
		}

	}
}
