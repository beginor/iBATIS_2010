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

namespace IBatisNet.Common.Utilities.Objects.Members
{
    /// <summary>
    /// Accessor factory
    /// </summary>
    public class AccessorFactory
    {
        private ISetAccessorFactory _setAccessorFactory = null;
        private IGetAccessorFactory _getAccessorFactory = null;

        /// <summary>
        /// The factory which build <see cref="ISetAccessor"/>
        /// </summary>
        public ISetAccessorFactory SetAccessorFactory
        {
            get { return _setAccessorFactory; }
        }

        /// <summary>
        /// The factory which build <see cref="IGetAccessor"/>
        /// </summary>
        public IGetAccessorFactory GetAccessorFactory
        {
            get { return _getAccessorFactory; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessorFactory"/> class.
        /// </summary>
        /// <param name="setAccessorFactory">The set accessor factory.</param>
        /// <param name="getAccessorFactory">The get accessor factory.</param>
        public AccessorFactory(ISetAccessorFactory setAccessorFactory,
            IGetAccessorFactory getAccessorFactory)
        {
            _setAccessorFactory = setAccessorFactory;
            _getAccessorFactory = getAccessorFactory;
        }
    }
}
