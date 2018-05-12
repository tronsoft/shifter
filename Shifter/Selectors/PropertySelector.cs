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
    public class PropertySelector : MultipleSelector<PropertyInfo>
    {
        public override IEnumerable<PropertyInfo> Select(IShifterContext context)
        {
            Assume.ArgumentNotNull(context, "context");

            var bindingFlags = new BindingFlagsCombiner().Execute(context.Container.Options.ResolvePrivateMembers);
            var propertyList = new List<PropertyInfo>(context.TypeToResolve.GetProperties(bindingFlags));
            
            return propertyList.Where(property => property.IsDefined(typeof(InjectAttribute), false));
        }
    }
}