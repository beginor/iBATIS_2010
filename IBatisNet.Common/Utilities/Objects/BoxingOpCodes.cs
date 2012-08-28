#region Apache Notice
/*****************************************************************************
 * $Revision: 374175 $
 * $LastChangedDate: 2006-02-19 12:37:22 +0100 (Sun, 19 Feb 2006) $
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
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using IBatisNet.Common.Exceptions;

namespace IBatisNet.Common.Utilities.Objects
{
    /// <summary>  
    /// Helper class that returns appropriate boxing opcode based on type  
    /// </summary>  
    /// <remarks>From Spring.NET</remarks>
    public sealed class BoxingOpCodes
    {
        private static IDictionary boxingOpCodes;

        /// <summary>
        /// Initializes the <see cref="BoxingOpCodes"/> class.
        /// </summary>
        static BoxingOpCodes()
        {
            boxingOpCodes = new Hashtable();
            boxingOpCodes[typeof(sbyte)] = OpCodes.Ldind_I1;
            boxingOpCodes[typeof(short)] = OpCodes.Ldind_I2;
            boxingOpCodes[typeof(int)] = OpCodes.Ldind_I4;
            boxingOpCodes[typeof(long)] = OpCodes.Ldind_I8;
            boxingOpCodes[typeof(byte)] = OpCodes.Ldind_U1;
            boxingOpCodes[typeof(ushort)] = OpCodes.Ldind_U2;
            boxingOpCodes[typeof(uint)] = OpCodes.Ldind_U4;
            boxingOpCodes[typeof(ulong)] = OpCodes.Ldind_I8;
            boxingOpCodes[typeof(float)] = OpCodes.Ldind_R4;
            boxingOpCodes[typeof(double)] = OpCodes.Ldind_R8;
            boxingOpCodes[typeof(char)] = OpCodes.Ldind_U2;
            boxingOpCodes[typeof(bool)] = OpCodes.Ldind_I1;
        }

        /// <summary>
        /// Gets the <see cref="OpCode"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static OpCode GetOpCode(Type type)
        {
            if (type.IsEnum)
            {
                type = Enum.GetUnderlyingType(type);
            }
            return (OpCode)boxingOpCodes[type];
        }

    }
}
