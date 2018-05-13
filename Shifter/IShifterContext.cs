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
using System.Collections.Generic;
using Shifter.Strategies;

namespace Shifter
{
    public interface IShifterContext
    {
        IShifterContainer Container { get; }
        Type TypeToResolve { get; }
        object Resolve();
        object Instance { get; set; }
        IList<IResolutionStrategy> Strategies { get; }
        bool CanCreate { get; }
    }
}