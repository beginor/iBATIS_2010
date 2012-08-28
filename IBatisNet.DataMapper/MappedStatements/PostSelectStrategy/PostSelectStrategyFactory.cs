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

using System.Collections;
using System.Collections.Specialized;

namespace IBatisNet.DataMapper.MappedStatements.PostSelectStrategy
{
	/// <summary>
	/// Factory to get <see cref="IPostSelectStrategy"/> implementation.
	/// </summary>
	public sealed class PostSelectStrategyFactory
	{
		private static IDictionary _strategies = new HybridDictionary();

		/// <summary>
		/// Initializes the <see cref="PostSelectStrategyFactory"/> class.
		/// </summary>
		static PostSelectStrategyFactory()
		{
			_strategies.Add(PostBindind.ExecuteMethod.ExecuteQueryForArrayList, new ArrayStrategy());
			_strategies.Add(PostBindind.ExecuteMethod.ExecuteQueryForIList, new ListStrategy());
			_strategies.Add(PostBindind.ExecuteMethod.ExecuteQueryForObject, new ObjectStrategy());
			_strategies.Add(PostBindind.ExecuteMethod.ExecuteQueryForStrongTypedIList, new StrongTypedListStrategy());
#if dotnet2           
			_strategies.Add(PostBindind.ExecuteMethod.ExecuteQueryForGenericIList, new GenericListStrategy());
#endif
		}


		/// <summary>
		/// Gets the <see cref="IPostSelectStrategy"/>.
		/// </summary>
		/// <param name="method">The <see cref="PostBindind.ExecuteMethod"/>.</param>
		/// <returns>The <see cref="IPostSelectStrategy"/></returns>
		public static IPostSelectStrategy Get(PostBindind.ExecuteMethod method)
		{
			return (IPostSelectStrategy)_strategies[method];
		}
	}
}
