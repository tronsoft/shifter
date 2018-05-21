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
using System.Linq;
using System.Reflection;
using Shifter.Utils;

namespace Shifter.Selectors
{
    internal class FieldSelector : MultipleSelector<FieldInfo>
    {
        public override IEnumerable<FieldInfo> Select(IShifterContext context)
        {
            Assume.ArgumentNotNull(context, "context");

            var bindingFlags = new BindingFlagsCombiner().Execute(context.Container.Options.ResolvePrivateMembers);
            var fieldList = new List<FieldInfo>(context.TypeToResolve.GetFields(bindingFlags));

            return fieldList.Where(field => field.IsDefined(typeof(InjectAttribute), false));
        }
    }
}
