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

using System.Reflection;
using Shifter.Exceptions;
using Shifter.Utils;

namespace Shifter.Injectors
{
    public class PropertyInjector : IInjector
    {
        private readonly PropertyInfo property;
        private readonly IShifterContext context;
        private readonly object value;

        public PropertyInjector(PropertyInfo property, IShifterContext context, object value)
        {
            Assume.ArgumentNotNull(property, "property");
            Assume.ArgumentNotNull(context, "context");

            this.property = property;
            this.context = context;
            this.value = value;
        }

        public void Inject()
        {
            if (context.Instance == null)
            {
                throw new TypeResolvingFailedException(string.Format(Strings.InstanceIsNull, context.TypeToResolve.FullName));
            }

            property.SetValue(context.Instance, value, null);
        }
    }
}
