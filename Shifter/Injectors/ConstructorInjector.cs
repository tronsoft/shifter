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
using Shifter.Utils;

namespace Shifter.Injectors
{
    internal class ConstructorInjector : IInjector
    {
        private readonly ConstructorInfo constructor;
        private readonly IShifterContext context;
        private readonly object[] arguments;

        public ConstructorInjector(ConstructorInfo constructor, IShifterContext context, object[] arguments)
        {
            Assume.ArgumentNotNull(constructor, "constructor");
            Assume.ArgumentNotNull(context, "context");

            this.constructor = constructor;
            this.context = context;
            this.arguments = arguments;
        }

        public void Inject()
        {
            context.Instance = constructor.Invoke(arguments);
        }
    }
}