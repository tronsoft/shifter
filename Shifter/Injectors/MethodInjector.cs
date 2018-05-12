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
    public class MethodInjector : IInjector
    {
        private readonly IShifterContext context;
        private readonly MethodBase method;
        private readonly object[] parameters;

        public MethodInjector(IShifterContext context, MethodBase method, object[] parameters)
        {
            Assume.ArgumentNotNull(context, "context");
            Assume.ArgumentNotNull(method, "method");
            Assume.ArrayNotNullOrEmpty(parameters, "parameters");

            this.context = context;
            this.method = method;
            this.parameters = parameters;
        }

        public void Inject()
        {
            method.Invoke(context.Instance, parameters);
        }
    }
}