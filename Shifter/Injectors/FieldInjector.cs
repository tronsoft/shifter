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
    internal class FieldInjector : IInjector
    {
        private readonly IShifterContext context;
        private readonly FieldInfo field;
        private readonly object value;

        public FieldInjector(IShifterContext context, FieldInfo field, object value)
        {
            Assume.ArgumentNotNull(context, "context");
            Assume.ArgumentNotNull(field, "field");
            Assume.ArgumentNotNull(value, "value");

            this.context = context;
            this.field = field;
            this.value = value;           
        }

        public void Inject()
        {
            if (context.Instance == null)
            {
                throw new TypeResolvingFailedException(string.Format(Strings.InstanceIsNull, context.TypeToResolve.FullName));
            }

            field.SetValue(context.Instance, value);
        }
    }
}