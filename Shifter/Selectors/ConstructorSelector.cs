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
using System.Reflection;
using Shifter.Utils;

namespace Shifter.Selectors
{
    internal class ConstructorSelector : UnarySelectorBase<ConstructorInfo>
    {
        public override ConstructorInfo Selecter(IShifterContext context)
        {
            Assume.ArgumentNotNull(context, "context");

            var bindingFlags = new BindingFlagsCombiner().Execute(context.Container.Options.ResolvePrivateMembers);
            var constructors = new List<ConstructorInfo>(context.TypeToResolve.GetConstructors(bindingFlags));
            var injectMarkedConstructors = constructors.FindAll(constructor => constructor.IsDefined(typeof(InjectAttribute), false));

            if (injectMarkedConstructors.Count > 1)
            {
                throw new NotSupportedException(Strings.MultipleConstructorInjectionIsNotSupported);
            }

            if (injectMarkedConstructors.Count == 1)
            {
                return injectMarkedConstructors[0];
            }

            if (constructors.Count > 0)
            {
                return constructors[0];
            }

            return context.TypeToResolve.GetConstructor(Type.EmptyTypes);
        }
    }
}