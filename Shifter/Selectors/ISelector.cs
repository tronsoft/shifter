﻿//===============================================================================
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
using System.Reflection;

namespace Shifter.Selectors
{
    internal interface ISelector<T> where T : MemberInfo
    {
        Type MemberType { get; }
    }
}
