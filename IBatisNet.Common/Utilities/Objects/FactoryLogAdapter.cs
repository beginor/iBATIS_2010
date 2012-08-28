#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-11-09 11:44:07 -0700 (Thu, 09 Nov 2006) $
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
using System.Reflection;
using System.Text;
using IBatisNet.Common.Logging;

namespace IBatisNet.Common.Utilities.Objects
{
	/// <summary>
    /// A wrapper arround an <see cref="IFactory"/> implementation which logs argument type and value
    /// when CreateInstance is called.
	/// </summary>
	public class FactoryLogAdapter : IFactory
	{
		private IFactory _factory = null;
		private string _typeName = string.Empty;
		private string _parametersTypeName = string.Empty;
		
		private static readonly ILog _logger = LogManager.GetLogger( MethodBase.GetCurrentMethod().DeclaringType );

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryLogAdapter"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="paramtersTypes">The paramters types.</param>
        /// <param name="factory">The factory.</param>
		public FactoryLogAdapter(Type type, Type[] paramtersTypes, IFactory factory)
		{
			_factory = factory;
			_typeName = type.FullName;
			_parametersTypeName = GenerateParametersName(paramtersTypes);
		}
		
		#region IFactory Members

        /// <summary>
        /// Create a new instance with the specified parameters
        /// </summary>
        /// <param name="parameters">An array of values that matches the number, order and type
        /// of the parameters for this constructor.</param>
        /// <returns>A new instance</returns>
        /// <remarks>
        /// If you call a constructor with no parameters, pass null.
        /// Anyway, what you pass will be ignore.
        /// </remarks>
		public object CreateInstance(object[] parameters)
		{
			object newObject = null;

			try
			{
				newObject = _factory.CreateInstance(parameters);
			}
			catch
			{
				_logger.Debug("Enabled to create instance for type '" + _typeName);
				_logger.Debug("  using parameters type : " + _parametersTypeName );
				_logger.Debug("  using parameters value : " + GenerateLogInfoForParameterValue(parameters) );
				throw;
			}
            			
			return newObject;
		}

		#endregion
		
		/// <summary>
		/// Generates the a string containing all parameter type names.
		/// </summary>
		/// <param name="arguments">The types of the constructor arguments</param>
		/// <returns>The string.</returns>
		private string GenerateParametersName(object[] arguments)
		{
			StringBuilder names = new StringBuilder();
			if ((arguments != null) && (arguments.Length != 0)) 
			{
				for (int i=0; i<arguments.Length; i++) 
				{
					names.Append("[").Append(arguments[i]).Append("] ");
				}
			}
			return names.ToString();
		}
		
		/// <summary>
		/// Generates the a string containing all parameters value.
		/// </summary>
		/// <param name="arguments">The arguments</param>
		/// <returns>The string.</returns>
		private string GenerateLogInfoForParameterValue(object[] arguments)
		{
			StringBuilder values = new StringBuilder();
			if ((arguments != null) && (arguments.Length != 0)) 
			{
				for (int i=0; i<arguments.Length; i++) 
				{
					if (arguments[i]!=null)
					{
						values.Append("[").Append(arguments[i].ToString()).Append("] ");
					}
					else
					{
						values.Append("[null] ");
					}
				}
			}
			return values.ToString();
		}
	}
}
