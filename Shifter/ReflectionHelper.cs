//===============================================================================
// TRONSoft
//
// Shifter
//
//===============================================================================
// Copyright © Ton de Ron.
//
// All rights reserved.
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//
//===============================================================================

using System.Reflection;

namespace Shifter
{
    internal static class ReflectionHelper
    {
        public static void GetConstructorParameterInfo(ConstructorInfo constructor)
        {
            //ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            //foreach (var constructor in constructors)
            //{
                foreach (var parameter in constructor.GetParameters())
                {
            //        parameter.
                }
           // }
        }
    }
}
