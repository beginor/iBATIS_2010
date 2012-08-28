
#region Apache Notice
/*****************************************************************************
 * $Revision: 383115 $
 * $LastChangedDate: 2006-03-04 07:21:51 -0700 (Sat, 04 Mar 2006) $
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

#region Using

using System.Data;
using IBatisNet.DataMapper.Configuration.ParameterMapping;

#endregion 

namespace IBatisNet.DataMapper.TypeHandlers
{
	/// <summary>
	/// A simple interface for implementing custom type handlers.
	/// <p/>
	/// Using this interface, you can implement a type handler that
	/// will perform customized processing before parameters are set
	/// on a IDbCommand and after values are retrieved from
	/// a IDataReader.  Using a custom type handler you can extend
	/// the framework to handle types that are not supported, or
	/// handle supported types in a different way.  For example,
	/// you might use a custom type handler to implement proprietary
	/// BLOB support (e.g. Oracle), or you might use it to handle
	/// booleans using "Y" and "N" instead of the more typical 0/1.
	/// </summary>
	public interface ITypeHandlerCallback
	{
		/// <summary>
		/// Performs processing on a value before it is used to set
		/// the parameter of a IDbCommand.
		/// </summary>
		/// <param name="setter">The interface for setting the value on the IDbCommand.</param>
		/// <param name="parameter">The value to be set</param>
		void SetParameter(IParameterSetter setter, object parameter);


		/// <summary>
		/// Performs processing on a value before after it has been retrieved
		/// from a IDataReader.
		/// </summary>
		/// <param name="getter">The interface for getting the value from the IDataReader.</param>
		/// <returns>The processed value.</returns>
		object GetResult(IResultGetter getter);


		/// <summary>
		/// Casts the string representation of a value into a type recognized by
		/// this type handler.  This method is used to translate nullValue values
		/// into types that can be appropriately compared.  If your custom type handler
		/// cannot support nullValues, or if there is no reasonable string representation
		/// for this type (e.g. File type), you can simply return the String representation
		/// as it was passed in.  It is not recommended to return null, unless null was passed
		/// in.
		/// </summary>
		/// <param name="s"></param>
		/// <returns></returns>
		object ValueOf(string s);

        /// <summary>
        /// The null value for this type
        /// </summary>
        object NullValue { get;}
	}
}
