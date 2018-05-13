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
using System.Runtime.Serialization;

namespace Shifter.Exceptions
{
    [Serializable]
    public class NoInheritanceDependencyException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NoInheritanceDependencyException()
        {
        }

        public NoInheritanceDependencyException(string message) : base(message)
        {
        }

        public NoInheritanceDependencyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NoInheritanceDependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}