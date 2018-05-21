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
    internal class TypeResolvingFailedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public TypeResolvingFailedException()
        {
        }

        public TypeResolvingFailedException(string message) : base(message)
        {
        }

        public TypeResolvingFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TypeResolvingFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}