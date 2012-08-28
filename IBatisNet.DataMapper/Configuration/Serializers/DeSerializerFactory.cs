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

using System.Collections;
using System.Collections.Specialized;
using IBatisNet.DataMapper.Scope;

namespace IBatisNet.DataMapper.Configuration.Serializers
{
	/// <summary>
	/// Summary description for DeSerializerFactory.
	/// </summary>
	public class DeSerializerFactory
	{
		private IDictionary _serializerMap = new HybridDictionary();

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="configScope"></param>
		public DeSerializerFactory(ConfigurationScope configScope)
		{
			_serializerMap.Add("dynamic", new DynamicDeSerializer(configScope));
			_serializerMap.Add("isEqual", new IsEqualDeSerializer(configScope));
			_serializerMap.Add("isNotEqual", new IsNotEqualDeSerializer(configScope));
			_serializerMap.Add("isGreaterEqual", new IsGreaterEqualDeSerializer(configScope));
			_serializerMap.Add("isGreaterThan", new IsGreaterThanDeSerializer(configScope));
			_serializerMap.Add("isLessEqual", new IsLessEqualDeSerializer(configScope));
			_serializerMap.Add("isLessThan", new IsLessThanDeSerializer(configScope));
			_serializerMap.Add("isNotEmpty", new IsNotEmptyDeSerializer(configScope));
			_serializerMap.Add("isEmpty", new IsEmptyDeSerializer(configScope));
			_serializerMap.Add("isNotNull", new IsNotNullDeSerializer(configScope));
			_serializerMap.Add("isNotParameterPresent", new IsNotParameterPresentDeSerializer(configScope));
			_serializerMap.Add("isNotPropertyAvailable", new IsNotPropertyAvailableDeSerializer(configScope));
			_serializerMap.Add("isNull", new IsNullDeSerializer(configScope));
			_serializerMap.Add("isParameterPresent", new IsParameterPresentDeSerializer(configScope));
			_serializerMap.Add("isPropertyAvailable", new IsPropertyAvailableDeSerializer(configScope));
			_serializerMap.Add("iterate", new IterateSerializer(configScope));		
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public IDeSerializer GetDeSerializer(string name) 
		{
			return (IDeSerializer) _serializerMap[name];
		}

	}
}
