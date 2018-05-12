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
using Shifter.Injectors;
using Shifter.Selectors;
using Shifter.Utils;

namespace Shifter.Materializers
{
    public class MethodMaterializer : IMaterializer
    {
        private readonly IShifterContext context;

        public MethodMaterializer(IShifterContext context)
        {
            Assume.ArgumentNotNull(context, "context");
            Assume.ArgumentNotNull(context.Instance, "context.Instance");

            this.context = context;
        }

        public virtual void Engage()
        {
            // Iterate the selected methods
            var selector = new MethodSelector();
            foreach (var method in selector.Select(context))
            {
                var methodInjector = new MethodInjector(context, method, ResolveArguments(method));
                methodInjector.Inject();
            }
        }

        private object[] ResolveArguments(MethodBase method)
        {
            // Now get the parameters of the method and register the types
            ParameterInfo[] parameters = method.GetParameters();
            var arguments = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                // if the field is not yet registered throw an exception 
                if (!context.Container.IsTypeRegistered(parameter.ParameterType))
                {
                    throw new TypeResolvingFailedException(Strings.TypeNotRegistered);
                }

                // Add arguments to context
                arguments[i] = context.Container.Resolve(parameter.ParameterType); 
            }

            return arguments;
        }
    }
}
