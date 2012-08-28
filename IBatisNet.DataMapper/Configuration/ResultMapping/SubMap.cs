
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 576082 $
 * $Date: 2007-09-16 06:04:01 -0600 (Sun, 16 Sep 2007) $
 * Author : Gilles Bayon
 * iBATIS.NET Data Mapper
 * Copyright (C) 2004 - Apache Fondation
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

using System;
using System.Xml.Serialization;

#endregion

namespace IBatisNet.DataMapper.Configuration.ResultMapping
{
	/// <summary>
	/// Summary description for SubMap.
	/// </summary>
	[Serializable]
	[XmlRoot("subMap", Namespace="http://ibatis.apache.org/mapping")]
	public class SubMap
	{
		// <resultMap id="document" class="Document">
		//			<result property="Id" column="Document_ID"/>
		//			<result property="Title" column="Document_Title"/>
		//			<discriminator column="Document_Type" [formula="CustomFormula, AssemblyName"] /> 
		//						-- attribute column (not used if discriminator use a custom formula)
		//						-- attribute formula (not required will used the DefaultFormula) calculate the discriminator value (DefaultFormula is default), else used an aliasType wich implement IDiscriminatorFormula), 
		//			<subMap value="Book" -- discriminator value
		//					resultMapping="book" />
		//	</resultMap>
		//
		//  <resultMap 
		//		id="book"  
		//		class="Book"
		//		extend="document">
		//  ...
		// </resultMap>

		#region Fields
		[NonSerialized]
		private string _discriminatorValue = string.Empty;
		[NonSerialized]
		private string _resultMapName = string.Empty;
		[NonSerialized]
		private IResultMap _resultMap = null;
		#endregion 

		#region Properties

		/// <summary>
		/// Discriminator value
		/// </summary>
		[XmlAttribute("value")]
		public string DiscriminatorValue
		{
			get { return _discriminatorValue; }
		}

		/// <summary>
		/// The name of the ResultMap used if the column value is = to the Discriminator Value
		/// </summary>
		[XmlAttribute("resultMapping")]
		public string ResultMapName
		{
			get { return _resultMapName; }
		}

		/// <summary>
		/// The resultMap used if the column value is = to the Discriminator Value
		/// </summary>
		[XmlIgnore]
		public IResultMap ResultMap
		{
			get { return _resultMap; }
			set { _resultMap = value; }
		}

		#endregion 

		#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SubMap"/> class.
        /// </summary>
        /// <param name="discriminatorValue">The discriminator value.</param>
        /// <param name="resultMapName">Name of the result map.</param>
        public SubMap(string discriminatorValue, string resultMapName)
		{
            _discriminatorValue = discriminatorValue;
            _resultMapName = resultMapName;
		}
		#endregion 

	}
}
