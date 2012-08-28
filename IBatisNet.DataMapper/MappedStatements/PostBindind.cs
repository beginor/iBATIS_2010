#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-04-25 19:40:27 +0200 (mar., 25 avr. 2006) $
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

using System.Data;
using IBatisNet.DataMapper.Configuration.ResultMapping;

namespace IBatisNet.DataMapper.MappedStatements
{
	/// <summary>
	/// All dataq tor retrieve 'select' <see cref="ResultProperty"/>
	/// </summary>
	/// <remarks>
	/// As ADO.NET allows one open <see cref="IDataReader"/> per connection at once, we keep
	/// all the datas to open the next <see cref="IDataReader"/> after having closed the current. 
	/// </remarks>
    public sealed class PostBindind
	{
		/// <summary>
		/// Enumeration of the ExecuteQuery method.
		/// </summary>
		public enum ExecuteMethod : int
		{
			/// <summary>
			/// Execute Query For Object
			/// </summary>
			ExecuteQueryForObject = 1,
			/// <summary>
			/// Execute Query For IList
			/// </summary>
			ExecuteQueryForIList,
			/// <summary>
			/// Execute Query For Generic IList
			/// </summary>
			ExecuteQueryForGenericIList,
			/// <summary>
			/// Execute Query For Array List
			/// </summary>
			ExecuteQueryForArrayList,
			/// <summary>
			/// Execute Query For Strong Typed IList
			/// </summary>
			ExecuteQueryForStrongTypedIList
		}

		#region Fields
		private IMappedStatement _statement = null;
		private ResultProperty _property = null;
		private object _target = null;
		private object _keys = null;
		private ExecuteMethod _method = ExecuteMethod.ExecuteQueryForIList;
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the statement.
		/// </summary>
		/// <value>The statement.</value>
		public IMappedStatement Statement
		{
			set { _statement = value; }
			get { return _statement; }
		}


		/// <summary>
		/// Gets or sets the result property.
		/// </summary>
		/// <value>The result property.</value>
		public ResultProperty ResultProperty
		{
			set { _property = value; }
			get { return _property; }
		}


		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>The target.</value>
		public object Target
		{
			set { _target = value; }
			get { return _target; }
		}



		/// <summary>
		/// Gets or sets the keys.
		/// </summary>
		/// <value>The keys.</value>
		public object Keys
		{
			set { _keys = value; }
			get { return _keys; }
		}


		/// <summary>
		/// Gets or sets the method.
		/// </summary>
		/// <value>The method.</value>
		public ExecuteMethod Method
		{
			set { _method = value; }
			get { return _method; }
		}
		#endregion
	}
}
