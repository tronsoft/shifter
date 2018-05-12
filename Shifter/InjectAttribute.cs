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

namespace Shifter
{
    /// <summary>
    /// Indicates that injection should be applied on the member.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor 
                    | AttributeTargets.Method 
                    | AttributeTargets.Property 
                    | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class InjectAttribute : Attribute
    {
    }
}
