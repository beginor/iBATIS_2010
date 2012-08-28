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
    /// The <see cref="IGet"/> interface defines a field/property get contrat to get the
    /// value of a field or a property.
    /// </summary>
    public interface IGet
    {
        /// <summary>
        /// Gets the value stored in the field/property for the specified target.
        /// </summary>
        /// <param name="target">Object to retrieve the field/property from.</param>
        /// <returns>The value.</returns>
        object Get(object target);
    }
}
