
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 477815 $
 * $Date: 2006-11-21 11:46:52 -0700 (Tue, 21 Nov 2006) $
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

#region Using

using System.Collections.Specialized;
using System.Data;

#endregion 

namespace IBatisNet.DataMapper.Configuration.Statements
{
	/// <summary>
	/// Construct the list of IDataParameters for the statement
	/// and prepare the sql
	/// </summary>
	public class PreparedStatement
	{
		#region Fields

		private string _preparedSsql = string.Empty;
		private StringCollection  _dbParametersName = new StringCollection ();
		private IDbDataParameter[] _dbParameters = null;

		#endregion

		#region Properties


		/// <summary>
		/// The list of IDataParameter name used by the PreparedSql.
		/// </summary>
		public StringCollection DbParametersName
		{
			get { return _dbParametersName; }
		}

		/// <summary>
		/// The list of IDataParameter to use for the PreparedSql.
		/// </summary>
        public IDbDataParameter[] DbParameters
		{
			get { return _dbParameters;}
			set { _dbParameters =value;}
		}

		/// <summary>
		/// The prepared statement.
		/// </summary>
		public string PreparedSql
		{
			get { return _preparedSsql; }
			set {_preparedSsql = value;}
		}

		#endregion
	}
}
