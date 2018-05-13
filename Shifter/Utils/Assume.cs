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

using System;
using System.Globalization;

namespace Shifter.Utils
{
    public static class Assume
    {
        public static void ArgumentNotNull(object argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argumentName)) throw new ArgumentNullException("argumentName");
            if (argument == null) throw new ArgumentNullException(argumentName);
        }

        public static void ArrayNotNullOrEmpty(object[] argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argumentName)) throw new ArgumentNullException("argumentName");
            if (argument == null) throw new ArgumentNullException(argumentName);
            if (argument.Length == 0) throw new ArgumentNullException(argumentName);
        }

        public static void NotNullOrEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argumentName)) throw new ArgumentNullException("argumentName");
            if (string.IsNullOrEmpty(argument)) throw new ArgumentNullException(argumentName);
        }

        public static void TypeIsAssignableFrom(Type from, Type to, string argumentName)
        {
            if (from == null) throw new ArgumentNullException("from");
            if (to == null) throw new ArgumentNullException("to");
            if (string.IsNullOrEmpty(argumentName)) throw new ArgumentNullException("argumentName");

            if (!to.IsAssignableFrom(from))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, 
                                                          Strings.TypesAreNotAssignableFrom, 
                                                          argumentName, 
                                                          to.FullName, 
                                                          from.FullName));
            }
        }
    }
}
