
#region Apache Notice
/*****************************************************************************
 * $Revision: 408164 $
 * $LastChangedDate: 2006-05-21 06:27:09 -0600 (Sun, 21 May 2006) $
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

#region Imports

using System.Collections;
using System.Text;
using IBatisNet.DataMapper.Configuration.ParameterMapping;
using IBatisNet.DataMapper.Configuration.Sql.Dynamic.Elements;

#endregion


namespace IBatisNet.DataMapper.Configuration.Sql.Dynamic.Handlers
{
	/// <summary>
	/// Summary description for SqlTagContext.
	/// </summary>
	public sealed class SqlTagContext
	{
		#region Fields
		private Hashtable _attributes = new Hashtable();
		private bool _overridePrepend = false;
		private SqlTag _firstNonDynamicTagWithPrepend = null;
		private ArrayList _parameterMappings = new ArrayList();
		private StringBuilder buffer = new StringBuilder();
		#endregion


		/// <summary>
		/// 
		/// </summary>
		public SqlTagContext() 
		{
			_overridePrepend = false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public StringBuilder GetWriter() 
		{
			return buffer;
		}

		/// <summary>
		/// 
		/// </summary>
		public string BodyText 
		{
			get
			{
				return buffer.ToString().Trim();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public bool IsOverridePrepend
		{
			set
			{
				_overridePrepend = value;
			}
			get
			{
				return _overridePrepend;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SqlTag FirstNonDynamicTagWithPrepend
		{
			get
			{
				return _firstNonDynamicTagWithPrepend;
			}
			set
			{
				_firstNonDynamicTagWithPrepend = value;
			}
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		public void AddAttribute(object key, object value) 
		{
			_attributes.Add(key, value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public object GetAttribute(object key) 
		{
			return _attributes[key];
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapping"></param>
		public void AddParameterMapping(ParameterProperty mapping) 
		{
			_parameterMappings.Add(mapping);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public IList GetParameterMappings() 
		{
			return _parameterMappings;
		}
	}
}
