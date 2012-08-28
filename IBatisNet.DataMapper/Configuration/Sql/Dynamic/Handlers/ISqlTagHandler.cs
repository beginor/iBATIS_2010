
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
using System.Text;

using IBatisNet.DataMapper.Configuration.Sql.Dynamic.Elements;
#endregion


namespace IBatisNet.DataMapper.Configuration.Sql.Dynamic.Handlers
{
	/// <summary>
	/// Summary description for ISqlTagHandler.
	/// </summary>
	public interface ISqlTagHandler
	{

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="tag"></param>
		/// <param name="parameterObject"></param>
		/// <returns></returns>
		int DoStartFragment(SqlTagContext ctx, SqlTag tag, object parameterObject);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="tag"></param>
		/// <param name="parameterObject"></param>
		/// <param name="bodyContent"></param>
		/// <returns></returns>
		int DoEndFragment(SqlTagContext ctx, SqlTag tag, object parameterObject, StringBuilder bodyContent);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="ctx"></param>
		/// <param name="tag"></param>
		/// <param name="parameterObject"></param>
		/// <param name="bodyContent"></param>
		void DoPrepend(SqlTagContext ctx, SqlTag tag, object parameterObject, StringBuilder bodyContent);

		/// <summary>
		/// 
		/// </summary>
		bool IsPostParseRequired
		{
			get;
		}
	}
}
