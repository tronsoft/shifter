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
using Shifter.Utils;

namespace Shifter.Selectors
{
    using System.Linq;

    public class MethodSelector : MultipleSelector<MethodInfo>
    {
        public override IEnumerable<MethodInfo> Select(IShifterContext context)
        {
            Assume.ArgumentNotNull(context, "context");

            var bindingFlags = new BindingFlagsCombiner().Execute(context.Container.Options.ResolvePrivateMembers);

            return new List<MethodInfo>(context.TypeToResolve.GetMethods(bindingFlags))
                .Where(m => m.IsDefined(typeof(InjectAttribute), false));
        }
    }
}