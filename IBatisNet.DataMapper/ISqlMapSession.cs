#region Apache Notice
/*****************************************************************************
 * $Revision: 474910 $
 * $LastChangedDate: 2006-11-14 19:33:12 +0100 (mar., 14 nov. 2006) $
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

using IBatisNet.Common;

namespace IBatisNet.DataMapper
{
    /// <summary>
    /// SqlMap Session contract
    /// </summary>
    public interface ISqlMapSession : IDalSession
    {

        /// <summary>
        /// Gets the SQL mapper.
        /// </summary>
        /// <value>The SQL mapper.</value>
        ISqlMapper SqlMapper { get; }

        /// <summary>
        /// Create the connection
        /// </summary>
        void CreateConnection();

        /// <summary>
        /// Create the connection
        /// </summary>
        void CreateConnection(string connectionString);
    }
}