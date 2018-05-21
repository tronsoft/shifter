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

using System.Collections.Generic;
using System.Reflection;

namespace Shifter.Selectors
{
    internal abstract class MultipleSelector<T> : SelectorBase<T> where T : MemberInfo 
    {
        public abstract IEnumerable<T> Select(IShifterContext context);
    }
}
